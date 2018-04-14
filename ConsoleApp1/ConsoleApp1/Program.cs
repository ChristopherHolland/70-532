using ConsoleApp1.Entities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("storageConnection"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("customers");

            table.CreateIfNotExists();

            //CreateCustomer(table, new CustomerUS("Chris", "Chris@localhost.local"));

            CreateCustomer(table, new CustomerUS("Stephen", "Stephen@localhost.local"));

            //GetCustomer(table, "US", "Chris@localhost.local");

            //GetAllCustomers(table);

            var stephen = GetCustomer(table, "US", "Stephen@localhost.local");

            stephen.Name = "Stephen";

            UpdateCustomer(table, stephen);

            GetAllCustomers(table);

            Console.ReadKey();
        }

        static void CreateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation insert = TableOperation.Insert(customer);

            table.Execute(insert);
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

                foreach(CustomerUS customer in table.ExecuteQuery(query))
            {
                Console.WriteLine(customer.Name);
            }
        }
        
        static void UpdateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation update = TableOperation.Replace(customer);

            table.Execute(update);
        }
    }
}