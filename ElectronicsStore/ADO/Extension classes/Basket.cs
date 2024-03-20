using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStore.ADO
{
    public partial class Basket
    {
        public double TotalPrice => (double)(Product.Price * Count);

        public double ProductPrice => (double)Product.Price;
    }
}
