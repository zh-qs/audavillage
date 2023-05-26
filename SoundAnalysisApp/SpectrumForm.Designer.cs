namespace SoundAnalysisApp
{
    partial class SpectrumForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpectrumForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            spectrumFormsPlot = new ScottPlot.FormsPlot();
            groupBox1 = new GroupBox();
            signalFormsPlot = new ScottPlot.FormsPlot();
            panel1 = new Panel();
            windowTypeComboBox = new ComboBox();
            label1 = new Label();
            refreshPlotsButton = new Button();
            logScaleCheckBox = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1116, 668);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(spectrumFormsPlot);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 314);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1110, 305);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Spectrum";
            // 
            // spectrumFormsPlot
            // 
            spectrumFormsPlot.Dock = DockStyle.Fill;
            spectrumFormsPlot.Location = new Point(3, 19);
            spectrumFormsPlot.Margin = new Padding(4, 3, 4, 3);
            spectrumFormsPlot.Name = "spectrumFormsPlot";
            spectrumFormsPlot.Size = new Size(1104, 283);
            spectrumFormsPlot.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(signalFormsPlot);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1110, 305);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Signal";
            // 
            // signalFormsPlot
            // 
            signalFormsPlot.Dock = DockStyle.Fill;
            signalFormsPlot.Location = new Point(3, 19);
            signalFormsPlot.Margin = new Padding(4, 3, 4, 3);
            signalFormsPlot.Name = "signalFormsPlot";
            signalFormsPlot.Size = new Size(1104, 283);
            signalFormsPlot.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(windowTypeComboBox);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(refreshPlotsButton);
            panel1.Controls.Add(logScaleCheckBox);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 625);
            panel1.Name = "panel1";
            panel1.Size = new Size(1110, 40);
            panel1.TabIndex = 3;
            // 
            // windowTypeComboBox
            // 
            windowTypeComboBox.FormattingEnabled = true;
            windowTypeComboBox.Items.AddRange(new object[] { "Rectangular", "Triangular", "Hamming", "van Hann", "Blackman", "Gaussian" });
            windowTypeComboBox.Location = new Point(252, 8);
            windowTypeComboBox.Name = "windowTypeComboBox";
            windowTypeComboBox.Size = new Size(188, 23);
            windowTypeComboBox.TabIndex = 17;
            windowTypeComboBox.Text = "Rectangular";
            windowTypeComboBox.SelectedIndexChanged += windowTypeComboBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(169, 11);
            label1.Name = "label1";
            label1.Size = new Size(77, 15);
            label1.TabIndex = 3;
            label1.Text = "Window type";
            // 
            // refreshPlotsButton
            // 
            refreshPlotsButton.Location = new Point(486, 7);
            refreshPlotsButton.Name = "refreshPlotsButton";
            refreshPlotsButton.Size = new Size(75, 23);
            refreshPlotsButton.TabIndex = 1;
            refreshPlotsButton.Text = "Refresh";
            refreshPlotsButton.UseVisualStyleBackColor = true;
            refreshPlotsButton.Click += refreshPlotsButton_Click;
            // 
            // logScaleCheckBox
            // 
            logScaleCheckBox.AutoSize = true;
            logScaleCheckBox.Location = new Point(9, 10);
            logScaleCheckBox.Name = "logScaleCheckBox";
            logScaleCheckBox.Size = new Size(119, 19);
            logScaleCheckBox.TabIndex = 0;
            logScaleCheckBox.Text = "Logarithmic scale";
            logScaleCheckBox.UseVisualStyleBackColor = true;
            logScaleCheckBox.CheckedChanged += logScaleCheckBox_CheckedChanged;
            // 
            // SpectrumForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1116, 668);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1132, 707);
            Name = "SpectrumForm";
            Text = "Spectrum";
            Load += SpectrumForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private ScottPlot.FormsPlot signalFormsPlot;
        private GroupBox groupBox2;
        private ScottPlot.FormsPlot spectrumFormsPlot;
        private Panel panel1;
        private CheckBox logScaleCheckBox;
        private Label label1;
        private Button refreshPlotsButton;
        private ComboBox windowTypeComboBox;
    }
}