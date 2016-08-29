using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Models;
using TeduShop.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;

namespace TeduShop.Web.Api
{
    [RoutePrefix("api/productcategory")]
    [Authorize]
    public class ProductCategoryController : ApiControllerBase
    {

        #region [Khởi tạo ]
        IProductCategoryService _productCategoryService;

        public ProductCategoryController (IErrorService errorService,IProductCategoryService productCategoryService) : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }
        #endregion


        #region [Get]

        [Route("getallparents")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () => {
            
                var model = _productCategoryService.GetAll();
              
               
                var responeData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                
                var respone = request.CreateResponse(HttpStatusCode.OK, responeData);
                return respone;
            });
        }

        [Route("getbyid/{id:int}")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request,int id)
        {
            return CreateHttpResponse(request, () => {

                var model = _productCategoryService.GetById(id);


                var responeData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(model);


                var respone = request.CreateResponse(HttpStatusCode.OK, responeData);
                return respone;
            });
        }



        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request,string keyword,int page,int pageSize=20)
        {
            return CreateHttpResponse(request, () => {
                int totalRow = 0;
                var model = _productCategoryService.GetAll(keyword);
                var query = model.Skip(page * pageSize).Take(pageSize).OrderByDescending(x => x.CreatedDate);
                totalRow = model.Count();
                var responeData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<ProductCategoryViewModel>() {

                    Items = responeData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPage =(int) Math.Ceiling((decimal)totalRow / pageSize)

                };
                var respone = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return respone;
            });
        }

        #endregion

        #region [Create]

        [Route("create")]
        [HttpPost]
 
        public HttpResponseMessage Create(HttpRequestMessage request,ProductCategoryViewModel productcategoryViewModel)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid) {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;
                    
                    }
                else
                {
                    var newproductcategory = new ProductCategory();
                    newproductcategory.UpdateProductCategory(productcategoryViewModel);
                    newproductcategory.CreatedDate = DateTime.Now;
                    newproductcategory.CreatedBy = User.Identity.Name;
                    _productCategoryService.Add(newproductcategory);
                    _productCategoryService.Save();
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newproductcategory);
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);
                    return respone;
                }
            });
        }

        #endregion

        #region [Update]
        [Route("update")]
        [HttpPut]
   
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productcategoryViewModel)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {
                    var dbproductcategory = _productCategoryService.GetById(productcategoryViewModel.ID);
                    dbproductcategory.UpdateProductCategory(productcategoryViewModel);
                    dbproductcategory.UpdatedDate = DateTime.Now;
                    dbproductcategory.UpdatedBy = User.Identity.Name;
                    _productCategoryService.Update(dbproductcategory);
                    _productCategoryService.Save();
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbproductcategory);
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);
                    return respone;
                }
            });
        }
        #endregion


        #region [Delete]
        [Route("delete")]
        [HttpDelete]
  
        public HttpResponseMessage Delete(HttpRequestMessage request,int id)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {

                 var oldProductCategory=   _productCategoryService.Delete(id);
                   
                    _productCategoryService.Save();
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(oldProductCategory);
                    respone = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return respone;
                }
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string listProductCategories)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {

                    var ids = new JavaScriptSerializer().Deserialize<List<int>>(listProductCategories);
                    foreach(var id in ids)
                    {
                        _productCategoryService.Delete(id);
                    }
                    _productCategoryService.Save();
                   
                    respone = request.CreateResponse(HttpStatusCode.OK, ids.Count);
                    return respone;
                }
            });
        }

        #endregion

    }
}
