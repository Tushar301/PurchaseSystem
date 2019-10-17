using PurchaseSystem.DTO;
using PurchaseSystem.Common;
using PurchaseSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PurchaseSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        
        ApplicationDbContext _db;
        public ProductController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: Product
        [HttpGet]
        public ActionResult AddUpdateProduct()
        {
            var pageLoadData = new ProductMstDTO
            {
                ProductTypeMstList = _db.ProductTypeMsts.ToList()
        };

            return View(pageLoadData);
        }
   
        public ActionResult ProductList()
        {
            IEnumerable<ProductListDTO> ProductList;
            if (User.IsInRole("Admin"))
            {
                ProductList = from a in _db.ProductMsts
                              join b in _db.ProductTypeMsts on a.fk_prodtypeid equals b.pk_prodtypeid
                              select new ProductListDTO
                              {
                                  pk_ProductId = a.pk_ProductId,
                                  productType = b.Description,
                                  ProductName = a.ProductName,
                                  oriPrice = a.oriPrice,
                                  productQuantity = a.productQuantity,
                                  sellingUpToPrice = a.sellingUpToPrice
                              };
            }
            else
            {
                ProductList = from a in _db.ProductMsts
                              join b in _db.ProductTypeMsts on a.fk_prodtypeid equals b.pk_prodtypeid
                              where a.username == User.Identity.Name
                              select new ProductListDTO
                              {
                                  pk_ProductId = a.pk_ProductId,
                                  productType = b.Description,
                                  ProductName = a.ProductName,
                                  oriPrice = a.oriPrice,
                                  productQuantity = a.productQuantity,
                                  sellingUpToPrice = a.sellingUpToPrice
                              };

            }
            return View(ProductList);
        }

        public ActionResult Delete(int id)
        {

            var datafordelete = _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == id);
            _db.ProductMsts.Remove(datafordelete);
            _db.SaveChanges();
               
            return RedirectToAction("ProductList");
        }
        public ActionResult Edit(int id)
        {
            var EditData = new ProductMstDTO
            {
                productMst = _db.ProductMsts.FirstOrDefault(a=> a.pk_ProductId == id),
                ProductTypeMstList = _db.ProductTypeMsts.ToList()
            };
            return View("AddUpdateProduct",EditData);
        }

        [HttpPost]
        public ActionResult AddUpdateProduct(ProductMstDTO product)
        {

            if(product.productMst.pk_ProductId == 0) {

                product.productMst.username = User.Identity.Name;
                _db.ProductMsts.Add(product.productMst);
                _db.SaveChanges();


            }
            else
            {
                var datainDb = _db.ProductMsts.FirstOrDefault(a => a.pk_ProductId == product.productMst.pk_ProductId);

                datainDb.fk_prodtypeid = product.productMst.fk_prodtypeid;
                datainDb.ProductName = product.productMst.ProductName;
                datainDb.productQuantity = product.productMst.productQuantity;
                datainDb.sellingUpToPrice = product.productMst.sellingUpToPrice;
                datainDb.oriPrice = product.productMst.oriPrice;

                _db.SaveChanges();
            }

            return RedirectToAction("ProductList");
        }
    }
}