using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanDoAnVaThucUong.Models.EF;

namespace WebsiteBanDoAnVaThucUong.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<ImageSlider> ImageSliders { get; set; }
        public List<StoreDTO> Stores { get; set; }

    }
}