using ModelLayer.Entity;
using ModelLayer.RequestBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IBookBusiness
    {

        public Task<IEnumerable<Book>> AddBook(BookBody books);

        public Task<IEnumerable<Book>> getAllBook();


    }
}
