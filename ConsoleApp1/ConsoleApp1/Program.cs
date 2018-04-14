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
            //Get a reference to a CloudStorageAccount and pass in the storageConnection setting from the AppSettings file
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("storageConnection"));

            //Get a reference to a CloudTableClient - The adaptor to the table
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Get a reference to the actual table
            CloudTable table = tableClient.GetTableReference("customers");

            //Creat the Table if it doesnt exist
            table.CreateIfNotExists();

            //Batch Operations on Azure Tables

            //TableBatchOperation batch = new TableBatchOperation();

            //var customer1 = new CustomerUS("Frank", "frank@localhost.local");
            //var customer2 = new CustomerUS("John", "John@localhost.local");
            //var customer3 = new CustomerUS("Joe", "Joe@localhost.local");

            //batch.Insert(customer1);
            //batch.Insert(customer2);
            //batch.Insert(customer3);

            //table.ExecuteBatch(batch);


            //Creating new Customers
            //CreateCustomer(table, new CustomerUS("Chris", "Chris@localhost.local"));
            //CreateCustomer(table, new CustomerUS("Stephen", "Stephen@localhost.local"));

            //Return a cusomter via the GetCustomer method
            //GetCustomer(table, "US", "Chris@localhost.local");



            //Deleting Rows
            //Define a variable with a CustomerUS object in it
            //var frank = GetCustomer(table, "US", "frank@localhost.local");

            //DeleteCustomer(table, frank);

            //Update Rows
            var john = GetCustomer(table, "US", "John@localhost.local");
            john.Name = "JJohn";
            UpdateCustomer(table, john);


            //Get All Customers
            GetAllCustomers(table);


            //Wait for key press to end program
            Console.ReadKey();
        }

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

                foreach(CustomerUS customer in table.ExecuteQuery(query))
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