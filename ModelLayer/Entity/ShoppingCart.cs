using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class ShoppingCart
    {
        public int CartId { get; set; }

        public int Quantity { get; set; }

        public int UserId { get;set; } //Foriegn key

        public int BookId { get; set; }  //Foriegn key


    }
}
