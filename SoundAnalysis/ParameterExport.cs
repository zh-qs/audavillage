using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public static class ParameterExport
    {
        public static readonly string[] ParameterHeaders = { "volume", "ste", "zcr", "f0_autocorrelation", "f0_amdf", "f0_cepstrum", "volume_freq", "freq_centroid", "eff_bw" };
        public static readonly string[] ClipParameterHeaders = { "silence_ratio", "vstd", "vdr", "volume_undulation", "lster", "energy_entropy", "zcrstd", "hzcrr", "f0_median", "f0_average", "f0_std" };
        public static void ExportAllFrameParametersCSV(this Clip clip, string filename, int channel = 0, int fftSize = 0)
        {
            List<string> text = new List<string>();
            text.Add("frame_start_sec;frame_end_sec;" + ParameterHeaders.Aggregate((s1, s2) => s1 + ";" + s2));
            text.AddRange(clip.GetFrames().Select(f =>
            {
                var ff = f.GetFFTFrame(0, fftSize);
                return $"{f.FrameTime.Start};{f.FrameTime.End};{f.GetVolume(channel)};{f.GetShortTimeEnergy(channel)};{f.GetZeroCrossingRate(channel)};{f.GetFundamentalFrequencyFromAutocorrelation(channel)};{f.GetFundamentalFrequencyFromAMDF(channel)};{f.GetFundamentalFrequencyFromCepstrum(0, fftSize)};{ff.GetVolume()};{ff.GetFrequencyCentroid()};{ff.GetEffectiveBandtwidth()}";
            }));

            File.WriteAllLines(filename, text);
        }

        public static void ExportCustomFrameParameterCSV(this Clip clip, string filename, string header, Func<Frame, double> map, int channel = 0)
        {
            List<string> text = new List<string>();
            text.Add($"frame_start_sec;frame_end_sec;{header}");
            text.AddRange(clip.GetFrames().Select(f => $"{f.FrameTime.Start};{f.FrameTime.End};{map(f)}"));

            File.WriteAllLines(filename, text);
        }

        public static void ExportAllClipParametersCSV(this IEnumerable<Clip> clipSequence, string filename, int channel = 0)
        {
            List<string> text = new List<string>();
            text.Add("clip_start_sec;clip_end_sec;" + ClipParameterHeaders.Aggregate((s1, s2) => s1 + ";" + s2));
            text.AddRange(clipSequence.Select(c => $"{c.StartTime};{c.Duration + c.StartTime};{c.GetSilenceRatio(channel)};{c.GetNormalizedStandardDeviation(channel)};{c.GetVolumeDynamicRange(channel)};{c.GetVolumeUndulation(channel)};{c.GetLowShortTimeEnergyRatio(channel)};{c.GetEnergyEntropy(100 /*TODO variable parameter*/, channel)};{c.GetZCRStandardDeviation(channel)};{c.GetHighZeroCrossingRateRatio(channel)};{c.GetFilteredFundamentalFrequencyAutocorrelation().Median()};{c.GetFilteredFundamentalFrequencyAutocorrelation().Average()};{c.GetFilteredFundamentalFrequencyAutocorrelation().StandardDeviation()}"));

            File.WriteAllLines(filename, text);
        }

        public static void ExportCustomClipParameterCSV(this IEnumerable<Clip> clipSequence, string filename, string header, Func<Clip, double> map, int channel = 0)
        {
            List<string> text = new List<string>();
            text.Add($"clip_start_sec;clip_end_sec;{header}");
            text.AddRange(clipSequence.Select(c => $"{c.StartTime};{c.Duration + c.StartTime};{map(c)}"));

            File.WriteAllLines(filename, text);
        }
    }
}
