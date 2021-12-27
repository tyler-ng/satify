using System.Collections.Generic;

namespace Satify.Services.Product
{
    public interface IProductService
    {
        List<Data.Models.Product> GetAllProducts();
        Data.Models.Product GetProductById(int id);
        ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product);
        ServiceResponse<Data.Models.Product> ArchieveProduct(int id);
        ServiceResponse<Data.Models.Product> EditProduct(Data.Models.Product product);
    }
}
