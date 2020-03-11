using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Contracts {
    public class Order {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Giftwrapping { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderId { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingStreet { get; set; }
        public decimal Total { get; set; }
    }
}
