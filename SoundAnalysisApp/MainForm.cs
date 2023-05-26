using SoundAnalysisLib;
using ScottPlot;
using System.Net.WebSockets;

namespace SoundAnalysisApp
{
    public partial class MainForm : Form
    {
        const int fftSize = 1 << 14;
        const string TITLE = "Audavillage";

        readonly FormsPlot[] formsPlots;
        WaveFile waveFile;
        Clip clip;
        MarkerSequence markerSequence;
        double overlapRatio = 0;
        double frameDurationMs = 20;

        Func<Frame, double> parameterPlotMap = f => f.GetVolume();
        bool drawSpectrogram = false;

        Predicate<Frame> fragmentPredicate = f => f.IsSilent();
        Predicate<Clip> clipPredicate = f => f.IsVoiceLSTERBased();
        bool plotClips = false;
        Func<Clip, double> clipParameterPlotMap = f => 0;

        ScottPlot.Plottable.IPlottable parameterSignalPlottable;
        ScottPlot.Plottable.IPlottable[] signalFragmentsPlottables;
        ScottPlot.Plottable.IPlottable clipParameterScatterPlottable;
        ScottPlot.Plottable.SignalPlot signalPlot;

        Color defaultColor = Color.RoyalBlue;

        readonly SpectrumForm spectrumForm;

        public MainForm()
        {
            InitializeComponent();

            overlapRatio = (double)overlapNumericUpDown.Value;
            frameDurationMs = (double)frameDurationNumericUpDown.Value;

            formsPlots = new FormsPlot[] { signalFormsPlot, parameterFormsPlot, clipParametersFormsPlot };
            foreach (FormsPlot plot in formsPlots)
            {
                plot.AxesChanged += OnAxesChanged;
            }
            Plotter.PerformActionOnPlots(formsPlots, p =>
            {
                p.Plot.Margins(0, 0);
            });
            tableLayoutPanel1.Visible = false;
            spectrumForm = new SpectrumForm();
        }

        private void OnAxesChanged(object sender, EventArgs e)
        {
            FormsPlot changedPlot = (FormsPlot)sender;
            var newAxisLimits = changedPlot.Plot.GetAxisLimits();

            foreach (var fp in formsPlots)
            {
                if (fp == changedPlot)
                    continue;

                // disable events briefly to avoid an infinite loop
                fp.Configuration.AxesChangedEventEnabled = false;
                fp.Plot.SetAxisLimitsX(newAxisLimits.XMin, newAxisLimits.XMax);
                //fp.Plot.SetAxisLimits(newAxisLimits);
                fp.Render();
                fp.Configuration.AxesChangedEventEnabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool caught = false;
                BinaryReader reader = null;
                try
                {
                    reader = new BinaryReader(new FileStream(openFileDialog1.FileName, FileMode.Open));
                    waveFile = WaveFile.FromReader(reader);
                }
                catch (InvalidWaveFormatException ex)
                {
                    MessageBox.Show(ex.Message, "Error loading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    caught = true;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                if (caught) return;

                if (waveFile.Duration == 0)
                {
                    MessageBox.Show("The WAV file contains no data", "Error loading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                tableLayoutPanel1.Visible = true;
                Text = TITLE + ": " + openFileDialog1.FileName;
                Plotter.PerformActionOnPlots(formsPlots, p =>
                {
                    p.Plot.Clear();
                    p.Plot.SetOuterViewLimits(0, waveFile.Duration);
                });

                clip = new Clip(waveFile, frameDurationMs, overlapRatio);
                markerSequence = MarkerSequence.DivideWaveFileEquallyByTime(waveFile, 1, AfterMarkerDrag, frameDurationMs, overlapRatio, showClipMarkersCheckbox.Checked);
                signalPlot = Plotter.PlotFile(signalFormsPlot, waveFile, autoScale: true, defaultColor);
                //signalFormsPlot.Plot.AddSignal(clip.FFT().ToArray());

                parameterSignalPlottable = Plotter.PlotClip(parameterFormsPlot, clip, parameterPlotMap, autoScale: true, defaultColor);
                signalFragmentsPlottables = Plotter.PlotRanges(signalFormsPlot, clip.GetFragments(fragmentPredicate), Color.Gray);
                clipCountNumericUpDown.Value = markerSequence.Count; // event renders markers

                ShowWaveFileInfo(waveFile);
            }
        }

        private void AfterMarkerDrag()
        {
            Plotter.PerformActionOnPlots(formsPlots, p => p.Refresh());
            refreshClipParametersButton.Enabled = true;
        }

        private void ShowWaveFileInfo(WaveFile waveFile)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("WAV file properties");

            var node = treeView1.Nodes[0];

            node.Nodes.Add($"File size: {waveFile.WaveHeader.fileSize + 8} B");
            node.Nodes.Add($"Duration: {waveFile.Duration} s");
            node.Nodes.Add($"Audio format: {waveFile.WaveHeader.audioFormat}");
            node.Nodes.Add($"Channels: {waveFile.WaveHeader.numberOfChannels} ({(waveFile.WaveHeader.numberOfChannels == 1 ? "mono" : "stereo")})");
            node.Nodes.Add($"Sample rate: {waveFile.WaveHeader.sampleRate} Hz");
            node.Nodes.Add($"Byterate: {waveFile.WaveHeader.byteRate} B/s");
            node.Nodes.Add($"Bits per sample: {waveFile.WaveHeader.bitsPerSample}");
            node.Nodes.Add($"Total samples: {waveFile.SampleCount}");

            treeView1.EndUpdate();
        }

        private void DrawParameterPlot()
        {
            Cursor = Cursors.WaitCursor;

            parameterFormsPlot.Plot.Remove(parameterSignalPlottable);
            parameterFormsPlot.Plot.SetOuterViewLimits(0, waveFile.Duration);
            if (drawSpectrogram)
            {
                parameterSignalPlottable = Plotter.PlotClipSpectrogram(parameterFormsPlot, clip, autoScale: true, ScottPlot.Drawing.Colormap.Turbo);
            }
            else
            {
                parameterSignalPlottable = Plotter.PlotClip(parameterFormsPlot, clip, parameterPlotMap, autoScale: true, defaultColor, cutSelectedFragmentsCheckBox.Checked ? fragmentPredicate : null);
            }
            Cursor = Cursors.Default;
        }

        private void overlapNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            markerSequence.ClipOverlapRatio = overlapRatio = (double)overlapNumericUpDown.Value;
            clip = new Clip(waveFile, frameDurationMs, overlapRatio);
            DrawParameterPlot();
        }

        private void parameterSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawSpectrogram = false;
            switch (parameterSelectionComboBox.SelectedIndex)
            {
                case 0: // Volume
                    parameterPlotMap = f => f.GetVolume();
                    break;
                case 1: // STE
                    parameterPlotMap = f => f.GetShortTimeEnergy();
                    break;
                case 2: // ZCR
                    parameterPlotMap = f => f.GetZeroCrossingRate();
                    break;
                case 3: // F0 (autocorrelation)
                    parameterPlotMap = f => f.GetFundamentalFrequencyFromAutocorrelation();
                    break;
                case 4: // F0 (AMDF)
                    parameterPlotMap = f => f.GetFundamentalFrequencyFromAMDF();
                    break;
                case 5: // F0 (Cepstrum)
                    parameterPlotMap = f => f.GetFundamentalFrequencyFromCepstrum(0, fftSize);
                    break;
                case 6: // Volume (Frequency-domain)
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetVolume();
                    break;
                case 7: // FC
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetFrequencyCentroid();
                    break;
                case 8: // BW
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetEffectiveBandtwidth();
                    break;
                case 9: // BE
                    {
                        (double min, double max) = ChooseFreqBandDialog.PromptMinMax(0, waveFile.WaveHeader.sampleRate * 0.5);
                        parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetBandEnergy(min, max);
                        break;
                    }
                case 10: // BER
                    {
                        (double min, double max) = ChooseFreqBandDialog.PromptMinMax(0, waveFile.WaveHeader.sampleRate * 0.5);
                        parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetBandEnergyRatio(min, max);
                        break;
                    }
                case 11: // ERSB1
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetEnergyRatioSubband(0, 630);
                    break;
                case 12: // ERSB2
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetEnergyRatioSubband(630, 1720);
                    break;
                case 13: // ERSB3
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetEnergyRatioSubband(1720, 4400);
                    break;
                case 14: // ERSB4
                    parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetEnergyRatioSubband(4400, waveFile.WaveHeader.sampleRate * 0.5);
                    break;
                case 15: // SFM
                    {
                        (double min, double max) = ChooseFreqBandDialog.PromptMinMax(0, waveFile.WaveHeader.sampleRate * 0.5);
                        parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetSpectralFlatnessMeasure(min, max);
                        break;
                    }
                case 16: // SCF
                    {
                        (double min, double max) = ChooseFreqBandDialog.PromptMinMax(0, waveFile.WaveHeader.sampleRate * 0.5);
                        parameterPlotMap = f => f.GetFFTFrame(0, fftSize).GetSpectralCrestFactor(min, max);
                        break;
                    }
                case 17: // Spectrogram
                    drawSpectrogram = true;
                    break;
            }
            DrawParameterPlot();
        }

        private void frameDurationNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            markerSequence.ClipSampleDurationMs = frameDurationMs = (double)frameDurationNumericUpDown.Value;
            clip = new Clip(waveFile, frameDurationMs, overlapRatio);
            DrawParameterPlot();
        }

        private void fragmentSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var p in signalFragmentsPlottables)
                signalFormsPlot.Plot.Remove(p);

            switch (fragmentSelectionComboBox.SelectedIndex)
            {
                case 0:
                    plotClips = false;
                    fragmentPredicate = f => f.IsSilent();
                    break;
                case 1:
                    plotClips = false;
                    fragmentPredicate = f => f.IsVoicedSpeech();
                    break;
                case 2:
                    plotClips = false;
                    fragmentPredicate = f => f.IsUnvoicedSpeech();
                    break;
                case 3:
                    plotClips = false;
                    fragmentPredicate = f => f.IsUnvoicedSpeech() || f.IsSilent();
                    break;
                case 4:
                    plotClips = true;
                    clipPredicate = c => c.IsVoiceLSTERBased();
                    break;
                case 5:
                    plotClips = true;
                    clipPredicate = c => c.IsVoiceZSTDBased();
                    break;
                case 6:
                    plotClips = true;
                    clipPredicate = c => c.IsVoiceVDRBased();
                    break;
                case 7:
                    plotClips = true;
                    clipPredicate = c => c.IsMaleVoiceMedian();
                    break;
                case 8:
                    plotClips = true;
                    clipPredicate = c => c.IsMaleVoiceAverage();
                    break;
                case 9:
                    plotClips = true;
                    clipPredicate = c => c.IsMaleVoiceCepstrumMedian();
                    break;
            }

            Cursor = Cursors.WaitCursor;

            if (plotClips)
            {
                signalFragmentsPlottables = Plotter.PlotRangesYesNo(signalFormsPlot, markerSequence.GetFragments(clipPredicate), waveFile.Duration, Color.Blue, Color.Red);
                cutSelectedFragmentsCheckBox.Checked = false;
                cutSelectedFragmentsCheckBox.Enabled = false;
            }
            else
            {
                signalFragmentsPlottables = Plotter.PlotRanges(signalFormsPlot, clip.GetFragments(fragmentPredicate), Color.Gray);
                cutSelectedFragmentsCheckBox.Enabled = true;
            }

            Cursor = Cursors.Default;

            if (cutSelectedFragmentsCheckBox.Checked)
                DrawParameterPlot();
        }

        private void clipParameterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (clipParameterComboBox.SelectedIndex)
            {
                case 0: // Silence ratio
                    clipParameterPlotMap = c => c.GetSilenceRatio();
                    break;
                case 1: // VSTD
                    clipParameterPlotMap = c => c.GetNormalizedStandardDeviation();
                    break;
                case 2: // Volume Dynamic Range
                    clipParameterPlotMap = c => c.GetVolumeDynamicRange();
                    break;
                case 3: // Volume Undulation(TODO)
                    clipParameterPlotMap = c => c.GetVolumeUndulation();
                    break;
                case 4: // LSTER
                    clipParameterPlotMap = c => c.GetLowShortTimeEnergyRatio();
                    break;
                case 5: // Energy entropy
                    clipParameterPlotMap = c => c.GetEnergyEntropy(100); // TODO variable segment size
                    break;
                case 6: // ZCR standard deviation
                    clipParameterPlotMap = c => c.GetZCRStandardDeviation();
                    break;
                case 7: // HZCRR
                    clipParameterPlotMap = c => c.GetHighZeroCrossingRateRatio();
                    break;
                case 8: // F0 median
                    clipParameterPlotMap = c => c.GetFilteredFundamentalFrequencyAutocorrelation().Median();
                    break;
                case 9: // F0 avg
                    clipParameterPlotMap = c => c.GetFilteredFundamentalFrequencyAutocorrelation().Average();
                    break;
                case 10: // F0 std
                    clipParameterPlotMap = c => c.GetFilteredFundamentalFrequencyAutocorrelation().StandardDeviation();
                    break;
            }

            Cursor = Cursors.WaitCursor;

            clipParametersFormsPlot.Plot.Remove(clipParameterScatterPlottable);
            clipParameterScatterPlottable = Plotter.PlotClips(clipParametersFormsPlot, markerSequence.GetClips(), clipParameterPlotMap, autoScale: true, defaultColor);
            refreshClipParametersButton.Enabled = false;

            Cursor = Cursors.Default;
        }

        private void clipCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var limits = signalFormsPlot.Plot.GetAxisLimits();
            Plotter.PerformActionOnPlots(formsPlots, p => Plotter.RemoveMarkerSequence(p, markerSequence));
            markerSequence = MarkerSequence.DivideWaveFileEqually(waveFile, (int)clipCountNumericUpDown.Value, AfterMarkerDrag, frameDurationMs, overlapRatio);
            clipParametersFormsPlot.Plot.Remove(clipParameterScatterPlottable);
            clipParameterScatterPlottable = Plotter.PlotClips(clipParametersFormsPlot, markerSequence.GetClips(), clipParameterPlotMap, autoScale: true, defaultColor);
            Plotter.PerformActionOnPlots(formsPlots, p =>
            {
                Plotter.PlotMarkerSequence(p, markerSequence, Color.Red);
                p.Plot.SetAxisLimitsX(limits.XMin, limits.XMax);
            });
            clipParametersFormsPlot.Plot.SetOuterViewLimits(0, waveFile.Duration);
        }

        private void refreshClipParametersButton_Click(object sender, EventArgs e)
        {
            clipParametersFormsPlot.Plot.Remove(clipParameterScatterPlottable);
            clipParameterScatterPlottable = Plotter.PlotClips(clipParametersFormsPlot, markerSequence.GetClips(), clipParameterPlotMap, autoScale: true, defaultColor);
            refreshClipParametersButton.Enabled = false;
        }



        private void exportAllFrameParametersButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;

            clip.ExportAllFrameParametersCSV(saveFileDialog1.FileName, fftSize: fftSize);

            Cursor = Cursors.Default;
        }

        private void exportCurrentFrameParameterButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;

            clip.ExportCustomFrameParameterCSV(saveFileDialog1.FileName, ParameterExport.ParameterHeaders[parameterSelectionComboBox.SelectedIndex], parameterPlotMap);

            Cursor = Cursors.Default;
        }

        private void exportAllClipParametersButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;

            markerSequence.GetClips().ExportAllClipParametersCSV(saveFileDialog1.FileName);

            Cursor = Cursors.Default;
        }

        private void exportCurrentClipParameterButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;

            markerSequence.GetClips().ExportCustomClipParameterCSV(saveFileDialog1.FileName, ParameterExport.ClipParameterHeaders[clipParameterComboBox.SelectedIndex], clipParameterPlotMap);

            Cursor = Cursors.Default;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Audavillage - program do analizy dŸwiêku, powsta³y jako projekt na AiPD.\n(C) Szymon Tur 2023", "About");
        }

        private void showClipMarkersCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var l in markerSequence.GetVLines())
                l.IsVisible = showClipMarkersCheckbox.Checked;
            Plotter.PerformActionOnPlots(formsPlots, p => p.Refresh());
        }

        private void cutSelectedFragmentsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DrawParameterPlot();
        }

        private void saveMarkersButton_Click(object sender, EventArgs e)
        {
            if (markerSaveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(markerSaveDialog.FileName, markerSequence.ToString());
            }
        }

        private void openMarkersButton_Click(object sender, EventArgs e)
        {
            if (markerOpenDialog.ShowDialog() == DialogResult.OK)
            {
                string res = File.ReadAllText(markerOpenDialog.FileName);
                MarkerSequence newSequence;
                try
                {
                    newSequence = MarkerSequence.FromString(waveFile, res, AfterMarkerDrag, frameDurationMs, overlapRatio, showClipMarkersCheckbox.Checked);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Invalid clip border file: {ex.Message}", "Error loading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var limits = signalFormsPlot.Plot.GetAxisLimits();
                Plotter.PerformActionOnPlots(formsPlots, p => Plotter.RemoveMarkerSequence(p, markerSequence));
                markerSequence = newSequence;
                clipParametersFormsPlot.Plot.Remove(clipParameterScatterPlottable);
                clipParameterScatterPlottable = Plotter.PlotClips(clipParametersFormsPlot, markerSequence.GetClips(), clipParameterPlotMap, autoScale: true, defaultColor);
                Plotter.PerformActionOnPlots(formsPlots, p =>
                {
                    Plotter.PlotMarkerSequence(p, markerSequence, Color.Red);
                    p.Plot.SetAxisLimitsX(limits.XMin, limits.XMax);
                });
                clipParametersFormsPlot.Plot.SetOuterViewLimits(0, waveFile.Duration);
            }
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
            clip.SetWindow(window);
            DrawParameterPlot();
            //signalFormsPlot.Plot.Remove(signalPlot);
            //signalPlot = Plotter.PlotWindowedFile(signalFormsPlot, waveFile, clip, true, defaultColor);
            //signalFormsPlot.Refresh();
        }

        private void showFFTButton_Click(object sender, EventArgs e)
        {
            spectrumForm.UpdateAudio(waveFile, clip);
            spectrumForm.ShowDialog();
        }
    }
}