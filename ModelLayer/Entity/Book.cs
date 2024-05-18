using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class Book
    {
        public int BookId { get; set; }

        public string BookName { get; set; }

        public string BookImage { get; set; }

        public string Description { get; set; }

        public string AuthorName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
