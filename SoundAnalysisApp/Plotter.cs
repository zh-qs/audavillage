using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;
using ScottPlot.Plottable;
using SoundAnalysisLib;

namespace SoundAnalysisApp
{
    static class Plotter
    {
        public static SignalPlot PlotWindowedFile(FormsPlot plot, WaveFile waveFile, Clip clip, bool autoScale, Color color)
        {
            double[] data = clip.GetWindowedData(false).ToArray();
            var p = plot.Plot.AddSignal(data, sampleRate: waveFile.WaveHeader.sampleRate, color);
            if (autoScale) plot.Plot.AxisAuto();
            plot.Refresh();
            return p;
        }
        
        public static SignalPlot PlotFile(FormsPlot plot, WaveFile waveFile, bool autoScale, Color color)
        {
            double[] data = waveFile.GetData();
            return PlotArray(plot, data, waveFile.WaveHeader.sampleRate, autoScale, color);
        }

        public static SignalPlot PlotArray(FormsPlot plot, double[] data, double sampleRate, bool autoScale, Color color)
        {
            var p = plot.Plot.AddSignal(data, sampleRate: sampleRate, color);
            if (autoScale) plot.Plot.AxisAuto();
            plot.Refresh();
            return p;
        }

        public static ScottPlot.Plottable.IPlottable PlotClip(FormsPlot plot, Clip clip, Func<Frame, double> map, bool autoScale, Color color, Predicate<Frame>? cutPredicate = null)
        {
            double[] data;
            if (cutPredicate == null)
            {
                data = clip.GetFrames().Select(map).ToArray();
                //data = clip.GetFrames().Select(f => f.GetCepstrumArray().Select(d=>double.IsFinite(d)?d:-1).ToArray()).Aggregate(new double[] { }, (a1, a2) => a1.Concat(a2).ToArray()).ToArray();
            }
            else
            {
                data = clip.GetFrames().Select(f => cutPredicate(f) ? 0 : map(f)).ToArray();
            }
            var signal = plot.Plot.AddSignal(data, sampleRate: clip.FrameRate, color);
            //var signal = plot.Plot.AddHeatmap(clip.GetSpectrogram(0, 1 << 14), ScottPlot.Drawing.Colormap.Turbo);
            //signal.XMin = 0;
            //signal.XMax = clip.Duration;
            //signal.YMin = 0;

            signal.OffsetX = clip.StartTime;
            if (autoScale) plot.Plot.AxisAutoY();
            plot.Refresh();
            return signal;
        }

        public static ScottPlot.Plottable.IPlottable PlotClipSpectrogram(FormsPlot plot, Clip clip, bool autoScale, ScottPlot.Drawing.Colormap colormap, Predicate<Frame>? cutPredicate = null)
        {
            //double[] data;
            //if (cutPredicate == null)
            //{
            //    data = clip.GetFrames().Select(map).ToArray();
            //    //data = clip.GetFrames().Select(f => f.GetCepstrumArray().Select(d=>double.IsFinite(d)?d:-1).ToArray()).Aggregate(new double[] { }, (a1, a2) => a1.Concat(a2).ToArray()).ToArray();
            //}
            //else
            //{
            //    data = clip.GetFrames().Select(f => cutPredicate(f) ? 0 : map(f)).ToArray();
            //}
            //var signal = plot.Plot.AddSignal(data, sampleRate: clip.FrameRate, color);
            var signal = plot.Plot.AddHeatmap(clip.GetSpectrogram(0, 1 << 14), colormap);
            signal.XMin = 0;
            signal.XMax = clip.Duration;
            signal.YMin = 0;
            signal.YMax = clip.WaveFile.WaveHeader.sampleRate * 0.5;

            signal.OffsetX = clip.StartTime;
            if (autoScale) plot.Plot.AxisAutoY();
            plot.Refresh();
            return signal;
        }

        public static ScottPlot.Plottable.IPlottable PlotClips(FormsPlot plot, IEnumerable<Clip> clips, Func<Clip, double> map, bool autoScale, Color color)
        {
            double[] dataX = clips.Select(c => c.StartTime + c.Duration / 2).ToArray();
            double[] dataY = clips.Select(map).ToArray();
            var p = plot.Plot.AddScatter(dataX, dataY, color);
            if (autoScale) plot.Plot.AxisAutoY();
            plot.Refresh();
            return p;
        }

        public static ScottPlot.Plottable.IPlottable PlotParameters(FormsPlot plot, MarkerSequence markerSequence, Func<Frame, double> map, bool autoScale, Color color)
        {
            var clips = markerSequence.GetClips();
            double[] data = clips.Select(c => c.GetFrames().Select(map).ToArray()).Aggregate<double[], IEnumerable<double>>(Enumerable.Empty<double>(), (a1, a2) => a1.Concat(a2)).ToArray(); //clip.GetFrames().Select(map).ToArray();
            var signal = plot.Plot.AddSignal(data, sampleRate: clips.First().FrameRate, color);
            signal.OffsetX = 0;
            if (autoScale) plot.Plot.AxisAuto();
            plot.Refresh();
            return signal;
        }

        public static ScottPlot.Plottable.IPlottable[] PlotRanges(FormsPlot plot, IEnumerable<DoubleTimeSpan> ranges, Color color)
        {
            List<ScottPlot.Plottable.IPlottable> plottables = new List<ScottPlot.Plottable.IPlottable>();
            Color displayColor = Color.FromArgb(128, color.R, color.G, color.B);
            foreach (var range in ranges)
            {
                plottables.Add(plot.Plot.AddHorizontalSpan(range.Start, range.End, displayColor));
            }
            plot.Refresh();
            return plottables.ToArray();
        }

        public static ScottPlot.Plottable.IPlottable[] PlotRangesYesNo(FormsPlot plot, IEnumerable<DoubleTimeSpan> ranges, double duration, Color colorYes, Color colorNo)
        {
            List<ScottPlot.Plottable.IPlottable> plottables = new List<ScottPlot.Plottable.IPlottable>();
            Color displayColorYes = Color.FromArgb(128, colorYes.R, colorYes.G, colorYes.B);
            Color displayColorNo = Color.FromArgb(128, colorNo.R, colorNo.G, colorNo.B);
            List<double> borders = new List<double>();
            foreach (var range in ranges)
            {
                borders.Add(range.Start);
                borders.Add(range.End);
                //plottables.Add(plot.Plot.AddHorizontalSpan(range.Start, range.End, displayColor));
            }

            if (borders.Count == 0)
            {
                plottables.Add(plot.Plot.AddHorizontalSpan(0, duration, displayColorNo));
                plot.Refresh();
                return plottables.ToArray();
            }

            bool displayYes = borders[0] == 0;
            double prevB = displayYes ? -1 : 0;

            foreach(var b in borders)
            {
                if (prevB >= 0)
                {
                    plottables.Add(plot.Plot.AddHorizontalSpan(prevB, b, displayYes ? displayColorYes : displayColorNo));
                    displayYes = !displayYes;
                }
                prevB = b;
            }

            if (prevB != duration)
                plottables.Add(plot.Plot.AddHorizontalSpan(prevB, duration, displayYes ? displayColorYes : displayColorNo));

            plot.Refresh();
            return plottables.ToArray();
        }

        public static void PlotMarkerSequence(FormsPlot plot, MarkerSequence markerSequence, Color color)
        {
            foreach (var vLine in markerSequence.GetVLines())
            {
                vLine.Color = color;
                plot.Plot.Add(vLine);
            }
            plot.Refresh();
        }

        public static void PlotWindowMarkers(FormsPlot plot, WindowMarkers markers, Color color)
        {
            markers.StartLine.Color = color;
            markers.EndLine.Color = color;
            plot.Plot.Add(markers.StartLine);
            plot.Plot.Add(markers.EndLine);
            plot.Refresh();
        }

        public static void RemoveMarkerSequence(FormsPlot plot, MarkerSequence markerSequence)
        {
            foreach (var vLine in markerSequence.GetVLines())
            {
                plot.Plot.Remove(vLine);
            }
        }

        public static void PerformActionOnPlots(FormsPlot[] plots, Action<FormsPlot> action)
        {
            foreach (FormsPlot plot in plots)
            {
                action(plot);
                plot.Refresh();
            }
        }

        //public class Markers
        //{
        //    ScottPlot.Plottable.VLine startLine, endLine;

        //    public Markers(ScottPlot.Plottable.VLine startLine, ScottPlot.Plottable.VLine endLine)
        //    {
        //        this.startLine = startLine;
        //        this.endLine = endLine;
        //        startLine.DragEnabled = true;
        //        endLine.DragEnabled = true;
        //    }
        //    public double Start { get => startLine.X; set => startLine.X = value; }
        //    public double End { get => endLine.X; set => endLine.X = value; }

        //    public void SetCallback(Action<double, double> callback)
        //    {
        //        startLine.Dragged += (s, e) =>
        //        {
        //            callback(startLine.X, endLine.X - startLine.X);
        //        };
        //        endLine.Dragged += (s, e) =>
        //        {
        //            callback(startLine.X, endLine.X - startLine.X);
        //        };
        //    }

        //    public void AddToPlot(FormsPlot plot)
        //    {
        //        plot.Plot.Add(startLine);
        //        plot.Plot.Add(endLine);
        //    }
        // }
    }
}
