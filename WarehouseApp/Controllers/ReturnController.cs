using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WarehouseApp.Models;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class ReturnController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /ReturnProduct/
        public ActionResult Index()
        {
            var returnproducts = db.ReturnProducts.Include(r => r.InvoiceProduct).Include(r => r.Product).Include(r => r.Return);
            return View(returnproducts.ToList());
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
