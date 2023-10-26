using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface ICommentsTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICommentsTabApi DeleteCommentRequest(string uri, string commentId, string apiKey = null);
        string DeleteCommentRequest(string uri, string commentId, string apiKey, bool checkStatusCode = true);
        GeneralResult<List<GetCommentResponse>> GetCommentsByClientIdRequest(string uri,
             string clientId, string apiKey = null, bool checkStatusCode = true);
        List<GetCommentResponse> GetDeletedCommentsByClientIdRequest(string uri, string clientId, string apiKey);
        string PatchEditCommentRequest(string uri, string commentId, string comment, string apiKey = null, bool checkStatusCode = true);
        GeneralResult<List<CommentResponse>> PostCommentRequest(string uri, string clientId, string actualComment = "automation comment", string apiKey = null, bool checkStatusCode = true);
    }
}