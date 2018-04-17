using ConsoleApp1.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Customer
    {
        static void CreateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation insert = TableOperation.Insert(customer);

            table.Execute(insert);
        }

        static void UpdateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation update = TableOperation.Replace(customer);

            table.Execute(update);
        }

        static void DeleteCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation delete = TableOperation.Delete(customer);

            table.Execute(delete);
        }

        static CustomerUS GetCustomer(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<CustomerUS>(partitionKey, rowKey);

            var result = table.Execute(retrieve);

            return (CustomerUS)result.Result;
        }

        static void GetAllCustomers(CloudTable table)
        {
            TableQuery<CustomerUS> query = new TableQuery<CustomerUS>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "US"));

            foreach (CustomerUS customer in table.ExecuteQuery(query))
            {
                Console.WriteLine(customer.Name);
                Console.WriteLine(customer.Email);
                Console.WriteLine(customer.PartitionKey);
                Console.WriteLine(customer.RowKey);
                Console.WriteLine("--------------------");
            }
        }
    }
}
