    using Microsoft.AspNetCore.Mvc;
using WisePro_Web.Models;
using WisePro_Web.Service;

namespace WisePro_Web.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinance_Services _services;

        public FinanceController(IFinance_Services services)
        {
            _services = services;
        }

        public async Task<IActionResult> Finance()
        {
            Resulfinance finance = new Resulfinance();

            finance.finances = await _services.GetAllTasks();
            return View(finance);
        }
        public async Task<IActionResult> Editfinance(int finance_id)
        {
            Resulfinance finance = new Resulfinance();

            if (finance_id != 0)
            {
                finance.finance = await _services.GetTaskById(finance_id);
                finance.finances = await _services.GetAllTasks();
            }
            return View(finance);
        }
        [HttpPost]
        public async Task<IActionResult> Createfinance(Resulfinance ob_finance)
        {

            bool result;
            if (ob_finance.finance.finance_id == 0)
            {
                result = await _services.create(ob_finance.finance);
            }
            else
            {
                result = await _services.update(ob_finance.finance);

            }
            if (result)
            {
                return RedirectToAction("Finance");
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Deletefinance(int finance_id)
        {

            bool result = await _services.delete(finance_id);


            if (result)
            {
                return RedirectToAction("Finance");
            }
            else
            {
                return NoContent();
            }
        }
    }
}

