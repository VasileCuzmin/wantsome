using AdventureWorks.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new AdventureWorksLt2022Context();

            //var products = context.Products.ToList();

            //foreach (var product in products)
            //{
            //    Console.WriteLine($"{product.Name} - {product.Weight}");
            //}


            var customers = context.Customers;
                //.Include(c => c.CustomerAddresses)
                //.ThenInclude(ca => ca.Address)
                //.ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.FirstName} {customer.LastName}");

                var addresses = customer.CustomerAddresses;

                foreach (var address in addresses)
                {
                    Console.WriteLine($"{address.Address.AddressLine1}");
                }
            }

            Console.Read();
        }
    }
}
