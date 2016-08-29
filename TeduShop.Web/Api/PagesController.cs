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

    [RoutePrefix("api/page")]
    [Authorize]
    public class PagesController : ApiControllerBase
    {
        #region [Khởi tạo ]
        IPageService _pageService;

        public PagesController(IErrorService errorService, IPageService pageService) : base(errorService)
        {
            this._pageService = pageService;
        }
        #endregion

        #region [Get]

        [Route("getallparents")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () => {

                var model = _pageService.GetAll();


                var responeData = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(model);


                var respone = request.CreateResponse(HttpStatusCode.OK, responeData);
                return respone;
            });
        }

        [Route("getbyid/{id:int}")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () => {

                var model = _pageService.GetById(id);
                var responeData = Mapper.Map<Page, PageViewModel>(model);
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
                var model = _pageService.GetAll(keyword);
                var query = model.Skip(page * pageSize).Take(pageSize).OrderByDescending(x => x.CreatedDate);
                totalRow = model.Count();
                var responeData = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(query);

                var paginationSet = new PaginationSet<PageViewModel>()
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

        public HttpResponseMessage Create(HttpRequestMessage request, PageViewModel pageviewmodel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {
                    var newpage = new Page();
                    newpage.UpdatePage(pageviewmodel);
                    newpage.CreatedDate = DateTime.Now;
                    newpage.CreatedBy = User.Identity.Name;
                    _pageService.Add(newpage);
                    _pageService.Save();
                    var responseData = Mapper.Map<Page, PageViewModel>(newpage);
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);
                    return respone;
                }
            });
        }

        #endregion

        #region [Update]
        [Route("update")]
        [HttpPut]

        public HttpResponseMessage Update(HttpRequestMessage request, PageViewModel pageViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {
                    var dbPage = _pageService.GetById(pageViewModel.ID);
                    dbPage.UpdatePage(pageViewModel);
                    dbPage.UpdatedDate = DateTime.Now;
                    dbPage.UpdatedBy = User.Identity.Name;
                    _pageService.Update(dbPage);
                    _pageService.Save();
                    var responseData = Mapper.Map<Page, PageViewModel>(dbPage);
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);
                    return respone;
                }
            });
        }
        #endregion


        #region [Delete]
        [Route("delete")]
        [HttpDelete]

        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {

                    var oldPage = _pageService.Delete(id);
                    _pageService.Save();
                    var responseData = Mapper.Map<Page, PageViewModel>(oldPage);
                    respone = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return respone;
                }
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string listPage)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage respone = null;
                if (!ModelState.IsValid)
                {
                    respone = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return respone;

                }
                else
                {

                    var ids = new JavaScriptSerializer().Deserialize<List<int>>(listPage);
                    foreach (var id in ids)
                    {
                        _pageService.Delete(id);
                    }
                    _pageService.Save();

                    respone = request.CreateResponse(HttpStatusCode.OK, ids.Count);
                    return respone;
                }
            });
        }

        #endregion



    }
}
