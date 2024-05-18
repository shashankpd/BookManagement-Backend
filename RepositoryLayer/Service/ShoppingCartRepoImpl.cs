﻿using Dapper;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class ShoppingCartRepoImpl : IShoppingCartRepo
    {
        private readonly DapperContext _context;

        public ShoppingCartRepoImpl(DapperContext context)
        {
            _context = context;
          
        }

        public async Task<List<Book>> GetCartBooks(int userId)
        {
            // Assuming there's a table named CartItems with columns: Id, UserId, BookId, Quantity
            string query = "select b.BookId,b.BookName,b.BookImage,b.Description,b.AuthorName,b.Price,ci.Quantity as Quantity FROM CartEntity ci INNER JOIN BookEntity b ON ci.BookId = b.BookId WHERE ci.UserId =@UserId";

            using (var connection = _context.CreateConnection())
            {
                var cartBooks = await connection.QueryAsync<Book>(query, new { UserId = userId });
                return cartBooks.ToList();
            }
        }

        public async Task<List<Book>> AddToCart(CartBody cartRequest, int userId)
        {
            // Assuming there's a table named CartItems with columns: Id, UserId, BookId, Quantity
            string insertQuery = "INSERT INTO CartEntity (UserId, BookId, Quantity) VALUES (@UserId, @BookId, @Quantity)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, new { UserId = userId, BookId = cartRequest.BookId, Quantity = cartRequest.Quantity });

                // Return updated cart items after insertion
                return await GetCartBooks(userId);
            }
        }


        public async Task<CartBody> UpdateQuantity(int userId, CartBody cartRequest)
        {
            // Assuming there's a table named CartItems with columns: Id, UserId, BookId, Quantity
            string updateQuery = "UPDATE CartEntity SET Quantity = @Quantity WHERE UserId = @UserId AND BookId = @BookId";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, new { Quantity = cartRequest.Quantity, UserId = userId, BookId = cartRequest.BookId });

                // Return updated cart request after update
                return cartRequest;
            }
        }

        public async Task<bool> DeleteCart(int userId, int id)
        {
            // Assuming there's a table named CartItems with columns: Id, UserId, BookId, Quantity
            string deleteQuery = "DELETE FROM CartEntity WHERE UserId = @UserId AND CartId = @Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(deleteQuery, new { UserId = userId, Id = id });

                // Check if any rows were affected
                return await IsCartItemExists(userId, id);
            }
        }

        private async Task<bool> IsCartItemExists(int userId, int id)
        {
            string query = "SELECT COUNT(*) FROM CartEntity WHERE UserId = @UserId AND CartId = @Id";
            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(query, new { UserId = userId, Id = id });
                return count > 0;
            }
        }
    



    }
}
