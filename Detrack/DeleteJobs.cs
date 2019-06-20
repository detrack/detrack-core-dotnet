using System;
using System.Net.Http;
using System.Threading.Tasks;
using Variables;
using Newtonsoft.Json;

namespace Detrack
{
    public class DeleteJobs
    {
        static void NotMain()
        {
            DeleteJob("5cdd206f28d79b258d241f2d").Wait();
        }

        private static async Task DeleteJob(string id)
        {
            var baseAddress = new Uri("https://app.detrack.com/api/v2/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {


                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-API-KEY", "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0");

                using (var response = await httpClient.DeleteAsync(String.Format("jobs/{0}", id)))
                {

                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                }
            }
        }
    }

}
