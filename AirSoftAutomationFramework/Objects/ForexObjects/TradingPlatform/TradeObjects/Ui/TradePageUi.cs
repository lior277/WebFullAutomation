// Ignore Spelling: Forex api Cloed Favorit Rgb Mounth

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AventStack.ExtentReports.Utils;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public class TradePageUi : ITradePageUi
    {
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _favoriteBtnOnAsset = "//tr[contains(.,'{0}')]//i[contains(.,'Favorites')]";
        private string _assetInAutoComplateExp = "//ul[contains(.,'{0}')]";
        private string _assetsListInFavoritesMenu = "//div[contains(@class,'row favorites-body')]" +
            "/div[contains(.,'{0}')]";

        private string _favoritesAssets = "//td[contains(@class,'asset_name')" +
            " and contains(.,'{0}')]";

        private string _assetsIsOpen = "//span[@class='highcharts-title'and contains(.,'{0}')]";
        private string _AssetName = "//tr//span[text()='{0}']";
        
        public TradePageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's             
        private readonly By ProfileInfoExp = By.CssSelector("span[class*='profile_info']");
        private readonly By CloseDealExp = By.CssSelector("button[class*='close-deal-button close-deal']");
        private readonly By CloseDealForCloedAssetExp = By.CssSelector("span[class*='close-deal-button close-deal-button']");
        private readonly By LiveTvExp = By.CssSelector("iframe[src*='https://www']");
        private readonly By AmountErrorMessageExp = By.XPath("//label[contains(@class,'amount_title')]/parent::div//child::span");
        private readonly By CardButtonExp = By.CssSelector("div[class*='card_buttons card_buttons_close not-graphs-view asset-info-animated-in']");
        private readonly By BuyButtonExp = By.CssSelector("div[class*='card_buttons'] span[class*='button_buy']");
        private readonly By SellButtonExp = By.CssSelector("div[class*='card_buttons'] span[class*='button_sell']");
        private readonly By PendingForBuyAndSellButtonExp = By.CssSelector("div[class*='card_buttons'] span[class='button_title']");
        private readonly By OpenAmountTextBoxExp = By.CssSelector("input[id='amount']");
        private readonly By SomeAssetIsOpenExp = By.CssSelector("div[class='dropdown menuDuration']");
        private readonly By SearchTradeExp = By.CssSelector("input[name='search-trade']");
        private readonly By CloseAmountTextBoxExp = By.CssSelector("input[name='amount']");
        private readonly By CurrentRateExp = By.CssSelector("span[class='card_rate high_value rate-info']");
        private readonly By AdvancedOrderButtonExp = By.CssSelector("div[class='advanced_order'] a");
        private readonly By AdvancedOrderMenuOpenExp = By.CssSelector("i[class='fa fa-angle-up']");
        private readonly By AdvancedItemMenuOpenExp = By.CssSelector("i[class='sprite sprite-close_icon']");
        private readonly By OnlyBuyWhenRateIsButtonExp = By.CssSelector("div[class*='only-buy-When-rate-is'] span[class='advanced-down-symbol']");
        private readonly By CloseAtLossWhenRateIsButtonExp = By.XPath("//span[contains(.,'Close at loss when rate is')]/preceding-sibling::span[@class='advanced-down-symbol']");
        private readonly By CloseAtProfitWhenRateIsButtonExp = By.XPath("//span[contains(.,'Close at profit when rate is')]/preceding-sibling::span[@class='advanced-down-symbol']");
        private readonly By PendingRateExp = By.CssSelector("input[name='pendingRate']");
        private readonly By TakeProfitExp = By.CssSelector("input[name='tp']");
        private readonly By StopLossExp = By.CssSelector("input[name='sl']");
        private readonly By AssetInAutoComplateExp = By.XPath("//ul[contains(.,'ETH')]");
        private readonly By CloseTradeButtonExp = By.CssSelector("div[class='buttons_tpsl'] button[class*='btn close-button custom-btn button-padding']");
        private readonly By NumOfPendingTradesExp = By.CssSelector("li[class*='pending-orders'] span[class*='trades-amount']");
        private readonly By NumOfOpenTradesExp = By.CssSelector("li[class*='open-trades-menu-item'] span[class*='trades-amount']");
        private readonly By NumOfCloseTradesExp = By.CssSelector("li[class*='closed-trades'] span[class*='trades-amount']");
        private readonly By OpenTradeCardAssetInfoExp = By.CssSelector("div[class*='asset-info asset-info-animated-in']");
        private readonly By ShowBuyCardExp = By.CssSelector("div[class*='show buy_card']");
        private readonly By SearchAssetExp = By.CssSelector("span[class='search_asset'] input");
        private readonly By TopMenuExp = By.CssSelector("div[class='right_top_menu']");
        //private readonly By ChooseAssetFromAutoComplateExp = By.XPath("ul[class*='sub-categories-ul-trade'][class*='show']");
        private readonly By OpenCardForAnimationExp = By.CssSelector("div[class='open_card_wrapper ng-star-inserted toggle-open-card']");
        private readonly By CardButtonsForAnimationExp = DataRep.CardButtonsForAnimationExp;
        private readonly By SpriteActionIconExp = By.CssSelector("i[class='sprite sprite-action_icon1 ng-star-inserted']");
        private readonly By DepositBoxExp = By.CssSelector("div[class*='deposit_box']");
        private readonly By GraphExp = By.CssSelector("div[class*='highcharts-container']");
        private readonly By AssetStatusExp = By.CssSelector("span[class*='trading-closed']");
        private readonly By TotalInvestmentExp = By.CssSelector("div[class*='profile_summary_total']");
        private readonly By WinPrecentageValueExp = By.CssSelector("span[class='profile_summary_value']");
        private readonly By DocumentTitlesExp = By.CssSelector("div[class*='document-div']");
        private readonly By DocumentTitleStatusExp = By.CssSelector("div[class*='document-div'] span");
        private readonly By DocumentPreviewExp = By.CssSelector("div[id='preview-document-modal']");
        private readonly By DocumentsListExp = By.CssSelector("div[class*='document-div']");
        private readonly By SidebarExp = By.CssSelector("app-sidebar[style='visibility: visible']");
        private readonly By TotalTradesMyProfileExp = By.CssSelector("span[class='profile_trading_value']");
        private readonly By ShortTradesMyProfileExp = By.CssSelector("span[class='profile_trading_short_value']");
        private readonly By LongTradesMyProfileExp = By.CssSelector("span[class='profile_trading_long_value']");
        private readonly By TotalPnlValueMyProfileExp = By.CssSelector("span[class='profile_trading_amount']");
        private readonly By AccountRoiMyProfileExp = By.CssSelector("span[class='profile_banking_year_value']");
        private readonly By AvMounthRoiMyProfileExp = By.CssSelector("span[class='profile_banking_month_value']");
        private readonly By SettingsContentExp = By.CssSelector("div[class='settings_content_wrapper']");
        private readonly By MarketBtnExp = By.CssSelector("label[class='btn button_market_title']");
        private readonly By MarketBtnActiveExp = By.CssSelector("label[class='btn button_market_title active']");
        private readonly By LimitBtnExp = By.CssSelector("label[class='btn button_limit_title ng-star-inserted']");
        private readonly By LimitBtnActiveExp = By.CssSelector("label[class='btn button_limit_title ng-star-inserted active']");
        private readonly By SetEntryPointExp = By.CssSelector("input[id='entry-price']");
        private readonly By SetTemplateBtnExp = By.CssSelector("div[class='toggle']");
        private readonly By FavoritesBtnOnTopMenuExp = By.CssSelector("li[class*='favorites-list-li']");
        private readonly By BannerExp = By.CssSelector("mark[class='marquee-style']");
        private readonly By ChatIconExp = By.CssSelector("span[class*='large-chat-icon']");
        private readonly By ChatWindowExp = By.CssSelector("div[class*='chat-window']");
        private readonly By WaitForTradeTablesExp = By.CssSelector("app-trade-table[style='visibility: visible;']");
        private readonly By WaitForMyProfileExp = By.CssSelector("app-myprofile[style='visibility: visible']");
        #endregion Locator's

        public string GetCurrentRate()
        {
            return _driver.SearchElement(CurrentRateExp)
                .GetElementText(_driver);
        }

        public string GetAmountErrorMessage()
        {
            return _driver.SearchElement(AmountErrorMessageExp)
                .GetElementText(_driver);
        }

        public ITradePageUi ClickOnCloseDealButton()
        {
           // _driver.WaitForAnimationToLoad(2000);

            _driver.SearchElement(CloseDealExp)
                .ForceClick(_driver, CloseDealExp);

            return this;
        }

        public ISearchResultsUi SearchTrade(string searchValue)
        {
            _driver.SearchElement(SearchTradeExp)
                .SendsKeysAuto(_driver, SearchTradeExp, searchValue);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ITradePageUi ClickOnCloseDealForCloedAssetButton()
        {
            //_driver.WaitForAnimationToLoad(1015);

            _driver.SearchElement(CloseDealForCloedAssetExp)
                .ForceClick(_driver, CloseDealForCloedAssetExp);

            return this;
        }

        public ITradePageUi ClickOnCloseDealWithErrorButton()
        {
            //_driver.WaitForAnimationToFinish(OpenCardForAnimationExp);
            //_driver.WaitForAnimationToLoad(1015);

            _driver.SearchElement(CloseDealExp)
                .ForceClick(_driver, CloseDealExp);

            return this;
        }

        public IProfilePageUi ClickOnClientFirstName()
        {
            _apiFactory
              .ChangeContext<IMenus>(_driver)
              .CheckIfPlatformLeftMenuExist();

            _driver.SearchElement(ProfileInfoExp);
            _driver.ClickAndWaitForNextElement(ProfileInfoExp, SettingsContentExp);

            return _apiFactory.ChangeContext<IProfilePageUi>(_driver);
        }

        public ITradePageUi ClickOnMarketBtn()
        {
            _driver.SearchElement(MarketBtnExp);
            _driver.ClickAndWaitForNextElement(MarketBtnExp, MarketBtnActiveExp);

            return this;
        }

        public ITradePageUi ClickOnChatBtn()
        {
            _driver.SearchElement(ChatIconExp);
            _driver.ClickAndWaitForNextElement(ChatIconExp, ChatWindowExp);

            return this;
        }

        public ITradePageUi VerifyChatBtnNotExist()
        {
            _driver.WaitForElementNotExist(ChatIconExp);

            return this;
        }

        public ITradePageUi ClickOnAssetFavoriteBtn(string assetName)
        {
            var favoriteBtnOnAssetExp = By.XPath(string.Format(_favoriteBtnOnAsset, assetName));

            _driver.SearchElement(favoriteBtnOnAssetExp)
                .ForceClick(_driver, favoriteBtnOnAssetExp);

            return this;
        }

        public ITradePageUi ClickOnFavoritesBtnOnTopMenu()
        {
            var element = _driver.SearchElement(FavoritesBtnOnTopMenuExp);
            _driver.ScrollDown(FavoritesBtnOnTopMenuExp);
            element.ForceClick(_driver, FavoritesBtnOnTopMenuExp);

            return this;
        }

        public ITradePageUi VerifyAssetOnFavoritesMenu(string assetName)
        {
            var assetsListInFavoritesMenuExp = By.XPath(string.Format(_assetsListInFavoritesMenu, assetName));

            _driver.SearchElement(assetsListInFavoritesMenuExp)
                .GetElementText(_driver, assetsListInFavoritesMenuExp)
                .StringContains(assetName);

            return this;
        }

        public ITradePageUi VerifyBanner(string bannerText)
        {
            var text = _driver.SearchElement(BannerExp)
               .GetElementText(_driver, BannerExp);

            for (var i = 0; i < 20; i++)
            {              
                if (text.IsNullOrEmpty())
                {
                    text = _driver.SearchElement(BannerExp)
                        .GetElementText(_driver, BannerExp);

                    Thread.Sleep(300); 

                    continue;
                }

                break;
            }
           
            text.StringContains(bannerText);

            return this;
        }

        public int VerifyFavoritAssetOnAssetsList(string assetName)
        {
            var favoritesAssetsExp = By.XPath(string.Format(_favoritesAssets, assetName));

            return _driver.WaitForExactNumberOfElements(favoritesAssetsExp, 1);
        }

        public ITradePageUi ClickOnSetTemplateBtn()
        {
            _driver.SearchElement(SetTemplateBtnExp)
                .ForceClick(_driver, SetTemplateBtnExp);

            return this;
        }

        public ITradePageUi ClickOnLimitBtn()
        {
            //_driver.WaitForAnimationToLoad(1015);

            _driver.SearchElement(LimitBtnExp)
                .ForceClick(_driver, LimitBtnExp);

            return this;
        }

        private int GetAmount()
        {
            return _driver.SearchElement(OpenAmountTextBoxExp)
                .GetElementText(_driver)
                .StringToInt();
        }

        public ITradePageUi SetAmount(int amount = 2)
        {
            //_driver.WaitForAnimationToLoad(1015);
            var element = _driver.SearchElement(OpenAmountTextBoxExp);
            element.SendsKeysAuto(_driver, OpenAmountTextBoxExp, amount.ToString());
            var textAfter = element.GetElementText(_driver);

            if (Convert.ToInt32(textAfter) != amount)
            {
                element.SendsKeysAuto(_driver, OpenAmountTextBoxExp, amount.ToString());
            }

            return this;
        }

        public ITradePageUi VerifyMessages(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            _driver.SearchElement(alertExp)
                .GetElementText(_driver, alertExp)
                .StringContains(message);

            return this;
        }

        public ITradePageUi VerifyNumOfPendingTrades(string numOfTrades = "1")
        {
            var actualNum = _driver.SearchElement(NumOfPendingTradesExp)
                 .GetElementText(_driver);

            actualNum.StringContains(numOfTrades);

            return this;
        }

        public ITradePageUi VerifyNumOfOpenTrades(string numOfTrades = "1")
        {
            var actualNum = _driver.SearchElement(NumOfOpenTradesExp)
                 .GetElementText(_driver);

            actualNum.StringContains(numOfTrades);

            return this;
        }

        public ITradePageUi VerifyNumOfCloseTrades(string numOfTrades = "1")
        {
            var actualNum = _driver.SearchElement(NumOfCloseTradesExp)
                 .GetElementText(_driver);

            actualNum.StringContains(numOfTrades);

            return this;
        }

        public ITradePageUi VerifyClientFirstName(string clientFirstName)
        {
            var actualName = _driver.SearchElement(DataRep.ProfileFirstNameExp)
                .GetElementText(_driver, DataRep.ProfileFirstNameExp);

            for (var i = 0; i < 10; i++)
            {
                if (actualName == string.Empty)
                {
                    Thread.Sleep(500);

                    actualName = _driver.SearchElement(DataRep.ProfileFirstNameExp)
                        .GetElementText(_driver, DataRep.ProfileFirstNameExp);

                    continue;
                }

                break;          
            }

            actualName.StringContains(clientFirstName);

            return this;
        }

        public ITradePageUi VerifyLiveTv()
        {
            _driver.SwitchTo().Frame(_driver.SearchElement(LiveTvExp));

            return this;
        }

        public string GetClientFirstName()
        {
            return _driver.SearchElement(DataRep.ProfileFirstNameExp)
               .GetElementText(_driver, DataRep.ProfileFirstNameExp);
        }

        public ITradePageUi SetTakeProfitRate(string Rate)
        {
            _driver.SearchElement(TakeProfitExp)
                .SendsKeysAuto(_driver, TakeProfitExp, Rate);

            return this;
        }

        public ITradePageUi SetStopLossRate(string Rate)
        {
            _driver.SearchElement(StopLossExp)
                .SendsKeysAuto(_driver, StopLossExp, Rate);

            return this;
        }

        public ITradePageUi ClickOnBuyButton()
        {
            _driver.WaitForAnimationToFinish(OpenTradeCardAssetInfoExp);

            _driver.SearchElement(BuyButtonExp)
                .ForceClick(_driver, BuyButtonExp);

            return this;
        }

        public ITradePageUi ClickOnSellButton()
        {
            _driver.WaitForAnimationToFinish(OpenTradeCardAssetInfoExp);

            //_driver.WaitForAnimationToLoad(2000);

            _driver.SearchElement(SellButtonExp);
            //.ForceClick(_driver, SellButtonExp);

            _driver.RetryClickTillElementNotDisplay(SellButtonExp);

            return this;
        }

        public ITradePageUi SetEntryPriceFiled(string Rate)
        {
            _driver.SearchElement(SetEntryPointExp)
                .SendsKeysAuto(_driver, SetEntryPointExp, Rate);

            return this;
        }

        public ITradePageUi WaitForTradeTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public ITradePageUi WaitForMyProfileToLoad()
        {
            _driver.SearchElement(WaitForMyProfileExp);

            return this;
        }

        public ITradePageUi ClickCloseTradeButton()
        {
            //_driver.WaitForAnimationToLoad(800);

            _driver.SearchElement(CloseTradeButtonExp)
                .ForceClick(_driver, CloseTradeButtonExp);
            //.RetryClickTillElementNotDisplayJavaScript(_driver, CloseTradeButtonExp);

            return this;
        }

        public ITradePageUi ClickToOpenAsset(string assetName)
        {
            var assetNameExp = By.XPath(string.Format(_AssetName, assetName));
            var assetsIsOpenExp = By.XPath(string.Format(_assetsIsOpen, assetName));

            SearchAssetPipe(assetName);
            // check if asset is open
            var isAssetOpen = _driver.SearchElements(assetsIsOpenExp, 60).Count == 0; 

            if (isAssetOpen)
            {
                _driver.SearchElement(assetNameExp)
                    .ForceClick(_driver, assetNameExp);
            } 
            
            //.RetryClickTillElementNotDisplayJavaScript(_driver, CloseTradeButtonExp);

            return this;
        }

        public ITradePageUi ClickCloseAtProfitWhenRateIsButton()
        {
            _driver.SearchElement(CloseAtProfitWhenRateIsButtonExp)
                .ForceClick(_driver, CloseAtProfitWhenRateIsButtonExp);
            //.RetryClickTillElementNotDisplayJavaScript(_driver, CloseAtProfitWhenRateIsButtonExp);

            return this;
        }

        public ITradePageUi ClickCloseAtLossWhenRateIsButton()
        {
            _driver.SearchElement(CloseAtLossWhenRateIsButtonExp)
                .ForceClick(_driver, CloseAtLossWhenRateIsButtonExp);
            //.RetryClickTillElementNotDisplayJavaScript(_driver, CloseAtLossWhenRateIsButtonExp);

            return this;
        }

        public ITradePageUi SearchAssetPipe(string assetName)
        {
            var assetsIsOpenExp = By.XPath(string.Format(_assetsIsOpen, assetName));
            var autoComplateIsOpenExp = By.XPath(string.Format(_assetInAutoComplateExp, assetName));

            // check if some asset is open
            _driver.SearchElement(SomeAssetIsOpenExp);
            var element = _driver.SearchElement(SearchAssetExp);
            element.ForceClick(_driver, SearchAssetExp);
            element.SendsKeysCharByChar(_driver, SearchAssetExp, assetName);
            Thread.Sleep(200);

            _driver.SearchElement(autoComplateIsOpenExp)
                .ForceClick(_driver, autoComplateIsOpenExp);

            var elements = _driver.SearchElements(assetsIsOpenExp).Count;

            for (var i = 0; i < 3; i++)
            {
                if (elements == 0)
                {
                    element = _driver.SearchElement(SearchAssetExp);
                    element.ForceClick(_driver, SearchAssetExp);
                    element.SendsKeysCharByChar(_driver, SearchAssetExp, assetName);
                    Thread.Sleep(200);

                    _driver.SearchElement(autoComplateIsOpenExp)
                        .ForceClick(_driver, autoComplateIsOpenExp);

                    elements = _driver.SearchElements(assetsIsOpenExp).Count;

                    continue;
                }

                break;
            }

            // check if asset is open
            _driver.SearchElement(assetsIsOpenExp);
           
            return this;
        }

        public void SearchGraph()
        {
            _driver.SearchElement(GraphExp, 1, log: "search for graph");
        }

        public string GetAssetStatus()
        {
            return _driver.SearchElement(AssetStatusExp)
                .GetElementText(_driver);
        }

        public string GetShortTradesRgbColorValue()
        {
            return _driver.SearchElement(ShortTradesMyProfileExp).GetCssValue("color");
        }

        public string GetLongTradesRgbColorValue()
        {
            return _driver.SearchElement(LongTradesMyProfileExp).GetCssValue("color");
        }

        public int GetTotalInvestmentValue()
        {
            var element = _driver.SearchElement(TotalInvestmentExp);
            var elementText = " ";

            for (var i = 0; i < 3; i++)
            {
                element = _driver.SearchElement(TotalInvestmentExp);
                elementText = element.GetElementText(_driver);

                if (elementText == "" || elementText == null)
                {
                    Thread.Sleep(200);
                    continue;
                }

                break;
            }
            var newText = elementText.Split('$').Last().Replace(",", "");

            return Convert.ToInt32(newText);
        }

        public string GetWinPercentageValue()
        {
            return _driver.SearchElement(WinPrecentageValueExp)
                .GetElementText(_driver, WinPrecentageValueExp);
        }

        public string GetDocumentTitles(string documentName)
        {
            _driver.WaitForExactNumberOfElements(DocumentTitlesExp, 1);

            return _driver.SearchElements(DocumentTitlesExp)
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()
                .GetElementText(_driver);
        }

        public ITradePageUi ClickOnDocumentStatus(string documentName)
        {
            _driver.SearchElements(DocumentTitlesExp)
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()?
                .FindElement(DocumentTitleStatusExp)
                .ForceClick(_driver, DocumentTitleStatusExp);

            return this;
        }

        public ITradePageUi VerifyNewDocumentStatus(string documentName,
            string newStatus)
        {
            _driver.WaitForExactNumberOfElements(DocumentTitlesExp, 1, fromSeconds: 50);

            var element = _driver.SearchElements(DocumentTitlesExp)
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()
                .WaitForElementTextToChange(_driver,
                DocumentTitleStatusExp, newStatus, 100);

            return this;
        }

        public ITradePageUi ClickOnDocumentPreview()
        {
            _driver.SearchElement(DocumentPreviewExp)
                .ForceClick(_driver, DocumentPreviewExp);

            return this;
        }

        public ITradePageUi SetSignature()
        {
            _driver.SearchElement(DataRep.SignaturePadExp)
              .DrawSignatureOnCanvas(_driver, DataRep.SignaturePadExp);

            return this;
        }

        public IHistoryPageUi ClickOnSaveSignatureForDodButton()
        {
            _driver.SearchElement(DataRep.SaveSignatureBtnExp)
              .ForceClick(_driver, DataRep.SaveSignatureBtnExp);

            return _apiFactory.ChangeContext<IHistoryPageUi>(_driver);
        }

        public ITradePageUi ClickOnSaveSignatureForDocumentButton()
        {
            _driver.SearchElement(DataRep.SaveSignatureBtnExp)
              .ForceClick(_driver, DataRep.SaveSignatureBtnExp);

            return _apiFactory.ChangeContext<ITradePageUi>(_driver);
        }

        public string GetTotalTradesValue()
        {
            return _driver.SearchElement(TotalTradesMyProfileExp)
                .GetElementText(_driver, TotalTradesMyProfileExp);
        }

        public string GetShortTradesValue()
        {
            return _driver.SearchElement(ShortTradesMyProfileExp)
                .GetElementText(_driver, ShortTradesMyProfileExp);
        }

        public string GetLongTradesValue()
        {
            return _driver.SearchElement(LongTradesMyProfileExp)
                .GetElementText(_driver, LongTradesMyProfileExp);
        }

        public int GetTotalPnlValue()
        {
            var elementText = " ";

            for (var i = 0; i < 3; i++)
            {
                var element = _driver.SearchElement(TotalPnlValueMyProfileExp);
                elementText = element.GetElementText(_driver);

                if (elementText == " " || elementText == null)
                {
                    Thread.Sleep(200);
                    continue;
                }

                break;
            }
            var newText = elementText.Split('$').Last();

            return Convert.ToInt32(newText);
        }

        public string GetTotalPnlCurrencySign()
        {
            return _driver.SearchElement(TotalPnlValueMyProfileExp)
                .GetElementText(_driver, TotalPnlValueMyProfileExp);
        }

        public string GetAccountRoi(string expectedText)
        {
           return _driver.SearchElement(AccountRoiMyProfileExp)
                .WaitForElementTextToChange(_driver, AccountRoiMyProfileExp, expectedText, 60);
        }

        public string AvMounthRoi(string expectedText)
        {
            return _driver.SearchElement(AccountRoiMyProfileExp)
                .WaitForElementTextToChange(_driver, AccountRoiMyProfileExp, expectedText, 60);
        }

        public ITradePageUi BuyAssetPipe(string assetName = DataRep.AssetNameShort,
            int amount = 2, bool verify = true)
        {
            var buyOrderActivatedMessage = string.Format(DataRep.BuyCfdOrderActivatedMessage,
                assetName, amount);

            SearchAssetPipe(assetName);
            ClickOnBuyButton();
            SetAmount(amount);
            ClickOnCloseDealButton();

            if (verify)
            {
                VerifyMessages(buyOrderActivatedMessage);
            }
           
            return this;
        }

        public ITradePageUi SellAssetPipe(string assetName = DataRep.AssetNameShort,
            int amount = 2)
        {
            var sellOrderActivatedMessage = string
                .Format(DataRep.SellOrderActivatedMessage, assetName, amount);

            SearchAssetPipe(assetName);
            ClickOnSellButton();
            SetAmount();
            ClickOnCloseDealButton();
            VerifyMessages(sellOrderActivatedMessage);
            VerifyNumOfOpenTrades();

            return this;
        }

        public ITradePageUi CreateConditionalTradePipe(string rate,
            string assetName = DataRep.AssetNameShort, int amount = 2, bool verify = true)
        {
            var pendingTradeOpenedMessage = string
                .Format(DataRep.PendingTradeOpenedMessage, assetName, amount);

            ClickToOpenAsset(assetName);
            ClickOnBuyButton();
            SetAmount(amount);
            ClickOnLimitBtn();
            SetEntryPriceFiled(rate);
            ClickOnCloseDealButton();

            if (verify)
            {
                VerifyMessages(pendingTradeOpenedMessage);
            }

            return this;
        }

        public ITradePageUi CreateTakeProfitTradePipe(string rate,
            string assetName = DataRep.AssetNameShort,
            int amount = 2, bool verify = true)
        {
            var pendingTradeOpenedMessage = string
                .Format(DataRep.PendingTradeOpenedMessage, assetName, amount);

            SearchAssetPipe(assetName);
            ClickOnBuyButton();
            SetAmount(amount);
            //ClickOnLimittBtn();
            SetTakeProfitRate(rate);
            ClickOnCloseDealButton();
           // Thread.Sleep(500); // wait for trade to open

            if (verify)
            {
                VerifyMessages(pendingTradeOpenedMessage);
            }

            return this;
        }

        public ITradePageUi CloseTradePipe()
        {
            ClickCloseTradeButton();
            VerifyMessages(DataRep.CloseTradeMessage);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}

