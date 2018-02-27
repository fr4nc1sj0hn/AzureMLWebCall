// This code requires the Nuget package Microsoft.AspNet.WebApi.Client to be installed.
// Instructions for doing this in Visual Studio:
// Tools -> Nuget Package Manager -> Package Manager Console
// Install-Package Microsoft.AspNet.WebApi.Client

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CallRequestResponseService
{
    class Program
    {
        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "Stock", "2GO"
                                            },
                                            {
                                                "Sector", "Services"
                                            },
                                            {
                                                "Year", "2013"
                                            },
                                            {
                                                "Net Income Growth", "1.5792"
                                            },
                                            {
                                                "EPS (Basic) Growth", "1.5792"
                                            },
                                            {
                                                "Net Income", "212"
                                            },
                                            {
                                                "EPS (Basic)", "0.09"
                                            },
                                            {
                                                "Assets - Total - Growth", "0.1083"
                                            },
                                            {
                                                "Current Ratio", "1.14"
                                            },
                                            {
                                                "Quick Ratio", "1.06"
                                            },
                                            {
                                                "YearEndReturn", ""
                                            },
                                            {
                                                "ReturnLabel", "Profit"
                                            },
                                            {
                                                "PE Ratio", "19"
                                            },
                                            {
                                                "PE Ratio Group", "ten to twenty"
                                            },
                                            {
                                                "PB Ratio", "1.367776325"
                                            },
                                            {
                                                "PB Ratio Group", "zero to two"
                                            },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "Removed"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://asiasoutheast.services.azureml.net/workspaces/fcdfe695d0db4a37b0b572c1b4a1a0a3/services/c3be3bee8f5f421786719f6e2af43d87/execute?api-version=2.0&format=swagger");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    var entries = scoreRequest.Inputs["input1"][0].Select(d =>
                        string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));

                    string request = "{" + string.Join(",", entries) + "}";

                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Request: {0}", request);
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}