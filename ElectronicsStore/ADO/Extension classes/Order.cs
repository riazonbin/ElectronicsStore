using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPractice.ADOModel
{
    public partial class Order
    {
        public double PriceOfOrder => (double)OrderContent.Sum(x => x.MenuItem.Price);

        public string CurrentStatus => App.Connection.OrderStatus.FirstOrDefault(x => x.Id == OrderStatus_Id).Name.ToString();
    }
}
