using BusinessLayer.Interface;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class ShoppingCartBusinessLogic : IShoppingBusiness
    {
        private readonly IShoppingCartRepo CartRepo;

        public ShoppingCartBusinessLogic(IShoppingCartRepo CartRepo)
        {
            this.CartRepo = CartRepo;
        }

        public async Task<List<Book>> GetCartBooks(int userId)
        {
            return await CartRepo.GetCartBooks(userId);
        }

        public async Task<List<Book>> AddToCart(CartBody cartRequest, int userId)
        {
            return await CartRepo.AddToCart(cartRequest, userId);
        }

        public async Task<CartBody> UpdateQuantity(int userId, CartBody cartRequest)
        {
            return await CartRepo.UpdateQuantity(userId, cartRequest);
        }

        public async Task<bool> DeleteCart(int userId, int id)
        {
            return await CartRepo.DeleteCart(userId, id);
        }

    }
}
