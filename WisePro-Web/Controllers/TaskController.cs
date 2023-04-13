using Microsoft.AspNetCore.Mvc;
using WisePro_Web.Models;
using WisePro_Web.Service;


namespace WisePro_Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITask_Services _services;

        public TaskController(ITask_Services services)
        {
            _services = services;
        }
        public async Task<IActionResult> Index()
        {
            Resultask tasks = new Resultask();

            tasks.Tasks = await _services.GetAllTasks();
            return View(tasks);
        }
        public async Task<IActionResult> Edittask(int task_id)
        {
            Resultask tasks = new Resultask();

            if (task_id != 0)
            {
                tasks.task = await _services.GetTaskById(task_id);
                tasks.Tasks = await _services.GetAllTasks();
            }
            return View(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(Resultask ob_task)
        {

            bool result;
            if (ob_task.task.tasks_id == 0)
            {
                result = await _services.create(ob_task.task);
            }
            else
            {
                result = await _services.update(ob_task.task);

            }
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int task_id)
        {

            bool result = await _services.delete(task_id);


            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NoContent();
            }
        }

    }
}
