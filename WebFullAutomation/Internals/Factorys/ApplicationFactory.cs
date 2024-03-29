﻿using Autofac;
using OpenQA.Selenium;
using WebFullAutomation.Internals.Container;

namespace AirSoftAutomationFramework.Internals.Factorys
{
    public class ApplicationFactory : IApplicationFactory
    {
        public T ChangeContext<T>(IWebDriver driver = null) where T : class
        {
            //if(typeof(T).Name.ToLower().Contains("api"))
            var containerInit = new ContainerInitialized();
            var container = containerInit.ContainerConfigure(driver);

            return container.Resolve<T>();
        }
    }
}
