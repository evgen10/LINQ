// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();




        [Category("My queries")]
        [Title("Task 1")]
        [Description("****")]
        public void Linq1()
        {
            int value = 20000;

            var customers = dataSource.Customers.Where(c => c.Orders.Sum(o => o.Total) > value).Select(c => new { c.CustomerID, TotalSum = c.Orders.Sum(o => o.Total) });

            Console.WriteLine("For value = ", value + "\n");

            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }



            value = 30000;

            Console.WriteLine("For value = ", value + "\n");

            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }

            value = 70000;

            Console.WriteLine("For value = ", value + "\n");

            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }





        }


        [Category("My queries")]
        [Title("Task 2(WithoutGrouping)")]
        [Description("****")]
        public void Linq2WithoutGrouping()
        {

            foreach (var customers in dataSource.Customers.Select(c => c))
            {
                Console.WriteLine("{0} from {1}, {2} ", customers.CustomerID, customers.Country, customers.City);

                var suppliers = dataSource.Suppliers.Where(s => s.Country == customers.Country && s.City == customers.City).Select(s => new { s.SupplierName, s.Country, s.City });

                ObjectDumper.Write(suppliers);

                if (suppliers.Count() == 0)
                {
                    Console.WriteLine("No suppliers");
                }
                else
                {

                }
            }

        }


        [Category("My queries")]
        [Title("Task 2(WithGrouping)")]
        [Description("****")]
        public void Linq2WithGrouping()
        {

            var list = (from s in dataSource.Suppliers
                       join c in dataSource.Customers on new { s.City, s.Country } equals new { c.City, c.Country }
                       select new { c.CustomerID, s.SupplierName, c.Country, c.City });



            var dffd = (from s in dataSource.Suppliers
                        join c in dataSource.Customers on new { s.City, s.Country } equals new { c.City, c.Country }
                        group c by c.CustomerID into cus
                        select new { cus.Key, Customers = cus });

            
                   
                         

            var f = dataSource.Customers.Join(dataSource.Suppliers,
                                              c => new { c.City, c.Country },
                                              s => new { s.City, s.Country },
                                              (c, s) => new { c.CustomerID, s.SupplierName, c.Country, c.City })
                                             .GroupBy(co => co.CustomerID);



            foreach (var item in f)
            {
                Console.WriteLine(item.Key);

                foreach (var i in item)
                {
                    ObjectDumper.Write(i);
                }
            }


            Console.WriteLine();

            foreach (var item in dffd)
            {
                Console.WriteLine(item.Key);

                foreach (var im in item.Customers)
                {
                    ObjectDumper.Write(im);
                }
            }


        }



        [Category("My queries")]
        [Title("Task 3")]
        [Description("****")]
        public void Linq3()
        {
            int value = 5000;

            var s = from c in dataSource.Customers
                    from o in c.Orders
                    where o.Total > value
                    select c;


            var d = s.Distinct();
            
            var customers = dataSource.Customers.SelectMany(c => c.Orders).Where(o => o.Total > value);

            //var dfdf = dataSource.Customers.Where();



            ObjectDumper.Write(customers);





        }
    }
}

