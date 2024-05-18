using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestBody
{
    public class CartBody
    {
        public int Quantity { get; set; }

        public int BookId { get; set; }  //Foriegn key
    }
}
