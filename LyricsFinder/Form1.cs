using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace LyricsFinder
{
    public partial class Form1 : Form
    {
        private string apiKey = "REDACTED";
        private string songUrl;

        public Form1()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            string lyrics = lyricsTextBox.Text;
            var songInfo = await SearchSongsByLyrics(lyrics);
            if (songInfo != null)
            {
                DisplaySongInfo(songInfo);
            }
            else
            {
                MessageBox.Show("No songs found with these lyrics", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task<JObject> SearchSongsByLyrics(string lyrics)
        {
            string baseUrl = "https://api.genius.com";
            string searchUrl = baseUrl + "/search";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                var response = await client.GetAsync($"{searchUrl}?q={Uri.EscapeDataString(lyrics)}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var results = JObject.Parse(json)["response"]["hits"];
                    foreach (var hit in results)
                    {
                        if ((string)hit["type"] == "song")
                        {
                            return (JObject)hit["result"];
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to retrieve songs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return null;
        }

        private void DisplaySongInfo(JObject songInfo)
        {
            lyricsTextBox.Visible = false;
            searchButton.Visible = false;

            string artistName = (string)songInfo["primary_artist"]["name"];
            string songName = (string)songInfo["title"];
            songUrl = (string)songInfo["url"];
            string thumbnailUrl = (string)songInfo["song_art_image_thumbnail_url"];

            artistLabel.Text = $"Artist: {artistName}";
            songLabel.Text = $"Song Name: {songName}";

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(thumbnailUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var imgData = response.Content.ReadAsByteArrayAsync().Result;
                    using (var ms = new System.IO.MemoryStream(imgData))
                    {
                        thumbnailPictureBox.Image = Image.FromStream(ms);
                    }
                }
            }

            resultPanel.Visible = true;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            lyricsTextBox.Visible = true;
            searchButton.Visible = true;
            resultPanel.Visible = false;
            lyricsTextBox.Clear();
        }
    }
}
