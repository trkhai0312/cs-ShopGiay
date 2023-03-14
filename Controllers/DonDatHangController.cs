using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class DonDatHangController : Controller
    {
        DataClassesDataContext data = new DataClassesDataContext();
        // GET: DonDatHang
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var ddh = from d in data.DONDATHANGs select d;
                return View(ddh);
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var ctdh = from ct in data.CTDONDATHANGs where ct.MADH == id select ct;
                return View(ctdh.Single());
            }
        }
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var ddh = from d in data.DONDATHANGs where d.MADH == id select d;
                return View(ddh.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                DONDATHANG ddh = data.DONDATHANGs.SingleOrDefault(n => n.MADH == id);
                data.DONDATHANGs.DeleteOnSubmit(ddh);
                data.SubmitChanges();
                return RedirectToAction("Index", "DonDatHang");
            }
        }
    }
}