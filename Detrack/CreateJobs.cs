using System;
using System.Net.Http;
using System.Threading.Tasks;
using Variables;
using Newtonsoft.Json;


namespace Detrack
{
    class CreateJobs
    {
        static void NotMain()
        {
            CreateJob().Wait();
        }

        static async Task CreateJob()
        {
            var baseAddress = new Uri("https://app.detrack.com/api/v2/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {


                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-API-KEY", "ed1037b346267186fc71a6ea4e15074df54f3a77d30ac1d0");

                using (var content = new StringContent(CreateJobData(), System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("jobs", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseData);
                    }
                }
            }

        }

        static string CreateJobData()
        {
            Console.WriteLine("If left empty they will have the default value");
            Console.WriteLine("Input do_number");
            string do_number = Console.ReadLine();
            Console.WriteLine("Input address");
            string address = Console.ReadLine();
            Console.WriteLine("Input date (yyyy-mm-dd format)");
            string date = Console.ReadLine();

            CreateInput data = new CreateInput(do_number, address, date);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            json = "{  \"data\": " + json + "}";
            Console.WriteLine(json);
            return json;
        }

    }
}
