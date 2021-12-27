using System;
using System.Collections.Generic;
using System.Linq;
using Satify.Data;
using Satify.Data.Models;

namespace Satify.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly TofuDbContext _db;

        public ProductService(TofuDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);

                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = product.IdealQuantity
                };

                _db.ProductInventories.Add(newInventory);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Saved new product",
                    IsSuccess = true
                };

            } catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Error saving new product",
                    IsSuccess = true
                };
            }
        }

        /// <summary>
        /// Retrieves all product from the database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        /// <summary>
        /// Retrieves a Product from the database by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }

        /// <summary>
        /// Archieves a Product by setting boolean IsArchieved to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ServiceResponse<Data.Models.Product> IProductService.ArchieveProduct(int id)
        {
            try
            {
                var product = _db.Products.Find(id);
                product.IsArchived = true;
                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Archieved Product",
                    IsSuccess = true
                };
            } catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccess = false
                };
            }
        }
        
        /// <summary>
        /// Edit a Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> EditProduct(Data.Models.Product product)
        {
            var now = DateTime.UtcNow;
            try
            {
                var editProduct = _db.Products.Find(product.Id);
                editProduct.Name = product.Name;
                editProduct.Price = product.Price;
                editProduct.UpdatedOn = now;
                editProduct.IdealQuantity = product.IdealQuantity;
                editProduct.Description = product.Description;
       
                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = editProduct,
                    Time = now,
                    Message = "Edited Product",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccess = false
                };
            }
        }
    }
}
