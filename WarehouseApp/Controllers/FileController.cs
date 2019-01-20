using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin.Security.Provider;
using WarehouseApp.Models;


namespace WarehouseApp.Controllers
{
     [Authorize]
    public class FileController : Controller
    {
         //private WmsDbContext db = new WmsDbContext();
        //
        // GET: /File/
        public FileResult Download(String p, String d)
        {
            var f = File(Path.Combine(Server.MapPath("~/Uploads/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
            var path = Path.Combine(Server.MapPath("~/Uploads/"), f.FileName);
            if (System.IO.File.Exists(path))
            {
                return f;
            }
            else
            {
                return null;
            }

        }

        public String FileExtensionThumbnail(string ext)
        {
            var extLower = ext.ToLower();
            var extThumb = "";
            switch (extLower)
            {
                case ".pdf": extThumb = "<img src='/Content/images/Extensions/file_extension_pdf.png'/>";
                    break;
                case ".xls": extThumb = "<img src='/Content/images/Extensions/file_extension_excel.png'/>";
                    break; 
                case ".xlsx": extThumb = "<img src='/Content/images/Extensions/file_extension_excel.png'/>";
                    break;
                case ".doc": extThumb = "<img src='/Content/images/Extensions/file_extension_word.png'/>";
                    break;
                case ".docx": extThumb = "<img src='/Content/images/Extensions/file_extension_word.png'/>";
                    break;  
                case ".ppt": extThumb = "<img src='/Content/images/Extensions/file_extension_ppt.png'/>";
                    break;
                case ".pptx": extThumb = "<img src='/Content/images/Extensions/file_extension_ppt.png'/>";
                    break;
                case ".jpg": extThumb = "<img src='/Content/images/Extensions/file_extension_jpeg.png'/>";
                    break; 
                case ".jpeg": extThumb = "<img src='/Content/images/Extensions/file_extension_jpeg.png'/>";
                    break;
                case ".png": extThumb = "<img src='/Content/images/Extensions/file_extension_png.png'/>";
                    break;  
                case ".gif": extThumb = "<img src='/Content/images/Extensions/file_extension_gif.png'/>";
                    break; 
                case ".psd": extThumb = "<img src='/Content/images/Extensions/file_extension_psd.png'/>";
                    break;  
                case ".txt": extThumb = "<img src='/Content/images/Extensions/file_extension_txt.png'/>";
                    break; 
            }
            return extThumb;
        }

        public String ImageThumbnail(string fileSource)
        {
            var thumbName = fileSource.Split('.').ElementAt(0) + "_thumb." + fileSource.Split('.').ElementAt(1);
            var imgThumb = "<img src='" + thumbName + "' class='img-thumbnail'/>";
            return imgThumb;
        }

 

	}
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//