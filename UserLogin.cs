using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopGiay
{
    [Serializable]
    public class UserLogin
    {
        public int makh { set; get; }
        public string tenkh { set; get; }
    }
}