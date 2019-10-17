using PurchaseSystem.Common;
using PurchaseSystem.DTO;
using PurchaseSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PurchaseSystem.Controllers
{
    public class ComBillController : Controller
    {

        ApplicationDbContext _db;
        public ComBillController()
        {
            _db = new ApplicationDbContext();
        }


        // GET: ComBill
        public ActionResult Index()
        {
            return View();
        }
     
        public Com_Bill_DTO GetPageData()
        {
            
            Com_Bill_DTO comBill = new Com_Bill_DTO();
            ProductMst pmst;

            if (User.IsInRole("Admin"))
            {
                var prodList = from productList in _db.ProductMsts
                               select new ProductDDL_DTO
                               {
                                   productId = productList.pk_ProductId,
                                   ProductName = productList.ProductName
                               };
                comBill.ProductList = prodList.ToList();


            }
            else if (User.IsInRole("General Store"))
            {
                var prodList = from productList in _db.ProductMsts.Where(a => a.username == User.Identity.Name)
                               select new ProductDDL_DTO
                               {
                                   productId = productList.pk_ProductId,
                                   ProductName = productList.ProductName
                               };
                comBill.ProductList = prodList.ToList();

            }
            else
            {
                var prodList = from productList in _db.ProductMsts.Where(a => a.username == User.Identity.Name)
                               select new ProductDDL_DTO
                               {
                                   productId = productList.pk_ProductId,
                                   ProductName = productList.ProductName
                               };
                comBill.ProductList = prodList.ToList();
            }


            return comBill;
        }

        public JsonResult GetProductList(string productName)
        {
            IEnumerable<ProductDDL_DTO> prodList = new List<ProductDDL_DTO>();
            if (User.IsInRole("Admin"))
            {
                prodList = from productList in _db.ProductMsts
                           where productList.ProductName.Contains(productName)
                           select new ProductDDL_DTO
                           {
                               productId = productList.pk_ProductId,
                               ProductName = productList.ProductName
                           };



            }
            else if (User.IsInRole("General Store"))
            {
                 prodList = from productList in _db.ProductMsts.Where(a => a.username == User.Identity.Name)
                               select new ProductDDL_DTO
                               {
                                   productId = productList.pk_ProductId,
                                   ProductName = productList.ProductName
                               };
               

            }
            else
            {
                prodList = from productList in _db.ProductMsts.Where(a => a.username == User.Identity.Name)
                               select new ProductDDL_DTO
                               {
                                   productId = productList.pk_ProductId,
                                   ProductName = productList.ProductName
                               };
                
            }
            return Json(prodList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalculatePrice(int selectedProductId, double countOrWeight)
        {
            ProductMst product = _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == selectedProductId);
            double price = countOrWeight * product.sellingUpToPrice;

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveUpdateBill(int id)
        {
            Com_Bill_DTO item = new Com_Bill_DTO();
            CustomerMst cust;
            if (User.IsInRole("Admin"))
            {
                cust = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == id);
            }
            else
            {
                cust = _db.CustomerMsts.FirstOrDefault(a => a.pk_Custid == id && a.Username == User.Identity.Name);
            }
            item = GetPageData();
            item.CustomerMst = cust;
            return View(item);
        }
    }
}