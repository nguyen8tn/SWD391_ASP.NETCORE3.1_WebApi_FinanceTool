using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD391.Service
{
    public class WebScrapingService
    {
        public async Task<List<HtmlNode>> CrawlListBankInMainAsync(String url)
        {
            HtmlDocument document = await loadDocAsync(url);
            string xPath = "//body/div/div/div/div[@class='box_main']/div/div[@class='tab-content']/div/ul/li";
            var crawlNode = Task.Run(() => document.DocumentNode.SelectNodes(xPath).ToList());
            var listBank = await crawlNode;
            var value = listBank;
            IDictionary<int, string> listViewDetails = new Dictionary<int, string>();
            for (int i = 0; i < value.Count; i++)
            {
                //li-al
                string img = "https://thebank.vn";
                string name = value[i].SelectSingleNode(".//a[2]/span").InnerText;
                string link = value[i].SelectSingleNode(".//a[1]").Attributes["href"].Value;
                img += value[i].SelectSingleNode(".//a/div/img").Attributes["src"].Value;
                listViewDetails.Add(i, link);
            }
            List<Task> taskLisk = new List<Task>();
            int c = 0;
            foreach (var item in listViewDetails)
            {
                //Task<int> task = new Task<int>(() => crawlDetails(item));
                //task.Start();
                //taskLisk.Add(task);
                c++;
                if (c > 2)
                {
                    ///await Task.WhenAll(taskLisk);
                    var paral = Task.Run(() => {
                        Parallel.ForEach(listViewDetails, crawItem =>
                        {
                            Console.WriteLine("1 -------------------------");
                            Console.WriteLine(crawItem.Key + " " + crawItem.Value);
                            var x = crawlDetails(crawItem);
                            string[] nextUrl = new string[2];
                            Console.WriteLine("2 -------------------------");
                            if (x.TryGetValue(crawItem.Key, out nextUrl))
                            {
                                crawlSavingRate(nextUrl[0]);
                                crawlLoanRate(nextUrl[1]);
                            }
                        });
                    });
                    await paral;
                    c = 0;
                }
            }

            return listBank;
        }

        public IDictionary<int, string[]> crawlDetails(KeyValuePair<int, string> link)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
                UsingCache = false
            };
            HtmlDocument document = htmlWeb.Load(link.Value);
            string xPath = "//body/div/div/div/div/div/div/div/div/ul[1]";
            var node = document.DocumentNode.SelectSingleNode(xPath);
            IDictionary<int, string[]> result = new Dictionary<int, string[]>();
            try
            {
                string vayTD = "", guiTK = "";

                try
                {
                    var t = node.SelectSingleNode("(//body/div/div/div/div/div/div/div/div/ul/li/a)[4]").Attributes["href"];
                    vayTD = t.Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                try
                {
                    var t = node.SelectSingleNode("(//body/div/div/div/div/div/div/div/div/ul/li/a)[6]").Attributes["href"];
                    guiTK = t.Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                string[] nextUrl = { guiTK, vayTD };
                result.Add(link.Key, nextUrl);
                Console.WriteLine("2" + guiTK);
                Console.WriteLine("1" + vayTD);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        private void crawlLoanRate(string url)
        {

        }
        private async void crawlSavingRate(string url)
        {
            if (url != null && url != "")
            {
                Console.WriteLine("this:" + url);
                HtmlDocument document = await loadDocAsync(url);
                string xPath = "//body/div/div/div/div/div/div/div[@class='tbl_gui_tietkiem_hidden div_tab ']";
                var crawlNode = Task.Run(() => document.DocumentNode.SelectNodes(xPath).ToList());
                var listRate = await crawlNode;
                var aNode = listRate[0].SelectNodes(".//ul/li/a").ToList();

                for (int i = 0; i < aNode.Count; i++)
                {
                    if (aNode[i].InnerText.Equals("Nhận lãi cuối kỳ"))
                    {
                        List<string> rateList = new List<string>();
                        var rateRow = listRate[0].SelectNodes(".//div/div").Last();
                        var rateItem = rateRow.SelectNodes(".//table/tbody/tr").ToList();
                        for (int y = 1; y < rateItem.Count; y++)
                        {
                            if (rateItem[i].HasChildNodes)
                            {
                                string rate = rateItem[i].FirstChild.InnerText;
                                rateList.Add(rate);

                                Console.WriteLine("3" + rate);
                            }
                        }
                    }
                }
            }
        }
        private async Task<HtmlDocument> loadDocAsync(String url)
        {
            if (url !="" && url != null)
            {
                HtmlWeb htmlWeb = new HtmlWeb()
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                    UsingCache = false
                };
                var myTask = Task.Run(() => htmlWeb.Load(url));
                HtmlDocument document = await myTask;
                return document;
            }
            return null;
        }
    }
}
