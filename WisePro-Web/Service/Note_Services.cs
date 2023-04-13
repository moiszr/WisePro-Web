using Newtonsoft.Json;
using System.Text;
using WisePro_Web.Models;

namespace WisePro_Web.Service
{
    public class Note_Services : INote_Services
    {
        private static string _url;

        public Note_Services()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _url = builder.GetSection("ApiSettings:url").Value;


        }
        public async Task<List<Note>> GetAllTasks()
        {
            List<Note> Notes = new List<Note>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.GetAsync("/note");
            if (response.IsSuccessStatusCode)
            {
                var result_json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Note>>(result_json);
                Notes = result;
            }
            return Notes;
        }

        public async Task<Note> GetTaskById(int id)
        {
            Note Notes = new Note();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var response = await client.GetAsync("/note");
            if (response.IsSuccessStatusCode)
            {
                var result_json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Note>(result_json);
                Notes = result;
            }
            return Notes;
        }
        public async Task<bool> create(Note note)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var content = new StringContent(JsonConvert.SerializeObject(note), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/note", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> update(Note note)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            var content = new StringContent(JsonConvert.SerializeObject(note), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/note/{note.note_id}", content);
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
            var response = await client.DeleteAsync($"/note/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }




    }
}
