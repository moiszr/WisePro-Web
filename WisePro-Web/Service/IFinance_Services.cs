using WisePro_Web.Models;

namespace WisePro_Web.Service
{
    public interface IFinance_Services
    {
        Task<List<Finance>> GetAllTasks();
        Task<Finance> GetTaskById(int id);
        Task<bool> create(Finance task);
        Task<bool> update(Finance task);
        Task<bool> delete(int id);
    }
}
