using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EBSM.Entities;

namespace WarehouseApp.Models.ViewModels
{
    public class TransferProductsViewModel
    {
        public List<TransferProduct> TransferProducts { get; set; }
    }
    public class ArticleTransfersViewModel
    {
        public List<ArticleTransfer> ArticleTransfers { get; set; }
    }
   
   
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : October 2017
//=======================================================================================//