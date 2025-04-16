namespace LyricsFinder
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox lyricsTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.PictureBox thumbnailPictureBox;
        private System.Windows.Forms.Label artistLabel;
        private System.Windows.Forms.Label songLabel;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Button resetButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";

            this.Text = "Lyrics Finder";
            this.Size = new System.Drawing.Size(800, 600);
            this.BackColor = System.Drawing.Color.FromArgb(113, 58, 190);

            lyricsTextBox = new System.Windows.Forms.TextBox()
            {
                Width = 400,
                Font = new System.Drawing.Font("Open Sans", 14),
                Location = new System.Drawing.Point(200, 100),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.FromArgb(91, 8, 136)
            };

            searchButton = new System.Windows.Forms.Button()
            {
                Text = "Search by Lyrics",
                Font = new System.Drawing.Font("Open Sans", 14, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(200, 40),
                Location = new System.Drawing.Point(300, 150),
                BackColor = System.Drawing.Color.FromArgb(91, 8, 136),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };
            searchButton.Click += new System.EventHandler(this.SearchButton_Click);

            resultPanel = new System.Windows.Forms.Panel()
            {
                Size = new System.Drawing.Size(700, 300),
                Location = new System.Drawing.Point(50, 200),
                BackColor = System.Drawing.Color.FromArgb(113, 58, 190),
                Visible = false
            };

            thumbnailPictureBox = new System.Windows.Forms.PictureBox()
            {
                Size = new System.Drawing.Size(150, 150),
                Location = new System.Drawing.Point(20, 20)
            };

            artistLabel = new System.Windows.Forms.Label()
            {
                Font = new System.Drawing.Font("Open Sans", 16, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(200, 20),
                AutoSize = true
            };

            songLabel = new System.Windows.Forms.Label()
            {
                Font = new System.Drawing.Font("Open Sans", 16, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(200, 60),
                AutoSize = true
            };

            linkLabel = new System.Windows.Forms.LinkLabel()
            {
                Text = "Go to Genius song page",
                Font = new System.Drawing.Font("Open Sans", 16, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline),
                LinkColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(200, 100),
                AutoSize = true
            };
            linkLabel.LinkClicked += (sender, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = songUrl,
                UseShellExecute = true
            });

            resetButton = new System.Windows.Forms.Button()
            {
                Text = "Search Again",
                Font = new System.Drawing.Font("Open Sans", 14, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(200, 40),
                Location = new System.Drawing.Point(300, 250),
                BackColor = System.Drawing.Color.FromArgb(91, 8, 136),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };
            resetButton.Click += new System.EventHandler(this.ResetButton_Click);

            resultPanel.Controls.Add(thumbnailPictureBox);
            resultPanel.Controls.Add(artistLabel);
            resultPanel.Controls.Add(songLabel);
            resultPanel.Controls.Add(linkLabel);
            resultPanel.Controls.Add(resetButton);

            this.Controls.Add(lyricsTextBox);
            this.Controls.Add(searchButton);
            this.Controls.Add(resultPanel);
        }
    }
}
