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
    public partial class ChooseFreqBandDialog : Form
    {
        public double MinFrequency { get; private set; }
        public double MaxFrequency { get; private set; }

        public ChooseFreqBandDialog(double minFreq, double maxFreq)
        {
            InitializeComponent();

            MinFrequency = minFreq;
            MaxFrequency = maxFreq;

            minFreqUpDown.Minimum = maxFreqUpDown.Minimum = (decimal)MinFrequency;
            minFreqUpDown.Maximum = maxFreqUpDown.Maximum = (decimal)MaxFrequency;

            minFreqUpDown.Value = (decimal)MinFrequency;
            maxFreqUpDown.Value = (decimal)MaxFrequency;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (minFreqUpDown.Value >= maxFreqUpDown.Value)
            {
                MessageBox.Show("Invalid range (min >= max)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MinFrequency = (double)minFreqUpDown.Value;
            MaxFrequency = (double)maxFreqUpDown.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        public static (double min, double max) PromptMinMax(double initialMin, double initialMax)
        {
            var dialog = new ChooseFreqBandDialog(initialMin, initialMax);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                (double, double) result = ((double)dialog.MinFrequency, (double)dialog.MaxFrequency);
                dialog.Dispose();
                return result;
            }
            return (initialMin, initialMax);
            //throw new InvalidOperationException("Unexpected value: ChooseFreqBandDialog.ShowDialog() shall return DialogResult.OK");
        }
    }
}
