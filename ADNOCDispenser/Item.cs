using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADNOCDispenser
{
    internal class Item
    {
        public string itemId { get; set; }
        public string itemName { get; set; }
        public decimal unitPrice { get; set; }
        public decimal availableQuantity { get; set; }
        public decimal capacity { get; set; }
    }
}
