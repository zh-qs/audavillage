using ScottPlot.Plottable;
using SoundAnalysisLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisApp
{
    internal class WindowMarkers
    {
        WaveFile waveFile;
        VLine startMarker, endMarker;
        Action refreshPlotsAction;

        public VLine StartLine => startMarker;
        public VLine EndLine => endMarker;

        public WindowMarkers(WaveFile waveFile, Action refreshPlotsAction)
        {
            this.waveFile = waveFile;
            this.refreshPlotsAction = refreshPlotsAction;

            startMarker = new VLine();
            startMarker.X = 0;
            startMarker.IsVisible = true;
            startMarker.DragEnabled = true;
            startMarker.Dragged += Line_Dragged;

            endMarker = new VLine();
            endMarker.X = waveFile.Duration;
            endMarker.IsVisible = true;
            endMarker.DragEnabled = true;
            endMarker.Dragged += Line_Dragged;
        }

        private void Line_Dragged(object? sender, EventArgs e)
        {
            VLine line = (VLine)sender;
            if (line == null) return;
            if (line.X < 0) line.X = 0;
            if (line.X > waveFile.Duration) line.X = waveFile.Duration;

            if (startMarker.X > endMarker.X)
            {
                var x = startMarker.X;
                startMarker.X = endMarker.X;
                endMarker.X = x;
            }

            refreshPlotsAction();
        }

        public Clip GetClip(Clip other)
        {
            return new Clip(other, startMarker.X, endMarker.X - startMarker.X);
        }
    }
}
