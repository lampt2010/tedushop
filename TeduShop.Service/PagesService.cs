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
    public interface IPageService
    {
        Page Add(Page page);

        void Update(Page page);

        Page Delete(int id);

        IEnumerable<Page> GetAll();
        IEnumerable<Page> GetAll(string keyword);



        Page GetByAlias(string alias);
        Page GetById(int id);

        void Save();
    }
  public  class PagesService : IPageService
    {
        private IPageRepository _PageRepository;
        private IUnitOfWork _unitOfWork;

        public PagesService(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            this._PageRepository =pageRepository ;
            this._unitOfWork = unitOfWork;
        }

        public Page Add(Page page)
        {
            return _PageRepository.Add(page);
        }

        public Page Delete(int id)
        {
            return _PageRepository.Delete(id);
        }

        public IEnumerable<Page> GetAll()
        {
            return _PageRepository.GetAll();
        }

        public IEnumerable<Page> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _PageRepository.GetMulti(x => x.Name.Contains(keyword) || x.Content.Contains(keyword));
            else
                return _PageRepository.GetAll();
        }

     

        public Page GetByAlias(string alias)
        {
            return _PageRepository.GetSingleByCondition(x => x.Alias == alias);
        }

        public Page GetById(int id)
        {
            return _PageRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Page page)
        {
            _PageRepository.Update(page);
        }
    }
}
