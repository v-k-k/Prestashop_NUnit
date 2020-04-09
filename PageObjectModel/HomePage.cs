using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;


namespace SelenSharp.PageObjectModel
{
    class HomePage : BasePage
    {
        public HomePage(IWebDriver driver): 
            base(driver)
        {

        }
        


        [FindsBy(How = How.XPath, Using = "//*[@id='_desktop_currency_selector']/div/span[2]")]
        private IWebElement currentCurrency;

        [FindsBy(How = How.XPath, Using = "//article")]
        private IList<IWebElement> mostPopularItems;

        [FindsBy(How = How.XPath, Using = "//*[@id='_desktop_currency_selector']/div/span[2]")]
        private IWebElement openCurrancies;

        [FindsBy(How = How.XPath, Using = "//div[@class='currency-selector dropdown js-dropdown open']//ul[@class='dropdown-menu hidden-sm-down']/li/a")]
        private IList<IWebElement> all_currencies;

        [FindsBy(How = How.XPath, Using = "//form/input[2]")]
        private IWebElement input_field;


        [Obsolete]
        public bool checkPopularItemsCurrencySign()
        {
            //wait.Until(driver => currentCurrency.Displayed);
            var currs = currentCurrency.Text;
            var signs = currs.Split(new char[] { ' ' });
            var prices = new List<string>();

            foreach (var pop in mostPopularItems)
            {
                var full_price = pop.FindElement(By.XPath("div/div/div/span")).Text;
                var parts = full_price.Split(' ');
                prices.Add(parts[1]);
            }

            var result = false;
            for (int i = 0; i<prices.Count; i++) if (signs[1].Equals(prices[i])) result = true;
            return result;
        }


        [Obsolete]
        public void makeDollar()
        {
            openCurrancies.Click();
            foreach (var currency in all_currencies)
            {
                var curr_text = currency.GetAttribute("title");
                if (curr_text.Contains("Доллар") && currency.Enabled)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    currency.Click();
                    break;
                }
            }

        }

        public void searchDress()
        {
            input_field.SendKeys("dress.");
            input_field.FindElement(By.XPath("../button")).Click();
        }
    }
}
