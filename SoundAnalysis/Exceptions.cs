using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public class InvalidWaveFormatException : Exception
    {
        public InvalidWaveFormatException(string msg) : base("Invalid WAV file format: " + msg) { }
    }
}
