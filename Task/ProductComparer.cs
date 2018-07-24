using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{

    public enum ProductPriceCategories
    {
        LowPrice,
        AveragePrice,
        HightPrice
    }

    public class ProductComparer : IEqualityComparer<decimal>
    {


        public bool Equals(decimal x, decimal y)
        {
            return DeterminePriceCategory(x) == DeterminePriceCategory(y);
        }

        public int GetHashCode(decimal obj)
        {

            return DeterminePriceCategory(obj).GetHashCode();
        }

        public ProductPriceCategories DeterminePriceCategory(decimal price)
        {



            if (price < 20.000M)
            {
                return ProductPriceCategories.LowPrice;
            }


            if (price >= 20.000M && price <= 40.000M)
            {
                return ProductPriceCategories.AveragePrice;
            }


            return ProductPriceCategories.HightPrice;
        }

    }
}
