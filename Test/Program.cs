using System;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test
{
    class Program
    {
        static HttpClient client = new HttpClient(); //inital HttpClient
        static async Task CallWebAPIAsync()
        {

            client.BaseAddress = new Uri("https://localhost:44391"); //domain
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //set data type for header
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
                Console.WriteLine("token: "+ token);
                bool isRun = true;
                while (isRun)
                {
                    Console.WriteLine("Please enter path API: ");
                    string path = Console.ReadLine();
                    switch (path)
                    {
                        case "/api/company/get-companies":
                            var companyList = await GetCompanyAsync(token, path);
                            Console.WriteLine(companyList);
                            break;
                        case "/api/company/get-detail-company":
                            Console.WriteLine("Please enter companyID: ");
                            string ID = Console.ReadLine();
                            var company = await GetDetailCompany(token, path, ID);
                            Console.WriteLine(company);
                            break;
                        case "/api/job-seekers/get-list-job-seeker":
                            var jobSeekerList = await GetJobSeekerList(token, path);
                            Console.WriteLine(jobSeekerList);
                            break;
                        case "//api/job-seekers/get-job-seeker-by-id":
                            Console.WriteLine("Please enter accountID: ");
                            string accountID = Console.ReadLine();
                            var jobSeeker = await GetDetailJobSeeker(token, path, accountID);
                            Console.WriteLine(jobSeeker);
                            break;
                        case "/api/inspection/get-company-movies":
                            var companyMovieList = await GetCompayVideoList(token, path);
                            Console.WriteLine(companyMovieList);
                            break;
                        case "/api/inspection/get-applicant-movies":
                            var gam = await GetCompayVideoList(token, path);
                            Console.WriteLine(gam);
                            break;
                        case "exit":
                            isRun = false;
                            break;
                        default:
                            Console.WriteLine("path API do not exist!!!");
                            break;
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();

        }
        //login and return token
        static async Task<string> LoginAsync(LoginParam param)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/admin/login", param); //call API 
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result; //get result (string)

            LoginResponse LoginResponse = JsonConvert.DeserializeObject<LoginResponse>(content); //convert json to object from string
            return LoginResponse.token;
        }
        //get company list
        static async Task<string> GetCompanyAsync(string token, string path)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token)); //attach token into header request
            HttpResponseMessage response = await client.GetAsync(path);//call API
            var content = response.Content.ReadAsStringAsync().Result; //get result(string)
            JObject json = JObject.Parse(content); //convert string to json


            return json.ToString();
        }

        //get company detail
        static async Task<string>  GetDetailCompany(string token, string path, string ID)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token)); //attach token into header request
            string parameter = "?companyId=" + ID;
            HttpResponseMessage response = await client.GetAsync(path + parameter);//call API
            var content = response.Content.ReadAsStringAsync().Result; //get result(string)
            JObject json = JObject.Parse(content); //convert string to json
            return json.ToString();
        }

        // get job seeker list
        static async Task<string> GetJobSeekerList(string token, string path)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token)); //attach token into header request
            HttpResponseMessage response = await client.GetAsync(path);//call API
            var content = response.Content.ReadAsStringAsync().Result; //get result(string)
            JObject json = JObject.Parse(content); //convert string to json
            return json.ToString();
        }

        //get job seeker detail
        static async Task<string> GetDetailJobSeeker(string token, string path, string ID)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token)); //attach token into header request
            string parameter = "?accountId=" + ID;
            HttpResponseMessage response = await client.GetAsync(path + parameter);//call API
            var content = response.Content.ReadAsStringAsync().Result; //get result(string)
            JObject json = JObject.Parse(content); //convert string to json
            return json.ToString();
        }

        //get company video list
        static async Task<string> GetCompayVideoList(string token, string path)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token)); //attach token into header request
            HttpResponseMessage response = await client.GetAsync(path);//call API
            var content = response.Content.ReadAsStringAsync().Result; //get result(string)
            JObject json = JObject.Parse(content); //convert string to json
            return json.ToString();
        }

        //get job seeker video list
        static async Task<string> GetJobSeekerVideoList(string token, string path)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token)); //attach token into header request
            HttpResponseMessage response = await client.GetAsync(path);//call API
            var content = response.Content.ReadAsStringAsync().Result; //get result(string)
            JObject json = JObject.Parse(content); //convert string to json
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
