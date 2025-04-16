using Microsoft.AspNetCore.Mvc;
using LyricsFinder.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TheLyricFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiKey = "chUnItIp5WOAQqk-ROxKPKquyvu6omVR-DNeCY4Lb2Egjsb_lbdoqalXanc49e-1";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string lyrics)
        {
            var songInfo = await SearchSongsByLyrics(lyrics);
            if (songInfo != null)
            {
                return View("Result", songInfo);
            }
            ViewBag.Message = "No songs found with these lyrics";
            return View("Index");
        }

        private async Task<SongInfo> SearchSongsByLyrics(string lyrics)
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
                            var result = hit["result"];
                            return new SongInfo
                            {
                                ArtistName = (string)result["primary_artist"]["name"],
                                SongName = (string)result["title"],
                                SongUrl = (string)result["url"],
                                ThumbnailUrl = (string)result["song_art_image_thumbnail_url"]
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
