using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WisePro_Web.Models;

namespace WisePro_Web.Service
{
    public class Task_services : ITask_Services
    {
        private static string _url;

        public Task_services()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _url = builder.GetSection("ApiSettings:url").Value;
            

        }

        public async Task<List<User_Task>> GetAllTasks()
        {
            List<User_Task> tasks = new List<User_Task>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.GetAsync("/tasks");
            if  (response.IsSuccessStatusCode)
            {
                var result_json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<User_Task>>(result_json);
                tasks = result;
            }
            return tasks;
        }

        public async Task<User_Task> GetTaskById(int id)
        {
            User_Task task = new User_Task();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.GetAsync($"/tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result_json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<User_Task>(result_json);
                task = result;
            }
            return task;
        }

       
        public async Task<bool> create(User_Task task)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/tasks", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> update(User_Task task)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/tasks/{task.tasks_id}", content);
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
            var response = await client.DeleteAsync($"/tasks/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }


        public Task<User_Task> GetTaskByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
