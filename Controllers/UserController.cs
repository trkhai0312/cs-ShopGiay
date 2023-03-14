using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace ShopGiay.Controllers
{
    public class UserController : Controller
    {
        DataClassesDataContext data = new DataClassesDataContext();

        public object CommonConstants { get; private set; }      
        // GET: User
        public ActionResult index()
        {
           
            var allacc = (from a in data.GIAYs
                         join b in data.THUONGHIEUs on a.MATH equals b.MATH
                         join c in data.LOAIs on a.MALOAI equals c.MALOAI
                         join d in data.MAUSACs on a.MAMAUSAC equals d.MAMAUSAC
                  
                         select new ProductViewModel
                         {
                             MAGIAY = a.MAGIAY,
                             TENGIAY = a.TENGIAY,
                             DONGIABAN = a.DONGIABAN,
                             HINHANH = a.HINHANH,
                             MATH = a.MATH,
                             MALOAI = a.MALOAI,
                             TENTH = b.TENTH,
                             TENLOAI = c.TENLOAI,
                             SOLUONG = a.SOLUONG,
                             MOTA = a.MOTA,
                             TENMAUSAC = d.TENMAUSAC,                 
                             LOGO = b.LOGO
                         }).OrderBy(x=>x.MAGIAY).Take(count:6).ToList();
            return View(allacc);
        }
        public ActionResult blogs()
        {
            return View();
        }
        public ActionResult sanpham(int? page)
        {
            if (page == null) page = 1;
            //Số mẫu tin tối đa cho 1 trang
            int pagesize = 9;
            //Nếu biến page là null thì pagenum=1, ngược pagenum = page.
            int pagenum = (page ?? 1);
            var allacc = (from a in data.GIAYs
                         join b in data.THUONGHIEUs on a.MATH equals b.MATH
                         join c in data.LOAIs on a.MALOAI equals c.MALOAI
                         join d in data.MAUSACs on a.MAMAUSAC equals d.MAMAUSAC

                         select new ProductViewModel
                         {
                             MAGIAY = a.MAGIAY,
                             TENGIAY = a.TENGIAY,
                             DONGIABAN = a.DONGIABAN,
                             HINHANH = a.HINHANH,
                             MATH = a.MATH,
                             MALOAI = a.MALOAI,
                             TENTH = b.TENTH,
                             TENLOAI = c.TENLOAI,
                             SOLUONG = a.SOLUONG,
                             MOTA = a.MOTA,
                             TENMAUSAC = d.TENMAUSAC,

                         }).OrderBy(x => x.MAGIAY);
            return View(allacc.ToPagedList(pagenum,pagesize));
        }
        public ActionResult hinhanh(int id)
        {
            var listhinhanh = from HINH in data.HINHs where HINH.MAGIAY == id select HINH;
            return PartialView(listhinhanh);
        }
        public ActionResult listhinhanhnho(int id)
        {
            var listhinhanhnho = from HINH in data.HINHs where HINH.MAGIAY == id select HINH;
            return PartialView(listhinhanhnho);
        }
        public ActionResult listhinhanhnhoduoi(int id)
        {
            var listhinhanhnho = from HINH in data.HINHs where HINH.MAGIAY == id select HINH;
            return PartialView(listhinhanhnho);
        }
        public ActionResult Chitiet(int id)
        {
            var detail = from a in data.GIAYs
                         join b in data.THUONGHIEUs on a.MATH equals b.MATH
                         join c in data.LOAIs on a.MALOAI equals c.MALOAI
                         join d in data.MAUSACs on a.MAMAUSAC equals d.MAMAUSAC                 
                         join h in data.HINHs on a.MAGIAY equals h.MAGIAY
                         where a.MAGIAY == id
                         select new ProductViewModel
                         {
                             MAGIAY = a.MAGIAY,                           
                             TENGIAY = a.TENGIAY,
                             DONGIABAN = a.DONGIABAN,
                             HINHANH = a.HINHANH,
                             MATH = a.MATH,
                             MALOAI = a.MALOAI,
                             TENTH = b.TENTH,
                             TENLOAI = c.TENLOAI,
                             SOLUONG = a.SOLUONG,
                             MOTA = a.MOTA,
                             TENMAUSAC = d.TENMAUSAC,                            
                             HINH1 = h.HINH1,
                             THANHTOANON = a.THANHTOANON
                         };
            return View(detail.FirstOrDefault());
        }       
        public ActionResult gioithieu()
        {
            return View();
        }
        
       
        public ActionResult loai()
        {
            var listloai = from LOAI in data.LOAIs select LOAI;
            return PartialView(listloai);
        }
        public ActionResult kichthuoc()
        {
            var listkichthuoc = from KICHTHUOC in data.KICHTHUOCs select KICHTHUOC;
            return PartialView(listkichthuoc);
        }
        public ActionResult thuonghieu()
        {
            var listthuonghieu = from THUONGHIEU in data.THUONGHIEUs select THUONGHIEU;
            return PartialView(listthuonghieu);
        }
        public ActionResult hinhthuonghieu()
        {
            var listthuonghieu = from THUONGHIEU in data.THUONGHIEUs select THUONGHIEU;
            return PartialView(listthuonghieu);
        }
        public ActionResult SPTheoloai(int id)
        {
            var sanpham = from GIAY in data.GIAYs where GIAY.MALOAI == id select GIAY;
            return View(sanpham);
        }
     
        public ActionResult SPTheothuonghieu(int id)
        {
            var sanpham = from GIAY in data.GIAYs where GIAY.MATH == id select GIAY;
            return View(sanpham);
        }


        //Đăng ký và Đăng Nhập
        [HttpGet]
        public ActionResult dangky()
        {
            return View();
        }      
        
      
        public bool kiemtratendn(string tendn)
        {
            return data.KHACHHANGs.Count(x => x.TENDNKH == tendn) > 0;
        }
        public bool kiemtraemail(string email)
        {
            return data.KHACHHANGs.Count(x => x.EMAIL == email) > 0;
        }
        [HttpPost]
        public ActionResult dangky(DangkyModel model)
        {
            if (ModelState.IsValid) {
                if (kiemtratendn(model.tendn))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (kiemtraemail(model.email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var kh = new KHACHHANG();
                    kh.HOTENKH = model.hoten;
                    kh.TENDNKH = model.tendn;
                    kh.MATKHAUKH = model.matkhau;                
                    kh.EMAIL = model.email;
                    kh.DIACHI = model.diachi;
                    kh.DIENTHOAI = model.dienthoai;
                    kh.HINHANH = model.hinhanh;
                    kh.NGAYSINH = model.ngaysinh;
                    data.KHACHHANGs.InsertOnSubmit(kh);
                    data.SubmitChanges();                   
                    return RedirectToAction("dangnhap");
                }
            } return View(model);
        }
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        }
        [HttpPost]      
        public ActionResult dangnhap(DangnhapModel model)
        {
            if (ModelState.IsValid)
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TENDNKH == model.tendn && n.MATKHAUKH == model.matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;               
                    return RedirectToAction("index","User");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            else
            {

            }
            return View(model);
        }
        public ActionResult thongtincanhan()
        {
            if(Session["Taikhoan"] == null)
            {
                return RedirectToAction("index", "User");

            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("index", "User");
            else
            {
                var thongtin = from tt in data.KHACHHANGs where tt.MAKH == id select tt;
                return View(thongtin.Single());
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateInput(false)]
        public ActionResult Capnhat(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoan"] == null)
                return RedirectToAction("index", "User");
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == id);
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
                    kh.HINHANH = fileName;
                    Session["Taikhoan"] = kh;
                    UpdateModel(kh);
                    data.SubmitChanges();
                    return RedirectToAction("thongtincanhan", "User");
                }             
            }
        }
        public ActionResult dangxuat()
        {
            Session.Clear();
            return RedirectToAction("index","User");
        }
        public ActionResult Ketquatimkiem(string searchString)
        {

            var links = from l in data.GIAYs
                        select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.TENGIAY.Contains(searchString));
            }

            return View(links);
        }
        public ActionResult KTtheoMaGiay(int id)
        {
            var KichThuoc = from KICHTHUOC in data.KICHTHUOCs where KICHTHUOC.MAGIAY == id select KICHTHUOC;
            return PartialView(KichThuoc);
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailId, string activationCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("lo9ngkhung@outlook.com.vn", "Vũ Quang Long");
            var toEmail = new MailAddress(emailId);
            var fromEmailPassword = "Longpi@123"; // replace with actual password
            string subject = "";
            string body = "";
            if (emailFor == "ResetPassword")
            {
                subject = "Đặt lại mật khẩu";
                body = "Xin chào,<br/><br/> Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu của bạn. Vui lòng nhấp vào liên kết dưới đây để thiết lập lại tài khoản của bạn " + "<br/><br/><a href=" + link + ">Link đặt lại mật khẩu</a>";
            }



            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) smtp.Send(message);
        }
        [HttpGet]
        public ActionResult QuenMK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuenMK(QuenMKModel quenMK)
        {
            if (ModelState.IsValid)
            {
                //Xác nhận email
                //tạo liên kết đặt lại mật khẩu
                //gửi email               
                
                var account = data.KHACHHANGs.SingleOrDefault(n => n.EMAIL == quenMK.Email);
                if (account != null)
                {
                    //send email for reset password
                    
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EMAIL, resetCode, "ResetPassword");
                    account.KHOIPHUCMATKHAU = resetCode;                  
                    data.SubmitChanges();
                    return RedirectToAction("QuenMKxacnhan", "User");

                }
                else {
                    
                    ViewBag.message = "Email không đúng";
                }
                    
            }
            else
            {

            }
            return View(quenMK);
        }
        public ActionResult ResetPassword(string id)
        {
            //xác nhận liên kết đặt lại mật khẩu
            //tìm tài khoản được chỉ định với liên kết này
            //chuyển hướng để đặt lại trang mật khẩu
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.KHOIPHUCMATKHAU == id);
            if(kh != null)
            {
                ResetPassword model = new ResetPassword();
                model.Resetcode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.KHOIPHUCMATKHAU == model.Resetcode);
                if(kh!= null)
                {
                    kh.MATKHAUKH = model.NewPassword;
                    kh.KHOIPHUCMATKHAU = "";
                    UpdateModel(kh);
                    data.SubmitChanges();
                    message = "Cập nhật mật khẩu mới thành công ";
                    return RedirectToAction("dangnhap", "User");
                }
            }
            else
            {
                message = "Điều gì đó không hợp lệ";

            }
            ViewBag.Message = message;
            return View(model);
        }
        public ActionResult QuenMKxacnhan()
        {
            return View();
        }
        [NonAction]
        public void sendcontact(string Name, string Email, string Subject, string Content)
        {

            var fromEmail = new MailAddress("lo9ngkhung@outlook.com.vn");
            var toEmail = new MailAddress("long9map@gmail.com");
            var fromEmailPassword = "Longpi@123"; // replace with actual password
            string subject = Subject;
            string body = "<br/> Họ tên: " + Name + "<br/><br/> Email: " + " " + Email + "<br/><br/> Nội dung: " + Content;

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) smtp.Send(message);
        }
        [HttpGet]
        public ActionResult Lienhe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Lienhe(LienheModel lienhe)
        {
            if (ModelState.IsValid)
            {
                sendcontact(lienhe.Name, lienhe.Email, lienhe.Subject, lienhe.Message);
                return RedirectToAction("thongbaolienhe", "User");

            }
            else
            {

            }
            return View(lienhe);
        }
        public ActionResult thongbaolienhe()
        {
            return View();
        }
    }
}