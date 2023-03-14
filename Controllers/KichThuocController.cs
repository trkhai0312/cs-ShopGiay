﻿using PagedList;
using ShopGiay.Models;
using System.Linq;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class KichThuocController : Controller
    {
        // GET: KichThuoc
        DataClassesDataContext data = new DataClassesDataContext();      
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {

                if (page == null) page = 1;
                int pageSize = 21;
                int pageNumber = (page ?? 1);
                var kichthuoc = from kt in data.KICHTHUOCs select kt;
                return View(kichthuoc.ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kichthuoc = from kt in data.KICHTHUOCs where kt.MAKICHTHUOC == id select kt;
                if (kichthuoc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kichthuoc.Single());
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MAGIAY = new SelectList(data.GIAYs.ToList().OrderBy(n => n.TENGIAY), "MAGIAY", "TENGIAY");
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(KICHTHUOC kichthuoc)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MAGIAY = new SelectList(data.GIAYs.ToList().OrderBy(n => n.TENGIAY), "MAGIAY", "TENGIAY");
                data.KICHTHUOCs.InsertOnSubmit(kichthuoc);
                data.SubmitChanges();
                return RedirectToAction("Index", "KichThuoc");

            }
        }

        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kichthuoc = from kt in data.KICHTHUOCs where kt.MAKICHTHUOC == id select kt;
                return View(kichthuoc.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                KICHTHUOC kichthuoc = data.KICHTHUOCs.SingleOrDefault(n => n.MAKICHTHUOC == id);
                data.KICHTHUOCs.DeleteOnSubmit(kichthuoc);
                data.SubmitChanges();
                return RedirectToAction("Index", "KichThuoc");
            }
        }
    }
}