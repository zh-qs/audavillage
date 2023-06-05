using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using SoundAnalysisLib;

namespace SpeakerRecognitionApp
{
    public partial class Form1 : Form
    {
        MFCCDatabase database = new MFCCDatabase();
        bool dbSaved = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void loadDatabaseButton_Click(object sender, EventArgs e)
        {
            if (!CheckSaved())
                return;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                database = JsonSerializer.Deserialize<MFCCDatabase>(File.ReadAllText(openFileDialog1.FileName));
                dbSaved = true;
            }
        }

        private void addKeyButton_Click(object sender, EventArgs e)
        {
            LoadToDatabase(false);
        }

        void LoadToDatabase(bool searchSubfolders)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                dbSaved = false;
                database.Train(textBox1.Text, folderBrowserDialog1.SelectedPath, (i, n) =>
                {
                    progressBar1.Value = n == 0 ? 0 : (100 * i / n);
                    progressLabel.Text = $"{i}/{n}";
                    Refresh();
                }, searchSubfolders);

                Cursor = Cursors.Default;
            }
            progressBar1.Value = 0;
        }

        void SaveDatabase()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                File.WriteAllText(saveFileDialog1.FileName, JsonSerializer.Serialize(database, options));
                dbSaved = true;
            }
        }

        private void saveDatabaseButton_Click(object sender, EventArgs e)
        {
            SaveDatabase();
        }

        private void loadSpeakerButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                var mffcs = MFCCVector.FromFile(openFileDialog2.FileName, database.FFTSize);
                var result = database.FindNearest(mffcs);
                nameLabel.Text = "Name: " + result;

                Cursor = Cursors.Default;
            }
        }

        bool CheckSaved()
        {
            if (dbSaved)
                return true;

            var result = MessageBox.Show("Database is not saved. Do you want to save?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SaveDatabase();
                return true;
            }
            else if (result == DialogResult.No)
                return true;
            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CheckSaved())
                e.Cancel = true;
        }

        private void addManyKeysButton_Click(object sender, EventArgs e)
        {
            LoadToDatabase(true);

            // dump values (not necessary in production)
            File.WriteAllText("dbDump" + DateTime.Now.ToString().Replace(":", "") + ".json", JsonSerializer.Serialize(database));
        }

        private void clearDbButton_Click(object sender, EventArgs e)
        {
            if (!CheckSaved())
                return;

            database.Clear();
            dbSaved = true;
        }

        private void bulkCheckButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                string key = textBox1.Text;
                List<string> text = new List<string>() { "file;key;found_key" };
                int n = Directory.EnumerateFiles(folderBrowserDialog1.SelectedPath, "*.wav", SearchOption.AllDirectories).Count();
                int i = 0;
                progressBar1.Value = 0;
                progressLabel.Text = $"{0}/{n}";
                foreach (var path in Directory.EnumerateFiles(folderBrowserDialog1.SelectedPath, "*.wav", SearchOption.AllDirectories))
                {
                    string relative = Path.GetRelativePath(folderBrowserDialog1.SelectedPath, path);
                    int idx = relative.LastIndexOf('\\');
                    string relativeKey;
                    if (idx < 0)
                        relativeKey = key;
                    else
                        relativeKey = key + (key.Length > 0 ? ":" : "") + relative.Substring(0, relative.LastIndexOf('\\'));
                    var mffcs = MFCCVector.FromFile(path, database.FFTSize);
                    var result = database.FindNearest(mffcs);

                    text.Add($"{path};{relativeKey};{result}");

                    progressBar1.Value = 100 * i / n;
                    progressLabel.Text = $"{i}/{n}";
                    Refresh();
                }

                progressBar1.Value = 0;
                Cursor = Cursors.Default;

                MessageBox.Show("Done. Now you can save CSV raport.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                    File.WriteAllLines(saveFileDialog2.FileName, text);
            }
        }
    }
}