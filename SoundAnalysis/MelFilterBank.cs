using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public class MelFilterBank
    {
        public const int DefaultFilterCount = 20;

        int filterCount;
        int[] filterIndices;
        double[] filterPoints;
        int fftSize;
        double indexToFreqCoefficient;

        public int FilterCount => filterCount;

        public MelFilterBank(WaveFile waveFile, int fftSize, int filterCount = DefaultFilterCount, double minFreq = 0, double maxFreq = 8000)
        {
            this.fftSize = fftSize;
            this.filterCount = filterCount;
            minFreq = Math.Max(minFreq, 0);
            maxFreq = Math.Min(maxFreq, waveFile.WaveHeader.sampleRate);
            double minMel = ToMel(minFreq);
            double maxMel = ToMel(maxFreq);
            filterIndices = new int[filterCount + 2];
            filterPoints = new double[filterCount + 2];
            indexToFreqCoefficient = ((double)waveFile.WaveHeader.sampleRate) / (fftSize - 1);
            for (int i = 0; i < filterCount + 2; i++)
            {
                double mel = minMel + (maxMel - minMel) * i / (filterCount + 1);
                filterIndices[i] = (int)Math.Round(ToHertz(mel) / indexToFreqCoefficient);
                filterPoints[i] = ToHertz(mel);
            }
        }

        private double ToMel(double hertz)
        {
            return 1127 * Math.Log(1 + hertz / 0.7);
        }

        private double ToHertz(double mel)
        {
            return 0.7 * (Math.Exp(mel / 1127) - 1);
        }

        // triangular filter
        public double[] GetFilteredFFTEnergy(double[] fft)
        {
            if (2 * fft.Length != fftSize)
                throw new ArgumentException();

            double[] result = new double[filterCount];
            for (int i = 1; i <= filterCount; i++)
            {
                double sum = 0;
                for (int j = filterIndices[i - 1]; j < filterIndices[i];++j)
                {
                    sum += fft[j] * fft[j] * Math.Max((j * indexToFreqCoefficient - filterPoints[i - 1]) / (filterPoints[i] - filterPoints[i - 1]), 0);
                }
                for (int j = filterIndices[i]; j <= filterIndices[i+1]; ++j)
                {
                    sum += fft[j] * fft[j] * Math.Max((filterPoints[i + 1] - j * indexToFreqCoefficient) / (filterPoints[i + 1] - filterPoints[i]), 0);
                }

                result[i - 1] = sum;
            }
            return result;
        }
    }
}
