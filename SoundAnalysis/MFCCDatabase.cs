using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public class MFCCVector
    {
        const double DefaultFrameDurationMs = 20.0;
        const double DefaultFrameOverlapRatio = 0.5;
        const int DefaultOutputVectorLength = 20;

        public double[] Data { get; set; }
        public MFCCVector() { }
        public MFCCVector(int size)
        {
            Data = new double[size];
        }
        public MFCCVector(double[] data)
        {
            Data = (double[])data.Clone();
        }

        public double DistanceTo(MFCCVector mfcc)
        {
            if (Data.Length != mfcc.Data.Length)
                throw new ArgumentException();

            double sum = 0;
            for (int i = 0; i < mfcc.Data.Length; ++i)
                sum += (Data[i] - mfcc.Data[i]) * (Data[i] - mfcc.Data[i]);

            return Math.Sqrt(sum);
        }

        public static IEnumerable<MFCCVector> FromFile(string path, int fftSize)
        {
            bool caught = false;
            BinaryReader reader = null;
            WaveFile waveFile = null;
            try
            {
                reader = new BinaryReader(new FileStream(path, FileMode.Open));
                waveFile = WaveFile.FromReader(reader);
                waveFile = new PreemphasizedWaveFileDecorator(waveFile, 0.97);
            }
            catch (InvalidWaveFormatException ex)
            {
                caught = true;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            if (caught) return Enumerable.Empty<MFCCVector>();

            MelFilterBank filterBank = new MelFilterBank(waveFile, fftSize);

            Clip clip = new Clip(waveFile, DefaultFrameDurationMs, DefaultFrameOverlapRatio);
            clip.SetWindow(new HammingWindow());

            var trimmedFrames = clip.GetFrames().SkipWhile(f => f.IsSilent()).SkipLastWhile(f => f.IsSilent());
            return trimmedFrames.Select(f => new MFCCVector(f.GetFFTFrame(0, fftSize).GetMFCC(filterBank, DefaultOutputVectorLength).ToArray()));
        }

        private static Random random = new Random();
        public static MFCCVector[] Cluster(IEnumerable<MFCCVector> vectors, int clusterCount)
        {
            MFCCVector[] input = vectors.ToArray();
            clusterCount = Math.Min(clusterCount, input.Length);
            MFCCVector[] result = new MFCCVector[clusterCount];

            // shuffle input randomly
            for (int i=input.Length-1; i>=1;--i)
            {
                int k = random.Next(i + 1);
                var tmp = input[k];
                input[k] = input[i];
                input[i] = tmp;
            }

            // choose first `clusterCount` of records to result
            for (int i = 0; i < clusterCount; ++i)
                result[i] = input[i];

            // assign nearest
            int[] assignment = new int[input.Length];
            int[] prevAssignment = new int[input.Length];
            for (int i=0;i<input.Length;++i)
            {
                prevAssignment[i] = -1;
            }
            while (!Enumerable.SequenceEqual(assignment, prevAssignment))
            {
                // fill prevAssignments
                for (int i=0;i<input.Length;++i)
                {
                    prevAssignment[i] = assignment[i];
                }
                // calculate assignments
                for (int i = 0; i < input.Length; ++i)
                {
                    int jmin = -1;
                    double minDist = double.PositiveInfinity;
                    for (int j = 0; j < clusterCount; ++j)
                    {
                        double d = input[i].DistanceTo(result[j]);
                        if (d < minDist)
                        {
                            minDist = d;
                            jmin = j;
                        }
                    }
                    assignment[i] = jmin;
                }
                // new centroids
                for (int i=0;i<result.Length;++i)
                {
                    result[i] = AverageMFCC(Enumerable.Range(0, input.Length).Where(j => assignment[j] == i).Select(j => input[j]));
                }
            }
            return result;
        }

        static MFCCVector AverageMFCC(IEnumerable<MFCCVector> vectors)
        {
            MFCCVector result = null;
            int count = 0;
            foreach (var vector in vectors)
            {
                if (result == null)
                    result = new MFCCVector(vector.Data.Length);

                for (int i=0;i<result.Data.Length;++i)
                    result.Data[i] += vector.Data[i];

                count++;
            }
            if (result == null)
                throw new ArgumentException("Empty sequence");
            for (int i=0;i<result.Data.Length;++i)
                result.Data[i] /= count;
            return result;
        }
    }

    public class MFCCDatabase
    {
        public int FFTSize { get; set; } = 1 << 14;
        public int ClusterCount { get; set; } = 10;

        public Dictionary<string, MFCCVector[]> Clusters { get; set; } = new Dictionary<string, MFCCVector[]>();

        public void Clear()
        {
            Clusters.Clear();
        }

        public void Train(string key, string directoryPath, Action<int, int> setProgress, bool searchSubfolders = false)
        {
            IEnumerable<string> GetFiles()
            {
                return Directory.EnumerateFiles(directoryPath, "*.wav", searchSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }

            Dictionary<string, List<MFCCVector>> map = new Dictionary<string, List<MFCCVector>>();

            int count = GetFiles().Count();
            int i = 0;
            setProgress(0, count);
            foreach (var path in GetFiles())
            {
                string relative = Path.GetRelativePath(directoryPath, path);
                int idx = relative.LastIndexOf('\\');
                string relativeKey;
                if (idx < 0)
                    relativeKey = key;
                else
                    relativeKey = key + (key.Length > 0 ? ":" : "") + relative.Substring(0, relative.LastIndexOf('\\'));
                if (!map.TryGetValue(relativeKey, out var list))
                {
                    list = new List<MFCCVector>();
                    map.Add(relativeKey, list);
                }

                list.AddRange(MFCCVector.FromFile(path, FFTSize));
                ++i;
                setProgress(i, count);
            }

            foreach (var entry in map)
            {
                Clusters.Add(entry.Key, MFCCVector.Cluster(entry.Value, ClusterCount));
            }
        }

        public string FindNearest(IEnumerable<MFCCVector> mfccs)
        {
            string minKey = "";
            double minDist = double.PositiveInfinity;
            var inputMFCCs = mfccs.ToArray();
            foreach (var entry in Clusters)
            {
                var dist = MinClusterDist(inputMFCCs, entry.Value);
                if (minDist > dist)
                {
                    minDist = dist;
                    minKey = entry.Key;
                }
            }
            return minKey;
        }

        private static double MinClusterDist(MFCCVector[] mfccs, MFCCVector[] clusters)
        {
            double minDistSum = 0;
            foreach (var vector in mfccs) 
            {
                double minDist = double.PositiveInfinity;
                for (int j=0;j<clusters.Length;++j)
                {
                    double d = vector.DistanceTo(clusters[j]);
                    if (minDist > d)
                    {
                        minDist = d;
                    }
                }
                minDistSum += minDist;
            }
            return minDistSum;
        }
    }
}
