using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private  CompanyProfileService _companyProfileService = new CompanyProfileService();
        private  AuditLogService _auditLogService = new AuditLogService();
        //
        // GET: /Configuration/
        //public ActionResult Vats()
        //{
        //    var vats = db.Vats.Include(t => t.Department).Where(v => v.Status > 0).OrderBy(t => t.DepartmentId).ToList();
        //    return View("../Settings/Vats", vats);
        //}
        //public ActionResult UpdateVat(int? id, double rate)
        //{
        //    if (id == null)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json(new { Result = "Error" });
        //    }
        //    Vat vat = db.Vats.Find(id);
        //    if (vat == null)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.NotFound;
        //        return Json(new { Result = "Error" });
        //    }
        //    vat.VatPercentage = rate;
        //    db.Entry(vat).State = EntityState.Modified;
        //    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
        //    return Json(new { Result = "OK" });
        //}

        [Roles("Global_SupAdmin,Company_Profile_Edit")]
        public ActionResult CompanyProfile()
        {
            CompanyProfile company = _companyProfileService.GetComapnyProfile();
            if (company == null)
            {
                return HttpNotFound();
            }
            return View("../Settings/CompanyProfile", company);
        }


        // POST: /Salesman/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyProfile(CompanyProfile company)
        {
            if (ModelState.IsValid)
            {
                CompanyProfile companyProfile = _companyProfileService.GetCompanyProfileById(company.CompanyId);
                companyProfile.CompanyName = company.CompanyName;
                companyProfile.CompanyAddress = company.CompanyAddress;
                companyProfile.Email = company.Email;
                companyProfile.Phone = company.Phone;
                companyProfile.WebSite = company.WebSite;
                companyProfile.Tin = company.Tin;
                companyProfile.VatRegNo = company.VatRegNo;
                companyProfile.CompanyLogo = company.CompanyLogo;
                companyProfile.UpdatedDate = DateTime.Now;
                companyProfile.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                _companyProfileService.Edit(companyProfile, AuthenticatedUser.GetUserFromIdentity().UserId);
                return RedirectToAction("Index","Home");
            }
            return View("../Settings/CompanyProfile", company);
        }

        public static CompanyProfile CompanyInfo()
        {
            var cx = new CompanyProfileService();
            CompanyProfile company = cx.GetComapnyProfile();
            if (company == null)
            {
                company=new CompanyProfile()
                {
                    CompanyName = "Company Name"
                };
            }
            return company;
        }
         [Roles("Global_SupAdmin,Db_Backup")]
        public ActionResult Backup()
        {
            return View();
        }
        public ActionResult SystemDbBackup()
        {
            var connString = ConfigurationManager.ConnectionStrings["WmsConnString"].ConnectionString;
            var csb = new SqlConnectionStringBuilder(connString);
            //var dataSrc = csb.DataSource;
            //var uId = csb.UserID;
            //var pass = csb.Password;
            //var initislCat = csb.InitialCatalog;

            string message = "";
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); 
            //IF SQL Server Authentication then Connection String  
            //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=" + YourDBName + ";uid=sa;pwd=password;";  

            //IF Window Authentication then Connection String  
            //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=Test;Integrated Security=true;";
           // con.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TrustPharmaDb080217;User ID=sa;Password=p@ssword";
            con.ConnectionString = connString;

            string backupDIR = ConfigurationManager.AppSettings["DbBackupDrive"] + ":\\BackupDB";
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            try
            {
                con.Open();
                sqlcmd = new SqlCommand("backup database " + csb.InitialCatalog + " to disk='" + backupDIR + "\\" + csb.InitialCatalog+"_"+DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", con);
                sqlcmd.ExecuteNonQuery();
                con.Close();
                message = "Backup database successfully in '"+ backupDIR+"'";
            }
            catch (Exception ex)
            {
               message = "Error Occured During DB backup process !<br>" + ex.ToString();
            }
            return BackupResult(message);
        }

        private ActionResult BackupResult(string message)
        {
                ViewBag.Message = message;
            
            return View("../Settings/BackupResult");
        }
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("{0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("{0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("{0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("{0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        [Roles("Global_SupAdmin")]
        public ActionResult AuditLogsView(AuditLogSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.AuditDateFrom);
            var toDate = Convert.ToDateTime(model.AuditDateTo);
            var auditLogs =_auditLogService.GetAllAuditLog(model.AuditDateFrom, model.AuditDateTo, model.EventType, model.AuditTable).ToList();
            model.AuditLogs = auditLogs.ToPagedList(model.Page, model.PageSize);
            ViewBag.AuditTable = new SelectList(_auditLogService.GetAllAuditTables().ToList(), "TableName", "TableName");
            return View("../Settings/AuditLogsView", model);
        }


        //number to currency===================================================================
        public static string NumberToCurrencyTextBdt(double number)
        {
            // Round the value just in case the decimal value is longer than two digits
            number = Math.Round(number, 2);

            string wordNumber = string.Empty;

            // Divide the number into the whole and fractional part strings
            string[] arrNumber = number.ToString().Split('.');

            // Get the whole number text
            long wholePart = long.Parse(arrNumber[0]);
            string strWholePart = NumberToWords(wholePart);


            wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " Taka" : " Taka");


            if (arrNumber.Length > 1)
            {
                wordNumber += " and ";
                // If the length of the fractional element is only 1, add a 0 so that the text returned isn't,
                // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
                long fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
                string strFarctionPart = NumberToWords(fractionPart);

                wordNumber += (fractionPart == 0 ? " No" : strFarctionPart) + (fractionPart == 1 ? " Paisa" : " Paisa");
            }
            else
                wordNumber += "";

            return wordNumber + " Only";
        }


        // Bangladeshi Taka Number to word===================================================================

        //this array specifies the names of the powers of 10
        static Tuple<int, string>[] powers =
{ 
    new Tuple<int, string>(0, ""),
    new Tuple<int, string>(3, "Thousand"),
    new Tuple<int, string>(5, "Lac"),
    new Tuple<int, string>(7, "Crore")
};

        //this array specifies the digits' names
        static string[] digits = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        static string[] extendedDigits = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        static string[] tensWords = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


        private static string NumberToWords(long number)
        {
            var sb = new StringBuilder();

            //begin with the left most digit (greatest power of 10)
            for (int i = powers.Length - 1; i >= 0; --i)
            {
                //translate the current part only (for a known power of 10)
                //usually part is a 3-digit number
                int part = (int)(number / (long)Math.Pow(10, powers[i].Item1));
                //if the part is 0, we don't have to add anything
                if (part > 0)
                {
                    //extract the hundreds
                    int hundreds = part / 100;
                    if (hundreds > 9)
                        throw new ArgumentException(number + " is too large and cannot be expressed.");
                    if (hundreds > 0)
                    {
                        //if there are hundreds, copy them to the output
                        sb.Append(digits[hundreds]);
                        sb.Append(" Hundred ");
                    }
                    //convert the next two digits
                    sb.Append(TwoDigitNumberToWord(part % 100));
                    sb.Append(" ");
                    //and append the name of the power of 10
                    sb.Append(powers[i].Item2);
                    sb.Append(" ");
                    //subtract the currently managed part
                    number -= part * (long)Math.Pow(10, powers[i].Item1);
                }
            }
            return sb.ToString();
        }

        private static string TwoDigitNumberToWord(int number)
        {
            //one digit case
            if (number < 10)
                return digits[number];
            //special case 10 <= n <= 19
            if (number < 20)
                return extendedDigits[number - 10];
            int tens = number / 10;
            int ones = number % 10;
            //concatenate the word from the two digits' words
            return tensWords[tens] + " " + digits[ones];
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _companyProfileService.Dispose();
            }
            base.Dispose(disposing);
        }
	}
    
    public class TicketPrintController : Controller
    {
        private static SalesService _salesService = new SalesService();
        private static ProductService _productService = new ProductService();
        private static CompanyProfileService _companyProfileService = new CompanyProfileService();
        //===========ticket printinng========================================================================
        public void PrintAllTicket(List<TicketPrintModel> ticketPrintList)
        {

            foreach (var pt in ticketPrintList)
            {

                var discountAmount = BankAccountController.DiscountCalculator(pt.DiscountAmount, pt.DiscountType, pt.PriceBeforeDeiscount).DiscountValueShow;
                //const int FIRST_COL_PAD = 24;
                //const int SECOND_COL_PAD = 24;
               

                //var ticketBodyStrBuider = new StringBuilder();
                //ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Ticket No :".PadRight(FIRST_COL_PAD), pt.TicketNo.PadRight(SECOND_COL_PAD)));
                //ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Customer Name :".PadRight(FIRST_COL_PAD), pt.FullName.PadRight(SECOND_COL_PAD)));
                //if (!string.IsNullOrEmpty(pt.ContactNo))
                //{
                //    ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Contact :".PadRight(FIRST_COL_PAD), pt.ContactNo.PadRight(SECOND_COL_PAD)));
                //}

                //ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Ticket Price :".PadRight(FIRST_COL_PAD), pt.PriceBeforeDeiscount.ToString("n").PadRight(SECOND_COL_PAD)));
                //if (pt.DiscountAmount > 0)
                //{
                //    ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Discount :".PadRight(FIRST_COL_PAD),
                //        discountAmount.PadRight(SECOND_COL_PAD)));
                //    ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Price (Discounted) :".PadRight(FIRST_COL_PAD),
                //        pt.PriceAfterDiscount + " Tk".PadRight(SECOND_COL_PAD)));
                //}
                //if (pt.VatAmount > 0)
                //{
                //    ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "VAT :".PadRight(FIRST_COL_PAD),
                //        pt.VatAmount + " Tk".PadRight(SECOND_COL_PAD)));
                //}
                //ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Ticket Price :".PadRight(FIRST_COL_PAD), pt.TicketNo.PadRight(SECOND_COL_PAD)));
                //ticketBodyStrBuider.AppendLine(string.Format("{0} {1}", "Ticket Price :".PadRight(FIRST_COL_PAD), pt.TicketNo.PadRight(SECOND_COL_PAD)));
               
                
                string ticketBody = "";
                ticketBody += string.Format("{0}{1}\n", "Ticket No:                     ", pt.TicketNo);
                ticketBody += string.Format("{0}{1}\n", "Name:                          ", pt.FullName);
                ticketBody += string.Format("{0}{1}\n", "Contact:                       ", pt.ContactNo);
                ticketBody += string.Format("{0}{1}\n", "Ticket Price:                 ", pt.PriceBeforeDeiscount + " Tk");
                if (pt.DiscountAmount > 0)
                {
                    ticketBody += string.Format("{0}{1}\n", "Discount:                     ", discountAmount);
                    ticketBody += string.Format("{0}{1}\n", "Price (Discounted):      ", pt.PriceAfterDiscount + " Tk");
                }
                ticketBody += string.Format("{0}{1}\n", "VAT:                             ", pt.VatAmount + " Tk");
                ticketBody += string.Format("{0}{1}\n", "Total Price:                  ", pt.TotalPrice + " Tk");
                PrintReceiptForTransaction(pt.TicketType, ticketBody);
            }
        }
        public void PrintReceiptForTransaction(string ticketType, string ticketBody)
        {
            var cx = new CompanyProfileService();
            ViewBag.Company = cx.GetComapnyProfile();
            ViewBag.TicketType = ticketType;
            ViewBag.TicketContent = ticketBody;

            PrintDocument recordDoc = new PrintDocument();
            recordDoc.DocumentName = "Customer Receipt";
            recordDoc.PrintPage += new PrintPageEventHandler(PrintReceiptPage);
            // function below
            recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
            // Comment if debugging 
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = GetDefaultPrinter();
            recordDoc.PrinterSettings = ps;
            recordDoc.Print();
            // --------------------------------------

            // Uncomment if debugging - shows dialog instead
            //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            //printPrvDlg.Document = recordDoc;
            //printPrvDlg.Width = 1200;
            //printPrvDlg.Height = 800;
            //printPrvDlg.ShowDialog();
            // --------------------------------------

            recordDoc.Dispose();

        }


        private void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {


            float x = 1;
            float y = 1;
            float width = 290.0F; // max width I found through trial and error
            float height = 0F;
            Font drawFontArial12Bold = new Font("Arial", 12, FontStyle.Bold);
            Font drawFontArial10Regular = new Font("Arial", 10, FontStyle.Regular);
            Font drawFontArial10Bold = new Font("Arial", 10, FontStyle.Bold);
            Font drawFontArial9Regular = new Font("Arial", 9, FontStyle.Regular);
            Font drawFontArial9Bold = new Font("Arial", 9, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            string ticketType = ViewBag.TicketType;
            string ticketBody = ViewBag.TicketContent;
            var company = ViewBag.Company;
            // Draw string to screen.
            string text = company.CompanyName;
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            text = company.CompanyAddress + "\r\n Phone:" + company.Phone;
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial9Regular).Height;

            text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            e.Graphics.DrawString(text, drawFontArial9Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial9Regular).Height;

            text = "-------------  " + ticketType + "  -------------";
            e.Graphics.DrawString(text, drawFontArial10Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial10Bold).Height;

            //StringBuilder sb=new StringBuilder();

            //    sb.AppendLine("Ticket No:         AQ-00000001");
            //    sb.AppendLine("Name:              Md. Mahid Choudhury");
            //    sb.AppendLine("Contact :          019XXXXXXXX");
            //    sb.AppendLine("Email:             mahid@mail.com");
            //    sb.AppendLine("Address:           Gulshan, Dhaka");
            //    sb.AppendLine("Gender:            Male");
            //    sb.AppendLine("Age:               27 Y");
            //    sb.AppendLine("Ticket Price:      200");
            //    sb.AppendLine("Discount:          50");
            //    sb.AppendLine("Price (DSCNT):     -300");
            //    sb.AppendLine("VAT:               7.5");
            //    sb.AppendLine("Total Price:       157.5");
            //    sb.AppendLine("*************Thank You for visit*************");
            //    sb.AppendLine(" ");
            //    sb.AppendLine(" ");
            //    sb.AppendLine(" ");

            text = ticketBody;
            e.Graphics.DrawString(text, drawFontArial9Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
            y += e.Graphics.MeasureString(text, drawFontArial9Regular).Height;

            text = "\r\n*************Thank you for visit*************\r\n\r\n\r\n\r\n";
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial9Regular).Height;
            // ... and so on

        }


        //===========Invoice printinng========================================================================
        public ActionResult RePrintInvoice(int? id)
        {
            InvoicePrintModel invoicePm = new InvoicePrintModel();
          
            if (id != null)
            {
               Invoice  invoice = _salesService.GetInvoiceById(id.Value);
                if (invoice != null)
                {
                    List<ProductPrintModel> itemPmList = new List<ProductPrintModel>();
                    var sl = 1;
                    foreach (var invoiceProduct in invoice.InvoiceProducts)
                    {
                        //====add to printing model==============================================
                        var itemInfo = _productService.GetProductById(invoiceProduct.ProductId);
                        ProductPrintModel productItem = new ProductPrintModel()
                        {
                            Sl = sl,
                            ItemName = itemInfo.ProductFullName,
                            ItemCode = itemInfo.ProductCode,
                            UnitPrice = invoiceProduct.Dp,
                            Quantity = invoiceProduct.Quantity,
                            Total = invoiceProduct.TotalPrice
                        };
                        sl++;
                        itemPmList.Add(productItem);
                    }
                    
                var discountAmount = BankAccountController.DiscountCalculator(invoice.DiscountAmount, invoice.DiscountType, invoice.TotalPrice).DiscountValue;
                invoicePm.InvoiceNumber = invoice.InvoiceNumber;
                invoicePm.InvoiceDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}",invoice.CreatedDate);
                invoicePm.NetTotal = Convert.ToDouble(invoice.TotalPrice);
                invoicePm.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                invoicePm.DiscountType = invoice.DiscountType;
                invoicePm.VatAmount = Convert.ToDouble(invoice.TotalVat);
                //int ShopDeptId = db.Departments.FirstOrDefault(x => x.DepartmentName.Equals("Shop") && x.Status != 0).DepartmentId;
                //invoicePm.VatPercentage = Convert.ToDouble(db.Vats.FirstOrDefault(x => x.DepartmentId == ShopDeptId).VatPercentage);
                invoicePm.Total = Convert.ToDouble((invoicePm.NetTotal-discountAmount)) +  Convert.ToDouble(invoicePm.VatAmount);
                invoicePm.CashierName = Membership.GetUser(User.Identity.Name, true).UserName;
                invoicePm.Items = itemPmList;
                invoicePm.CashPaid = Convert.ToDouble(invoice.CashPaid);
                invoicePm.ReturnAmount = Convert.ToDouble(invoice.CashPaid) - invoicePm.Total;
                
                PrintInvoice(invoicePm);
               
                }
            }
          
           return RedirectToAction("InvoiceDetails", "Invoice", new { id = id });
        }
        public void PrintInvoice(InvoicePrintModel invoice)
        {
                var discountAmount = BankAccountController.DiscountCalculator(invoice.DiscountAmount, invoice.DiscountType, invoice.NetTotal);
                const int FIRST_COL_PAD =2;
                const int SECOND_COL_PAD = 28;
                const int THIRD_COL_PAD = 8;
                const int FORTH_COL_PAD = 3;
                const int FIFTH_COL_PAD = 7;
               
                var invoiceBodyBuider = new StringBuilder();
                invoiceBodyBuider.AppendLine("-----------------  RETAIL INVOICE  ----------------");
                invoiceBodyBuider.AppendLine(string.Format("{0} {1}", "Invoice No :".PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD), invoice.InvoiceNumber.PadRight(FIRST_COL_PAD + SECOND_COL_PAD)));
                invoiceBodyBuider.AppendLine(string.Format("{0} {1:dd-MM-yyyy hh:mm tt}", "Date & Time :".PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD), invoice.InvoiceDateTime.PadRight(FIRST_COL_PAD + SECOND_COL_PAD)));
                invoiceBodyBuider.AppendLine(string.Format("{0} {1}", "Cashier :".PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD), invoice.CashierName.PadRight(FIRST_COL_PAD + SECOND_COL_PAD)));
                if (invoice.PosNumber != null)
                {
                    invoiceBodyBuider.AppendLine(string.Format("{0} {1}", "POS Number :".PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD), invoice.PosNumber.PadRight(FIRST_COL_PAD + SECOND_COL_PAD)));
                }
                invoiceBodyBuider.AppendLine("---------------------------------------------------");
                invoiceBodyBuider.AppendLine(string.Format("{0} {1}{2} {3}{4}", "SL".PadRight(FIRST_COL_PAD),"Item Description".PadRight(SECOND_COL_PAD),"MRP".PadLeft(THIRD_COL_PAD),"Qty".PadRight(FORTH_COL_PAD), "Total".PadLeft(FIFTH_COL_PAD)));
                invoiceBodyBuider.AppendLine("---------------------------------------------------");
                foreach (var item in invoice.Items)
                {
                    invoiceBodyBuider.AppendLine(string.Format("{0} {1}{2} {3}{4}", item.Sl.ToString().PadRight(FIRST_COL_PAD), item.ItemName.PadRight(SECOND_COL_PAD), item.UnitPrice.ToString().PadLeft(THIRD_COL_PAD), item.Quantity.ToString().PadRight(FORTH_COL_PAD), string.Format("{0:0.00}", item.Total).PadLeft(FIFTH_COL_PAD)));
                }
                invoiceBodyBuider.AppendLine("---------------------------------------------------");
                invoiceBodyBuider.AppendLine(string.Format("{0}  {1}", "Sub Total :".PadLeft(FIRST_COL_PAD+SECOND_COL_PAD), invoice.NetTotal.ToString("0.00").PadLeft(THIRD_COL_PAD+FORTH_COL_PAD+FIFTH_COL_PAD)));
                
                if (discountAmount.DiscountValue > 0)
                {
                    invoiceBodyBuider.AppendLine(string.Format("{0}  {1}", "(-) Discount :".PadLeft(FIRST_COL_PAD + SECOND_COL_PAD), String.Format("{0}", discountAmount.DiscountValueShow).PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD)));
                }
                if (invoice.VatAmount > 0)
                {
                    invoiceBodyBuider.AppendLine(string.Format("{0}  {1}", "(+) VAT :".PadLeft(FIRST_COL_PAD + SECOND_COL_PAD), String.Format("{0:0.00}", invoice.VatAmount).PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD)));
                }
                invoiceBodyBuider.AppendLine("---------------------------------------------------");
                invoiceBodyBuider.AppendLine(string.Format("{0}  {1}", "Total Payable :".PadLeft(FIRST_COL_PAD + SECOND_COL_PAD), invoice.Total.ToString("0.00").PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD)));
                invoiceBodyBuider.AppendLine("---------------------------------------------------");
                invoiceBodyBuider.AppendLine(string.Format("{0}  {1}", "Cash Paid :".PadLeft(FIRST_COL_PAD + SECOND_COL_PAD), String.Format("{0:0.00}", invoice.CashPaid).PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD)));
                invoiceBodyBuider.AppendLine(string.Format("{0}  {1}", "Return Amount :".PadLeft(FIRST_COL_PAD + SECOND_COL_PAD), String.Format("{0:0.00}", invoice.ReturnAmount).PadLeft(THIRD_COL_PAD + FORTH_COL_PAD + FIFTH_COL_PAD)));
                invoiceBodyBuider.AppendLine("---------------------------------------------------");
                PrintInvoiceForTransaction(invoiceBodyBuider.ToString());
         
        }
        public void PrintInvoiceForTransaction(string invoiceBody)
        {
            //RfwDbContext cx = new RfwDbContext();
            ViewBag.Company = _companyProfileService.GetComapnyProfile();
            ViewBag.InvoiceContent = invoiceBody;

            PrintDocument recordDoc = new PrintDocument();
            recordDoc.DocumentName = "Customer Receipt";
            recordDoc.PrintPage += new PrintPageEventHandler(PrintInvoicePage);
            // function below
            recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
            // Comment if debugging 
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = GetDefaultPrinter();
            recordDoc.PrinterSettings = ps;
            recordDoc.Print();
            // --------------------------------------

            // Uncomment if debugging - shows dialog instead
            //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            //printPrvDlg.Document = recordDoc;
            //printPrvDlg.Width = 1200;
            //printPrvDlg.Height = 800;
            //printPrvDlg.ShowDialog();
            // --------------------------------------

            recordDoc.Dispose();

        }


        private void PrintInvoicePage(object sender, PrintPageEventArgs e)
        {


            float x = 0;
            float y = 1;
            float width = 290.0F; // max width I found through trial and error
            float height = 0F;

            Font drawFontMerchantCopy17Bold = new Font("Merchant Copy", 17, FontStyle.Bold);
            Font drawFontMerchantCopy12Regular = new Font("Merchant Copy", 12, FontStyle.Regular);
            Font drawFontMerchantCopy11Regular = new Font("Merchant Copy", 11, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            string invoiceContent = ViewBag.InvoiceContent;
            var company = ViewBag.Company;

           

           

            // Draw string to screen.
            //------------------------------Logo Printing----------------------------------
            //string logoPath = Server.MapPath("~/Content/Images/company-favicon.png");
            //using (Image logo = Image.FromFile(logoPath))
            //{
            //    e.Graphics.DrawImage(logo, new Point(0, 0));
            //    y += logo.Height;
            //}

            //------------------------------formatted Header----------------------------------
            var printText = new PrintText(company.CompanyName + "\r\n", drawFontMerchantCopy17Bold, drawFormatCenter);
            e.Graphics.DrawString(printText.Text, printText.Font, drawBrush, new RectangleF(x, y, width, height), printText.StringFormat);
            y += e.Graphics.MeasureString(printText.Text, printText.Font).Height;

         
            var headerText =new StringBuilder();
            headerText.AppendLine(company.CompanyAddress);
            if (company.WebSite != null)
            {
                headerText.AppendLine("Web: " + company.WebSite);
            }
            if (company.VatRegNo != null)
            {
                headerText.AppendLine("VAT Reg No: " + company.VatRegNo);
            }
            headerText.AppendLine();
            printText = new PrintText(headerText.ToString(), drawFontMerchantCopy12Regular, drawFormatCenter);
            e.Graphics.DrawString(printText.Text, printText.Font, drawBrush, new RectangleF(x, y, width, height), printText.StringFormat);
            y += e.Graphics.MeasureString(printText.Text, printText.Font).Height;

            //------------------------------formatted Body----------------------------------
            printText = new PrintText(invoiceContent, drawFontMerchantCopy11Regular, drawFormatLeft);
            e.Graphics.DrawString(printText.Text, printText.Font, drawBrush, new RectangleF(x, y, width, height), printText.StringFormat);
            y += e.Graphics.MeasureString(printText.Text, printText.Font).Height;

            //--------------------------------Formated Footer-----------------------------------

            var footerText = new StringBuilder();
            footerText.AppendLine();
            footerText.AppendLine("\r\nPurchase of defected item must be exchanged by 24 hours with receipt.");
            footerText.AppendLine("For any query please call @ " +company.Phone + " (9am-9pm).");
            footerText.AppendLine("\r\n************* Thank you for shopping *************\r\n");
            footerText.AppendLine("Developed By: Domain Software Technologies Ltd.\r\n");

            printText = new PrintText(footerText.ToString(), drawFontMerchantCopy11Regular, drawFormatLeft);
            e.Graphics.DrawString(printText.Text, printText.Font, drawBrush, new RectangleF(x, y, width, height), printText.StringFormat);
            y += e.Graphics.MeasureString(printText.Text, printText.Font).Height;
            // ... and so on

        }
        public string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;

                if (settings.IsDefaultPrinter)

                    return printer;

            }

            return "RONGTA RP80 Printer";

        }
    }

}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//