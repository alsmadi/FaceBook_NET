
namespace FacebookSharp
{
    using System;
    using System.Collections.Generic;
    using RestSharp;

    internal class RestSharpContext<TMessage, TSyncResult, TAsyncResult>
    {
        private readonly Func<TMessage, RestRequest, RestClient> _prepareRestSharpClient;
        private readonly Func<TMessage, Method, RestRequest> _prepareRestSharpRequest;
        private readonly Func<TMessage, RestResponse, TSyncResult> _processSyncRestSharpResponse;
        private readonly Func<TMessage, RestResponse, TAsyncResult> _processAsyncRestSharpResponse;

        public RestSharpContext(
            Func<TMessage, RestRequest, RestClient> prepareRestSharpClient,
            Func<TMessage, Method, RestRequest> prepareRestSharpRequest,
            Func<TMessage, RestResponse, TSyncResult> processSyncRestSharpResponse,
            Func<TMessage, RestResponse, TAsyncResult> processAsyncRestSharpResponse)
        {
            _prepareRestSharpClient = prepareRestSharpClient;
            _prepareRestSharpRequest = prepareRestSharpRequest;
            _processSyncRestSharpResponse = processSyncRestSharpResponse;
            _processAsyncRestSharpResponse = processAsyncRestSharpResponse;
        }

#if !(SILVERLIGHT || WINDOWS_PHONE)  // NOR logic !(A+B) : if its running on desktop version

        #region Synchronous Helpers

        public RestResponse Execute(TMessage data, RestRequest request)
        {
            var client = _prepareRestSharpClient(data, request);
            return client.Execute(request);
        }

        public TSyncResult Execute(TMessage data, Method httpMethod)
        {
            var request = _prepareRestSharpRequest(data, httpMethod);

            var response = Execute(data, request);

            return _processSyncRestSharpResponse(data, response);
        }

        #endregion

#endif

        #region Asynchronous Helpers

        public void ExecuteAsync(TMessage data, RestRequest request, Action<RestResponse> callback)
        {
            var client = _prepareRestSharpClient(data, request);

            client.ExecuteAsync(
                request,
                response =>
                {
                    if (callback != null)
                        callback(response);
                });
        }

        public void ExecuteAsync(TMessage data, Method httpMethod, Action<TAsyncResult> callback)
        {
            var request = _prepareRestSharpRequest(data, httpMethod);

            ExecuteAsync(
                data,
                request,
                response =>
                {
                    var asyncResult = _processAsyncRestSharpResponse(data, response);
                    if (callback != null)
                        callback(asyncResult);
                });
        }

        #endregion
    }

    public partial class Facebook
    {
        internal class FacebookGraphRestSharpMessage
        {
            public FacebookGraphRestSharpMessage(Facebook fb)
            {
                Facebook = fb;

                BaseUrl = GraphBaseUrl;
                AddAccessToken = true;
            }

            public string Resource { get; set; }
            public string BaseUrl { get; set; }

            public Facebook Facebook { get; private set; }
            public bool AddAccessToken { get; set; }
            public IDictionary<string, string> Parameters { get; set; }

            public OAuth2Authenticator GetAuthenticator()
            {
                if (!string.IsNullOrEmpty(Facebook.Settings.AccessToken))
                {
                    // Note: OAuth2 Authorization Request Header is preferred.
                    // but for now since Facebook doesn't have clientaccesspolicy.xml file,
                    // we need to use the QueryString to pass oauth_token to Facebook.
                    // I have requested Facebook to add clientaccesspolicy.xml at
                    // http://bugs.developers.facebook.net/show_bug.cgi?id=12818
                    // Please vote up.
                    // If it is a silverlight or windows phone app, it uses querystring to 
                    // pass oauth_token, if it is a desktop app it uses Authorization header
#if (SILVERLIGHT || WINDOWS_PHONE)
                    return new OAuth2UriQueryParameterAuthenticator(Facebook.Settings.AccessToken);
#else
                    return new OAuth2AuthorizationRequestHeaderAuthenticator(Facebook.Settings.AccessToken);
#endif
                }

                return null;
            }
        }

        internal class FacebookApiRestSharpMessage : FacebookGraphRestSharpMessage
        {
            public FacebookApiRestSharpMessage(Facebook fb)
                : base(fb)
            {
                BaseUrl = ApiBaseUrl;
            }
        }

        private static RestSharpContext<FacebookGraphRestSharpMessage, string, FacebookAsyncResult> _graphContext;
        internal static RestSharpContext<FacebookGraphRestSharpMessage, string, FacebookAsyncResult> GraphContext
        {
            get
            {
                return _graphContext ??
                       (_graphContext =
                        new RestSharpContext<FacebookGraphRestSharpMessage, string, FacebookAsyncResult>(
                            PrepareRestSharpClient,
                            PrepareRestSharpRequest,
                            ProcessSyncRestSharpResponse,
                            ProcessAsyncRestSharpResponse));
            }
        }

        private static RestSharpContext<FacebookApiRestSharpMessage, string, FacebookAsyncResult> _restApiContext;
        internal static RestSharpContext<FacebookApiRestSharpMessage, string, FacebookAsyncResult> RestApiContext
        {
            get
            {
                return _restApiContext ??
                       (_restApiContext =
                        new RestSharpContext<FacebookApiRestSharpMessage, string, FacebookAsyncResult>(
                            PrepareRestSharpClient,
                            PrepareRestSharpRequest,
                            ProcessSyncRestSharpResponse,
                            ProcessAsyncRestSharpResponse));
            }
        }

        #region Helpers

        private static RestClient PrepareRestSharpClient(FacebookGraphRestSharpMessage message, RestRequest request)
        {
            var client = new RestClient(message.BaseUrl);

            client.UserAgent = message.Facebook.Settings.UserAgent;

            if (message.AddAccessToken)
                client.Authenticator = message.GetAuthenticator();

            return client;
        }

        private static RestRequest PrepareRestSharpRequest(FacebookGraphRestSharpMessage message, Method httpMethod)
        {
            var request = new RestRequest(message.Resource, httpMethod);

            if (message.Parameters != null)
            {
                foreach (var keyValuePair in message.Parameters)
                    request.AddParameter(keyValuePair.Key, keyValuePair.Value);
            }

            return request;
        }

        private static string ProcessSyncRestSharpResponse(FacebookGraphRestSharpMessage message, RestResponse response)
        {
            Exception exception;
            var result = ProcessRestSharpResponse(message, response, out exception);

            if (exception != null)
                throw exception;

            return result;
        }

        private static FacebookAsyncResult ProcessAsyncRestSharpResponse(FacebookGraphRestSharpMessage message, RestResponse response)
        {
            Exception exception;
            var result = ProcessRestSharpResponse(message, response, out exception);

            return new FacebookAsyncResult(result, exception);
        }

        private static string ProcessRestSharpResponse(FacebookGraphRestSharpMessage message, RestResponse response, out Exception exception)
        {
            string result = string.Empty;

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                exception = (FacebookException)response.Content;

                result = response.Content;
            }
            else
            {
                if (response.ErrorException is System.Security.SecurityException)
                    exception = new ClientAccessPolicyException();
                else
                {
                    // incase the net is not connected or some other exception
                    exception = new FacebookRequestException(response);
                }
            }

            return result;
        }

        #endregion

    }
}