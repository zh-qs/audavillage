using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundAnalysisLib;
using ScottPlot.Plottable;

namespace SoundAnalysisApp
{
    public class MarkerSequence
    {
        WaveFile waveFile;
        List<VLine> internalMarkerPositions;
        Action refreshPlotsAction;

        public double ClipSampleDurationMs { get; set; }
        public double ClipOverlapRatio { get; set; }
        public int Count { get => internalMarkerPositions.Count + 1; }

        private MarkerSequence(WaveFile waveFile, List<VLine> vLines, Action refreshPlotsAction, double frameDurationMs, double overlapRatio)
        {
            internalMarkerPositions = vLines;
            this.refreshPlotsAction = refreshPlotsAction;
            this.waveFile = waveFile;
            ClipSampleDurationMs = frameDurationMs;
            ClipOverlapRatio = overlapRatio;
            foreach (var line in vLines)
            {
                line.DragEnabled = true;
                line.Dragged += Line_Dragged;
            }
        }

        private void Line_Dragged(object? sender, EventArgs e)
        {
            VLine line = (VLine)sender;
            if (line == null) return;
            if (line.X < 0) line.X = 0;
            if (line.X > waveFile.Duration) line.X = waveFile.Duration;
            refreshPlotsAction();
        }

        public static MarkerSequence DivideWaveFileEquallyByTime(WaveFile waveFile, double suggestedClipDurationSeconds, Action refreshPlotsAction, double frameDurationMs, double overlapRatio, bool visible = true)
        {
            int segments = (int)Math.Round(waveFile.Duration / suggestedClipDurationSeconds);
            return DivideWaveFileEqually(waveFile, segments, refreshPlotsAction, frameDurationMs, overlapRatio, visible);
        }

        public static MarkerSequence DivideWaveFileEqually(WaveFile waveFile, int segments, Action refreshPlotsAction, double frameDurationMs, double overlapRatio, bool visible = true)
        {
            List<VLine> vLines = new List<VLine>();
            double clipDuration = waveFile.Duration / segments;
            for (int i = 1; i < segments; i++)
            {
                VLine vLine = new VLine();
                vLine.X = i * clipDuration;
                vLine.IsVisible = visible;
                vLines.Add(vLine);
            }
            return new MarkerSequence(waveFile, vLines, refreshPlotsAction, frameDurationMs, overlapRatio);
        }

        public IEnumerable<Clip> GetClips()
        {
            internalMarkerPositions.Sort((l1,l2) => Math.Sign(l1.X - l2.X));
            double prev = 0;
            foreach (var vLine in internalMarkerPositions)
            {
                yield return new Clip(waveFile, ClipSampleDurationMs, ClipOverlapRatio, prev, vLine.X - prev);
                prev = vLine.X;
            }
            yield return new Clip(waveFile, ClipSampleDurationMs, ClipOverlapRatio, prev, -1);
        }

        public IEnumerable<DoubleTimeSpan> GetFragments(Predicate<Clip> predicate)
        {
            bool isInFragment = false;
            double fragmentStart = 0;
            double clipEnd = 0;
            foreach (var clip in GetClips())
            {
                if (!predicate(clip))
                {
                    if (isInFragment)
                    {
                        isInFragment = false;
                        yield return new DoubleTimeSpan() { Start = fragmentStart, End = clip.StartTime };
                    }
                }
                else
                {
                    if (!isInFragment)
                    {
                        isInFragment = true;
                        fragmentStart = clip.StartTime;
                    }
                }
                clipEnd = clip.StartTime + clip.Duration;
            }
            if (isInFragment)
            {
                yield return new DoubleTimeSpan() { Start = fragmentStart, End = clipEnd };
            }
        }

        public IEnumerable<VLine> GetVLines() => internalMarkerPositions;

        public override string ToString()
        {
            return internalMarkerPositions.Select(l => l.X).Aggregate("", (s, d) => s + d.ToString(System.Globalization.CultureInfo.InvariantCulture) + " ");
        }

        public static MarkerSequence FromString(WaveFile waveFile, string str, Action refreshPlotsAction, double frameDurationMs, double overlapRatio, bool visible = true)
        {
            List<VLine> vLines = new List<VLine>();
            var split = str.Trim().Split(' ');
            foreach (var line in split)
            {
                double d = double.Parse(line, System.Globalization.CultureInfo.InvariantCulture);
                if (d > waveFile.Duration)
                {
                    throw new InvalidDataException($"Clip border {d} is greater than WAV file duration");
                }
                VLine vLine = new VLine();
                vLine.X = d;
                vLine.IsVisible = visible;
                vLines.Add(vLine);
            }
            return new MarkerSequence(waveFile, vLines, refreshPlotsAction, frameDurationMs, overlapRatio);
        }
    }
}
