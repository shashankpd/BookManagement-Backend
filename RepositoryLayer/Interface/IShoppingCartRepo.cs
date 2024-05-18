using ModelLayer.Entity;
using ModelLayer.RequestBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IShoppingCartRepo
    {
        Task<List<Book>> GetCartBooks(int userId);
        Task<List<Book>> AddToCart(CartBody cartRequest, int userId);
        /* Task<double> GetPrice(int userId);*/
        Task<CartBody> UpdateQuantity(int userId, CartBody cartRequest);
        Task<bool> DeleteCart(int userId, int id);


    }
}
