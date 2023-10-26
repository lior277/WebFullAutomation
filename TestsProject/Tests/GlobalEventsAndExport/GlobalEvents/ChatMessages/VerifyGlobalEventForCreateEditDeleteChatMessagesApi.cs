// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.SavingAccount
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteChatMessagesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _chatMessageId;
        private string _expectedChatType = "user_left_chat_sentence";

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create new chat message
            _apiFactory
               .ChangeContext<IChatTabApi>()
               .PostCreateChatMessageRequestPipe(_crmUrl,
               _expectedChatType, _currentUserApiKey);

            var chatMessageData = _apiFactory
                .ChangeContext<IChatTabApi>()
                .GetChatMessagesRequest(_crmUrl)
                .Where(p => p.type.Equals(_expectedChatType))
                .FirstOrDefault();

            // edit chat message
            var actualEditGroupWithDiferenteTitle = _apiFactory
                .ChangeContext<IChatTabApi>()
                .PutChatMessageRequest(_crmUrl,
                chatMessageData, _currentUserApiKey);

            _chatMessageId = chatMessageData._id;

            // delete chat message 
            _apiFactory
               .ChangeContext<IChatTabApi>()
               .DeleteChatMessageRequest(_crmUrl,
               chatMessageData._id, _currentUserApiKey);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                var chatMessageData = _apiFactory
                    .ChangeContext<IChatTabApi>()
                    .GetChatMessagesRequest(_crmUrl)
                    .Where(p => p.type.Equals(_expectedChatType))
                    .FirstOrDefault();

                if (chatMessageData != null)
                {
                    // delete chat message 
                    _apiFactory
                       .ChangeContext<IChatTabApi>()
                       .DeleteChatMessageRequest(_crmUrl,
                       chatMessageData._id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {               
                AfterTest();
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalEventForCreateEditDeleteChatMessagesApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_chat_messages" }, { "edit_chat_messages" }, { "delete_chat_messages" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;

            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();
            var actualMadeByUserIdList = new List<string>();
            var actualChatMessageIdList = new List<string>();
            var actualChatMessageTypeList = new List<string>();

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualChatMessageIdList.Add(p.chat_message_id));
            actualGlobals.ForEach(p => actualChatMessageTypeList.Add(p.chat_message_type));
          
            Assert.Multiple(() =>
            {
                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual user name : {actualMadeByList.ListToString()}" +
                    $" expected user name: {_userName}");

                Assert.True(actualMadeByUserIdList.All(p => p.Equals(expectedActionMadeByUserId)),
                    $" actual user name : {actualMadeByUserIdList.ListToString()}" +
                    $" expected user name: {expectedActionMadeByUserId}");

                Assert.True(actualChatMessageIdList[0].Equals(_chatMessageId) &&
                    actualChatMessageIdList[1].Equals(_chatMessageId),
                    $" actual Chat Message Id : {actualChatMessageIdList.ListToString()}" +
                    $" expected Chat Message Id: {_chatMessageId}");

                Assert.True(actualChatMessageTypeList.All(p => p.Equals(_expectedChatType)),
                    $" actual Chat Message type : {actualChatMessageTypeList.ListToString()}" +
                    $" expected Chat Message type: {_expectedChatType}");
            });
        }
    }
}