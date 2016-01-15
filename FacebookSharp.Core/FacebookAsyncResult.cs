namespace FacebookSharp
{
    using System;

    public class FacebookAsyncResult
    {
        public FacebookAsyncResult(string result, Exception exception)
        {
            Exception = exception;
            RawResponse = result;
        }

        public Exception Exception { get; private set; }

        public string RawResponse { get; private set; }

        public T GetResponseAs<T>()
        {
            return FacebookUtils.DeserializeObject<T>(RawResponse);
        }

        public bool IsSuccessful
        {
            get { return Exception == null; }
        }
    }

    public class FacebookAsyncResult<T>
        where T : class
    {
        public FacebookAsyncResult(string result, Exception exception)
        {
            Exception = exception;
            RawResponse = result;
        }

        public Exception Exception { get; private set; }

        public string RawResponse { get; private set; }

        private T _response;
        public T Response
        {
            get { return _response ?? (_response = GetResponseAs<T>()); }
        }

        public T GetResponseAs<T>()
        {
            return FacebookUtils.DeserializeObject<T>(RawResponse);
        }

        public bool IsSuccessful
        {
            get { return Exception == null; }
        }

        public static implicit operator FacebookAsyncResult(FacebookAsyncResult<T> TFacebookAsyncResult)
        {
            if (TFacebookAsyncResult == null)
                return null;
            
            return new FacebookAsyncResult(TFacebookAsyncResult.RawResponse, TFacebookAsyncResult.Exception);
        }

        public static explicit operator FacebookAsyncResult<T>(FacebookAsyncResult TFacebookAsyncResult)
        {
            if (TFacebookAsyncResult == null)
                return null;
            return new FacebookAsyncResult<T>(TFacebookAsyncResult.RawResponse, TFacebookAsyncResult.Exception);
        }
    }
}