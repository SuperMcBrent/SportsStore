﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Contracts {
    public class Product {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
}
