namespace SpeakerRecognitionApp
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            loadDatabaseButton = new Button();
            addKeyButton = new Button();
            saveDatabaseButton = new Button();
            loadSpeakerButton = new Button();
            groupBox1 = new GroupBox();
            clearDbButton = new Button();
            addManyKeysButton = new Button();
            progressLabel = new Label();
            label1 = new Label();
            progressBar1 = new ProgressBar();
            textBox1 = new TextBox();
            nameLabel = new Label();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog2 = new OpenFileDialog();
            bulkCheckButton = new Button();
            saveFileDialog2 = new SaveFileDialog();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // loadDatabaseButton
            // 
            loadDatabaseButton.Location = new Point(6, 22);
            loadDatabaseButton.Name = "loadDatabaseButton";
            loadDatabaseButton.Size = new Size(75, 23);
            loadDatabaseButton.TabIndex = 0;
            loadDatabaseButton.Text = "Load";
            loadDatabaseButton.UseVisualStyleBackColor = true;
            loadDatabaseButton.Click += loadDatabaseButton_Click;
            // 
            // addKeyButton
            // 
            addKeyButton.Location = new Point(6, 80);
            addKeyButton.Name = "addKeyButton";
            addKeyButton.Size = new Size(156, 23);
            addKeyButton.TabIndex = 1;
            addKeyButton.Text = "Add key from single dir";
            addKeyButton.UseVisualStyleBackColor = true;
            addKeyButton.Click += addKeyButton_Click;
            // 
            // saveDatabaseButton
            // 
            saveDatabaseButton.Location = new Point(87, 22);
            saveDatabaseButton.Name = "saveDatabaseButton";
            saveDatabaseButton.Size = new Size(75, 23);
            saveDatabaseButton.TabIndex = 2;
            saveDatabaseButton.Text = "Save";
            saveDatabaseButton.UseVisualStyleBackColor = true;
            saveDatabaseButton.Click += saveDatabaseButton_Click;
            // 
            // loadSpeakerButton
            // 
            loadSpeakerButton.Location = new Point(321, 16);
            loadSpeakerButton.Name = "loadSpeakerButton";
            loadSpeakerButton.Size = new Size(106, 41);
            loadSpeakerButton.TabIndex = 3;
            loadSpeakerButton.Text = "Load audio and identify";
            loadSpeakerButton.UseVisualStyleBackColor = true;
            loadSpeakerButton.Click += loadSpeakerButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(clearDbButton);
            groupBox1.Controls.Add(addManyKeysButton);
            groupBox1.Controls.Add(progressLabel);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(progressBar1);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(addKeyButton);
            groupBox1.Controls.Add(loadDatabaseButton);
            groupBox1.Controls.Add(saveDatabaseButton);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(254, 176);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Database";
            // 
            // clearDbButton
            // 
            clearDbButton.Location = new Point(168, 22);
            clearDbButton.Name = "clearDbButton";
            clearDbButton.Size = new Size(80, 23);
            clearDbButton.TabIndex = 6;
            clearDbButton.Text = "Clear";
            clearDbButton.UseVisualStyleBackColor = true;
            clearDbButton.Click += clearDbButton_Click;
            // 
            // addManyKeysButton
            // 
            addManyKeysButton.Location = new Point(7, 51);
            addManyKeysButton.Name = "addManyKeysButton";
            addManyKeysButton.Size = new Size(155, 23);
            addManyKeysButton.TabIndex = 7;
            addManyKeysButton.Text = "Add many keys";
            addManyKeysButton.UseVisualStyleBackColor = true;
            addManyKeysButton.Click += addManyKeysButton_Click;
            // 
            // progressLabel
            // 
            progressLabel.AutoSize = true;
            progressLabel.Location = new Point(176, 146);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new Size(10, 15);
            progressLabel.TabIndex = 6;
            progressLabel.Text = ".";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 112);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 6;
            label1.Text = "Key name:";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(7, 138);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(163, 23);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 6;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(74, 109);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(174, 23);
            textBox1.TabIndex = 3;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(293, 92);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(45, 15);
            nameLabel.TabIndex = 5;
            nameLabel.Text = "Name: ";
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.Filter = "JSON database|*.json";
            openFileDialog1.Title = "Open database";
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.Description = "Choose folder with samples";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.Filter = "JSON Database|*.json";
            saveFileDialog1.Title = "Save database";
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog2";
            openFileDialog2.Filter = "WAV files|*.wav";
            openFileDialog2.Title = "Load audio";
            // 
            // bulkCheckButton
            // 
            bulkCheckButton.Location = new Point(321, 134);
            bulkCheckButton.Name = "bulkCheckButton";
            bulkCheckButton.Size = new Size(106, 39);
            bulkCheckButton.TabIndex = 6;
            bulkCheckButton.Text = "Bulk check";
            bulkCheckButton.UseVisualStyleBackColor = true;
            bulkCheckButton.Click += bulkCheckButton_Click;
            // 
            // saveFileDialog2
            // 
            saveFileDialog2.DefaultExt = "csv";
            saveFileDialog2.Filter = "CSV files|*.csv";
            saveFileDialog2.Title = "Save CSV raport";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(469, 212);
            Controls.Add(bulkCheckButton);
            Controls.Add(nameLabel);
            Controls.Add(groupBox1);
            Controls.Add(loadSpeakerButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "WhoAmI";
            FormClosing += Form1_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button loadDatabaseButton;
        private Button addKeyButton;
        private Button saveDatabaseButton;
        private Button loadSpeakerButton;
        private GroupBox groupBox1;
        private Label nameLabel;
        private OpenFileDialog openFileDialog1;
        private FolderBrowserDialog folderBrowserDialog1;
        private SaveFileDialog saveFileDialog1;
        private TextBox textBox1;
        private OpenFileDialog openFileDialog2;
        private ProgressBar progressBar1;
        private Label progressLabel;
        private Label label1;
        private Button addManyKeysButton;
        private Button clearDbButton;
        private Button bulkCheckButton;
        private SaveFileDialog saveFileDialog2;
    }
}