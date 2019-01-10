using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EBSM.Entities;

namespace WarehouseApp.Models.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public Customer Customer { get; set; }

        public BillPaymentViewModel CustomerPayment { get; set; }
    }
}