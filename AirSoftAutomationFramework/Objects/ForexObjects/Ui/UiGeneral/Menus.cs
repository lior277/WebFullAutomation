// Ignore Spelling: Forex api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class Menus : IMenus
    {
        private readonly IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _crmUrl = Config.appSettings.CrmUrl;

        public Menus(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's

        #region Crm
        private readonly By _broadCastPopapBtnExp = By.CssSelector("div[aria-hidden='false'] button[id='mgm-message-button']");
        private readonly By BankingMenuItemExp = By.CssSelector("span[class*='banking-menu-item']");
        private readonly By DepositsMenuItemExp = By.CssSelector("span[class*='deposit-menu-item']");
        private readonly By CfdMenuItemExp = By.CssSelector("a[class*='crypto-menu-item']");
        private readonly By CfdGroupsMenuItemExp = By.CssSelector("a[uisref='groups.list']");
        private readonly By MarketExposureExp = By.CssSelector("a[uisref='groups.market-exposure'] span");
        private readonly By BulkTradingExp = By.CssSelector("a[uisref='groups.mass-trading'] span");
        private readonly By OpenTratedsMenuItemExp = By.CssSelector("span[class*='open-trades-menu-item']");
        private readonly By RiskMenuItemExp = By.CssSelector("a[uisref='groups.risk']");
        private readonly By CloseTratedsMenuItemExp = By.CssSelector("span[class*='closed-trades-menu-item']");
        private readonly By pendingTratedsMenuItemExp = By.CssSelector("span[class*='pending-trades-menu-item']");
        private readonly By CampaignsMapPageUiMenuItemExp = By.CssSelector("a[uisref='campaigns.map-statistics']");
        #endregion

        #region Trading
        private readonly By TradingSettingsMenuItemExp = By.CssSelector("span[class*='settings']");
        private readonly By TradingSettingsMenuExp = By.CssSelector("ul[class='dropdown-menu sub-categories-ul setting-menu show']");
        private readonly By HistoryMenuItemExp = By.XPath("//a[contains(.,' Histo')]"); // By.CssSelector("i[class='fa fa-credit-card']");
        private readonly By LiveTvMenuItemExp = By.XPath("//i[contains(@class,'sprite sprite-side_live-tv-icon')]/parent::*");
        private readonly By ChronoPlatformMenuItemExp = DataRep.ChronoPlatformMenuItemExp;
        private readonly By NftPlatformMenuItemExp = By.CssSelector("div[class*='deposit_box nft-button']");
        private readonly By NftLoaderExp = By.CssSelector("div[class*='no-collections-loading']");
        private readonly By CloseOpenAndCloseTradeMenuItemExp = By.CssSelector("div[class='close-chrono-tab ng-star-inserted']");
        private readonly By TradeMenuItemExp = By.CssSelector("li[class*='trade-menu-item']");
        private readonly By OpenTradeMenuItemExp = By.CssSelector("li[class*='open-trades-menu-item']");
        private readonly By PendingOrdersMenuItemExp = By.CssSelector("li[class*='pending-orders']");
        private readonly By FavoritesMenuItemExp = By.XPath("//i[contains(@class,'favorites')]/parent::*");
        private readonly By ClosedTradesMenuItemExp = By.CssSelector("li[class*='closed-trades']");
        private readonly By MyProfileMenuItemExp = By.CssSelector("a[class='sidebar-router-a'] i[class='sprite sprite-side_profile sidebar-router-i']");
        private readonly By DocumentMenuItemExp = By.CssSelector("a[class='sidebar-router-a'] i[class='sprite sprite-side_documents-icon sidebar-router-i']");
        private readonly By ChronoOpenTradeMenuItemExp = By.CssSelector("li[class*='chrono-open-trades-tab']");
        private readonly By ChronoCloseTradeMenuItemExp = By.CssSelector("li[class*='chrono-closed-trades-tab']");
        private readonly By PlatformLeftMenuExp = By.CssSelector("app-sidebar[style='visibility: visible']");
        #endregion

        #region Crm
        private readonly By NavigationMenuExp = By.CssSelector("minotaur-nav[id='navigation']");
        private readonly By NavigationMenuIsCloseExp = By.CssSelector("minotaur-nav[class*='inserted navigation-sm']");
        private readonly By CrmMenuItemExp = By.CssSelector("a[class*='crm-menu-item']");
        private readonly By QAndAMenuItemExp = By.CssSelector("span[class*='qna-menu-item']");
        private readonly By AccountsMenuItemExp = By.CssSelector("span[class*='accounts-menu-item']");
        private readonly By UsersMenuItemExp = By.CssSelector("a[href='/accounts/users']");
        private readonly By RolesMenuItemExp = By.CssSelector("span[class*='roles-menu-item']");
        private readonly By ClientsMenuItemExp = By.CssSelector("span[class*='clients-menu-item']");
        private readonly By SalesMenuItemExp = By.CssSelector("a[uisref='crm.sales']");
        private readonly By PlanningMenuItemExp = By.CssSelector("a[uisref='crm.planning']");
        private readonly By DashboardMenuItemExp = By.CssSelector("a[uisref*='dashboard']");
        private readonly By CampaignsMenuItemExp = By.CssSelector("span[class*='campaigns-menu-item']");
        private readonly By CampaignsDashboardMenuItemExp = By.CssSelector("span[class*='campaigns-dashboard-menu-item']");
        private readonly By HamburgerMenuExp = By.CssSelector("button[class*='navigation-toggle']");
        private readonly By MenuSectionOpenExp = By.CssSelector("minotaur-nav[id = 'navigation'][class='ng-star-inserted']");
        private readonly By ActiveClientsMenuItemExp = By.CssSelector("li[uisrefactive][class*='open active'] a[uisref*='clients']");
        #endregion

        #region General
        private readonly By CrmSettingsMenuItemExp = By.CssSelector("i[class='fa fa-cogs']");
        private readonly By CrmSecurityMenuItemExp = By.CssSelector("a[id='security-link']");
        #endregion       

        #endregion Locator's

        public T ClickOnMenuItem<T>(string menuItemName = null) where T : class
        {
            //_clickOnHumborger = clickOnHumborger; 


            if (menuItemName != null)
            {
                GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name.Contains(menuItemName,
                StringComparison.OrdinalIgnoreCase))?.Invoke(this, Array.Empty<T>());
            }
            else
            {
                var menuItem = GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name.Contains(typeof(T).Name.Substring(1)));

                if (menuItem != null)
                {
                    menuItem.Invoke(this, Array.Empty<T>());
                }
                else
                {
                    var exceMessage = ($" no method found for menu item: {menuItemName}");

                    throw new NullReferenceException(exceMessage);
                }
            }

            return ChangeContext<T>(_driver);
        }

        private void ClickOnHamburgerMenuToOpen()
        {
            var element = _driver.SearchElement(HamburgerMenuExp);
            element.ForceClick(_driver, HamburgerMenuExp);
            _driver.WaitForAnimationToLoad(700);
            var close = _driver.SearchElements(NavigationMenuIsCloseExp).Count;

            for (var i = 0; i < 5; i++)
            {
                if (close == 1)
                {
                    element = _driver.SearchElement(HamburgerMenuExp);
                    element.ForceClick(_driver, HamburgerMenuExp);
                    element.WaitElementToStopMoving(_driver, NavigationMenuExp);
                    _driver.WaitForAnimationToLoad(700);

                    continue;
                }

                break;
            }
            //_driver.SearchElement(NavigationMenuIsCloseExp);
        }

        private void ClickOnHamburgerMenuToClose()
        {
            _driver.SearchElement(HamburgerMenuExp)
                .ForceClick(_driver, HamburgerMenuExp);

            //_driver.WaitForAnimationToLoad(700);
            _driver.SearchElement(NavigationMenuIsCloseExp);
        }

        private void VerifyMenuGroupIsOpen()
        {
            _driver.SearchElement(MenuSectionOpenExp);
        }

        private IMenus ClickOnBroadcastPopap()
        {
            var popUp = _driver.SearchElements(_broadCastPopapBtnExp, 3).Count;

            if (popUp == 1)
            {
                _driver.SearchElement(_broadCastPopapBtnExp)
                    .ClickAndWaitForElementNotExist(_driver);
            }

            return this;
        }

        public void CheckIfPlatformLeftMenuExist()
        {
            try
            {
                _driver.SearchElement(PlatformLeftMenuExp, 10);
            }
            catch
            {
                _driver.Navigate().Refresh();
                _driver.WaitForPageLoad("trade");
            }
        }

        private IMenus ClientsPageUiMenuItem()
        {
            //Thread.Sleep(3000);

            //_apiFactory
            //    .ChangeContext<ISharedStepsGenerator>(_driver)
            //    .NavigateToPageByName(_crmUrl, "/crm/clients");

            //_apiFactory
            //   .ChangeContext<ISharedStepsGenerator>(_driver)
            //   .WaitForTableToLoad();

            //Thread.Sleep(3000); // wait for page after navigate
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CrmMenuItemExp)
                .ForceClick(_driver, CrmMenuItemExp);

            VerifyMenuGroupIsOpen();

            //for (var i = 0; i < 5; i++)
            //{
            _driver.SearchElement(ClientsMenuItemExp)
                .ForceClick(_driver, ClientsMenuItemExp);

            //    var numOfElements = _driver.SearchElements(ActiveClientsMenuItemExp).Count();

            //    if (numOfElements == 0)
            //    {
            //        _driver.SearchElement(ClientsMenuItemExp)
            //            .ForceClick(_driver, ClientsMenuItemExp);

            //        numOfElements = _driver.SearchElements(ActiveClientsMenuItemExp).Count();
            //        Thread.Sleep(200);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            //ClickOnHamburgerMenuToClose(); // to close the menu

            return this;
        }

        private IMenus MarketExposurePageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
                .ForceClick(_driver, CfdMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(MarketExposureExp)
                .ForceClick(_driver, MarketExposureExp);

            return this;
        }

        private IMenus BulkTradePageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
                .ForceClick(_driver, CfdMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(BulkTradingExp)
                .ForceClick(_driver, BulkTradingExp);

            return this;
        }

        private IMenus SalesPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CrmMenuItemExp)
                .ForceClick(_driver, CrmMenuItemExp);

            VerifyMenuGroupIsOpen();

            _driver.SearchElement(SalesMenuItemExp)
                .ForceClick(_driver, SalesMenuItemExp);

            return this;
        }

        private IMenus DepositsPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CrmMenuItemExp, 10)
                .ForceClick(_driver, CrmMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(BankingMenuItemExp)
                .ForceClick(_driver, BankingMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(DepositsMenuItemExp)
               .ForceClick(_driver, DepositsMenuItemExp);

            ClickOnHamburgerMenuToClose();

            return this;
        }

        private IMenus UsersPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(AccountsMenuItemExp)
                .ForceClick(_driver, AccountsMenuItemExp);

            VerifyMenuGroupIsOpen();

            _driver.SearchElement(UsersMenuItemExp)
                .ForceClick(_driver, UsersMenuItemExp);

            ClickOnHamburgerMenuToClose();

            return this;
        }

        private IMenus PlanningPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CrmMenuItemExp, 10)
             .ForceClick(_driver, CrmMenuItemExp);

            _driver.SearchElement(PlanningMenuItemExp)
                .ForceClick(_driver, PlanningMenuItemExp);

            return this;
        }

        private IMenus QAndAPageUiMenuItem()
        {
            //if (_clickOnHumborger)
            //{
            ClickOnHamburgerMenuToOpen();
            //}

            _driver.SearchElement(QAndAMenuItemExp)
                .ForceClick(_driver, QAndAMenuItemExp);

            return this;
        }

        private IMenus CrmSettingsMenuItem()
        {
            _driver.SearchElement(CrmSettingsMenuItemExp)
                .ForceClick(_driver, CrmSettingsMenuItemExp);

            return this;
        }

        private IMenus OfficesTabUiMenuItem()
        {
            _driver.SearchElement(CrmSettingsMenuItemExp)
                .ForceClick(_driver, CrmSettingsMenuItemExp);

            return this;
        }

        private IMenus SecurityTabUiMenuItem()
        {
            _driver.SearchElement(CrmSecurityMenuItemExp)
                .ForceClick(_driver, CrmSecurityMenuItemExp);

            return this;
        }

        private IMenus RolesPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(AccountsMenuItemExp)
                .ForceClick(_driver, AccountsMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(RolesMenuItemExp)
                .ForceClick(_driver, RolesMenuItemExp);

            return this;
        }

        private IMenus CampaignsMapPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CampaignsMenuItemExp)
               .ForceClick(_driver, CampaignsMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(CampaignsMapPageUiMenuItemExp)
                .ForceClick(_driver, CampaignsMapPageUiMenuItemExp);

            return this;
        }

        private IMenus CampaignsPageUiMenuItem()
        {
            //if (_clickOnHumborger)
            //{
            ClickOnHamburgerMenuToOpen();
            //}

            _driver.SearchElement(CampaignsMenuItemExp)
               .ForceClick(_driver, CampaignsMenuItemExp);

            VerifyMenuGroupIsOpen();

            _driver.SearchElement(CampaignsDashboardMenuItemExp)
                .ForceClick(_driver, CampaignsDashboardMenuItemExp);

            ClickOnHamburgerMenuToClose();

            return this;
        }

        private IMenus DashboardPageUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(DashboardMenuItemExp)
                .ForceClick(_driver, DashboardMenuItemExp);

            ClickOnHamburgerMenuToClose();

            return this;
        }

        private IMenus TradeMenuItem()
        {
            _driver.SearchElement(TradeMenuItemExp)
                .ForceClick(_driver, TradeMenuItemExp);

            return this;
        }

        private IMenus LiveTvMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(LiveTvMenuItemExp)
                .ForceClick(_driver, LiveTvMenuItemExp);

            return this;
        }

        private IMenus TradingOpenTradesMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(OpenTradeMenuItemExp)
                .ForceClick(_driver, OpenTradeMenuItemExp);

            return this;
        }

        private IMenus TradingPendingTradeMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(PendingOrdersMenuItemExp)
             .ForceClick(_driver, PendingOrdersMenuItemExp);

            return this;
        }

        private IMenus FavoritesMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(FavoritesMenuItemExp)
                .ForceClick(_driver, FavoritesMenuItemExp);

            return this;
        }

        private IMenus TradingCloseTradeMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(ClosedTradesMenuItemExp)
                .ForceClick(_driver, ClosedTradesMenuItemExp);

            return this;
        }

        private IMenus MyProfileMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(MyProfileMenuItemExp)
                .ForceClick(_driver, MyProfileMenuItemExp);

            return this;
        }

        private IMenus DocumentsMenuItem()
        {
            CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(DocumentMenuItemExp)
                .ForceClick(_driver, DocumentMenuItemExp);

            return this;
        }

        private IMenus ChronoCloseAllOpenAndCloseTradeMenuItem()
        {
            var elements = _driver.FindElements(CloseOpenAndCloseTradeMenuItemExp);

            if (elements.Count > 0)
            {
                _driver.SearchElement(CloseOpenAndCloseTradeMenuItemExp);

                foreach (var element in elements)
                {
                    element.ForceClick(_driver, CloseOpenAndCloseTradeMenuItemExp);
                }
            }

            return this;
        }

        private IMenus ChronoPageUiMenuItem()
        {
            _driver.SearchElement(ChronoPlatformMenuItemExp)
                .ForceClick(_driver, ChronoPlatformMenuItemExp);

            _driver.WaitForAnimationToLoad(400); // dont delete for the search asset

            return this;
        }

        private IMenus NftPageUiMenuItem()
        {
            _driver.SearchElement(NftPlatformMenuItemExp)
                      .ForceClick(_driver, NftPlatformMenuItemExp);

            _driver.WaitForElementNotExist(NftLoaderExp);

            return this;
        }

        private IMenus ChronoOpenTradesMenuItem()
        {
            ChronoCloseAllOpenAndCloseTradeMenuItem();
            //_driver.WaitForAnimationToLoad(600);

            _driver.SearchElement(ChronoOpenTradeMenuItemExp)
                .ForceClick(_driver, ChronoOpenTradeMenuItemExp);

            return this;
        }

        private IMenus ChronoCloseTradesMenuItem()
        {
            ChronoCloseAllOpenAndCloseTradeMenuItem();
            //_driver.WaitForAnimationToLoad();

            _driver.SearchElement(ChronoCloseTradeMenuItemExp)
                .ForceClick(_driver, ChronoCloseTradeMenuItemExp);

            return this;
        }

        private IMenus CrmOpenTradesMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
                .ForceClick(_driver, CfdMenuItemExp);

            _driver.SearchElement(OpenTratedsMenuItemExp)
                .ForceClick(_driver, OpenTratedsMenuItemExp);

            ClickOnHamburgerMenuToClose();

            return this;
        }

        private IMenus ForexRiskMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
                .ForceClick(_driver, CfdMenuItemExp);

            VerifyMenuGroupIsOpen();

            _driver.SearchElement(RiskMenuItemExp)
                .ForceClick(_driver, RiskMenuItemExp);

            return this;
        }

        private IMenus CrmPendingTradeMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
            .ForceClick(_driver, CfdMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(pendingTratedsMenuItemExp)
            .ForceClick(_driver, pendingTratedsMenuItemExp);

            return this;
        }

        private IMenus CrmCloseTradeMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
              .ForceClick(_driver, CfdMenuItemExp);

            VerifyMenuGroupIsOpen();

            _driver.SearchElement(CloseTratedsMenuItemExp)
            .ForceClick(_driver, CloseTratedsMenuItemExp);

            return this;
        }

        private IMenus TradeGroupsUiMenuItem()
        {
            ClickOnHamburgerMenuToOpen();

            _driver.SearchElement(CfdMenuItemExp)
              .ForceClick(_driver, CfdMenuItemExp);

            //VerifyMenuGroupIsOpen();

            _driver.SearchElement(CfdGroupsMenuItemExp)
                .ForceClick(_driver, CfdGroupsMenuItemExp);

            return this;
        }

        #region Treade
        private IMenus HistoryPageUiMenuItem()
        {
            CheckIfPlatformLeftMenuExist();
            _driver.ClickAndWaitForNextElement(TradingSettingsMenuItemExp, TradingSettingsMenuExp);

            _driver.SearchElement(HistoryMenuItemExp)
                .ForceClick(_driver, HistoryMenuItemExp);

            return this;
        }
        #endregion
        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
