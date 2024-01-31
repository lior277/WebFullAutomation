using AirSoftAutomationFramework.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.UsersPage
{
   public interface IGridSearchResultUsersUi : IApplicationFactory
    {
        IEditUserUi ClickOnEditUserButton();
    }
}
