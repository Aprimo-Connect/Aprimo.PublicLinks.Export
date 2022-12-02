using System.Configuration;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace Aprimo.PublicLinks.Export
{
    class AprimoRESTApi
    {
        // Auth info
        private static string AprimoUsername = ConfigurationManager.AppSettings.Get("AprimoUsername");
        private static string ClientID = ConfigurationManager.AppSettings.Get("AprimoClientID");
        private static string ClientSecret = ConfigurationManager.AppSettings.Get("AprimoClientSecret");
        private static string AprimoTenant = ConfigurationManager.AppSettings.Get("AprimoTenant");

        private static string basicAuthString = "dGNoYXdsYTo3MTU5ZTAzYTc5Mzk0MWRiYTYxOWIxOTViZjBjMDIwZA==";
        private static string loginUrl = "/login/connect/token";
        private static string accessToken = "";

        // Test with productstrategy1 before going to knowledge libray
       /* private static string username = "jratini";
        private static string ClientCredentialsClientID = "RN8G5ECT-RN8G";
        private static string oldMethodClientID = "RY3WL3AR-RY3W";
        private static string clientSecret = "aprimo123";
        private static string aprimoTenant = "productstrategy1";
        private static string loginUrl = "/api/oauth/create-native-token";
        private string basicAuthString = "anJhdGluaUludGVncmF0aW9uOjI3MGJkODBhNjEyNzQzMDBiZDVmMGVkMTlkMmQ1ZjY5";*/

        public bool getAccessTokenClientCredentials()
        {
            HttpClient client = new HttpClient();
            string tokenUrl = $"https://{AprimoTenant}.aprimo.com{loginUrl}";
            string retVal = "";
            List<KeyValuePair<string, string>> requestBody = new List<KeyValuePair<string, string>>();

            requestBody.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            requestBody.Add(new KeyValuePair<string, string>("scope", "api")); //api is the only supported scope at the time of writing
            requestBody.Add(new KeyValuePair<string, string>("client_id", ClientID));
            requestBody.Add(new KeyValuePair<string, string>("client_secret", ClientSecret));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };

            HttpResponseMessage response = client.SendAsync(request).Result;
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObj = JsonConvert.DeserializeObject(responseBody);

                    accessToken = jsonObj["access_token"];
                    return true;
                }
                else
                {
                    throw new HttpRequestException();
                   
                }
            }
            catch(Exception ex)
            {
                // log exception
                return false;
            }
        }
        public string getAccessTokenOldMethod()
        {
            HttpClient client = new HttpClient();
            // User service account to get an aprimo access token
            string retVal;

            Uri accessTokenUri = new Uri("https://" + AprimoTenant + ".aprimo.com" + loginUrl);
            HttpResponseMessage response;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, accessTokenUri))
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthString);
                requestMessage.Headers.Add("client-id", ClientID);


                response = client.SendAsync(requestMessage).Result;
            }

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                dynamic jsonObject = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                retVal = jsonObject["accessToken"];
            }
            else
            {
                // Throw a generic HttpRequestException error with the status code and reason phrase
                throw new HttpRequestException(response.StatusCode.ToString() + response.ReasonPhrase);
            }

            return retVal;
        }
        public string uploadFile(string csvData, string accessToken, string fileName)
        {
            string retVal = "null";
            
            HttpClient client = new HttpClient();
            var builder = new UriBuilder($"https://upload.aprimo.com/uploads");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "VoiceApp");
            client.DefaultRequestHeaders.Add("API-VERSION", "1");

            var content = new MultipartFormDataContent("Boundary----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            content.Add(new StringContent(csvData), "file", $"{fileName}");
            var request = new HttpRequestMessage
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Post,
                Content = content
            };

            HttpResponseMessage response = client.SendAsync(request).Result;

            if(response.IsSuccessStatusCode)
            {
                dynamic jsonObject = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                retVal = jsonObject["token"];
            }

            return retVal;

        }
        public bool addNewVersion(string masterRecordId, string uploadToken, string accessToken, string fileName)
        {
            HttpClient client = new HttpClient();
            Uri updateRecordUri = new Uri($"https://{AprimoTenant}.dam.aprimo.com/api/core/record/{masterRecordId}");
            HttpResponseMessage response;
            string json = @"{" +
                            @"   ""files"":{" +
                            @"      ""master"": """ + uploadToken + @""",          " +
                            @"      ""addOrUpdate"":[" +
                            @"         {  " +
                            @"            ""versions"":{  " +
                            @"               ""addOrUpdate"":[" +
                            @"                  {  " +
                            @"                     ""id"":""" + uploadToken + @""",              " +
                            @"                     ""filename"":""" + fileName + @"""           " +
                            @"                  }" +
                            @"               ]" +
                            @"            }" +
                            @"         }" +
                            @"      ]" +
                            @"   }" +
                            @"}";

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, updateRecordUri))
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Headers.Add("API-VERSION", "1");
                requestMessage.Headers.Add("User-Agent", "VoiceApp");
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                requestMessage.Content = new StringContent(json);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                

                response = client.SendAsync(requestMessage).Result;
            }

           if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                // Aprimo Update Record returns 204 on success
                return true;
            }
           else
            {
                return false;
            }
        }

        public dynamic searchAprimoRecords(HttpContent expression, List<Tuple<string,string>> additionalHeaders, string pageSize, string page)
        {

            dynamic retVal = "";
            using(HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://{AprimoTenant}.dam.aprimo.com/api/core/search/records"),
                    Method = HttpMethod.Post,
                    Headers =
                    {
                        { System.Net.HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}" },
                        { "API-VERSION", "1" },
                        { System.Net.HttpRequestHeader.Accept.ToString(), "application/json" },
                        { "User-Agent", "Aprimo.PublicLinks.Export" },
                        { "pageSize", pageSize },
                        { "page", page }
                    },
                    Content = expression,
                };

                // Add any additional header from the caller
                foreach(Tuple<string,string>item in additionalHeaders)
                {
                    request.Headers.Add(item.Item1, item.Item2);
                }

                HttpResponseMessage response = null;
                response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObj = JsonConvert.DeserializeObject(responseContent);
                    retVal = jsonObj;

                }
                else
                {
                    Console.WriteLine(response.ToString());
                }
            }

            return retVal;

           
        }
    }
}
