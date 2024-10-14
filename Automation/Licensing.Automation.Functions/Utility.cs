using Grpc.Core;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using Okta.Sdk.Client;
using PnP.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Licensing.Automation.Functions
{
    

    internal static class Utility
    {

        internal const string SPO_MANAGED_MEMBERS_GROUP = "Managed Members";
        internal const string SPO_LISTTITLE_DOCUMENTS = "Documents";
        internal const string SPO_FOLDER_UPLOAD = "Upload - to NHSE IP team";
        internal const string SPO_FOLDER_DOWNLOAD = "Download - from NHSE IP team";
        internal const string SPO_ROLE_GUEST_CONTRIBUTE = "Guest Contribute";
        internal const string SPO_ROLE_GUEST_READ = "Guest Read";
        internal const string SPO_OWNERGROUP_TOKEN = "owner";
        internal const string SPO_MEMBERGROUP_TOKEN = "member";
        internal const string SPO_VISITORGROUP_TOKEN = "visitor";
        internal const string SPO_ERRORSTRING_UNEXPECTED_GROUP_TOKEN = "Unexpected group token";
        internal const string SPO_ERRORSTRING_GROUP_NOT_FOUND = "Group not found";

        internal const string NOTIFY_TOKEN_FIRST_NAME = "firstname";
        internal const string NOTIFY_TOKEN_LAST_NAME = "lastname";
        internal const string NOTIFY_TOKEN_ATTACHMENT = "attachment";
        internal const string NOTIFY_ERRORSTRING_NO_EMAIL_OR_ID = "No EmailAddress or OktaUserId was provided";

        internal const string OKTA_ERRORSTRING_NO_GROUPID_OR_NAME = "GroupId or Name not specified";
        internal const string OKTA_ERRORSTRING_NO_GROUPID = "GroupId not specified";
        internal const string OKTA_ERRORSTRING_NO_USERID = "UserId not specified";
        internal const string OKTA_ERRORSTRING_NO_APP = "Application not specified";
        internal const string OKTA_ERRORSTRING_INVALID_GROUP_TOKEN = "Invalid group token";
        internal const string OKTA_ERRORSTRING_MISSING_TOKEN_VALUE = "Token value not found in settings";
        internal const string OKTA_APP_LICENSING = "licensing";
        internal const string OKTA_APP_APPLICATION = "application";

        internal static HttpResponseData FormatFunctionAppResponse(HttpRequestData req, HttpStatusCode statusCode, string message)
        {
            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(new
            {
                StatusCode = (int)statusCode,
                Message = message
            }));

            return response;
        }

        internal static HttpResponseData FormatFunctionAppResponse(HttpRequestData req, Exception exception)
        {
            dynamic responseBody = new ExpandoObject();
            responseBody.ExceptionType = exception.GetType().ToString();
            responseBody.Message = exception.Message;

            if (exception is ApiException apiException)
            {
                // handle Okta ApiException
                responseBody.StatusCode = (HttpStatusCode)apiException.ErrorCode;

                dynamic? errorContent = JsonConvert.DeserializeObject((string)apiException.ErrorContent);
                if (errorContent != null)
                {
                    responseBody.Message = errorContent.errorSummary;
                }
 
                responseBody.ErrorContent = errorContent;
            }
            else if (exception is Microsoft.Graph.ServiceException serviceException)
            {
                // handle Graph Service Exception
                responseBody.StatusCode = (HttpStatusCode)serviceException.StatusCode;
                responseBody.Message = serviceException.Error.Message;
                responseBody.ErrorContent = serviceException.Error;
            }
            else if (exception is PnP.Core.SharePointRestServiceException sharePointRestServiceException)
            {
                // handle SharePoint Rest Service Exception
                if (sharePointRestServiceException.Error is SharePointRestError sharePointRestError)
                {
                    responseBody.StatusCode = sharePointRestError.HttpResponseCode;
                    responseBody.Message = sharePointRestError.Message;
                }
                else
                {
                    responseBody.StatusCode = HttpStatusCode.InternalServerError;
                }
                responseBody.ErrorContent = sharePointRestServiceException.Error;
            }
            else
            {
                // handle anything else
                responseBody.StatusCode = HttpStatusCode.InternalServerError;
                responseBody.ErrorContent = exception;
            }

            var response = req.CreateResponse((HttpStatusCode)responseBody.StatusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString((string)JsonConvert.SerializeObject(responseBody));
            return response;
        }
        internal static HttpResponseData CreateResponse(HttpRequestData req, Exception exception)
        {
            return FormatFunctionAppResponse(req, exception);
        }

        internal static HttpResponseData CreateResponse(HttpRequestData req, Object content)
        {
            try
            {
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(content));
                            
                return response;
            }
            catch(Exception ex)
            {
                return FormatFunctionAppResponse(req, ex);
            }
        }

        internal static HttpResponseData CreateResponse(HttpRequestData req)
        {
            return req.CreateResponse(HttpStatusCode.OK);
        }

        internal static HttpResponseData CreateResponse(HttpRequestData req, HttpStatusCode statusCode, string message)
        {
            return FormatFunctionAppResponse(req, statusCode, message);
        }

        internal static string UntokenizeOktaGroupId(AzureFunctionSettings settings, string groupId, bool tokensOnly = false)
        {
            switch (groupId.ToLower())
            {
                case Utility.OKTA_APP_APPLICATION:
                    if (settings.OktaApplicationGroupId == null)
                    {
                        throw new ArgumentException(Utility.OKTA_ERRORSTRING_MISSING_TOKEN_VALUE);
                    }
                    return settings.OktaApplicationGroupId;
                case Utility.OKTA_APP_LICENSING:
                    if (settings.OktaLicensingGroupId == null)
                    {
                        throw new ArgumentException(Utility.OKTA_ERRORSTRING_MISSING_TOKEN_VALUE);
                    }
                    return settings.OktaLicensingGroupId;
                default:
                    if(tokensOnly)
                    {
                        throw new ArgumentException(Utility.OKTA_ERRORSTRING_INVALID_GROUP_TOKEN);
                    }
                    else
                    {
                        return groupId;
                    }
            }
        }
    }
}
