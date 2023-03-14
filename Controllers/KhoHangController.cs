using ShopGiay.Models;
using System.Linq;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class KhoHangController : Controller
    {
        DataClassesDataContext data = new DataClassesDataContext();
        // GET: Kho Hang
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kho = from k in data.PHIEUNHAPKHOs select k;
                return View(kho);
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kho = from k in data.PHIEUNHAPKHOs where k.MAPHIEUNK == id select k;
                return View(kho.Single());
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
        public ActionResult Create(PHIEUNHAPKHO kho)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MAGIAY = new SelectList(data.GIAYs.ToList().OrderBy(n => n.TENGIAY), "MAGIAY", "TENGIAY");
                data.PHIEUNHAPKHOs.InsertOnSubmit(kho);
                GIAY giay = data.GIAYs.Single(n => n.MAGIAY == kho.MAGIAY);
                giay.SOLUONG = giay.SOLUONG + kho.SOLUONG;
                data.SubmitChanges();
                return RedirectToAction("Index", "KhoHang");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kho = from k in data.PHIEUNHAPKHOs where k.MAPHIEUNK == id select k;
                ViewBag.MAGIAY = new SelectList(data.GIAYs.ToList().OrderBy(n => n.TENGIAY), "MAGIAY", "TENGIAY");
                return View(kho.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                ViewBag.MAGIAY = new SelectList(data.GIAYs.ToList().OrderBy(n => n.TENGIAY), "MAGIAY", "TENGIAY");
                PHIEUNHAPKHO kho = data.PHIEUNHAPKHOs.SingleOrDefault(n => n.MAPHIEUNK == id);
                GIAY giay = data.GIAYs.SingleOrDefault(n => n.MAGIAY == kho.MAGIAY);
                var lstkho = from k in data.PHIEUNHAPKHOs where k.MAGIAY == giay.MAGIAY select k;
                UpdateModel(kho);
                giay.SOLUONG = 0;
                foreach (var item in lstkho)
                {
                    giay.SOLUONG = giay.SOLUONG + item.SOLUONG;
                }
                data.SubmitChanges();
                return RedirectToAction("Index", "KhoHang");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                var kho = from k in data.PHIEUNHAPKHOs where k.MAPHIEUNK == id select k;
                return View(kho.Single());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("dangnhap", "Admin");
            else
            {
                PHIEUNHAPKHO kho = data.PHIEUNHAPKHOs.SingleOrDefault(n => n.MAPHIEUNK == id);
                data.PHIEUNHAPKHOs.DeleteOnSubmit(kho);
                GIAY giay = data.GIAYs.Single(n => n.MAGIAY == kho.MAGIAY);
                if (giay.SOLUONG > kho.SOLUONG)
                {
                    giay.SOLUONG = giay.SOLUONG - kho.SOLUONG;
                }
                else
                {
                    return RedirectToAction("ThongBao", "KhoHang");
                }
                data.SubmitChanges();
                return RedirectToAction("Index", "KhoHang");
            }
        }
        public ActionResult ThongBao()
        {
            return View();
        }
    }
}