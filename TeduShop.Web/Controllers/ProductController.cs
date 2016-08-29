using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeduShop.Service;
using TeduShop.Model.Models;
using TeduShop.Web.Models;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Common;
using System.Web.Script.Serialization;

namespace TeduShop.Web.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        IProductService _productService;
        IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService,IProductCategoryService productCategoryService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProductByAlias(string alias)
        {
            var product = _productService.GetProductByAlias(alias);
            var productViewModel = Mapper.Map<Product,ProductViewModel>(product);

            var relatedProduct = _productService.GetReatedProducts(product.ID,6);
            ViewBag.RelatedProduct = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(relatedProduct);
            List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(productViewModel.MoreImages);
            ViewBag.MoreImages = listImages;

            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_productService.GetListTagsByProductId(product.ID));

            return View(productViewModel);
        }

        public ActionResult GetProductCategory(string alias,int page=1,string sort="")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var category = _productCategoryService.GetByAlias(alias);
            
            var productModel = _productService.GetListProductByCategoryId(category.ID, page,sort, pageSize, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>() {
                Items= productViewModel,
                MaxPage= int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page=page,
                TotalCount=totalRow,
                TotalPage=totalPage
            };


            return View(paginationSet);
        }



    }
}