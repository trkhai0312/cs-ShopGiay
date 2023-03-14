using PagedList;
using ShopGiay.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class GiayController : Controller
    {
        DataClassesDataContext data = new DataClassesDataContext();
        // GET: Sach
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {

                if (page == null) page = 1;
                int pageSize = 9;
                int pageNumber = (page ?? 1);
                var giay = from g in data.GIAYs select g;
                return View(giay.ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var giay = from g in data.GIAYs where g.MAGIAY == id select g;
                if (giay == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(giay.Single());
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MALOAI = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
                ViewBag.MATH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TENTH), "MATH", "TENTH");
                ViewBag.MAMAUSAC = new SelectList(data.MAUSACs.ToList().OrderBy(n => n.TENMAUSAC), "MAMAUSAC", "TENMAUSAC");
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(GIAY giay, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MALOAI = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
                ViewBag.MATH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TENTH), "MATH", "TENTH");
                ViewBag.MAMAUSAC = new SelectList(data.MAUSACs.ToList().OrderBy(n => n.TENMAUSAC), "MAMAUSAC", "TENMAUSAC");
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    giay.HINHANH = fileName;
                    giay.SOLUONG = 0;
                    data.GIAYs.InsertOnSubmit(giay);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "Giay");
                }

            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var giay = from g in data.GIAYs where g.MAGIAY == id select g;

                ViewBag.MALOAI = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
                ViewBag.MATH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TENTH), "MATH", "TENTH");
                ViewBag.MAMAUSAC = new SelectList(data.MAUSACs.ToList().OrderBy(n => n.TENMAUSAC), "MAMAUSAC", "TENMAUSAC");
                return View(giay.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                GIAY giay = data.GIAYs.SingleOrDefault(g => g.MAGIAY == id);
                ViewBag.MALOAI = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TENLOAI), "MALOAI", "TENLOAI");
                ViewBag.MATH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TENTH), "MATH", "TENTH");
                ViewBag.MAMAUSAC = new SelectList(data.MAUSACs.ToList().OrderBy(n => n.TENMAUSAC), "MAMAUSAC", "TENMAUSAC");
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                    fileUpload.SaveAs(path);
                    giay.HINHANH = fileName;
                    UpdateModel(giay);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "Giay");
                }
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var giay = from g in data.GIAYs where g.MAGIAY == id select g;
                return View(giay.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                GIAY giay = data.GIAYs.SingleOrDefault(g => g.MAGIAY == id);
                var kichthuoc = from KICHTHUOC in data.KICHTHUOCs where KICHTHUOC.MAGIAY == id select KICHTHUOC;
                var hinh = from HINH in data.HINHs where HINH.MAGIAY == id select HINH;
                var kho = from PHIEUNHAPKHO in data.PHIEUNHAPKHOs where PHIEUNHAPKHO.MAGIAY == id select PHIEUNHAPKHO;
                var dathang = from CTDONDATHANG in data.CTDONDATHANGs where CTDONDATHANG.MAGIAY == id select CTDONDATHANG;
                var dondathang = from DONDATHANG in data.DONDATHANGs select DONDATHANG;
                foreach (var item in dathang)
                {
                    data.CTDONDATHANGs.DeleteOnSubmit(item);
                }
                /*foreach (var item in dathang)
                {
                    foreach (var itam in dondathang)
                    {
                        if (itam.MADH != item.MADH)
                        {
                            data.DONDATHANGs.DeleteOnSubmit(itam);
                        }
                    }
                }*/
                foreach (var item in hinh)
                {
                    data.HINHs.DeleteOnSubmit(item);
                }
                foreach (var item in kho)
                {
                    data.PHIEUNHAPKHOs.DeleteOnSubmit(item);
                }
                foreach (var item in kichthuoc)
                {
                    data.KICHTHUOCs.DeleteOnSubmit(item);
                }
                data.GIAYs.DeleteOnSubmit(giay);
                data.SubmitChanges();
                return RedirectToAction("Index", "Giay");
            }
        }
    }
}