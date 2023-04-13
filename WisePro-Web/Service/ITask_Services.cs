using WisePro_Web.Models;

namespace WisePro_Web.Service
{
    public interface ITask_Services
    {

        Task<List<User_Task>> GetAllTasks();
        Task<User_Task> GetTaskById(int id);
        Task<User_Task> GetTaskByName(string name);
        Task<bool> create(User_Task task);
        Task<bool> update(User_Task task);
        Task<bool> delete(int id);


    }
}
