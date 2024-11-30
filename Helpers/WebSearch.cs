using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace DataDial.Helpers
{
    public static class WebSearch
    {
        public static void SearchOnlineWithDorks(string phoneNumber)
        {
            Console.WriteLine($"[BUSCA] Verificando redes sociais e informações públicas para {phoneNumber}...");

            var options = new ChromeOptions();
            options.AddArgument("--headless");

            using var driver = new ChromeDriver(options);

            try
            {
                string dorkQuery = $"\"{phoneNumber}\" site:facebook.com OR site:twitter.com OR site:whatsapp.com OR site:linkedin.com";
                driver.Navigate().GoToUrl($"https://www.google.com/search?q={dorkQuery}");

                var results = driver.FindElements(By.CssSelector(".tF2Cxc"));

                if (results.Count == 0)
                {
                    Console.WriteLine("[INFO] Nenhuma informação encontrada diretamente.");
                }
                else
                {
                    foreach (var result in results)
                    {
                        Console.WriteLine($"- {result.Text} ({result.FindElement(By.CssSelector(".yuRUbf a")).GetAttribute("href")})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante a busca online: {ex.Message}");
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
