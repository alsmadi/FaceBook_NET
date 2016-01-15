
namespace FacebookSharp
{
    using System;
    using RestSharp;

    public partial class Facebook
    {

#if !SILVERLIGHT

        public static string ExchangeAccessTokenForCode(string code, string applicationKey, string applicationSecret, string postAuthorizeUrl, string userAgent, out long expiresIn)
        {
            if (string.IsNullOrEmpty(applicationKey))
                throw new ArgumentNullException("applicationKey");
            if (string.IsNullOrEmpty(applicationSecret))
                throw new ArgumentNullException("applicationSecret");
            if (string.IsNullOrEmpty(postAuthorizeUrl))
                throw new FacebookSharpException("postAuthorizeUrl");

            var client = new RestClient(GraphBaseUrl);

            var request = new RestRequest(Method.GET) { Resource = "oauth/access_token" };
            request.AddParameter("client_id", applicationKey);
            request.AddParameter("redirect_uri", postAuthorizeUrl);
            request.AddParameter("client_secret", applicationSecret);
            request.AddParameter("code", code);

            var response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {   // facebook gave us some result.
                string result = response.Content;

                // but mite had been an error
                var fbException = (FacebookException)result;
                if (fbException != null)
                    throw fbException;

                // the result comes like the querystring
                // this allows us to make use of ParseUrlQueryString(string query) method.
                var pars = FacebookUtils.ParseUrlQueryString(result);

                expiresIn = pars.ContainsKey("expires_in") ? Convert.ToInt64(pars["expires_in"][0]) : 0;

                return pars["access_token"][0];
            }

            throw new FacebookRequestException(response);
        }

#endif

#if SILVERLIGHT

        public static string ExchangeAccessTokenForCode(string code, string applicationKey, string applicationSecret, string postAuthorizeUrl, string userAgent, out long expiresIn)
        {
            throw new NotImplementedException();
        }
#endif

        public string ExchangeAccessTokenForCode(string code, out long expiresIn)
        {
            if (string.IsNullOrEmpty(Settings.ApplicationKey))
                throw new ArgumentNullException("Settings.ApplicationKey missing.");
            if (string.IsNullOrEmpty(Settings.ApplicationSecret))
                throw new ArgumentNullException("Settings.ApplicationSecret missing.");
            if (string.IsNullOrEmpty(Settings.PostAuthorizeUrl))
                throw new ArgumentNullException("Settings.PostAuthorizeUrl missing.");

            return ExchangeAccessTokenForCode(code, Settings.ApplicationKey, Settings.ApplicationSecret,
                                              Settings.PostAuthorizeUrl, DefaultUserAgent, out expiresIn);
        }
    }
}