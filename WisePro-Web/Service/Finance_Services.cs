using Newtonsoft.Json;
using System.Text;
using WisePro_Web.Models;

namespace WisePro_Web.Service
{
    public class Finance_Services : IFinance_Services
    {
        private static string _url;

        public Finance_Services()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _url = builder.GetSection("ApiSettings:url").Value;


        }

        public async Task<List<Finance>> GetAllTasks()
        {
            List<Finance> Finances = new List<Finance>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.GetAsync("/finance");
            if (response.IsSuccessStatusCode)
            {
                var result_json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Finance>>(result_json);
                Finances = result;
            }
            return Finances;
        }

        public async Task<Finance> GetTaskById(int id)
        {
            Finance Finances = new Finance();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.GetAsync($"/finance/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result_json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Finance>(result_json);
                Finances = result;
            }
            return Finances;

        }
        public async Task<bool> create(Finance finance)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var content = new StringContent(JsonConvert.SerializeObject(finance), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/finance", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> update(Finance finance)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var content = new StringContent(JsonConvert.SerializeObject(finance), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/finance/{finance.finance_id}", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> delete(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.DeleteAsync($"/finance/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}
