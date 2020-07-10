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
        public List<HtmlNode> CrawlListBankInMain(String url)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };

            //Load trang web, nạp html vào document
            HtmlDocument document = htmlWeb.Load(url);
            String xPath = "//body/div/div/div/div[@class='box_main']/div/div[@class='tab-content']/div/ul/li";
            var listBank = document.DocumentNode.SelectNodes(xPath).ToList();

            return listBank;
        }
    }
}
