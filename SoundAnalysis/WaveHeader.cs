using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace SoundAnalysisLib
{
    public enum AudioFormat : UInt16
    {
        PCM = 1,
        [Description("Microsoft ADPCM")]
        MSADPCM = 2,
        Float = 3,
        [Description("ITU G.711 A-law")]
        G711A = 6,
        [Description("ITU G.711 μ-law")]
        G711U = 7,
        [Description("IMA ADPCM")]
        IMAADPCM = 17,
        [Description("ITU G.723 ADPCM")]
        G723ADPCM = 20,
        [Description("GSM 6.10")]
        GSM610 = 49,
        [Description("ITU G.721 ADPCM")]
        G721ADPCM = 64,
        MPEG = 80,
        Experimental = 65535
    }

    public struct WaveHeader
    {
        public UInt32 fileSize;
        public UInt32 subChunk1Size;
        public AudioFormat audioFormat;
        public UInt16 numberOfChannels;
        public UInt32 sampleRate;
        public UInt32 byteRate;
        public UInt16 blockAlign;
        public UInt16 bitsPerSample;
        public UInt32 dataSize;

        public static WaveHeader ReadFromStream(BinaryReader reader)
        {
            AssertHeader(reader, "RIFF");

            WaveHeader header = new WaveHeader();

            header.fileSize = reader.ReadUInt32();

            AssertHeader(reader, "WAVE");
            AssertHeader(reader, "fmt ");

            header.subChunk1Size = reader.ReadUInt32();
            header.audioFormat = (AudioFormat)reader.ReadUInt16();
            header.numberOfChannels = reader.ReadUInt16();
            header.sampleRate = reader.ReadUInt32();
            header.byteRate = reader.ReadUInt32();
            header.blockAlign = reader.ReadUInt16();
            header.bitsPerSample = reader.ReadUInt16();

            for (int i=0;i<header.subChunk1Size - 16;++i)
            {
                reader.ReadByte();
            }

            string msg = Encoding.Default.GetString(reader.ReadBytes(4));
            while (msg == "LIST" || msg == "fact" || msg == "PEAK")  // LIST, fact, PEAK
            {
                var size = reader.ReadUInt32();
                for (int i = 0; i < size; ++i)
                {
                    reader.ReadByte();
                }
                msg = Encoding.Default.GetString(reader.ReadBytes(4));
            }

            AssertHeader(msg, "data");

            header.dataSize = reader.ReadUInt32();

            return header;
        }

        private static void AssertHeader(BinaryReader reader, string header)
        {
            string msg = Encoding.Default.GetString(reader.ReadBytes(4));
            AssertHeader(msg, header);
        }

        private static void AssertHeader(string msg, string header)
        {
            if (msg != header)
                throw new InvalidWaveFormatException($"missing \"{header}\" header");
        }
    }
}
