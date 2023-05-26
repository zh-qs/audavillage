namespace SoundAnalysisApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            signalFormsPlot = new ScottPlot.FormsPlot();
            groupBox2 = new GroupBox();
            cutSelectedFragmentsCheckBox = new CheckBox();
            parameterFormsPlot = new ScottPlot.FormsPlot();
            groupBox3 = new GroupBox();
            treeView1 = new TreeView();
            groupBox4 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label8 = new Label();
            parameterSelectionComboBox = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            overlapNumericUpDown = new NumericUpDown();
            label3 = new Label();
            frameDurationNumericUpDown = new NumericUpDown();
            label4 = new Label();
            fragmentSelectionComboBox = new ComboBox();
            clipParameterComboBox = new ComboBox();
            label9 = new Label();
            clipCountNumericUpDown = new NumericUpDown();
            showClipMarkersCheckbox = new CheckBox();
            label11 = new Label();
            windowTypeComboBox = new ComboBox();
            showFFTButton = new Button();
            groupBox5 = new GroupBox();
            openMarkersButton = new Button();
            saveMarkersButton = new Button();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            exportCurrentClipParameterButton = new Button();
            exportAllClipParametersButton = new Button();
            exportCurrentFrameParameterButton = new Button();
            exportAllFrameParametersButton = new Button();
            groupBox6 = new GroupBox();
            refreshClipParametersButton = new Button();
            clipParametersFormsPlot = new ScottPlot.FormsPlot();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            contextMenuStrip1 = new ContextMenuStrip(components);
            saveFileDialog1 = new SaveFileDialog();
            markerSaveDialog = new SaveFileDialog();
            markerOpenDialog = new OpenFileDialog();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)overlapNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)frameDurationNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)clipCountNumericUpDown).BeginInit();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox3, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox4, 1, 1);
            tableLayoutPanel1.Controls.Add(groupBox5, 1, 2);
            tableLayoutPanel1.Controls.Add(groupBox6, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.Size = new Size(1116, 844);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(signalFormsPlot);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(775, 275);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Signal";
            // 
            // signalFormsPlot
            // 
            signalFormsPlot.Dock = DockStyle.Fill;
            signalFormsPlot.Location = new Point(3, 19);
            signalFormsPlot.Margin = new Padding(4, 3, 4, 3);
            signalFormsPlot.Name = "signalFormsPlot";
            signalFormsPlot.Size = new Size(769, 253);
            signalFormsPlot.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cutSelectedFragmentsCheckBox);
            groupBox2.Controls.Add(parameterFormsPlot);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 284);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(775, 275);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Frame parameters";
            // 
            // cutSelectedFragmentsCheckBox
            // 
            cutSelectedFragmentsCheckBox.AutoSize = true;
            cutSelectedFragmentsCheckBox.Location = new Point(113, 0);
            cutSelectedFragmentsCheckBox.Name = "cutSelectedFragmentsCheckBox";
            cutSelectedFragmentsCheckBox.Size = new Size(148, 19);
            cutSelectedFragmentsCheckBox.TabIndex = 1;
            cutSelectedFragmentsCheckBox.Text = "Cut selected fragments";
            cutSelectedFragmentsCheckBox.UseVisualStyleBackColor = true;
            cutSelectedFragmentsCheckBox.CheckedChanged += cutSelectedFragmentsCheckBox_CheckedChanged;
            // 
            // parameterFormsPlot
            // 
            parameterFormsPlot.Dock = DockStyle.Fill;
            parameterFormsPlot.Location = new Point(3, 19);
            parameterFormsPlot.Margin = new Padding(4, 3, 4, 3);
            parameterFormsPlot.Name = "parameterFormsPlot";
            parameterFormsPlot.Size = new Size(769, 253);
            parameterFormsPlot.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(treeView1);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(784, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(329, 275);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Properties";
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(3, 19);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(323, 253);
            treeView1.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel2);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(784, 284);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(329, 275);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Settings";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel2.Controls.Add(label8, 0, 2);
            tableLayoutPanel2.Controls.Add(parameterSelectionComboBox, 1, 1);
            tableLayoutPanel2.Controls.Add(label1, 0, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 5);
            tableLayoutPanel2.Controls.Add(overlapNumericUpDown, 1, 6);
            tableLayoutPanel2.Controls.Add(label3, 0, 6);
            tableLayoutPanel2.Controls.Add(frameDurationNumericUpDown, 1, 5);
            tableLayoutPanel2.Controls.Add(label4, 0, 0);
            tableLayoutPanel2.Controls.Add(fragmentSelectionComboBox, 1, 0);
            tableLayoutPanel2.Controls.Add(clipParameterComboBox, 1, 2);
            tableLayoutPanel2.Controls.Add(label9, 0, 3);
            tableLayoutPanel2.Controls.Add(clipCountNumericUpDown, 1, 3);
            tableLayoutPanel2.Controls.Add(showClipMarkersCheckbox, 1, 9);
            tableLayoutPanel2.Controls.Add(label11, 0, 8);
            tableLayoutPanel2.Controls.Add(windowTypeComboBox, 1, 8);
            tableLayoutPanel2.Controls.Add(showFFTButton, 0, 9);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 10;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(323, 253);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(3, 58);
            label8.Name = "label8";
            label8.Size = new Size(123, 29);
            label8.TabIndex = 0;
            label8.Text = "Clip parameter";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // parameterSelectionComboBox
            // 
            parameterSelectionComboBox.Dock = DockStyle.Top;
            parameterSelectionComboBox.FormattingEnabled = true;
            parameterSelectionComboBox.Items.AddRange(new object[] { "Volume", "Short Time Energy", "Zero Crossing Rate", "Fundamental Frequency (Autocorrelation)", "Fundamental Frequency (AMDF)", "Fundamental Frequency (Cepstrum)", "Volume (Frequency-domain)", "Frequency Centroid", "Effective Bandwidth", "Band Energy", "Band Energy Ratio", "ERSB1 (0-630 Hz)", "ERSB2 (630-1720 Hz)", "ERSB3 (1720-4400 Hz)", "ERSB4 (4400 Hz-SampleRate/2)", "Spectral Flatness Measure", "Spectral Crest Factor", "Spectrogram" });
            parameterSelectionComboBox.Location = new Point(132, 32);
            parameterSelectionComboBox.Name = "parameterSelectionComboBox";
            parameterSelectionComboBox.Size = new Size(188, 23);
            parameterSelectionComboBox.TabIndex = 2;
            parameterSelectionComboBox.Text = "Volume";
            parameterSelectionComboBox.SelectedIndexChanged += parameterSelectionComboBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 29);
            label1.Name = "label1";
            label1.Size = new Size(123, 29);
            label1.TabIndex = 3;
            label1.Text = "Frame parameter";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 116);
            label2.Name = "label2";
            label2.Size = new Size(123, 29);
            label2.TabIndex = 4;
            label2.Text = "Frame duration (ms)";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // overlapNumericUpDown
            // 
            overlapNumericUpDown.DecimalPlaces = 2;
            overlapNumericUpDown.Dock = DockStyle.Top;
            overlapNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            overlapNumericUpDown.Location = new Point(132, 148);
            overlapNumericUpDown.Maximum = new decimal(new int[] { 95, 0, 0, 131072 });
            overlapNumericUpDown.Name = "overlapNumericUpDown";
            overlapNumericUpDown.Size = new Size(188, 23);
            overlapNumericUpDown.TabIndex = 6;
            overlapNumericUpDown.ValueChanged += overlapNumericUpDown_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 145);
            label3.Name = "label3";
            label3.Size = new Size(123, 29);
            label3.TabIndex = 7;
            label3.Text = "Overlap ratio";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frameDurationNumericUpDown
            // 
            frameDurationNumericUpDown.Dock = DockStyle.Fill;
            frameDurationNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            frameDurationNumericUpDown.Location = new Point(132, 119);
            frameDurationNumericUpDown.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            frameDurationNumericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            frameDurationNumericUpDown.Name = "frameDurationNumericUpDown";
            frameDurationNumericUpDown.Size = new Size(188, 23);
            frameDurationNumericUpDown.TabIndex = 8;
            frameDurationNumericUpDown.Value = new decimal(new int[] { 20, 0, 0, 0 });
            frameDurationNumericUpDown.ValueChanged += frameDurationNumericUpDown_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(123, 29);
            label4.TabIndex = 9;
            label4.Text = "Select fragments";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // fragmentSelectionComboBox
            // 
            fragmentSelectionComboBox.Dock = DockStyle.Fill;
            fragmentSelectionComboBox.FormattingEnabled = true;
            fragmentSelectionComboBox.Items.AddRange(new object[] { "Silence", "Voiced speech", "Unvoiced speech", "Unvoiced speech + silence", "Voice (blue)/music (red) (LSTER)", "Voice (blue)/music (red) (ZSTD)", "Voice (blue)/music (red) (VDR+SR)", "Male (blue)/female (red) (Median)", "Male (blue)/female (red) (Average)", "Male (blue)/female (red) (Cepstrum, Med)" });
            fragmentSelectionComboBox.Location = new Point(132, 3);
            fragmentSelectionComboBox.Name = "fragmentSelectionComboBox";
            fragmentSelectionComboBox.Size = new Size(188, 23);
            fragmentSelectionComboBox.TabIndex = 10;
            fragmentSelectionComboBox.Text = "Silence";
            fragmentSelectionComboBox.SelectedIndexChanged += fragmentSelectionComboBox_SelectedIndexChanged;
            // 
            // clipParameterComboBox
            // 
            clipParameterComboBox.Dock = DockStyle.Fill;
            clipParameterComboBox.FormattingEnabled = true;
            clipParameterComboBox.Items.AddRange(new object[] { "Silence Ratio", "VSTD", "Volume Dynamic Range", "Volume Undulation", "LSTER", "Energy entropy", "ZCR standard deviation", "HZCRR", "F0 median (Autocorrelation, filtered)", "F0 average (Autocorrelation, filtered)", "F0 STD (Autocorrelation, filtered)" });
            clipParameterComboBox.Location = new Point(132, 61);
            clipParameterComboBox.Name = "clipParameterComboBox";
            clipParameterComboBox.Size = new Size(188, 23);
            clipParameterComboBox.TabIndex = 11;
            clipParameterComboBox.Text = "Select...";
            clipParameterComboBox.SelectedIndexChanged += clipParameterComboBox_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(3, 87);
            label9.Name = "label9";
            label9.Size = new Size(123, 29);
            label9.TabIndex = 12;
            label9.Text = "Clip count";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // clipCountNumericUpDown
            // 
            clipCountNumericUpDown.Dock = DockStyle.Fill;
            clipCountNumericUpDown.Location = new Point(132, 90);
            clipCountNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            clipCountNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            clipCountNumericUpDown.Name = "clipCountNumericUpDown";
            clipCountNumericUpDown.Size = new Size(188, 23);
            clipCountNumericUpDown.TabIndex = 13;
            clipCountNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            clipCountNumericUpDown.ValueChanged += clipCountNumericUpDown_ValueChanged;
            // 
            // showClipMarkersCheckbox
            // 
            showClipMarkersCheckbox.AutoSize = true;
            showClipMarkersCheckbox.Checked = true;
            showClipMarkersCheckbox.CheckState = CheckState.Checked;
            showClipMarkersCheckbox.Location = new Point(132, 206);
            showClipMarkersCheckbox.Name = "showClipMarkersCheckbox";
            showClipMarkersCheckbox.Size = new Size(122, 19);
            showClipMarkersCheckbox.TabIndex = 6;
            showClipMarkersCheckbox.Text = "Show clip markers";
            showClipMarkersCheckbox.UseVisualStyleBackColor = true;
            showClipMarkersCheckbox.CheckedChanged += showClipMarkersCheckbox_CheckedChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(3, 174);
            label11.Name = "label11";
            label11.Size = new Size(123, 29);
            label11.TabIndex = 15;
            label11.Text = "Window type (frame)";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // windowTypeComboBox
            // 
            windowTypeComboBox.Dock = DockStyle.Fill;
            windowTypeComboBox.FormattingEnabled = true;
            windowTypeComboBox.Items.AddRange(new object[] { "Rectangular", "Triangular", "Hamming", "van Hann", "Blackman", "Gaussian" });
            windowTypeComboBox.Location = new Point(132, 177);
            windowTypeComboBox.Name = "windowTypeComboBox";
            windowTypeComboBox.Size = new Size(188, 23);
            windowTypeComboBox.TabIndex = 16;
            windowTypeComboBox.Text = "Rectangular";
            windowTypeComboBox.SelectedIndexChanged += windowTypeComboBox_SelectedIndexChanged;
            // 
            // showFFTButton
            // 
            showFFTButton.Location = new Point(3, 206);
            showFFTButton.Name = "showFFTButton";
            showFFTButton.Size = new Size(102, 23);
            showFFTButton.TabIndex = 17;
            showFFTButton.Text = "View Spectrum";
            showFFTButton.UseVisualStyleBackColor = true;
            showFFTButton.Click += showFFTButton_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(openMarkersButton);
            groupBox5.Controls.Add(saveMarkersButton);
            groupBox5.Controls.Add(label7);
            groupBox5.Controls.Add(label6);
            groupBox5.Controls.Add(label5);
            groupBox5.Controls.Add(exportCurrentClipParameterButton);
            groupBox5.Controls.Add(exportAllClipParametersButton);
            groupBox5.Controls.Add(exportCurrentFrameParameterButton);
            groupBox5.Controls.Add(exportAllFrameParametersButton);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(784, 565);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(329, 276);
            groupBox5.TabIndex = 5;
            groupBox5.TabStop = false;
            groupBox5.Text = "Export";
            // 
            // openMarkersButton
            // 
            openMarkersButton.Location = new Point(228, 82);
            openMarkersButton.Name = "openMarkersButton";
            openMarkersButton.Size = new Size(75, 23);
            openMarkersButton.TabIndex = 8;
            openMarkersButton.Text = "Open";
            openMarkersButton.UseVisualStyleBackColor = true;
            openMarkersButton.Click += openMarkersButton_Click;
            // 
            // saveMarkersButton
            // 
            saveMarkersButton.Location = new Point(123, 82);
            saveMarkersButton.Name = "saveMarkersButton";
            saveMarkersButton.Size = new Size(99, 23);
            saveMarkersButton.TabIndex = 7;
            saveMarkersButton.Text = "Save";
            saveMarkersButton.UseVisualStyleBackColor = true;
            saveMarkersButton.Click += saveMarkersButton_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 86);
            label7.Name = "label7";
            label7.Size = new Size(111, 15);
            label7.TabIndex = 6;
            label7.Text = "Clip border markers";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 57);
            label6.Name = "label6";
            label6.Size = new Size(90, 15);
            label6.TabIndex = 5;
            label6.Text = "Clip parameters";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 28);
            label5.Name = "label5";
            label5.Size = new Size(102, 15);
            label5.TabIndex = 4;
            label5.Text = "Frame parameters";
            // 
            // exportCurrentClipParameterButton
            // 
            exportCurrentClipParameterButton.Location = new Point(123, 53);
            exportCurrentClipParameterButton.Name = "exportCurrentClipParameterButton";
            exportCurrentClipParameterButton.Size = new Size(99, 23);
            exportCurrentClipParameterButton.TabIndex = 3;
            exportCurrentClipParameterButton.Text = "Export current";
            exportCurrentClipParameterButton.UseVisualStyleBackColor = true;
            exportCurrentClipParameterButton.Click += exportCurrentClipParameterButton_Click;
            // 
            // exportAllClipParametersButton
            // 
            exportAllClipParametersButton.Location = new Point(228, 53);
            exportAllClipParametersButton.Name = "exportAllClipParametersButton";
            exportAllClipParametersButton.Size = new Size(75, 23);
            exportAllClipParametersButton.TabIndex = 2;
            exportAllClipParametersButton.Text = "Export all";
            exportAllClipParametersButton.UseVisualStyleBackColor = true;
            exportAllClipParametersButton.Click += exportAllClipParametersButton_Click;
            // 
            // exportCurrentFrameParameterButton
            // 
            exportCurrentFrameParameterButton.Location = new Point(123, 24);
            exportCurrentFrameParameterButton.Name = "exportCurrentFrameParameterButton";
            exportCurrentFrameParameterButton.Size = new Size(99, 23);
            exportCurrentFrameParameterButton.TabIndex = 1;
            exportCurrentFrameParameterButton.Text = "Export current";
            exportCurrentFrameParameterButton.UseVisualStyleBackColor = true;
            exportCurrentFrameParameterButton.Click += exportCurrentFrameParameterButton_Click;
            // 
            // exportAllFrameParametersButton
            // 
            exportAllFrameParametersButton.Location = new Point(228, 24);
            exportAllFrameParametersButton.Name = "exportAllFrameParametersButton";
            exportAllFrameParametersButton.Size = new Size(75, 23);
            exportAllFrameParametersButton.TabIndex = 0;
            exportAllFrameParametersButton.Text = "Export all";
            exportAllFrameParametersButton.UseVisualStyleBackColor = true;
            exportAllFrameParametersButton.Click += exportAllFrameParametersButton_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(refreshClipParametersButton);
            groupBox6.Controls.Add(clipParametersFormsPlot);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(3, 565);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(775, 276);
            groupBox6.TabIndex = 6;
            groupBox6.TabStop = false;
            groupBox6.Text = "Clip parameters";
            // 
            // refreshClipParametersButton
            // 
            refreshClipParametersButton.Enabled = false;
            refreshClipParametersButton.Location = new Point(104, 0);
            refreshClipParametersButton.Name = "refreshClipParametersButton";
            refreshClipParametersButton.Size = new Size(73, 23);
            refreshClipParametersButton.TabIndex = 0;
            refreshClipParametersButton.Text = "Refresh";
            refreshClipParametersButton.UseVisualStyleBackColor = true;
            refreshClipParametersButton.Click += refreshClipParametersButton_Click;
            // 
            // clipParametersFormsPlot
            // 
            clipParametersFormsPlot.Dock = DockStyle.Fill;
            clipParametersFormsPlot.Location = new Point(3, 19);
            clipParametersFormsPlot.Margin = new Padding(4, 3, 4, 3);
            clipParametersFormsPlot.Name = "clipParametersFormsPlot";
            clipParametersFormsPlot.Size = new Size(769, 254);
            clipParametersFormsPlot.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1116, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(112, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(116, 22);
            aboutToolStripMenuItem.Text = "About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "Waveform Audio Format|*.wav";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "CSV File|*.csv";
            // 
            // markerSaveDialog
            // 
            markerSaveDialog.Filter = "Clip borders|*.clb";
            // 
            // markerOpenDialog
            // 
            markerOpenDialog.FileName = "openFileDialog2";
            markerOpenDialog.Filter = "Clip borders|*.clb";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1116, 868);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1132, 907);
            Name = "MainForm";
            Text = "Audavillage";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)overlapNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)frameDurationNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)clipCountNumericUpDown).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private ScottPlot.FormsPlot signalFormsPlot;
        private GroupBox groupBox2;
        private ScottPlot.FormsPlot parameterFormsPlot;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private ComboBox parameterSelectionComboBox;
        private GroupBox groupBox3;
        private TreeView treeView1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown frameDurationNumericUpDown;
        private NumericUpDown overlapNumericUpDown;
        private Label label4;
        private ComboBox fragmentSelectionComboBox;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private Label label8;
        private ComboBox clipParameterComboBox;
        private Label label9;
        private NumericUpDown clipCountNumericUpDown;
        private ScottPlot.FormsPlot clipParametersFormsPlot;
        private Button refreshClipParametersButton;
        private Label label6;
        private Label label5;
        private Button exportCurrentClipParameterButton;
        private Button exportAllClipParametersButton;
        private Button exportCurrentFrameParameterButton;
        private Button exportAllFrameParametersButton;
        private SaveFileDialog saveFileDialog1;
        private CheckBox showClipMarkersCheckbox;
        private CheckBox cutSelectedFragmentsCheckBox;
        private Button openMarkersButton;
        private Button saveMarkersButton;
        private Label label7;
        private SaveFileDialog markerSaveDialog;
        private OpenFileDialog markerOpenDialog;
        private Label label10;
        private Label label11;
        private ComboBox windowTypeComboBox;
        private Button showFFTButton;
    }
}