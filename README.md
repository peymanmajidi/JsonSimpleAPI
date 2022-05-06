# JsonSimpleAPI

This repo makes simple any API JSON request and response. simply make a new `Request()`m then specify class will back via request and send any type of data. this library serializes any classes or data types to JSON then deserialize any clases or data types for you back. You don't have to worry about the implementation.


Nuget Package
===============

Download via Microsoft >>>> [Download Now](https://www.nuget.org/packages/Peyman.JsonSimpleAPI/1.0.0)


## Example
```csharp    
    var api_request = new Request(URL, "Authorization", $"Basic blahblahapikey==");
    var result = api.Send<MyClassResult>(MyClassDataToSend);
    if (result != null)
    {
            print(result.Message);
    }
```

`MyClassResult` could be any data type like int or string or any custom class   
`MyClassDataToSend` could be any data type like int or string or any custom class   


## Example 2

```csharp
  int age = 33;
  var api_request = new Request("www.age-to-year-api.com", "Authorization", $"Basic blahblahapikey==");
  var yearIWasBorn = api.Send<DateTime>(age); // "1988"
```
  
  
How it Works
==============
  
  
  ```csharp
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
```
  
