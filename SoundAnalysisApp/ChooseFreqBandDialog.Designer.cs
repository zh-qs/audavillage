namespace SoundAnalysisApp
{
    partial class ChooseFreqBandDialog
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
            label1 = new Label();
            minFreqUpDown = new NumericUpDown();
            maxFreqUpDown = new NumericUpDown();
            label2 = new Label();
            okButton = new Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)minFreqUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)maxFreqUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(114, 44);
            label1.Name = "label1";
            label1.Size = new Size(125, 15);
            label1.TabIndex = 0;
            label1.Text = "Insert frequency band:";
            // 
            // minFreqUpDown
            // 
            minFreqUpDown.Location = new Point(29, 80);
            minFreqUpDown.Name = "minFreqUpDown";
            minFreqUpDown.Size = new Size(120, 23);
            minFreqUpDown.TabIndex = 1;
            // 
            // maxFreqUpDown
            // 
            maxFreqUpDown.Location = new Point(173, 80);
            maxFreqUpDown.Name = "maxFreqUpDown";
            maxFreqUpDown.Size = new Size(120, 23);
            maxFreqUpDown.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(155, 82);
            label2.Name = "label2";
            label2.Size = new Size(12, 15);
            label2.TabIndex = 3;
            label2.Text = "-";
            // 
            // okButton
            // 
            okButton.Location = new Point(137, 121);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 4;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(299, 82);
            label3.Name = "label3";
            label3.Size = new Size(21, 15);
            label3.TabIndex = 5;
            label3.Text = "Hz";
            // 
            // ChooseFreqBandDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(347, 176);
            ControlBox = false;
            Controls.Add(label3);
            Controls.Add(okButton);
            Controls.Add(label2);
            Controls.Add(maxFreqUpDown);
            Controls.Add(minFreqUpDown);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ChooseFreqBandDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Frequency band";
            ((System.ComponentModel.ISupportInitialize)minFreqUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)maxFreqUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown minFreqUpDown;
        private NumericUpDown maxFreqUpDown;
        private Label label2;
        private Button okButton;
        private Label label3;
    }
}