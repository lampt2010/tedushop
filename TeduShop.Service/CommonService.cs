using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
        void UpdateFooter(Footer footer);
        IEnumerable<Slide> GetSlides();
        IEnumerable<Post> SearchPost(string keyword);
        IEnumerable<Product> SearchProduct(string keyword);

    }

    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;
        IUnitOfWork _unitOfWork;
        ISlideRepository _slideRepository;
        IProductRepository _productRepository;
        IPostRepository _postRepository;
        public CommonService(IFooterRepository FooterRepository,ISlideRepository slideRepository,IProductRepository productRepository, IPostRepository postRepository,IUnitOfWork unitOfWork)
        {

            this._footerRepository = FooterRepository;
            this._unitOfWork = unitOfWork;
            this._slideRepository = slideRepository;
            this._postRepository = postRepository;
            this._productRepository = productRepository;
        }
        
        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == Common.CommonConstants.FooterIdDefault);
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x=>x.Status==true);
        }

        public void UpdateFooter(Footer footer)
        {
            _footerRepository.Update(footer);
            _unitOfWork.Commit();
        }

        public IEnumerable<Post> SearchPost(string keyword)
        {
            var query = _postRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));
            return query;

        }

        public IEnumerable<Product> SearchProduct(string keyword)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));
            return query;

        }

        


    }
}
