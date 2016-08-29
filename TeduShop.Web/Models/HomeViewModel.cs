﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<SlideViewModel> Slides { get; set; }
        public IEnumerable<ProductViewModel> SpecialProducts { get; set; }
        public IEnumerable<ProductViewModel> HotProducts { get; set; }
        public IEnumerable<ProductViewModel> NewProducts { get; set; }
    }
}