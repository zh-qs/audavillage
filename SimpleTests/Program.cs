using SoundAnalysisLib;
using System.IO;

List<string> text = new();
//text.Add("filepath;clip_start_sec;clip_end_sec;silence_ratio;vstd;vdr;volume_undulation;lster;energy_entropy;zcrstd;hzcrr;f0mode;f0avg;f0med;f0std");
text.Add("filepath;clip_start_sec;clip_end_sec;energy_entropy");

var files = Directory.EnumerateFiles(@"E:\pw_archiwum\sem8\aipd\nagrania", "*.wav", SearchOption.AllDirectories);
foreach (string path in files)
{
    Console.WriteLine("Processing " + path + "...");
    BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
    WaveFile file = WaveFile.FromReader(reader);
    if (file.Duration == 0) continue;
    // batch classify files

    Clip c = new Clip(file, 40.0, 0.5);
    //var f0s = c.GetFrames().Select(f => f.GetFundamentalFrequencyFromAutocorrelation());
    //List<double> filtered = new List<double>();
    //foreach (var d in f0s)
    //{
    //    if (d < 50 || d > 500) continue;
    //    filtered.Add(d);
    //}
    //double f0mode = filtered.Mode();
    //filtered.Sort();
    //double f0std = filtered.StandardDeviation();
    //double f0avg = filtered.Average();
    //double f0med;
    //if (filtered.Count % 2 == 0)
    //{
    //    f0med = (filtered[filtered.Count / 2 - 1] + filtered[filtered.Count / 2]) / 2;
    //}
    //else
    //{
    //    f0med = filtered[filtered.Count / 2];
    //}
    int channel = 0;
    //text.Add($"{path};{c.StartTime};{c.Duration - c.StartTime};{c.GetSilenceRatio(channel)};{c.GetNormalizedStandardDeviation(channel)};{c.GetVolumeDynamicRange(channel)};{c.GetVolumeUndulation(channel)};{c.GetLowShortTimeEnergyRatio(channel)};{c.GetEnergyEntropy(100 /*TODO variable parameter*/, channel)};{c.GetZCRStandardDeviation(channel)};{c.GetHighZeroCrossingRateRatio(channel)};{f0mode};{f0avg};{f0med};{f0std}");
    text.Add($"{path};{c.StartTime};{c.Duration - c.StartTime};{c.GetEnergyEntropy(100 /*TODO variable parameter*/, channel)}");
}

File.WriteAllLines(@"E:\pw_archiwum\sem8\aipd\result.csv", text);


