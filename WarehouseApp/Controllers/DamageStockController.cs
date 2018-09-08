
using System.Linq;

using System.Web.Mvc;

using WarehouseApp.Models;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class DamageStockController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /DamageStock/
        public ActionResult Index()
        {
            var damagestocks = db.DamageStocks;
            return View(damagestocks.ToList());
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//