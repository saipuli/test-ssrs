using System;
using System.Net.Http;
using System.Net;
using System.Text;

namespace test_ssrs
{
    class Program
    {
        static void Main(string[] args)
        {
            var userName = Environment.GetEnvironmentVariable("Creds_UserName");
            var password = Environment.GetEnvironmentVariable("Creds_Password");
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(
                new Uri("https://beyondssrsdev.eastus.cloudapp.azure.com")
                , "NTLM"
                , new NetworkCredential(
                    userName
                    , password
                    , "beyondssrsdev.eastus.cloudapp.azure.com"));
            using (var httpClientHandler = new HttpClientHandler() { Credentials = credentialCache })
            {
                // Use ServerCertificateCustomValidationCallback with extreme caution.
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (var client = new HttpClient(httpClientHandler))
                {
                    // Set up the call.
                    var requestUri = new Uri("https://beyondssrsdev.eastus.cloudapp.azure.com/ReportServer/Pages/ReportViewer.aspx?%2fCompensation%2fPortfolioHealthbyDD&rs:Command=Render");
                    var payload = "{\"DDEmail\": \"ken.tow@getbeyond.com\"}";
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    // Make the call.
                    var response = client.PostAsync(requestUri, content).Result;
                    // Manipulate the results.
                    var result = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine("Server Response: " + result);
                    Console.ReadLine();
                }
            }
        }
    }
}
