using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magazin
{
    public class HouseholdGoods : IProduct
    {
        public int id { get; set; }
        public string destiny { get; set; }
        public string sub_destiny { get; set; }
        public string name { get; set; }
        public string producer { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public string supplier { get; set; }

        public HouseholdGoods(int Id, string Destiny, string Sub_destiny, string Name, string Producer, int Quantity, int Price, string Supplier)
        {
            this.id = Id;
            this.destiny = Destiny;
            this.sub_destiny = Sub_destiny;
            this.name = Name;
            this.producer = Producer;
            this.quantity = Quantity;
            this.price = Price;
            this.supplier = Supplier;
        }
        
        private void ShowHouseholdGoods(int Id, string Destiny, string Sub_destiny, string Name, string Producer, int Quantity, int Price, string Supplier)
        {
            
        }
    }
}
