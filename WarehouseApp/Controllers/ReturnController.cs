﻿using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WarehouseApp.Models;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class ReturnController : Controller
    {
        private SalesReturnService salesReturnService = new SalesReturnService();

        // GET: /ReturnProduct/
        //public ActionResult Index()
        //{
        //    var returnproducts = salesReturnService.GetAll();
        //    return View(returnproducts.ToList());
        //}

     
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
