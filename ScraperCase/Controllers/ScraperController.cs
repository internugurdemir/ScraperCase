using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using ScraperCase.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Net.Mail;
using System.Net;

namespace ScraperCase.Controllers
{
    public class ScraperController : Controller
    {
        [HttpGet]
        public IActionResult ProductScraper()
        {
            return View();
        }

        /*
         * Acessing to spread sheets by google drive api
         */

        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        static readonly string ApplicationName = "Scraper";

        static readonly string SpreadSheetId = "1JsRD6MQXwMyliPyZkplJ-T4t8Lur1QZEWEpnqFxL9ho";

        static readonly string sheet = "ProductList";

        static SheetsService service;

        [HttpPost]
        public IActionResult ProductScraper(string url)
        {
            Product product = new Product();
            using (var driver = new ChromeDriver())
            {
                var abc = checkValidUrl(url);
                if (!checkValidUrl(url))
                {
                    TempData["Information"] = "Please Check URL";
                    return RedirectToAction("Index","Home");
                }
                driver.Navigate().GoToUrl(url);

                #region ProductURL
                if (!string.IsNullOrEmpty(url))
                {
                    product.URL = url;
                }
                else
                {
                    product.URL = "İsim Bilgisi bulunamadı";
                }
                #endregion

                #region ProductSKU
                if (!string.IsNullOrEmpty(driver.FindElement(By.CssSelector(".fl .col-12 .product-name")).GetAttribute("data-id").ToString()))
                {
                    product.SKU = driver.FindElement(By.CssSelector(".fl .col-12 .product-name")).GetAttribute("data-id").ToString();
                }
                else
                {
                    product.SKU = "SKU Bilgisi bulunamadı";
                }
                #endregion

                #region ProductName
                if (driver.FindElements(By.CssSelector(".fl .col-12 .product-name")).Count() > 0)
                {
                    product.ProductName = driver.FindElement(By.CssSelector(".fl .col-12 .product-name")).Text;
                }
                else
                {
                    product.ProductName = "İsim Bilgisi bulunamadı";
                }
                #endregion

                #region ProductAvailability
                if (driver.FindElements(By.CssSelector("div.variantList a.col.box-border")).Count() > 0)
                {
                    double totalCount = (double)driver.FindElements(By.CssSelector("div.variantList a.col.box-border")).Count();
                    double passiveCount = (double)driver.FindElements(By.CssSelector("div.variantList a.col.box-border.passive")).Count();
                    double availability = (passiveCount / totalCount) * 100;
                    var availabilityPercent = string.Format("{0:0.00}", availability);
                    product.Availability = availabilityPercent + " %";

                }
                else
                {
                    product.Availability = "Mevcut Stok Bilgisi bulunamadı";
                }
                #endregion

                #region ProductOffer
                if (driver.FindElements(By.CssSelector(".fl .col-12 .price-discount-sec")).Count() > 0)
                {
                    product.Offer = driver.FindElement(By.CssSelector(".fl .col-12 .price-discount-sec div.detay-indirim span")).Text;
                }
                else
                {
                    product.Offer = "Teklif Bilgisi bulunamadı";
                }
                #endregion

                #region ProductPrice
                if (driver.FindElements(By.ClassName(".currencyPrice.discountedPrice")).Count() > 0)
                {
                    product.ProductPrice = driver.FindElement(By.CssSelector(".currencyPrice.discountedPrice")).Text;
                }
                else
                {
                    product.ProductPrice = "Fiyat Bilgisi bulunamadı";
                }
                #endregion

                #region ProductSalePrice
                if (driver.FindElements(By.CssSelector(".fl .priceLine .discountPrice")).Count() > 0)
                {
                    product.ProductSalePrice = driver.FindElement(By.CssSelector(".fl .priceLine .discountPrice")).Text;
                }
                else
                {
                    product.ProductSalePrice = "Satış Fiyatı Bilgisi bulunamadı";
                }
                #endregion
                
                #region ProductCode - Not Ready yet
      
                //if (driver.FindElements(By.CssSelector(".product-feature-content")).Count() > 0)
                //{
                //    var checkProductCode = driver.FindElement(By.CssSelector(".product-feature-content"));
                //    product.ProductCode = checkProductCode.Text;

                //}
                //else
                //{
                //    product.ProductCode = "Kod Bilgisi bulunamadı";
                //}
                #endregion

            }

            /*
            * Getting access to credential
            */
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret_generator.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                                             .CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
            //Entries
            var range = $"{sheet}!A:A";
            var valueRange = new ValueRange();

            var objectDetailstoWrite = new List<object> 
            {
                product.URL,
                product.SKU,
                product.ProductName,
                product.Availability,
                product.UserEmail,
                product.Offer,
                product.ProductSalePrice,
                product.ProductCode,
            };

            valueRange.Values = new List<IList<object>> { objectDetailstoWrite };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadSheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();

            //https://docs.google.com/spreadsheets/d/1JsRD6MQXwMyliPyZkplJ-T4t8Lur1QZEWEpnqFxL9ho/edit?usp=sharing

            SendMail(url);

            TempData["Information"] = "The Product is added. Please check from ";
            return RedirectToAction("Index", "Home");
        }

        private void SendMail(string url)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            mail.From = new MailAddress("DenemedemirDenemedemir@gmail.com");
            mail.To.Add(new MailAddress("okan@analyticahouse.com"));

            mail.Subject = "New Product Added";

            mail.IsBodyHtml = true;

            string MailText = url + " product added to https://docs.google.com/spreadsheets/d/1JsRD6MQXwMyliPyZkplJ-T4t8Lur1QZEWEpnqFxL9ho/edit?usp=sharing";
            mail.Body = MailText;
            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("DenemedemirDenemedemir@gmail.com", "DenemedemirDenemedemirDenemedemir123");
            smtpClient.Send(mail);

        }

        private bool checkValidUrl(string url)
        {
            Uri uriResult;
            bool tryCreateResult = Uri.TryCreate(url, UriKind.Absolute, out uriResult);
            if (tryCreateResult == true && uriResult != null)
                return true;
            else
                return false;
        }
    }
}
