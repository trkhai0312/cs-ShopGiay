using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopGiay.Models
{
    public class Giohang
    {
        DataClassesDataContext data = new DataClassesDataContext();
        /*FormCollection f;*/
        public int iMAGIAY { set; get; }
        public string gTENGIAY { set; get; }
        public string gHINHANH { set; get; }
        public Double dDONGIA { set; get; }
        public int iSOLUONG { set; get; }
        public Double dTHANHTIEN
        {
            get { return iSOLUONG * dDONGIA; }

        }
        public Giohang(int MAGIAY)
        {

            iMAGIAY = MAGIAY;
            GIAY giay = data.GIAYs.Single(n => n.MAGIAY == iMAGIAY);
            gTENGIAY = giay.TENGIAY;
            gHINHANH = giay.HINHANH;
            dDONGIA = double.Parse(giay.DONGIABAN.ToString());
            iSOLUONG = 1;
            /*iSOLUONG = int.Parse(f["SoLuong"].ToString());*/
        }
    }
}