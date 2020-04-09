using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SelenSharp.PageObjectModel;


namespace Prestashop_NUnit
{
    public class Tests
    {
        public IWebDriver driver = new ChromeDriver();


        [SetUp]
        public void Setup()
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://prestashop-automation.qatestlab.com.ua/ru/");
        }


        [Test]
        public void Test1()
        {
            var homePage = new HomePage(driver);
            Assert.IsTrue(homePage.checkPopularItemsCurrencySign());


            homePage.makeDollar();
            homePage.searchDress();
            var searchPage = new SearchResultPage(driver);
            Assert.IsTrue(searchPage.checkResultsCount());


            Assert.IsTrue(searchPage.check_dollar());


            searchPage.setSortFromHigh();
            Assert.IsTrue(searchPage.isSorted());


            Assert.IsTrue(searchPage.checkDiscount());
        }


        [TearDown]
        public void Closing()
        {
            driver.Quit();
        }
    }
}