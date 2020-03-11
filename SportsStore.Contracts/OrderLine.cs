using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Contracts {
    public class OrderLine {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
}
