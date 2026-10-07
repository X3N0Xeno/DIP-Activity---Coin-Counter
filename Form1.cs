using System;
using System.Drawing;
using System.Windows.Forms;

namespace DIP_Activity___Coin_Counter
{
    public partial class Form1 : Form
    {
        Bitmap source, result;

        public Form1()
        {
            InitializeComponent();
            btnCount.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                Bitmap loaded;
                using (Bitmap tmp = new Bitmap(dlg.FileName))
                    loaded = new Bitmap(tmp);

                if (source != null) source.Dispose();
                source = loaded;
                pbSource.Image = source;

                if (result != null) { pbResult.Image = null; result.Dispose(); result = null; }
                rtbSummary.Clear();
                btnCount.Enabled = true;
                btnSave.Enabled = false;
            }
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            if (source == null) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                CoinResult r = CoinCounter.Analyze(source);

                if (result != null) result.Dispose();
                result = r.Image;
                pbResult.Image = result;
                ShowSummary(r);
                btnSave.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Counting failed"); }
            finally { Cursor = Cursors.Default; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (result == null) return;
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "PNG|*.png|BMP|*.bmp|JPEG|*.jpg";
                if (dlg.ShowDialog() == DialogResult.OK) result.Save(dlg.FileName);
            }
        }

        void ShowSummary(CoinResult r)
        {
            rtbSummary.Clear();
            int totalCoins = 0, totalCentavos = 0, found = 0;
            for (int i = 0; i < CoinCounter.Names.Length; i++)
            {
                int n = r.Counts[i];
                totalCoins += n;
                totalCentavos += n * CoinCounter.Centavos[i];
                if (n > 0) found++;
                AppendColored(string.Format("{0,-9} x {1,2}   P{2,7:N2}\n",
                    CoinCounter.Names[i], n, n * CoinCounter.Centavos[i] / 100.0),
                    CoinCounter.Colors[i], false);
            }
            AppendColored(string.Format("Denominations found: {0} of {1}\n", found, CoinCounter.Names.Length),
                Color.Black, false);
            AppendColored(string.Format("Total: {0} coins   P{1:N2}\n", totalCoins, totalCentavos / 100.0),
                Color.Black, true);
            if (r.Unknown > 0)
                AppendColored(string.Format("{0} coin(s) could not be classified\n", r.Unknown), Color.Gray, false);
            if (r.Note.Length > 0)
                AppendColored(r.Note + "\n", Color.Gray, false);
        }

        void AppendColored(string text, Color color, bool bold)
        {
            rtbSummary.SelectionStart = rtbSummary.TextLength;
            rtbSummary.SelectionLength = 0;
            rtbSummary.SelectionColor = color;
            rtbSummary.SelectionFont = new Font(rtbSummary.Font, bold ? FontStyle.Bold : FontStyle.Regular);
            rtbSummary.AppendText(text);
        }
    }
}