namespace FacebookSharp.Extensions
{
    using System.Collections.Generic;
    using System.Text;

    public static partial class FacebookExtensions
    {
        /// <summary>
        /// Adds 'limit' parameter.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/api#paging
        /// 
        /// https://graph.facebook.com/me/likes?limit=3
        /// </remarks>
        public static IDictionary<string, string> LimitTo(this IDictionary<string, string> parameters, int limit)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("limit", limit.ToString());
            return parameters;
        }

        /// <summary>
        /// Adds 'offset' parameter.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/api#paging
        /// 
        /// https://graph.facebook.com/me/likes?offset=3
        /// </remarks>
        public static IDictionary<string, string> Offset(this IDictionary<string, string> parameters, int offset)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("offset", offset.ToString());
            return parameters;
        }

        /// <summary>
        /// Adds 'until' parameter.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="until">
        /// A unix timestamp or any date accepted by strtotime
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/api#paging
        /// 
        /// https://graph.facebook.com/search?until=yesterday
        /// 
        /// strtotime: http://php.net/manual/en/function.strtotime.php)
        /// </remarks>
        public static IDictionary<string, string> Until(this IDictionary<string, string> parameters, string until)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("until", until);
            return parameters;
        }

        /// <summary>
        /// Adds 'since' parameter.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="since">
        /// A unix timestamp or any date accepted by strtotime
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/api#paging
        /// 
        /// https://graph.facebook.com/search?since=yesterday
        /// 
        /// strtotime: http://php.net/manual/en/function.strtotime.php)
        /// </remarks>
        public static IDictionary<string, string> Since(this IDictionary<string, string> parameters, string since)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("since", since);
            return parameters;
        }

        /// <summary>
        /// Add fields parameter, if fields already exisits appends it.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="fieldNames">The field names.</param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, string> SelectFields(this IDictionary<string, string> parameters, string[] fieldNames)
        {
            return parameters.Select("fields", fieldNames);
        }

        /// <summary>
        /// Add fields parameter, if fields already exisits appends it.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, string> SelectField(this IDictionary<string, string> parameters, string fieldName)
        {
            return parameters.Select("fields", new[] { fieldName });
        }

        /// <summary>
        /// Add ids parameters, if ids already exists appends it.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="ids">
        /// The ids.
        /// </param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, string> SelectIds(this IDictionary<string, string> parameters, string[] ids)
        {
            return parameters.Select("ids", ids);
        }

        /// <summary>
        /// Add ids parameters, if ids already exists appends it.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, string> SelectId(this IDictionary<string, string> parameters, string id)
        {
            return parameters.Select("ids", new[] { id });
        }

        private static IDictionary<string, string> Select(this IDictionary<string, string> parameters, string key, string[] values)
        {
            if (values == null || values.Length == 0) // don't do anything
                return parameters;

            if (parameters == null)
                parameters = new Dictionary<string, string>();

            StringBuilder oldValues;

            if (parameters.ContainsKey(key))
            {
                oldValues = new StringBuilder(parameters[key]);
                parameters.Remove(key);
            }
            else
                oldValues = new StringBuilder();

            if (oldValues.Length > 0)
                oldValues.Append(',');

            foreach (var field in values)
            {
                oldValues.Append(field);
                oldValues.Append(',');
            }

            oldValues.Remove(oldValues.Length - 1, 1); // remove the last comma

            parameters.Add(key, oldValues.ToString());

            return parameters;
        }
    }
}