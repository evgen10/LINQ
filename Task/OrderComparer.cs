using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Data;

namespace Task
{
    public enum DateComparers
    {
        ByMonth,
        ByYear,
        ByMonthYear
    }
    public class OrderComparer : IEqualityComparer<Order>
    {
        private DateComparers modifier = DateComparers.ByMonth;

        public OrderComparer()
        {
            
        }

        public OrderComparer(DateComparers modifier)
        {
            this.modifier = modifier;
        }

   


        public bool Equals(Order x, Order y)
        {
            if (modifier == DateComparers.ByMonth)
            {
                return x.OrderDate.Month == y.OrderDate.Month;
            }
            if (modifier ==DateComparers.ByYear)
            {
                return x.OrderDate.Year == y.OrderDate.Year;
            }

            return new { x.OrderDate.Year, x.OrderDate.Month } == new { y.OrderDate.Year, y.OrderDate.Month };

        }

        public int GetHashCode(Order obj)
        {
            return obj.GetHashCode();
        }


       


    }
}
