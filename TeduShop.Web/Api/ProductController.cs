using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : ApiControllerBase
    {
        #region [Khởi tạo ]
        IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService) : base(errorService)
        {
            this._productService = productService;
        }
        #endregion
        #region [Get]

        [Route("getallparents")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () => {

                var model = _productService.GetAll();


                var responeData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);


                var respone = request.CreateResponse(HttpStatusCode.OK, responeData);
                return respone;
            });
        }

        [Route("getbyid/{id:int}")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () => {

                var model = _productService.GetById(id);


                var responeData = Mapper.Map<Product, ProductViewModel>(model);


                var respone = request.CreateResponse(HttpStatusCode.OK, responeData);
                return respone;
            });
        }



        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () => {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);
                var query = model.Skip(page * pageSize).Take(pageSize).OrderByDescending(x => x.CreatedDate);
                totalRow = model.Count();
                var responeData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
                {

                    Items = responeData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPage = (int)Math.Ceiling((decimal)totalRow / pageSize)

                };
                var respone = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return respone;
            });
        }

        #endregion

        #region [Create]

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productViewModel)
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
                    var newproduct = new Product();
                    newproduct.UpdateProduct(productViewModel);
                    newproduct.CreatedDate = DateTime.Now;
                    newproduct.CreatedBy = User.Identity.Name;
                    _productService.Add(newproduct);
                    _productService.Save();
                    var responseData = Mapper.Map<Product, ProductViewModel>(newproduct);
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);
                    return respone;
                }
            });
        }

        #endregion

        #region [Update]
        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productViewModel)
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
                    var dbproduct = _productService.GetById(productViewModel.ID);
                    dbproduct.UpdateProduct(productViewModel);
                    dbproduct.UpdatedDate = DateTime.Now;
                    _productService.Update(dbproduct);
                    _productService.Save();
                    var responseData = Mapper.Map<Product, ProductViewModel>(dbproduct);
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);
                    return respone;
                }
            });
        }
        #endregion


        #region [Delete]
        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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

                    var oldProduct = _productService.Delete(id);

                    _productService.Save();
                    var responseData = Mapper.Map<Product, ProductViewModel>(oldProduct);
                    respone = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return respone;
                }
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string listProducts)
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

                    var ids = new JavaScriptSerializer().Deserialize<List<int>>(listProducts);
                    foreach (var id in ids)
                    {
                        _productService.Delete(id);
                    }
                    _productService.Save();

                    respone = request.CreateResponse(HttpStatusCode.OK, ids.Count);
                    return respone;
                }
            });
        }

        #endregion

    }
}
