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
using TeduShop.Web.Infrastructure.Extensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Api
{
    [RoutePrefix("api/common")]
    [Authorize]
    public class CommonController : ApiControllerBase
    {

        #region [Khởi tạo ]
        ICommonService _commonService;//footer o day

        public CommonController(IErrorService errorService, ICommonService commonService) : base(errorService)
        {
            this._commonService = commonService;
        }

        #endregion

        #region [Update]
        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, FooterViewModel footerViewModel)
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
                    var footer = _commonService.GetFooter();//lay footer ve
                    footer.UpdateFooter(footerViewModel);// chuyen ve dang viewmodel
                    _commonService.UpdateFooter(footer);// thuc hien update                
                    var responseData = Mapper.Map<Footer, FooterViewModel>(footer);//map lai ve viewmodel
                    respone = request.CreateResponse(HttpStatusCode.Created, responseData);//tra ve request
                    return respone;
                }
            });
        }
        #endregion

        #region [Get]

        [Route("getfooter")]// ham nay de goi ajax load danh muc cha
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () => {
                var model = _commonService.GetFooter();
                var responeData = Mapper.Map<Footer, FooterViewModel>(model);
                var respone = request.CreateResponse(HttpStatusCode.OK, responeData);
                return respone;
            });
        }

      

        #endregion

    }
}
