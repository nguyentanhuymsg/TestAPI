using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test
{
    class Program
    {
        static HttpClient client = new HttpClient();
            static async Task CallWebAPIAsync()
        {

            client.BaseAddress = new Uri("https://localhost:44391");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var LoginParam = new LoginParam
                {
                    Email = "string",
                    Password = "string",
                    RecapchaToken = "string",
                    Service = "string"
                };
                var token = await LoginAsync(LoginParam);
                Console.WriteLine(token);
                var companies = await GetCompanyAsync(token);
               
                Console.WriteLine(companies);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();

        }
             static async Task<string> LoginAsync(LoginParam param)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/admin/login", param);
                response.EnsureSuccessStatusCode();
                var content =  response.Content.ReadAsStringAsync().Result;
           
                LoginResponse LoginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);
                return LoginResponse.token;
            }
             static async Task<string> GetCompanyAsync(string token)
            {
                string path = "/api/company/get-companies";
                client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
                HttpResponseMessage response = await client.GetAsync(path);
                var content = response.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(content);

           
                return json.ToString();
        }

        static void Main(string[] args)
        {
            Program.CallWebAPIAsync().GetAwaiter().GetResult();
           
        }
        public class LoginParam
        {
            [Required(ErrorMessage = "メールアドレスを入力して下さい")]
            public string Email { get; set; }

            [Required(ErrorMessage = "パスワードを入力して下さい")]
            public string Password { get; set; }

            public string RecapchaToken { get; set; }

            public string Service { get; set; }
        }
    }
}
