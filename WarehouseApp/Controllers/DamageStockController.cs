
using System.Linq;

using System.Web.Mvc;

using WarehouseApp.Models;
using EBSM.Entities;
using EBSM.Services;
namespace WarehouseApp.Controllers
{
    [Authorize]
    public class DamageStockController : Controller
    {
        private DamageService _damageService = new DamageService();

        // GET: /DamageStock/
        public ActionResult Index()
        {
            var damagestocks = _damageService.GetAllDamageStocks();
            return View(damagestocks.ToList());
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _damageService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//