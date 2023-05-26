using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public class Clip
    {
        const double voiceLSTER = 0.000175;
        const double voiceZSTD = 0.0275;
        const double voiceSR = 0.0001;
        const double voiceVDR = 0.825;
        const double maleF0 = 170;

        public double FrameRate { get => waveFile.WaveHeader.sampleRate / SampleInterval; }
        private double SampleInterval { get => samplesPerFrame * (1 - overlapRatio); }
        private int FrameCount => sampleCount / (int)SampleInterval;
        int samplesPerFrame;
        double overlapRatio;
        WaveFile waveFile;
        WindowFunction window = new RectangularWindow();

        int startFrameIndex, sampleCount;

        public double StartTime { get => (double)startFrameIndex / waveFile.WaveHeader.sampleRate; }
        public double Duration { get => (double)sampleCount / waveFile.WaveHeader.sampleRate; }
        public WaveFile WaveFile => waveFile;

        public int StartIndex { get => startFrameIndex; }
        public int SampleCount { get => sampleCount; }

        public Clip(WaveFile waveFile, int samplesPerFrame, double overlapRatio, int startFrameIndex = 0, int sampleCount = -1)
        {
            if (overlapRatio < 0 || overlapRatio >= 1)
            {
                throw new ArgumentOutOfRangeException("Overlap ratio should be from the range [0,1)");
            }
            this.waveFile = waveFile;
            this.samplesPerFrame = samplesPerFrame;
            this.overlapRatio = overlapRatio;

            this.startFrameIndex = startFrameIndex;
            this.sampleCount = sampleCount == -1 ? waveFile.SampleCount - startFrameIndex : sampleCount;

            window.GetBordersFromClip(this);
        }

        public Clip(WaveFile waveFile, double sampleDurationMs, double overlapRatio, double startTimeSeconds = 0, double clipDuration = -1)
            : this(waveFile,
                  (int)Math.Round(waveFile.WaveHeader.sampleRate * sampleDurationMs / 1000),
                  overlapRatio,
                  (int)Math.Round(startTimeSeconds * waveFile.WaveHeader.sampleRate),
                  clipDuration == -1 ? -1 : (int)Math.Round(clipDuration * waveFile.WaveHeader.sampleRate))
        { }

        public Clip(Clip other) : this(other.waveFile, other.samplesPerFrame, other.overlapRatio, other.startFrameIndex, other.sampleCount) { }
        public Clip(Clip other, double start, double duration) : this(other.waveFile, other.samplesPerFrame, other.overlapRatio, start, duration) { }

        public void SetWindow(WindowFunction window)
        {
            this.window = window;
            //this.window.GetBordersFromClip(this);
        }

        public IEnumerable<Frame> GetFrames()
        {
            double index = startFrameIndex;
            while (index + samplesPerFrame <= startFrameIndex + sampleCount)
            {
                yield return new Frame(waveFile, (uint)index, (uint)samplesPerFrame, window);
                index += SampleInterval;
            }
        }

        public IEnumerable<DoubleTimeSpan> GetFragments(Predicate<Frame> predicate)
        {
            bool isInFragment = false;
            double fragmentStart = 0;
            double clipEnd = 0;
            foreach (var frame in GetFrames())
            {
                if (!predicate(frame))
                {
                    if (isInFragment)
                    {
                        isInFragment = false;
                        yield return new DoubleTimeSpan() { Start = fragmentStart, End = frame.FrameTime.Start };
                    }
                }
                else
                {
                    if (!isInFragment)
                    {
                        isInFragment = true;
                        fragmentStart = frame.FrameTime.Start;
                    }
                }
                clipEnd = frame.FrameTime.End;
            }
            if (isInFragment)
            {
                yield return new DoubleTimeSpan() { Start = fragmentStart, End = clipEnd };
            }
        }

        public double GetNormalizedStandardDeviation(int channel = 0)
        {
            double sum = 0, sumSquares = 0;
            for (int i = startFrameIndex; i < startFrameIndex + sampleCount; i++)
            {
                sum += waveFile[channel, i, window];
                sumSquares += waveFile[channel, i, window] * waveFile[channel, i, window];
            }
            sum /= sampleCount;
            sumSquares /= sampleCount;
            return Math.Sqrt(sumSquares - sum * sum) / GetMaxVolume(channel);
        }

        public double GetVolumeDynamicRange(int channel = 0)
        {
            double min = GetMinVolume(channel), max = GetMaxVolume(channel);
            return (max - min) / max;
        }

        public double GetVolumeUndulation(int channel = 0)
        {
            double prev = double.NaN, pprev = double.NaN;
            List<double> peaks = new List<double>();
            foreach (var v in GetFrames().Select(f => f.GetVolume()))
            {
                if (!double.IsNaN(prev))
                {
                    if (double.IsNaN(pprev) || (pprev - prev)*(prev - v) < 0)
                    {
                        peaks.Add(prev);
                    }
                }
                if (v != prev) pprev = prev;
                prev = v;
            }
            peaks.Add(prev); // add last volume (assumed as peak)
            prev = double.NaN;
            double sum = 0;
            foreach(var v in peaks)
            {
                if (!double.IsNaN(prev))
                    sum += Math.Abs(prev - v);
                prev = v;
            }
            return sum;
        }

        public double GetLowShortTimeEnergyRatio(int channel = 0)
        {
            double avSTE = GetFrames().Select(f => f.GetShortTimeEnergy(channel)).Average(); // TODO 1-sekundowe okno!!!
            return 0.5 / sampleCount * GetFrames().Select(f => Math.Sign(0.5 * avSTE - f.GetShortTimeEnergy(channel)) + 1).Sum();
        }

        public bool IsVoiceLSTERBased(int channel = 0)
        {
            return GetLowShortTimeEnergyRatio(channel) >= voiceLSTER;
        }

        public bool IsMusicLSTERBased(int channel = 0) => !IsVoiceLSTERBased(channel);

        public bool IsVoiceZSTDBased(int channel = 0)
        {
            return GetZCRStandardDeviation(channel) >= voiceZSTD;
        }

        public bool IsMusicZSTDBased(int channel = 0) => !IsVoiceZSTDBased(channel);

        public bool IsVoiceVDRBased(int channel = 0)
        {
            return GetVolumeDynamicRange(channel) >= voiceVDR || GetSilenceRatio(channel) >= voiceSR;
        }

        public bool IsMusicVDRBased(int channel = 0) => !IsVoiceVDRBased(channel);

        public double GetEnergyEntropy(uint segmentSize, int channel = 0)
        {
            return GetFrames().Select(f => f.GetEnergyEntropy(segmentSize, channel)).Sum();
        }

        public bool IsMaleVoiceMedian(int channel = 0)
        {
            var f0med = GetFilteredFundamentalFrequencyAutocorrelation(channel).Median();
            return f0med < maleF0;
        }

        public bool IsFemaleVoiceMedian(int channel = 0) => !IsMaleVoiceMedian(channel);

        public bool IsMaleVoiceAverage(int channel = 0)
        {
            var f0med = GetFilteredFundamentalFrequencyAutocorrelation(channel).Average();
            return f0med < maleF0;
        }

        public bool IsFemaleVoiceAverage(int channel = 0) => !IsMaleVoiceAverage(channel);

        public bool IsMaleVoiceCepstrumMedian(int channel = 0)
        {
            var f0med = GetFilteredFundamentalFrequencyCepstrum(channel).Median();
            return f0med < maleF0;
        }

        public bool IsFemaleVoiceCepstrumMedian(int channel = 0) => !IsMaleVoiceCepstrumMedian(channel);

        public IEnumerable<double> GetFilteredFundamentalFrequencyAutocorrelation(int channel = 0)
        {
            return GetFrames().Where(f => f.IsVoicedSpeech()).Select(f => f.GetFundamentalFrequencyFromAutocorrelation(channel)).Where(d => d >= 50 && d <= 500);
        }

        public IEnumerable<double> GetFilteredFundamentalFrequencyCepstrum(int channel = 0)
        {
            return GetFrames().Where(f => f.IsVoicedSpeech()).Select(f => f.GetFundamentalFrequencyFromCepstrum(channel)).Where(d => d >= 50 && d <= 500);
        }

        public double GetZCRStandardDeviation(int channel = 0)
        {
            //double sum = 0, sumSquares = 0;
            //foreach (var frame in GetFrames())
            //{
            //    double zcr = frame.GetZeroCrossingRate(channel);
            //    sum += zcr;
            //    sumSquares += zcr * zcr;
            //}
            //sum /= sampleCount;
            //sumSquares /= sampleCount;
            //return Math.Sqrt(sumSquares - sum * sum);
            return GetFrames().Select(f => f.GetZeroCrossingRate(channel)).StandardDeviation();
        }

        public double GetHighZeroCrossingRateRatio(int channel = 0)
        {
            double avZCR = GetFrames().Select(f => f.GetZeroCrossingRate(channel)).Average(); // TODO 1-sekundowe okno!!!
            return 0.5 / sampleCount * GetFrames().Select(f => Math.Sign(f.GetZeroCrossingRate(channel) - 1.5 * avZCR) + 1).Sum();
        }

        public double GetSilenceRatio(int channel = 0)
        {
            var f = GetFrames().Where(f => f.IsSilent(channel));
            var g = GetFrames().Select(f => f.IsSilent(channel));
            double silentFrames = GetFrames().Where(f => f.IsSilent(channel)).Count();
            return silentFrames / FrameCount;
        }

        public double GetAverageVolume(int channel = 0)
        {
            return GetFrames().Select(f => f.GetVolume(channel)).Average();
        }

        public double GetMaxVolume(int channel = 0)
        {
            return GetFrames().Select(f => f.GetVolume(channel)).Max();
        }

        public double GetMinVolume(int channel = 0)
        {
            return GetFrames().Select(f => f.GetVolume(channel)).Min();
        }

        public IEnumerable<double> GetWindowedData(bool alignToPowerOfTwo, int channel = 0)
        {
            window.GetBordersFromClip(this);
            long size = waveFile.SampleCount;//sampleCount;
            if (alignToPowerOfTwo)
            {
                size = 1;
                while (size < waveFile.SampleCount) size <<= 1;
            }
            long offsetL = size - waveFile.SampleCount, offsetR = offsetL / 2;
            if (offsetL % 2 == 0)
            {
                offsetL /= 2;
            }
            else
            {
                offsetL = offsetL / 2 + 1;
            }
            for (long i = 0; i < offsetL; ++i)
                yield return 0.0;
            for (int i = 0; i < startFrameIndex; ++i)
                yield return 0.0;
            for (int i = startFrameIndex; i < startFrameIndex + sampleCount; ++i)
                yield return waveFile[channel, i, window];
            for (int i = startFrameIndex + sampleCount; i < waveFile.SampleCount; ++i)
                yield return 0.0;
            for (long i = 0; i < offsetR; ++i)
                yield return 0.0;
        }

        public double[,] GetSpectrogram(int channel = 0, int minFreqSize = 0)
        {
            var ffts = GetFrames().Select(f => f.GetData(channel, true, minFreqSize).FFT());
            double[,] result = new double[ffts.First().Length, FrameCount];
            int i = 0;
            foreach (var f in ffts)
            {
                for (int j = 0; j < f.Length; j++)
                    result[f.Length - j - 1, i] = f[j];
                ++i;
            }
            return result;
        }
    }
}
