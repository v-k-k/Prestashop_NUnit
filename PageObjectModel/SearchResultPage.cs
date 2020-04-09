using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;


namespace SelenSharp.PageObjectModel
{
    class SearchResultPage : BasePage
    {
        public SearchResultPage(IWebDriver driver):
            base(driver)
        {

        }
        

        [FindsBy(How = How.XPath, Using = "//*[@id='_desktop_currency_selector']/div/span[2]")]
        private IWebElement currentCurrency;

        [FindsBy(How = How.XPath, Using = "//*[@id='js-product-list']/div[1]")]
        private IWebElement result_list;

        [FindsBy(How = How.XPath, Using = "//*[@id='js-product-list-top']/div[1]/p")]
        private IWebElement shown_text;

        [FindsBy(How = How.XPath, Using = "//*[@id='js-product-list-top']/div[2]/div/div")]
        private IWebElement popup;

        [FindsBy(How = How.XPath, Using = "//*[@id='js-product-list']/div[1]")]
        private IWebElement new_result_list;


        private string getCurrencySign()
        {
            wait.Until(driver => currentCurrency.Displayed);
            var currencySigns = currentCurrency.Text;
            var c = currencySigns.Split(new char[] { ' ' });
            return c[1];
        }


        private List<List<double>> priceCustomizer(List<List<string>> prices_list)
        {
            var result = new List<List<double>>();
            foreach (var price_group in prices_list)
            {
                var temp = new List<double>();
                for (int i = 0; i < price_group.Count; i++)
                {
                    double number;
                    if (price_group.Count > 1 && i == 1)
                    {
                        var len = price_group[i].Length;
                        price_group[i] = price_group[i].Substring(0, len - 1);
                    }
                    else
                    {
                        var tmp = price_group[i].Split(' ');
                        price_group[i] = tmp[0];
                    }
                    number = Convert.ToDouble(price_group[i]);
                    temp.Add(number);
                }
                result.Add(temp);
            }
            return result;
        }


        [Obsolete]
        private List<IWebElement> findItems()
        {
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var lst = result_list.FindElements(By.XPath("article"));
            var res = new List<IWebElement>();
            foreach (var r in lst)
            {
                res.Add(r);
            }
            return res;                
        }


        [Obsolete]
        public bool checkResultsCount()
        {
            var items = findItems();
            String check = String.Format("Товаров: {0}.", items.Count);
            return shown_text.Text.Equals(check);            
        }


        [Obsolete]
        public bool check_dollar()
        {
            var dollar = getCurrencySign();
            var items = findItems();
            var resultList = new List<string>();
            foreach (var item in items)
            {
                var txt = (item.FindElement(By.XPath("div/div/div/span[@class='price']"))).Text;
                if (txt.Contains(dollar)) resultList.Add(txt);
            }
            return items.Count.Equals(resultList.Count);
        }


        [Obsolete]
        public void setSortFromHigh()
        {
            popup.Click();
            var sortTypes = popup.FindElements(By.XPath("div/a"));
            sortTypes[sortTypes.Count - 1].Click();
        }

        [Obsolete]
        private bool checkSort()
        {
            var items = findItems();
            var all = new List<List<string>>();
            foreach (var item in items)
            {
                var item_prices = item.FindElements(By.XPath("div/div/div/span"));
                var tmp = new List<string>();
                foreach (var line in item_prices)
                {
                    var txt = line.Text;
                    if (!String.IsNullOrEmpty(txt)) tmp.Add(txt);
                }
                all.Add(tmp);
            }
            AllPrices = priceCustomizer(all);
            var after_sorting = new List<double>();
            foreach (var item in AllPrices) after_sorting.Add(item[0]);

            var sorted = new List<double>(after_sorting);

            sorted.Sort();
            sorted.Reverse();

            var result = true;
            for (int i = 0; i < sorted.Count; i++) result = after_sorting[i] == sorted[i];

            return result;
        }


        [Obsolete]
        public bool isSorted()
        {
            try
            {
                wait.Until(ExpectedConditions.StalenessOf(result_list));
                return checkSort();
            }
            catch
            {
                return checkSort();
            }
        }


        public bool checkDiscount()
        {
            var check_list = new List<List<double>>();
            foreach (var item in AllPrices)
            {
                if (item.Count > 1) check_list.Add(item);
            }
            var bool_lst = new List<bool>();
            foreach (var price_block in check_list)
            {
                var discounted = Math.Round(price_block[2] - price_block[0] * price_block[1] / 100, 2, MidpointRounding.AwayFromZero);
                bool_lst.Add(discounted == price_block[0]);
            }
            var result = true;
            foreach (var log in bool_lst) result = log;
            return result;
        }
    }
}
