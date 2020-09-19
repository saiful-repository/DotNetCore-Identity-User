using DotNetCoreIdentity.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIdentity.Model;

namespace DotNetCoreIdentity.Repository
{
    public class ProductRepository : IProductRepository
    {
        public readonly ProductContext _dbContext;

        public ProductRepository(ProductContext productContext)
        {
            _dbContext = productContext;
        }
        public Product GetProductByID(int ProductID)
        {
            return _dbContext.Products.Where(x => x.Id == ProductID).FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _dbContext.Products.ToList();
        }

        public void InsertProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
