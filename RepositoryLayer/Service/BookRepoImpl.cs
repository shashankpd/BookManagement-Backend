using Dapper;
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
    public class BookRepoImpl : IBookRepo
    {
        private readonly DapperContext _context;

        public BookRepoImpl(DapperContext context)
        {
            _context = context;
        }

        //start

        public async Task<IEnumerable<Book>> AddBook(BookBody books)
        {
            var query = @"
        INSERT INTO BookEntity (BookName, BookImage, Description, AuthorName, Quantity, Price)
        VALUES (@bookname, @bookimage, @description, @authorname, @quantity, @price);
        SELECT * FROM BookEntity WHERE BookId = SCOPE_IDENTITY();";

            using (var connection = _context.CreateConnection())
            {
                var insertedBook = await connection.QueryFirstOrDefaultAsync<Book>(query, new
                {
                    bookname = books.BookName,
                    bookimage = books.BookImage,
                    description = books.Description,
                    authorname = books.AuthorName,
                    quantity = books.Quantity,
                    price = books.Price
                });

                return new List<Book> { insertedBook };
            }
        }

        public async Task<IEnumerable<Book>> getAllBook()
        {
            string query = "SELECT * FROM BookEntity";
            using (var connection = _context.CreateConnection())
            {
                var books = await connection.QueryAsync<Book>(query);
                return books.Reverse().ToList();

            }
        }


    }
}
