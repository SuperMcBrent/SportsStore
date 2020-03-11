using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Contracts {
    public class Customer {
        #region Properties
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        #endregion
    }
}
