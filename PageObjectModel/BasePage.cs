using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;


namespace SelenSharp.PageObjectModel
{
    public abstract class BasePage
    {
        public IWebDriver driver;
        public WebDriverWait wait;
        public List<List<double>> AllPrices;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
            wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        }
    }
}
