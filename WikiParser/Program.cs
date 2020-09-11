using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WikiParser
{
    class Program
    {
        const string jsonPath = "birds.json";

        const string outPath = "birdSummary.json";

        static void Main()
        {
            Parser parser;
            List<BirdSummary> birdSummaries = new List<BirdSummary>();
            List<Bird> birds = ReadJson();
            List<string> names = new List<string>();

            string name_old, new_name;
            for (int i = 0; i < birds.Count; i++)
            {
                name_old = birds[i].Name;
                string nname = name_old.Split('(')[1];
                string newname = nname.Substring(0, nname.Length - 1);
                names.Add(newname);
            }

            for (int i = 0; i < names.Count; i++)
            {
                parser = new Parser(names[i]);
                birdSummaries.Add(parser.GetData().GetAwaiter().GetResult());
                Console.WriteLine(birdSummaries[i]);
            }

            WriteJson(birdSummaries);
            Console.ReadKey();
        }

        private static List<Bird> ReadJson()
        {
            string json = File.ReadAllText(jsonPath);
            List<Bird> birds = JsonConvert.DeserializeObject<List<Bird>>(json);
            return birds;
        }

        private static void WriteJson(List<BirdSummary> bs)
        {
            string json = JsonConvert.SerializeObject(bs);
            File.WriteAllText(outPath, json);
        }
    }
}
