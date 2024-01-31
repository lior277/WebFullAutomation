using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface IChatTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string DeleteChatMessageRequest(string url, string chatMessagesId, string apiKey = null, bool checkStatusCode = true);
        List<GetChatMessagesResponse> GetChatMessagesRequest(string url, string apiKey = null);
        string PatchEnableChatForUserRequest(string url, string userId, string apiKey = null, bool checkStatusCode = true);
        IChatTabApi PostCreateChatMessageRequest(string url, string chatType, bool active = true, string apiKey = null);
        IChatTabApi PostCreateChatMessageRequestPipe(string url, string chatType, string apiKey = null);
        string PutChatMessageRequest(string url, GetChatMessagesResponse getChatMessagesResponse, string apiKey = null, bool checkStatusCode = true);
    }
}