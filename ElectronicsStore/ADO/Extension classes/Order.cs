using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStore.ADO
{
    public partial class Order
    {
        public double PriceOfOrder => (double)OrderContent.Sum(x => x.Product.Price);

        public string CurrentStatus => App.Connection.OrderStatus.FirstOrDefault(x => x.Id == OrderStatus_Id).Name.ToString();
    }
}
