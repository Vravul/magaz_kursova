using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magazin
{
    public interface IProduct
    {
        int id { get; set; }
        string destiny { get; set; }
        string sub_destiny { get; set; }
        string name { get; set; }
        string producer { get; set; }
        int quantity { get; set; }
        int price { get; set; }
        string supplier { get; set; }
    }
}
