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
        [Description("1. Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X. Продемонстрируйте выполнение запроса с различными X (подумайте, можно ли обойтись без копирования запроса несколько раз)")]
        public void Linq1()
        {
            int value = 20000;

            var customers1 = from c in dataSource.Customers
                             where c.Orders.Sum(o => o.Total) > value
                             select new { c.CompanyName, TotalSum = c.Orders.Sum(o => o.Total) };


            var customers2 = dataSource.Customers
                                .Where(c => c.Orders.Sum(o => o.Total) > value)
                                .Select(c => new { c.CompanyName, TotalSum = c.Orders.Sum(o => o.Total) });

            Console.WriteLine("For value = ", value + "\n");

            foreach (var item in customers2)
            {
                Console.WriteLine(item);
            }



            value = 30000;

            Console.WriteLine("For value = ", value + "\n");

            foreach (var item in customers2)
            {
                Console.WriteLine(item);
            }

            value = 70000;

            Console.WriteLine("For value = ", value.ToString() + "\n");

            foreach (var item in customers2)
            {
                Console.WriteLine(item);
            }





        }


        [Category("My queries")]
        [Title("Task 2(WithoutGrouping)")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе")]
        public void Linq2WithoutGrouping()
        {

            foreach (var customers in dataSource.Customers.Select(c => c))
            {
                Console.WriteLine("{0} from {1}, {2} ", customers.CompanyName, customers.Country, customers.City);

                var suppliers = dataSource.Suppliers
                                    .Where(s => s.Country == customers.Country && s.City == customers.City)
                                    .Select(s => new { s.SupplierName, s.Country, s.City });


                foreach (var item in suppliers)
                {
                    Console.WriteLine(item);
                }

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
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе")]
        public void Linq2WithGrouping()
        {


            var customers1 = dataSource.Customers
                              .Join(dataSource.Suppliers,
                                   c => new { c.City, c.Country },
                                   s => new { s.City, s.Country },
                                   (c, s) => c)
                              .GroupBy(cus => cus.CompanyName);


            var customers2 = from s in dataSource.Suppliers
                             join c in dataSource.Customers on new { s.City, s.Country } equals new { c.City, c.Country }
                             group c by c.CompanyName;





            foreach (var item in customers2)
            {
                Console.WriteLine("For customer {0}", item.Key);

                foreach (var i in item)
                {
                    ObjectDumper.Write(i);
                }
                Console.WriteLine();
            }


            Console.WriteLine();




        }



        [Category("My queries")]
        [Title("Task 3")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq3()
        {
            int value = 2000;

            var customers1 = from c in dataSource.Customers
                             where c.Orders.Any(o => o.Total > value)
                             select c;


            var customers2 = dataSource.Customers
                                .Where(c => c.Orders.Any(o => o.Total > value));

            

            ObjectDumper.Write(customers2);



        }




        [Category("My queries")]
        [Title("Task 4")]
        [Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq4()
        {

            var customers1 = dataSource.Customers
                             .Select(c => new
                             {
                                 c.CompanyName,
                                 OrderDate = c.Orders.Min(o => (DateTime?)o.OrderDate)

                             });


            var customers2 = from c in dataSource.Customers
                             select new
                             {
                                 c.CompanyName,
                                 OrderDate = c.Orders.Min(o => (DateTime?)o.OrderDate)

                             };




            foreach (var item in customers1)
            {

                if (item.OrderDate == null)
                {
                    Console.WriteLine("Customer {0} has no orders", item.CompanyName);
                }
                else
                {
                    Console.WriteLine("Customer {0} made his first order on {1} ", item.CompanyName, item.OrderDate.Value.ToLongDateString());
                }


            }




        }


        [Category("My queries")]
        [Title("Task 5")]
        [Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]
        public void Linq5()
        {                 
          

            var customers1 = dataSource.Customers.
                          Select(c => new
                          {
                              c.CompanyName,
                              OrderDate = c.Orders.Min(o=>(DateTime?)o.OrderDate),
                              Total = c.Orders.Sum(s => s.Total)

                          });
           

            var customers2 = from c in dataSource.Customers
                             select new
                             {
                                 c.CompanyName,
                                 OrderDate = c.Orders.Min(o => (DateTime?)o.OrderDate),
                                 Total = c.Orders.Sum(s => s.Total)
                             };


            Console.WriteLine("\nSort by date\n");
            var sortByDateList = customers1.OrderBy(o => o.OrderDate);
            ObjectDumper.Write(sortByDateList);

            Console.WriteLine("\nSort by total sum\n");
            var sortByTotalSumList = customers2.OrderByDescending(o => o.Total);
            ObjectDumper.Write(sortByTotalSumList);

            Console.WriteLine("\nSort by customer name\n");
            var sortByNameListt = customers1.OrderBy(o => o.CompanyName);
            ObjectDumper.Write(sortByNameListt);
            

            Console.WriteLine("\nSort by multiple fields \n");
            var sortMultipleField = customers1.OrderBy(n => n.CompanyName).ThenBy(d=>d.OrderDate).ThenByDescending(s=>s.Total);
            ObjectDumper.Write(sortByNameListt);

            Console.WriteLine();



        }


      


    }
}

