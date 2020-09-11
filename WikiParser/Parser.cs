using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikiParser
{
    class Parser
    {
        private string name;
        private const string baseUrl = "https://ru.wikipedia.org/wiki/";
        private HtmlDocument document;
        public static HttpClient Client { get; set; } = new HttpClient();

        public Parser(string name)
        {
            this.name = name;
        }


        private async Task<string> GetSource()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla / 5.0 (Windows NT 6.3; WOW64; rv: 31.0) Gecko / 20100101 Firefox / 31.0");

            try
            {
                HttpResponseMessage response;
                response = await Client.GetAsync(baseUrl + $"{name}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private async Task<HtmlDocument> GetDocument()
        {

            var source = await GetSource();
            document = new HtmlDocument();
            document.LoadHtml(source);
            return document;
        }

        public async Task<BirdSummary> GetData()
        {
            document = await GetDocument();
            BirdSummary birdSummary = new BirdSummary();
            var articleContent = document?.DocumentNode?.SelectSingleNode(".//div[@class='mw-parser-output']");
            var table = articleContent?.SelectSingleNode(".//table");
            birdSummary.Picture = "https:" + table?.SelectSingleNode(".//img")?.Attributes["src"]?.Value;
            birdSummary.Name = name;
            return birdSummary;

        }
    }
}
