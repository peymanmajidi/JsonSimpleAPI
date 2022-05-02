using Newtonsoft.Json;
using System.Net;

namespace Peyman.JsonSimpleAPI
{
    public class Request
    {
        public string URL { get; set; }
        public string Method { get; set; } = "POST";
        public string AuthorizationKey { get; set; } = "Authorization";
        public string AuthorizationValue { get; set; } = "Basic blahblahblah==";
        public string ContentType { get; set; } = "application/json";

        public Request(string uRL,  string authorizationKey, string authorizationValue)
        {
            URL = uRL;
            AuthorizationKey = authorizationKey;
            AuthorizationValue = authorizationValue;
        }

        public T Send<T>(object data_to_send)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(this.URL);
            httpWebRequest.ContentType = this.ContentType;
            httpWebRequest.Method = this.Method;
            httpWebRequest.Headers.Add(this.AuthorizationKey, this.AuthorizationValue);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(data_to_send);
                streamWriter.Write(json);
            }

            try
            {
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        var json_result = JsonConvert.DeserializeObject<List<T>>(result);
                        if (json_result.Count() > 0)
                        {

                            return json_result.FirstOrDefault();

                        }
                    }
                }

                return default;

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return default;
                    }
                }
            }

            return default;


        }

    }
}