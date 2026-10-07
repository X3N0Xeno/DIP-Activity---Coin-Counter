namespace DIP_Activity___Coin_Counter
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.bar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCount = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.body = new System.Windows.Forms.TableLayoutPanel();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.pbSource = new System.Windows.Forms.PictureBox();
            this.pbResult = new System.Windows.Forms.PictureBox();
            this.rtbSummary = new System.Windows.Forms.RichTextBox();
            this.bar.SuspendLayout();
            this.body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).BeginInit();
            this.SuspendLayout();

            this.bar.Controls.Add(this.btnLoad);
            this.bar.Controls.Add(this.btnCount);
            this.bar.Controls.Add(this.btnSave);
            this.bar.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar.Height = 48;
            this.bar.Name = "bar";
            this.bar.Padding = new System.Windows.Forms.Padding(6);
            this.bar.TabIndex = 0;

            this.btnLoad.AutoSize = true;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Text = "Load image";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            this.btnCount.AutoSize = true;
            this.btnCount.Name = "btnCount";
            this.btnCount.Text = "Count coins";
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);

            this.btnSave.AutoSize = true;
            this.btnSave.Name = "btnSave";
            this.btnSave.Text = "Save result";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.body.ColumnCount = 2;
            this.body.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.body.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.body.Controls.Add(this.lblSource, 0, 0);
            this.body.Controls.Add(this.lblResult, 1, 0);
            this.body.Controls.Add(this.pbSource, 0, 1);
            this.body.Controls.Add(this.pbResult, 1, 1);
            this.body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.body.Name = "body";
            this.body.RowCount = 2;
            this.body.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.body.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.body.TabIndex = 1;

            this.lblSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSource.Name = "lblSource";
            this.lblSource.Text = "Original";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Name = "lblResult";
            this.lblResult.Text = "Detected coins";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.pbSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSource.Name = "pbSource";
            this.pbSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSource.TabStop = false;

            this.pbResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbResult.Name = "pbResult";
            this.pbResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbResult.TabStop = false;

            this.rtbSummary.BackColor = System.Drawing.Color.White;
            this.rtbSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbSummary.Font = new System.Drawing.Font("Consolas", 10F);
            this.rtbSummary.Height = 150;
            this.rtbSummary.Name = "rtbSummary";
            this.rtbSummary.ReadOnly = true;
            this.rtbSummary.TabIndex = 2;
            this.rtbSummary.TabStop = false;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 760);
            this.Controls.Add(this.body);
            this.Controls.Add(this.rtbSummary);
            this.Controls.Add(this.bar);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Philippine Coin Counter";
            this.bar.ResumeLayout(false);
            this.bar.PerformLayout();
            this.body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel bar;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel body;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.PictureBox pbSource;
        private System.Windows.Forms.PictureBox pbResult;
        private System.Windows.Forms.RichTextBox rtbSummary;
    }
}