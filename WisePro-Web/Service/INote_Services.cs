using WisePro_Web.Models;

namespace WisePro_Web.Service
{
    public interface INote_Services
    {
        Task<List<Note>> GetAllTasks();
        Task<Note> GetTaskById(int id);
        Task<bool> create(Note task);
        Task<bool> update(Note task);
        Task<bool> delete(int id);
    }
}
