﻿

using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Internals.DAL.Logger
{
    public interface IWriteToFile: IApplicationFactory
    {
        void WriteText(string text);
    }
}