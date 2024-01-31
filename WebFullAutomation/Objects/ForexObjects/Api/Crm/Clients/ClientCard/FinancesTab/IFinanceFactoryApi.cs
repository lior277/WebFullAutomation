using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab
{
    public interface IFinanceFactoryApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IFinanceFactoryApi FinanceFactoryByName(string clientId, GetLoginResponse loginData,
            string currentUserApiKey, TestCase testCase);
        Tuple<TestCase.ExpectedFinanceData, List<double>> GetFinanceData(string clientId = null);
        //List<FundsSnapshot> GetSnapshotTableIsData(string clientId = null);
    }
}