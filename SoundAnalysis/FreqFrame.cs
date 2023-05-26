using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public class FreqFrame
    {
        double[] data;
        WaveFile waveFile;

        private readonly double windowedDataCount;

        public FreqFrame(double[] data, WaveFile waveFile, WindowFunction window)
        {
            this.data = data;
            this.waveFile = waveFile;  
            windowedDataCount = window.Area();
        }

        private double GetFrequencyAtIndex(int index)
        {
            return waveFile.WaveHeader.sampleRate * index / data.Length * 0.5; // 0.5 because data contains only one half of FFT (second one is mirrored first, thus useless)
        }

        private int GetIndexAtFrequency(double freq)
        {
            return (int)Math.Round(2.0 * data.Length / waveFile.WaveHeader.sampleRate * freq);
        }

        public double GetEnergy()
        {
            return data.Select(d => d * d).Average();
        }

        public double GetEnergyWindowed()
        {
            return data.Select(d => d * d).Sum() / windowedDataCount;
        }

        public double GetVolume()
        {
            return Math.Sqrt(GetEnergy());
        }

        public double GetVolumeWindowed()
        {
            return Math.Sqrt(GetEnergyWindowed());
        }

        public double GetFrequencyCentroid()
        {
            double sumWithFreq = 0;
            for (int i=0;i<data.Length; i++)
            {
                sumWithFreq += data[i] * GetFrequencyAtIndex(i);
            }
            return sumWithFreq / data.Sum();
        }

        public double GetEffectiveBandtwidthSquared()
        {
            double sumWithFreq = 0, fc = GetFrequencyCentroid();
            for (int i = 0; i < data.Length; i++)
            {
                sumWithFreq += data[i] * data[i] * (GetFrequencyAtIndex(i) - fc) * (GetFrequencyAtIndex(i) - fc);
            }
            return sumWithFreq / data.Select(d => d * d).Sum();
        }

        public double GetEffectiveBandtwidth() => Math.Sqrt(GetEffectiveBandtwidthSquared());

        public double GetBandEnergy(double low, double high)
        {
            int lowIdx = GetIndexAtFrequency(low), highIdx = GetIndexAtFrequency(high);

            if (highIdx >= data.Length) highIdx = data.Length - 1;
            if (lowIdx < 0) lowIdx = 0;

            if (lowIdx >= highIdx) return 0.0;

            var rangeSq = data.Skip(lowIdx).Take(highIdx - lowIdx).Select(d => d * d);

            return rangeSq.Sum() / windowedDataCount;
        }

        public double GetBandEnergyRatio(double low, double high)
        {
            return GetBandEnergy(low, high) / GetVolumeWindowed();
        }

        public double GetEnergyRatioSubband(double low, double high) => GetBandEnergyRatio(low, high);

        public double GetSpectralFlatnessMeasure(double low, double high)
        {
            int lowIdx = GetIndexAtFrequency(low), highIdx = GetIndexAtFrequency(high);

            if (highIdx >= data.Length) highIdx = data.Length - 1;
            if (lowIdx < 0) lowIdx = 0;

            if (lowIdx >= highIdx) return 1.0;

            var rangeSq = data.Skip(lowIdx).Take(highIdx - lowIdx).Select(d => d * d);

            double avg = rangeSq.Average();

            if (avg == 0.0) return 1.0;

            return rangeSq.GeometricAverage() / avg;
        }

        public double GetSpectralCrestFactor(double low, double high)
        {
            int lowIdx = GetIndexAtFrequency(low), highIdx = GetIndexAtFrequency(high);

            if (highIdx >= data.Length) highIdx = data.Length - 1;
            if (lowIdx < 0) lowIdx = 0;

            if (lowIdx >= highIdx) return 1.0;

            var rangeSq = data.Skip(lowIdx).Take(highIdx - lowIdx).Select(d => d * d);

            double avg = rangeSq.Average();

            if (avg == 0.0) return 1.0;

            return rangeSq.Max() / avg;
        }
    }
}
