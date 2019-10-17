using PurchaseSystem.Common;
using PurchaseSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PurchaseSystem.Controllers
{
    public class productTypeController : Controller
    {
        ApplicationDbContext _db;
        public productTypeController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: productType
        public ActionResult Create()
        { 
            return View("CreateUpdateForm");
        }

        [HttpPost]
        public ActionResult Create(ProductTypeMst PT)
        {
            _db.ProductTypeMsts.Add(PT);
            _db.SaveChanges();
            return View();
        }

        public ActionResult ProductTypeList()
        {
            var prodlist = _db.ProductTypeMsts.ToList();
            return View(prodlist);
        }

        public ActionResult Edit(int id)
        {
            var productType = _db.ProductTypeMsts.FirstOrDefault(a=>a.pk_prodtypeid == id);
            return View("CreateUpdateForm", productType);

        }


    }
}