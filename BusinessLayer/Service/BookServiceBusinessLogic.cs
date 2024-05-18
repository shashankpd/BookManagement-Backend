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
    public class BookServiceBusinessLogic : IBookBusiness
    {

        private readonly IBookRepo BookRepository;

        public BookServiceBusinessLogic(IBookRepo BookRepository)
        {
            this.BookRepository = BookRepository;
        }

        //start

        public Task<IEnumerable<Book>> AddBook(BookBody books)
        { 
           return BookRepository.AddBook( books);
        }

        public Task<IEnumerable<Book>> getAllBook()
        {
            return BookRepository.getAllBook();
        }



    }
}
