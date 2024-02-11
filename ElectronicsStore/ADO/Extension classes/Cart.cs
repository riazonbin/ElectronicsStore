using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPractice.ADOModel
{
    public partial class Cart
    {
        public int CountOfDish => App.Connection.Cart.Where(x => x.MenuItem_Id == MenuItem_Id && x.User_Id== User_Id).Count();

        public double TotalPriceOfDish => (double)App.Connection.Cart.Where(x => x.MenuItem_Id == MenuItem_Id).Sum(x => x.MenuItem.Price);


    }
};
