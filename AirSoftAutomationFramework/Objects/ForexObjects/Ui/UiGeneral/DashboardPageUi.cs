// Ignore Spelling: Ign Calander Donat Forex api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class DashboardPageUi : IDashboardPageUi
    {
        #region Members
        //private string _saleTitle = "//li[@class='deposit ng-star-inserted']//child::span[contains(.,'{0}')]";
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public DashboardPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By FrontTotalCustomersBoxExp = By.CssSelector(
            "div[class='front total-customers'] p");

        private readonly By DepositTableRowsExp = By.CssSelector(
           "table[id='depositDashTable'] tbody tr");

        private readonly By BackTotalDepositsBoxExp = By.CssSelector(
            "ul[class='deposit-list ng-star-inserted'] h4");

        private readonly By BackTotalCanceledOrdersBoxExp = By.CssSelector(
           "div[class='back total-withdrawal'] h4");

        private readonly By BackTotalOrdersBoxExp = By.CssSelector(
         "div[class='back total-deposits'] h4");

        private readonly By FrontTotalOrderOrDepositsBoxExp = By.CssSelector(
           "div[class='front total-deposits'] div[class='statistics-data'] p");

        private readonly By FrontTotalDepositsBoxExp = By.CssSelector(
       "div[class='front total-deposits'] p");

        private readonly By FrontTotalNetDepositsBoxExp = By.CssSelector(
         "div[class='front total-deposits'] div[class='statistics-notes deposit-color ']");

        private readonly By BackWithdrawalBoxExp = By.CssSelector(
           "div[class='back total-withdrawal'] h4");

        private readonly By BackClientsBoxExp = By.CssSelector(
           "div[class='back total-customers'] h4");

        private readonly By BackWithdrawalBoxEurSignExp = By.CssSelector(
          "h4 i[class='fa fa-eur custom-icon-dash']");

        private readonly By FrontTotalWithdrawalBoxExp = By.CssSelector(
         "div[class='front total-withdrawal'] p[class*='number-line']");

        private readonly By FrontTotalWithdrawalEurSIgnBoxExp = By.CssSelector(
         "div[class='front total-withdrawal'] i[class='fa fa-eur custom-icon-dash withdrawal-icon']");

        private readonly By FrontClosePnlBoxExp = By.CssSelector(
            "div[class='front sales-perfomance'] div[class='statistics-data'] p");

        private readonly By FrontClosePnlEurSIgnBoxExp = By.CssSelector(
           "div[class='icon-wrapper'] i[class='fa fa-eur fa-4x custom-icon-dash']");

        private readonly By RecentActivitiesDepositOrOrdersBtnExp = By.CssSelector(
            "a[class*='recent-activities-deposit'], a[class*='recent-activities-orders'] i[class*='credit-card']");

        private readonly By RecentActivitiesDepositOrdersBtnStatusExp = By.CssSelector(
          "a[class*='recent-activities-deposit filterActive']," +
            " a[class*='recent-activities-orders filterActive']");

        private readonly By RecentActivitiesLoginBtnExp = By.CssSelector(
            "a[class*='recent-activities-login']");

        private readonly By RecentActivitiesRegisterBtnExp = By.CssSelector(
            "a[class*='recent-activities-register']");

        private readonly By RecentActivitiesCommentsBtnExp = By.CssSelector(
            "a[class*='recent-activities-comments']");

        private readonly By DepositTableSearchExp =
            By.CssSelector("div[id='depositDashTable_filter'] input");

        private readonly By CloseClientProfileBtnExp =
          By.CssSelector("button[class='close pull-right close-sales-profile']");

        private readonly By WithdrawalTableSearchExp =
            By.CssSelector("div[id='withdrawalDashTable_filter'] input");

        private readonly By SaleTitleExp =
            By.CssSelector("span[class='sale-title']");

        private readonly By CalanderExp =
            By.CssSelector("section[class='tile-header calendar-heading']");

        private readonly By PerformanceAndDonatExp =
            By.CssSelector("div[class*='box-wrapper middleBoxRemov']");

        private readonly By ActiveCampaignsExp =
            By.CssSelector("div[id='campaignActive']");

        private readonly By DepositsExp =
            By.CssSelector("div[class='tile dashboard-transaction ']");

        private readonly By WithdrawalsExp =
           By.CssSelector("div[class='tile dashboard-transaction']");

        private readonly By LastRegistrationExp =
           By.CssSelector("div[class='tile recent-leads-box']");


        //private readonly By OrderIdFilterExp = By.CssSelector(
        //    "div[id*='dashboardOrdersTable'] input[type='search']");

        private readonly By InActiveCampaignGridSearchExp = By.CssSelector(
            "div[id*='inActiveCampaignTable'] input[type='search']");

        private readonly By ActiveCampaignGridSearchExp = By.CssSelector(
            "div[id*='activeCampaignTable'] input[type='search']");
        #endregion

        public IDashboardPageUi VerifyFrontCardTotalCustomers(int numOfCustomers)
        {
            _driver.SearchElement(FrontTotalCustomersBoxExp)
                .WaitForElementTextToChange(_driver,
                FrontTotalCustomersBoxExp, numOfCustomers.ToString());

            return this;
        }

        public IDashboardPageUi VerifyFrontCardTotalCanCelledOrdersAmount(double
            canceledOrdersAmount)
        {
            _driver.SearchElement(FrontTotalWithdrawalBoxExp)
                .WaitForElementTextToChange(_driver, FrontTotalWithdrawalBoxExp,
                canceledOrdersAmount.ToString());

            return this;
        }

        public string GetFrontTotalOrdersGross(double ordersAmount)
        {
            return _driver.SearchElement(FrontTotalOrderOrDepositsBoxExp)
                .WaitForElementTextToChange(_driver, FrontTotalOrderOrDepositsBoxExp,
                ordersAmount.ToString());
        }

        public string GetBackTotalOrders()
        {
            return _driver.SearchElement(BackTotalOrdersBoxExp)
                .GetElementText(_driver, BackTotalOrdersBoxExp);
        }

        public List<string> GetBackCardTotalDeposits()
        {
            var deposits = new List<string>();
            _driver.WaitForExactNumberOfElements(BackTotalDepositsBoxExp, 1);
            var elements = _driver.SearchElements(BackTotalDepositsBoxExp);

            foreach (var item in elements)
            {
                deposits.Add(item.GetElementText(_driver));
            }

            return deposits;
        }

        public IDashboardPageUi VerifyBackCardWithdrawalData(List<string> expectedWithdrawalData)
        {
            var elementsText = new List<string>();

            _driver
              .WaitForExactNumberOfElements(BackWithdrawalBoxExp, 3);

            _driver.SearchElements(BackWithdrawalBoxExp)
                .ForEach(p => elementsText.Add(p.GetElementText(_driver)));

            for (var i = 0; i < 10; i++)
            {
                if (elementsText.CompareTwoListOfString(expectedWithdrawalData).Count() != 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                break;
            }

            return this;
        }

        public string VerifyBackCardWithdrawalData()
        {
            return _driver
                .SearchElement(BackWithdrawalBoxExp)
                .GetElementText(_driver, BackWithdrawalBoxExp);
        }

        public IDashboardPageUi VerifyCalanderNotExist()
        {
            _driver.WaitForElementNotExist(CalanderExp);

            return this;
        }

        public IDashboardPageUi VerifyPerformanceAndDoNotExist()
        {
            _driver.WaitForElementNotExist(PerformanceAndDonatExp);

            return this;
        }

        public IDashboardPageUi VerifyActiveCampaignsNotExist()
        {
            _driver.WaitForElementNotExist(ActiveCampaignsExp);

            return this;
        }

        public IDashboardPageUi VerifyDepositsNotExist()
        {
            _driver.WaitForElementNotExist(DepositsExp);

            return this;
        }

        public IDashboardPageUi VerifyWithdrawalsNotExist()
        {
            _driver.WaitForElementNotExist(WithdrawalsExp);

            return this;
        }

        public IDashboardPageUi VerifyLastRegistrationNotExist()
        {
            _driver.WaitForElementNotExist(LastRegistrationExp);

            return this;
        }

        public IDashboardPageUi VerifyBackCardClientsData(List<string> expectedClientsData)
        {
            var elementsText = new List<string>();

            _driver.SearchElements(BackClientsBoxExp)
              .ForEach(p => elementsText.Add(p.GetElementText(_driver)));

            for (var i = 0; i < 10; i++)
            {
                if (elementsText.CompareTwoListOfString(expectedClientsData).Count() != 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                break;
            }

            return this;
        }

        public IDashboardPageUi VerifyBackCardWithdrawalForEurCurrency()
        {
            var numOfElements = _driver
                .WaitForExactNumberOfElements(BackWithdrawalBoxEurSignExp, 3);

            if (numOfElements < 3)
            {
                throw new InvalidOperationException($"not all values contains EUR sign");
            }

            return this;
        }

        public IDashboardPageUi VerifyFrontCardTotalWithdrawal(string expectedTotalWithdrawal)
        {
            string actualTotalWithdrawal = null;

            for (var i = 0; i < 10; i++)
            {
                actualTotalWithdrawal = _driver.SearchElement(FrontTotalWithdrawalBoxExp)
                    .GetElementText(_driver);

                if (actualTotalWithdrawal != $"{expectedTotalWithdrawal}.00")
                {
                    Thread.Sleep(100);

                    continue;
                }

                break;
            }

            actualTotalWithdrawal.StringContains(expectedTotalWithdrawal);

            return this;
        }

        public IDashboardPageUi VerifyFrontCardTotalWithdrawalEurCurrency()
        {
            var numOfElements = _driver
                .WaitForExactNumberOfElements(FrontTotalWithdrawalEurSIgnBoxExp, 1);

            if (numOfElements < 1)
            {
                Assert.Fail("not all values contains EUR sign");
                throw new InvalidOperationException(
                   $"not all values contains EUR sign");
            }

            return this;
        }

        public IDashboardPageUi VerifyFrontCardPnl(string expectedClosePnl)
        {
            string actualClosePnl = null;

            for (var i = 0; i < 10; i++)
            {
                actualClosePnl = _driver.SearchElement(FrontClosePnlBoxExp)
                    .GetElementText(_driver);

                if (actualClosePnl != $"{expectedClosePnl}.00")
                {
                    Thread.Sleep(100);
                    continue;
                }

                break;
            }

            actualClosePnl.StringContains(expectedClosePnl);

            return this;
        }

        public IDashboardPageUi VerifyFrontCardPnlEurCurrency()
        {
            var numOfElements = _driver
                .WaitForExactNumberOfElements(FrontClosePnlEurSIgnBoxExp, 1);

            if (numOfElements < 1)
            {
                throw new InvalidOperationException(
                   $"not all values contains EUR sign");
            }

            return this;
        }

        public IDashboardPageUi VerifyFrontCardTotalDeposit(string expectedDeposit)
        {
            string actualDeposit = null;

            for (var i = 0; i < 10; i++)
            {
                actualDeposit = _driver.SearchElement(FrontTotalDepositsBoxExp)
                    .GetElementText(_driver);

                if (actualDeposit != $"{expectedDeposit}.00")
                {
                    Thread.Sleep(100);
                    continue;
                }

                break;
            }

            actualDeposit.StringContains(expectedDeposit);

            return this;
        }

        public IDashboardPageUi VerifyFrontCardNetTotalDeposit(string expectedNetDeposit)
        {

            string actualNetDeposit = null;

            for (var i = 0; i < 10; i++)
            {
                actualNetDeposit = _driver.SearchElement(FrontTotalNetDepositsBoxExp)
                    .GetElementText(_driver);

                if (actualNetDeposit.Replace("Net Deposit ", "") != $"{expectedNetDeposit}.00")
                {
                    Thread.Sleep(100);
                    continue;
                }

                break;
            }

            actualNetDeposit.StringContains(expectedNetDeposit);

            return this;
        }

        public string GetBackCardTotalOrders()
        {
            var text = _driver.SearchHiddenElement(BackTotalDepositsBoxExp)
                .GetElementText(_driver, BackTotalDepositsBoxExp);

            return Regex.Match(text, @"\d+").Value;
        }

        public int GetBackCardTotalPendingWithdrawals(int amount)
        {
            //_driver.WaitForAnimationToLoad(10000);
            //var elements = _driver.SearchElements(BackWithdrawalBoxExp).ToList();
            string text = null;
            List<IWebElement> elements = null;

            for (var i = 0; i < 15; i++)
            {
                elements = _driver.SearchElements(BackWithdrawalBoxExp).ToList();

                if (elements.Count == 0)
                {
                    elements = _driver.SearchElements(BackWithdrawalBoxExp).ToList();
                    Thread.Sleep(300);

                    continue;
                }

                text = elements
                    .FirstOrDefault()
                    .GetElementText(_driver);

                if (text != $"{amount}.00")
                {
                    elements = _driver.SearchElements(BackWithdrawalBoxExp).ToList();
                    text = elements[0].GetElementText(_driver);
                    Thread.Sleep(100);

                    continue;
                }

                break;
            }

            try
            {
                return Regex.Match(text, @"\d+").Value.StringToInt();
            }
            catch (Exception ex)
            {
                var exceMessage = ($" Exception Message: {ex.Message}, element text: {text}");

                throw new Exception(exceMessage);
            }
        }

        public ISearchResultsUi SearchDeposit(string searchText)
        {
            _driver.WaitForAtListOneElement(DepositTableRowsExp, 150);

            _driver.SearchElement(DepositTableSearchExp)
                .SendsKeysAuto(_driver, DepositTableSearchExp, searchText);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SearchInActiveCampaign(string campaignName)
        {
            _driver.SearchElement(InActiveCampaignGridSearchExp)
                .SendsKeysAuto(_driver, InActiveCampaignGridSearchExp, campaignName);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SearchActiveCampaign(string campaignName)
        {
            _driver.SearchElement(ActiveCampaignGridSearchExp, 150)
                .SendsKeysAuto(_driver, ActiveCampaignGridSearchExp, campaignName, 150);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SearchWithdrawal(string searchText)
        {
            _driver.SearchElement(WithdrawalTableSearchExp)
                .SendsKeysAuto(_driver, WithdrawalTableSearchExp, searchText);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SearchOrderById(string orderId)
        {
            //_driver.SearchElement(DataRep.SearchByOrderId, 150)
            //    .SetTextUsingJavaScript(_driver, orderId);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi ClickOnRecentActivitiesDeposit()
        {
            _driver.SearchElement(RecentActivitiesDepositOrOrdersBtnExp)
                    .ForceClick(_driver, RecentActivitiesDepositOrOrdersBtnExp);


            //var elementStatus = _driver.CheckIfElementExist(RecentActivitiesDepositOrdersBtnStatusExp);

            //if (elementStatus)
            //{
            //    _driver.SearchElement(RecentActivitiesDepositOrOrdersBtnExp)
            //        .ForceClick(_driver, RecentActivitiesDepositOrOrdersBtnExp);
            //}

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public IAgentProfileUi ClickOnSaleTitle(string userName)
        {
            //var saleTitleExp = By.XPath(string.Format(_saleTitle, userName));

            _driver.SearchElement(SaleTitleExp)
                .ForceClick(_driver, SaleTitleExp);

            return _apiFactory.ChangeContext<IAgentProfileUi>(_driver);
        }

        public ISearchResultsUi ClickOnRecentActivitiesOrders()
        {
            _driver.SearchElement(RecentActivitiesDepositOrOrdersBtnExp, 150)
                .ForceClick(_driver, RecentActivitiesDepositOrOrdersBtnExp, 150);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi ClickOnRecentActivitiesLogin()
        {
            _driver.SearchElement(RecentActivitiesLoginBtnExp)
                .ForceClick(_driver, RecentActivitiesLoginBtnExp);

            //_driver.WaitForAnimationToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi ClickOnRecentActivitiesRegister()
        {
            _driver.SearchElement(RecentActivitiesRegisterBtnExp)
                .ForceClick(_driver, RecentActivitiesRegisterBtnExp);

            //_driver.WaitForAnimationToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi ClickOnRecentActivitiesComments()
        {
            _driver.SearchElement(RecentActivitiesCommentsBtnExp)
                .ForceClick(_driver, RecentActivitiesCommentsBtnExp);

            //_driver.WaitForAnimationToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
