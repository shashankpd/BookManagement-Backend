using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestBody
{
    public class UserBody
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public long MobileNumber { get; set; }
    }
}
