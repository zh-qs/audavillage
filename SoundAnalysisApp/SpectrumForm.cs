using ScottPlot;
using ScottPlot.Plottable;
using SoundAnalysisLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundAnalysisApp
{
    public partial class SpectrumForm : Form
    {
        readonly FormsPlot[] formsPlots;
        WaveFile waveFile;
        Clip clip;

        SignalPlot signalPlot, spectrumPlot;
        Color defaultColor = Color.RoyalBlue;
        WindowMarkers markers;

        double[] fft;

        bool logScale;

        public double FrameDurationMs { get; set; }
        public double OverlapRatio { get; set; }

        public SpectrumForm()
        {
            InitializeComponent();

            formsPlots = new FormsPlot[] { signalFormsPlot, spectrumFormsPlot };
            Plotter.PerformActionOnPlots(formsPlots, p =>
            {
                p.Plot.Margins(0, 0);
            });

            logScale = logScaleCheckBox.Checked;
            refreshPlotsButton.Enabled = false;
        }

        public void UpdateAudio(WaveFile waveFile, Clip clip)
        {
            this.waveFile = waveFile;
            this.clip = new Clip(clip);
            markers = new WindowMarkers(waveFile, AfterLineDrag);
        }

        private void AfterLineDrag()
        {
            Plotter.PerformActionOnPlots(formsPlots, p => p.Refresh());
            refreshPlotsButton.Enabled = true;
        }

        void RefreshPlots()
        {
            Cursor = Cursors.WaitCursor;

            signalFormsPlot.Plot.Remove(signalPlot);
            spectrumFormsPlot.Plot.Remove(spectrumPlot);
            //signalPlot = Plotter.PlotFile(signalFormsPlot, waveFile, autoScale: true, defaultColor);
            signalPlot = Plotter.PlotArray(signalFormsPlot, clip.GetWindowedData(false).ToArray(), waveFile.WaveHeader.sampleRate, autoScale: true, defaultColor);
            signalFormsPlot.Plot.SetOuterViewLimits(0, waveFile.Duration);
            fft = clip.FFT();

            double[] data = !logScale ? fft : (fft.Select(d => 10.0 * Math.Log(d)).ToArray());
            spectrumPlot = Plotter.PlotArray(spectrumFormsPlot, data, 2.0f * data.Length / waveFile.WaveHeader.sampleRate, autoScale: true, defaultColor);
            spectrumFormsPlot.Plot.SetOuterViewLimits(0, waveFile.WaveHeader.sampleRate * 0.5f);

            Cursor = Cursors.Default;
        }

        private void SpectrumForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            signalFormsPlot.Plot.Clear();
            spectrumFormsPlot.Plot.Clear();
            //signalPlot = Plotter.PlotFile(signalFormsPlot, waveFile, autoScale: true, defaultColor);
            signalPlot = Plotter.PlotArray(signalFormsPlot, clip.GetWindowedData(false).ToArray(), waveFile.WaveHeader.sampleRate, autoScale: true, defaultColor);
            signalFormsPlot.Plot.SetOuterViewLimits(0, waveFile.Duration);
            fft = clip.FFT();

            double[] data = !logScale ? fft : (fft.Select(d => 10.0 * Math.Log(d)).ToArray());
            spectrumPlot = Plotter.PlotArray(spectrumFormsPlot, data, 2.0f * data.Length / waveFile.WaveHeader.sampleRate, autoScale: true, defaultColor);
            spectrumFormsPlot.Plot.SetOuterViewLimits(0, waveFile.WaveHeader.sampleRate * 0.5f);

            Plotter.PlotWindowMarkers(signalFormsPlot, markers, Color.DarkGreen);

            Cursor = Cursors.Default;
        }

        private void logScaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            logScale = logScaleCheckBox.Checked;
            spectrumFormsPlot.Plot.Remove(spectrumPlot);

            double[] data = !logScale ? fft : (fft.Select(d => 10.0 * Math.Log(d)).ToArray());
            spectrumPlot = Plotter.PlotArray(spectrumFormsPlot, data, 2.0f * data.Length / waveFile.WaveHeader.sampleRate, autoScale: true, defaultColor);
            spectrumFormsPlot.Plot.SetOuterViewLimits(0, waveFile.WaveHeader.sampleRate * 0.5f);
        }

        private void windowTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WindowFunction window;
            switch (windowTypeComboBox.SelectedIndex)
            {
                case 0: // Rectangular
                    window = new RectangularWindow();
                    break;
                case 1: // Triangular
                    window = new TriangularWindow();
                    break;
                case 2: // Hamming
                    window = new HammingWindow();
                    break;
                case 3: // Hanning
                    window = new HanningWindow();
                    break;
                case 4: // Blackman
                    window = new BlackmanWindow();
                    break;
                case 5: // Gaussian
                    window = new GaussianWindow();
                    break;
                default:
                    throw new ArgumentException($"Unexpected index: {windowTypeComboBox.SelectedIndex}");
            }
            if (refreshPlotsButton.Enabled)
                clip = markers.GetClip(clip);
            clip.SetWindow(window);
            refreshPlotsButton.Enabled = false;
            RefreshPlots();
        }

        private void refreshPlotsButton_Click(object sender, EventArgs e)
        {
            refreshPlotsButton.Enabled = false;
            clip = markers.GetClip(clip);
            RefreshPlots();
        }
    }
}
