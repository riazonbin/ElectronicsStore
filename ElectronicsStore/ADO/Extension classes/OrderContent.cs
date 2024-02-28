using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStore.ADO
{
    public partial class OrderContent
    {
        public double TotalSumForProduct => (double)(Product.Price * Count);
    }
}
