namespace SoundAnalysisLib
{
    public abstract class WaveFile
    {
        protected WaveHeader waveHeader;
        protected abstract Array WaveData { get; }
        public WaveHeader WaveHeader { get => waveHeader; }
        public abstract int SampleCount { get; }
        public double Duration { get => ((double)SampleCount) / waveHeader.sampleRate; }
        public abstract double[] GetData(int channel = 0);
        public abstract double this[int channel, int index] { get; }
        public double this[int channel, int index, WindowFunction window] => this[channel, index] * window.GetWindowedValue(index);

        public static WaveFile FromReader(BinaryReader reader)
        {
            WaveHeader header = WaveHeader.ReadFromStream(reader);
            switch (header.audioFormat)
            {
                case AudioFormat.Float:
                    switch (header.bitsPerSample)
                    {
                        case 16:
                            return new GenericWaveFile<Half>(header, new SymbolStream<Half>(() => reader.ReadHalf()));
                        case 32:
                            return new GenericWaveFile<float>(header, new SymbolStream<float>(() => reader.ReadSingle()));
                        case 64:
                            return new GenericWaveFile<double>(header, new SymbolStream<double>(() => reader.ReadDouble()));
                        default:
                            throw new InvalidWaveFormatException($"incorrect bits per sample value: {header.bitsPerSample}");
                    }
                default:
                    switch (header.bitsPerSample)
                    {
                        case 8:
                            return new GenericWaveFile<sbyte>(header, new SymbolStream<sbyte>(() => (sbyte)(reader.ReadByte() - 128)));
                        case 16:
                            return new GenericWaveFile<Int16>(header, new SymbolStream<Int16>(() => reader.ReadInt16()));
                        case 32:
                            return new GenericWaveFile<Int32>(header, new SymbolStream<Int32>(() => reader.ReadInt32()));
                        case 64:
                            return new GenericWaveFile<Int64>(header, new SymbolStream<Int64>(() => reader.ReadInt64()));
                        default:
                            throw new InvalidWaveFormatException($"incorrect bits per sample value: {header.bitsPerSample}");
                    }
            }

        }
    }

    public class SymbolStream<T>
    {
        Func<T> streamFn;
        public SymbolStream(Func<T> streamFn)
        {
            this.streamFn = streamFn;
        }
        public T Next()
        {
            return streamFn();
        }
    }

    public class GenericWaveFile<T> : WaveFile
        where T : notnull, IComparable<T>
    {
        T[,] waveData;
        T[] maxValue;

        protected override Array WaveData => waveData;

        public T GetValue(int channel, int index) => waveData[channel, index];
        public override double this[int channel, int index] { get => Convert.ToDouble(waveData[channel, index]) / Convert.ToDouble(maxValue[channel]); }

        public override int SampleCount { get => waveData.GetLength(1); }

        public GenericWaveFile(WaveHeader header, SymbolStream<T> stream)
        {
            waveHeader = header;

            uint totalSamples = waveHeader.dataSize * 8 / waveHeader.bitsPerSample / waveHeader.numberOfChannels;

            waveData = new T[waveHeader.numberOfChannels, totalSamples];
            maxValue = new T[waveHeader.numberOfChannels];

            for (int i = 0; i < totalSamples; i++)
            {
                for (int j = 0; j < waveHeader.numberOfChannels; j++)
                {
                    waveData[j, i] = stream.Next();
                    if (maxValue[j].CompareTo(waveData[j, i]) < 0) maxValue[j] = waveData[j, i]; // TODO absolute value
                }
            }
        }

        public override double[] GetData(int channel = 0)
        {
            double[] data = new double[SampleCount];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Convert.ToDouble(waveData[channel, i]);
            }
            return data;
        }
    }

    public class PreemphasizedWaveFileDecorator : WaveFile
    {
        double[] data;
        public override int SampleCount { get => data.Length; }
        public PreemphasizedWaveFileDecorator(WaveFile waveFile, double preemprasisCoefficient, int channel = 0)
        {
            waveHeader = waveFile.WaveHeader;
            data = waveFile.GetData(channel);
            for (int i = data.Length - 1; i > 0; --i)
                data[i] -= preemprasisCoefficient * data[i - 1];
        }

        public override double this[int channel, int index] => data[index];

        protected override Array WaveData => data;

        public override double[] GetData(int channel = 0) => data;
    }
}