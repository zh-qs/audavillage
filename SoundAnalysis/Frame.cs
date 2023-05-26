using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public class Frame
    {
        const double silenceSTE = 5e-5;
        const double silenceZCR = 0.05;
        const double voicelessZCR = 0.1; // TODO dopracować

        private WaveFile waveFile;
        private uint sampleIndex;
        private uint frameLength;

        public int StartIndex => (int)sampleIndex;
        public int FrameLength => (int)frameLength;

        public DoubleTimeSpan FrameTime { get; }
        WindowFunction window;

        public Frame(WaveFile waveFile, uint sampleIndex, uint frameLength, WindowFunction window)
        {
            this.waveFile = waveFile;
            this.sampleIndex = sampleIndex;
            this.frameLength = frameLength;
            FrameTime = new DoubleTimeSpan() { Start = (double)sampleIndex / waveFile.WaveHeader.sampleRate, End = (double)(sampleIndex + frameLength) / waveFile.WaveHeader.sampleRate };
            this.window = window;
            window.GetBordersFromFrame(this);
        }

        public Frame(WaveFile waveFile, double timeInSeconds, uint durationMs, WindowFunction window)
            : this(waveFile, (uint)Math.Round(waveFile.WaveHeader.sampleRate * timeInSeconds), waveFile.WaveHeader.sampleRate * durationMs / 1000, window) { }

        public IEnumerable<double> GetData(int channel = 0, bool alignToPowerOfTwo = false, int minSize = 0)
        {
            long size = frameLength;
            if (alignToPowerOfTwo)
            {
                size = 1;
                while (size < frameLength || size < minSize) size <<= 1;
            }
            long offsetL = size - frameLength, offsetR = offsetL / 2;
            //long offsetL = 0, offsetR = size - frameLength;
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
            for (int i = (int)sampleIndex; i < sampleIndex + frameLength; ++i)
                yield return waveFile[channel, i, window];
            for (long i = 0; i < offsetR; ++i)
                yield return 0.0;
        }

        public double GetVolume(int channel = 0)
        {
            return Math.Sqrt(GetShortTimeEnergy(channel));
        }

        private double GetTotalEnergy(int channel = 0)
        {
            double sum = 0;
            for (int i = (int)sampleIndex; i < sampleIndex + frameLength; i++)
            {
                sum += waveFile[channel, i, window] * waveFile[channel, i, window];
            }
            return sum;
        }

        public double GetShortTimeEnergy(int channel = 0)
        {
            return GetTotalEnergy() / frameLength;
        }

        public double GetZeroCrossingRate(int channel = 0)
        {
            double sum = 0;
            for (int i = (int)sampleIndex + 1; i < sampleIndex + frameLength; i++)
            {
                if (waveFile[channel, i, window] == 0 && waveFile[channel, i - 1, window] == 0)
                    sum += 0.5;
                else 
                    sum += 0.5 * Math.Abs(Math.Sign(waveFile[channel, i, window]) - Math.Sign(waveFile[channel, i - 1, window]));
            }
            return sum / frameLength;
        }

        public double GetFundamentalFrequencyFromAutocorrelation(int channel = 0)
        {
            double max_r = double.NegativeInfinity;
            double arg_max = 0;

            bool isFirstPeak = true;
            double prev = double.PositiveInfinity;

            for (int l = 0; l < frameLength; ++l)
            {
                double r = 0;
                for (int i = (int)sampleIndex; i < sampleIndex + frameLength - l; ++i)
                {
                    r += waveFile[channel, i, window] * waveFile[channel, i + l, window];
                }

                if (isFirstPeak) // musimy pominąć pierwsze maksimum
                {
                    if (r < prev)
                        prev = r;
                    else isFirstPeak = false;
                }
                else if (r > max_r)
                {
                    max_r = r;
                    arg_max = l;
                }
            }
            if (arg_max == 0)
            {
                return 0;
            }
            return waveFile.WaveHeader.sampleRate / arg_max;
        }

        public double[] GetAutocorrelationArray(int channel = 0)
        {
            double[] res = new double[frameLength];
            for (int l = 1; l < frameLength; ++l)
            {
                double r = 0;
                for (int i = (int)sampleIndex; i < sampleIndex + frameLength - l; ++i)
                {
                    r += waveFile[channel, i, window] * waveFile[channel, i + l, window];
                }
                res[l] = r;
            }
            return res;
        }

        public double[] GetAMDFArray(int channel = 0)
        {
            double[] res = new double[frameLength];
            for (int l = 1; l < frameLength; ++l)
            {
                double r = 0;
                for (int i = (int)sampleIndex; i < sampleIndex + frameLength - l; ++i)
                {
                    r += Math.Abs(waveFile[channel, i + l, window] - waveFile[channel, i, window]);
                }
                r /= frameLength - l;
                res[l] = r;
            }
            return res;
        }

        public double GetFundamentalFrequencyFromAMDF(int channel = 0)
        {
            double min_r = double.PositiveInfinity;
            double arg_min = 0;

            bool isFirstPeak = true;
            double prev = double.NegativeInfinity;

            for (int l = 1; l < frameLength; ++l)
            {
                double r = 0;
                for (int i = (int)sampleIndex; i < sampleIndex + frameLength - l; ++i)
                {
                    r += Math.Abs(waveFile[channel, i + l, window] - waveFile[channel, i, window]);
                }
                r /= frameLength - l;

                if (isFirstPeak) // musimy pominąć pierwsze minimum
                {
                    if (r > prev)
                        prev = r;
                    else isFirstPeak = false;
                }
                else if (r < min_r)
                {
                    min_r = r;
                    arg_min = l;
                }
            }
            if (arg_min == 0)
            {
                return 0;
            }
            return waveFile.WaveHeader.sampleRate / arg_min;
        }

        
        public bool IsSilent(int channel = 0)
        {
            return GetShortTimeEnergy(channel) < silenceSTE && GetZeroCrossingRate(channel) > silenceZCR;
        }
        
        public bool IsVoicedSpeech(int channel = 0)
        {
            return GetShortTimeEnergy(channel) >= silenceSTE && GetZeroCrossingRate(channel) < voicelessZCR;
        }

        public bool IsUnvoicedSpeech(int channel = 0) // NOT the same as !IsVoicedSpeech() !
        {
            return GetShortTimeEnergy(channel) >= silenceSTE && GetZeroCrossingRate(channel) >= voicelessZCR;
        }

        public double GetEnergyEntropy(uint segmentSize, int channel = 0)
        {
            double totalEnergy = GetTotalEnergy();
            double sum = 0;
            uint i;
            for (i = sampleIndex; i < sampleIndex + frameLength; i += segmentSize)
            {
                double energy = 0;
                for (int j = 0; j < segmentSize; ++j)
                {
                    if ((int)i + j >= sampleIndex + frameLength) break;
                    energy += waveFile[channel, (int)i + j, window] * waveFile[channel, (int)i + j, window];
                }
                if (energy <= 0) continue;
                energy /= totalEnergy;
                sum -= energy * Math.Log2(energy);
            }
            if (i + 1 < sampleIndex + frameLength)
            {
                double energy = 0;
                for (uint j = i + 1; j < sampleIndex + frameLength; ++j)
                {
                    energy += waveFile[channel, (int)j, window] * waveFile[channel, (int)j, window];
                }
                if (energy > 0)
                {
                    energy /= totalEnergy;
                    sum -= energy * Math.Log2(energy);
                }
            }
            return sum;
        }

        public double[] GetCepstrumArray(int channel = 0, int minSize = 0)
        {
            return GetData(channel, true, minSize).Cepstrum();
        }

        public double[] GetFFTArray(int channel = 0, int minSize = 0)
        {
            return GetData(channel, true, minSize).FFT();
        }

        public double GetFundamentalFrequencyFromCepstrum(int channel = 0, int minCepstrumSize = 0)
        {
            const double min_f = 50, max_f = 500;
            double max_time = waveFile.WaveHeader.sampleRate / min_f, min_time = waveFile.WaveHeader.sampleRate / max_f;

            var cepstrum = GetData(channel, true, minCepstrumSize).Cepstrum();

            double max_r = double.NegativeInfinity;
            double arg_max = 0;

            bool isFirstPeak = true;
            double prev = double.PositiveInfinity;

            for (int l = (int)Math.Round(min_time); l < Math.Min(cepstrum.Length / 2, (int)Math.Round(max_time)); ++l)
            {
                double r = 0;
                for (int i = (int)sampleIndex; i < sampleIndex + frameLength - l; ++i)
                {
                    r += waveFile[channel, i, window] * waveFile[channel, i + l, window];
                }

                if (isFirstPeak) // musimy pominąć pierwsze maksimum
                {
                    if (r < prev)
                        prev = r;
                    else isFirstPeak = false;
                }
                else if (r > max_r)
                {
                    max_r = r;
                    arg_max = l;
                }
            }
            if (arg_max == 0)
            {
                return 0;
            }
            return waveFile.WaveHeader.sampleRate / arg_max;
        }

        public FreqFrame GetFFTFrame(int channel = 0, int minSize = 0)
        {
            return new FreqFrame(GetData(channel, true, minSize).FFT(), waveFile, window);
        }
    }
}
