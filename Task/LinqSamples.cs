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

            var customers = dataSource.Customers.Where(c => c.Orders.Sum(o => o.Total) > value).Select(c=> new { c.CustomerID, TotalSum =  c.Orders.Sum(o=>o.Total)});

            Console.WriteLine("For value = ", value+"\n");

            foreach (var item in customers)
            {
                Console.WriteLine(item);
            }

            

            value = 30000;

            Console.WriteLine("For value = ", value+"\n");

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






    }
}
