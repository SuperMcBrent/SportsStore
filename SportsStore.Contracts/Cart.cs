using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Contracts {
    public class Cart {
        public int Id { get; set; }
        public string OpvulVeld { get; set; }
        public List<CartLine> Cartlines { get; set; }
    }
}
