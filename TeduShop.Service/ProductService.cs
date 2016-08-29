using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Common;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IProductService
    {
        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAll(string keyword);
        IEnumerable<Product> GetNewest(int top);
        IEnumerable<Product> GetHotProduct(int top);
        IEnumerable<Product> GetSpecialProduct(int top);

        IEnumerable<Product> GetListProductByCategoryId(int categoryId,int page,string sort,int pageSize,out int totalRow);
        IEnumerable<Product> Search(string keyword, int page, int pageSize, out int totalRow);
        IEnumerable<Product> GetListProductByTag(string tagId,int page,int pageSize, out int totalRow);
        void IncreateViewCount(int productId);
        IEnumerable<Tag> GetListTagsByProductId(int productId);

        IEnumerable<Product> GetReatedProducts(int id,int top);
        Product GetProductByAlias(string alias);

        //IEnumerable<ProductCategory> GetAllByParentId(int parentId);

        IEnumerable<string> GetListProductByName(string name);

        Tag GetTagById(string tagId);
        Product GetById(int id);

        void Save();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _ProductRepository;
        private IUnitOfWork _unitOfWork;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;

        public ProductService(IProductRepository productRepository,IProductTagRepository productTagRepository,ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            this._ProductRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;
            this._unitOfWork = unitOfWork;
        }

        public Product Add(Product product)
        {
             var Product= _ProductRepository.Add(product);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(product.Tag))
            {
                string[] tags = product.Tag.Split(',');
                for(int i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);

                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);

                }
              
            }

            return Product;
        }

        public Product Delete(int id)
        {
            return _ProductRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _ProductRepository.GetAll();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _ProductRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _ProductRepository.GetAll();
        }

        //public IEnumerable<ProductCategory> GetAllByParentId(int parentId)
        //{
        //    return _ProductRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        //}

        public Product GetById(int id)
        {
            return _ProductRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product product)
        {
            _ProductRepository.Update(product);
            if (!string.IsNullOrEmpty(product.Tag))
            {
                string[] tags = product.Tag.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);// chuyen ve dang tag

                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }

                    _productTagRepository.DeleteMulti(x => x.ProductID == product.ID);// vi cap nhat nen phai xoa het cac tag cu di
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
              
            }
            _unitOfWork.Commit();
        }

        public IEnumerable<Product> GetNewest(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status && x.HotFlag==true).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetSpecialProduct(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status && x.HomeFlag == true).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public Product GetProductByAlias(string alias)
        {
            return _ProductRepository.GetMulti(x => x.Status && x.Alias == alias,new string[] { "ProductCategory"}).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public IEnumerable<Product> GetListProductByCategoryId(int categoryId, int page,string sort, int pageSize, out int totalRow)
        {
            var query = _ProductRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);
            switch (sort.ToLower())
            {
                case "new": query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "popular": query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount": query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":query = query.OrderByDescending(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }


            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);

        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _ProductRepository.GetMulti(x => x.Status && x.Name.Contains(name)).Select(x=>x.Name);
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, out int totalRow)
        {
            var query = _ProductRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));
          

            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            var product = _ProductRepository.GetSingleById(id);

            return _ProductRepository.GetMulti(x => x.Status && x.ID !=id && x.CategoryID == product.ID).OrderByDescending(x=>x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListProductByTag(string tagId,int page,int pageSize,out int totalRow)
        {
            return  _ProductRepository.GetListProductByTag(tagId, page, pageSize, out totalRow);
                
         
          

        }

        public void IncreateViewCount(int productId)
        {
            var product = _ProductRepository.GetSingleById(productId);
            if (product.ViewCount.HasValue)
            {
                product.ViewCount += 1;
            }else
            {
                product.ViewCount = 1;
            }
        }

        public IEnumerable<Tag> GetListTagsByProductId(int productId )
        {
            return _productTagRepository.GetMulti(x => x.ProductID == productId, new string[] { "Tag" }).Select(x => x.Tag);
        }

        public Tag GetTagById(string tagId)
        {
         return   _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }
    }
}
