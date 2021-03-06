﻿// Copyright © Microsoft Corporation.  All Rights Reserved.
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
using Task;

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

            //в виде выражения 
            var customers1 = from c in dataSource.Customers
                             where c.Orders.Sum(o => o.Total) > value
                             select new { c.CompanyName, TotalSum = c.Orders.Sum(o => o.Total) };

            //в виде методов
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

                //в виде методов
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

            //в виде методов
            var customers1 = dataSource.Customers
                              .Join(dataSource.Suppliers,
                                   c => new { c.City, c.Country },
                                   s => new { s.City, s.Country },
                                   (c, s) => c)
                              .GroupBy(cus => cus.CompanyName);

            //в виде выражения 
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

            //в виде выражения 
            var customers1 = from c in dataSource.Customers
                             where c.Orders.Any(o => o.Total > value)
                             select c;

            //в виде методов
            var customers2 = dataSource.Customers
                                .Where(c => c.Orders.Any(o => o.Total > value));



            ObjectDumper.Write(customers2);



        }




        [Category("My queries")]
        [Title("Task 4")]
        [Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq4()
        {
            //в виде методов
            var customers1 = dataSource.Customers
                             .Select(c => new
                             {
                                 c.CompanyName,
                                 OrderDate = c.Orders.Min(o => (DateTime?)o.OrderDate)

                             });

            //в виде выражения 
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

            //в виде методов
            var customers1 = dataSource.Customers.
                          Select(c => new
                          {
                              c.CompanyName,
                              OrderDate = c.Orders.Min(o => (DateTime?)o.OrderDate),
                              Total = c.Orders.Sum(s => s.Total)

                          });


            //в виде выражения 
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
            var sortMultipleField = customers1.OrderBy(n => n.CompanyName).ThenBy(d => d.OrderDate).ThenByDescending(s => s.Total);
            ObjectDumper.Write(sortByNameListt);

            Console.WriteLine();



        }

        [Category("My queries")]
        [Title("Task 6")]
        [Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион или в телефоне не указан код оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»)")]
        public void Linq6()
        {

            //в виде методов
            var customers1 = dataSource.Customers
                           .Where(cus => !int.TryParse(cus.PostalCode, out int res)
                                       || cus.Region == null
                                       || !cus.Phone.StartsWith("("))
                           .Select(c => new { c.CustomerID, c.PostalCode, c.Region, c.Phone });


            //в виде выражения 
            int result;
            var customers2 = from c in dataSource.Customers
                             where !int.TryParse(c.PostalCode, out result)
                                   || c.Region == null
                                   || !c.Phone.StartsWith("(")
                             select new { c.CustomerID, c.PostalCode, c.Region, c.Phone };



            Console.WriteLine(customers2.Count());
            ObjectDumper.Write(customers2);

        }


        [Category("My queries")]
        [Title("Task 7")]
        [Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости)")]
        public void Linq7()
        {

            //в виде методов
            var products1 = dataSource.Products
                                     .GroupBy(p => p.Category)
                                     .Select(i => new
                                     {
                                         Category = i.Key,
                                         CategoryGroup = i.GroupBy(s => s.UnitsInStock)
                                                    .Select(price => new
                                                    {
                                                        UnitInStock = price.Key,
                                                        InStockGroup = price.OrderByDescending(o => o.UnitPrice)
                                                    })
                                     });


            //в виде выражения 
            var products2 = from product in dataSource.Products
                            group product by product.Category into categoryGroup
                            select new
                            {
                                Category = categoryGroup.Key,
                                CategoryGroup = from categories in categoryGroup
                                                group categories by categories.UnitsInStock into inStockGroup
                                                select new
                                                {
                                                    UnitInStock = inStockGroup.Key,
                                                    InStockGroup = inStockGroup.OrderByDescending(o => o.UnitPrice)
                                                }
                            };

            ObjectDumper.Write(products1, 3);

            Console.WriteLine();

            ObjectDumper.Write(products2, 3);

        }


        [Category("My queries")]
        [Title("Task 8")]
        [Description("Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами")]
        public void Linq8()
        {

            ProductComparer comparer = new ProductComparer();


            var products = dataSource.Products.GroupBy(p => p.UnitPrice, comparer);






            foreach (var product in products)
            {
                if (comparer.DeterminePriceCategory(product.Key) == ProductPriceCategories.LowPrice)
                {
                    Console.WriteLine("Дешевые товары");
                }


                if (comparer.DeterminePriceCategory(product.Key) == ProductPriceCategories.AveragePrice)
                {
                    Console.WriteLine("Средние товары");
                }

                if (comparer.DeterminePriceCategory(product.Key) == ProductPriceCategories.HightPrice)
                {
                    Console.WriteLine("Дорогие товары");
                }

                foreach (var i in product)
                {
                    Console.WriteLine(i.UnitPrice + " " + i.ProductName);
                }

            }



        }



        [Category("My queries")]
        [Title("Task 9")]
        [Description("Рассчитайте среднюю прибыльность каждого города (среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность (среднее количество заказов, приходящееся на клиента из каждого города)")]
        public void Linq9()
        {

            var cities1 = from c in dataSource.Customers
                          group c by c.City into cityGroup
                          select new
                          {
                              City = cityGroup.Key,
                              AverageOrderSum = (from ct in cityGroup
                                                 from o in ct.Orders
                                                 select o.Total)
                                                 .Average(),
                              AverageIntensity = (from ct in cityGroup
                                                  select ct.Orders.Length).Average()
                          };


            var cities2 = dataSource.Customers
                                        .GroupBy(c => c.City)
                                        .Select(grC =>
                                        new
                                        {
                                            City = grC.Key,
                                            AverageOrderSum = grC
                                                                .SelectMany(ct => ct
                                                                                    .Orders
                                                                                    .Select(o => o.Total))
                                                                                    .Average(),
                                            AverageIntensity = grC
                                                                 .Select(ct => ct
                                                                                 .Orders.Length)
                                                                                 .Average()
                                        });


            ObjectDumper.Write(cities1);

            Console.WriteLine("   ");

            ObjectDumper.Write(cities2);
        }



        [Category("My queries")]
        [Title("Task 10")]
        [Description("Сделайте среднегодовую статистику активности клиентов по месяцам (без учета года), статистику по годам, по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение).")]
        public void Linq10()
        {

            //var stat = dataSource.Customers
            //                        .SelectMany(o =>  o.Orders)
            //                        .GroupBy(p => p.OrderDate.Month, h=>new { Name = dataSource.Customers.Where(op=>op.Orders == op) })
            //                          //new
            //                          //{
            //                          //    p.OrderDate.Year,
            //                          //    p.OrderDate.Month
            //                          //})
            //                        .Select(t => new
            //                        {
            //                            t.Key,
            //                            Orders = t
            //                        });



                                        //Console.WriteLine("hjghjgjhg");




            //ObjectDumper.Write(stat,3);

            


            //foreach (var item in stat)
            //{
            //    Console.WriteLine("Key" + item.Key);
            //    foreach (var it in item)
            //    {
            //        Console.WriteLine(it.CompanyName+" ");

            //        foreach (var i in it.Orders)
            //        {
            //            Console.WriteLine(i.OrderDate);
            //        }
            //    }
            //}


            //foreach (var item in stat)
            //{
            //    Console.WriteLine("Key" + item.Key);
            //    foreach (var it in item)
            //    {
            //        ObjectDumper.Write(it, 1);

            //    }
            //}


        }








    }
}

