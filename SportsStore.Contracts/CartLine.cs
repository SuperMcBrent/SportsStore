using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Contracts {
    public class CartLine {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
