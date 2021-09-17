using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _08162021batchDemoStore;
using DemoStoreBusinessLayer;
using DemoStoreDbContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace StoreDemoUi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StoreController : Controller
  {
    //private readonly ICustomerRepository _customerrepo;\
    private readonly Demo_08162021batchContext _context;
    private readonly ILogger<StoreController> _logger;

    private List<string> storeNames = new List<string>();

    public StoreController(Demo_08162021batchContext context, ILogger<StoreController> logger)
    {
      _context = context;
      _logger = logger;
    }

    [HttpGet("stores")]
    public List<string> Get()
    {
      Store s1 = ModelMapper.ViewModelStoreToStore(new ViewModelStore());
      Store storeTask = new Store();
      string sql = "SELECT storeName FROM Stores";

      SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
      SqlCommand cmd = new SqlCommand(sql, connection);
      using (SqlCommand command = new SqlCommand(sql, connection))
      {
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            storeNames.Add(reader.GetString(0));
          }
        }
      }
      // storeTask = await _context.Stores.FromSqlRaw<Store>("SELECT * FROM Stores").FirstOrDefaultAsync();
      if (storeTask == null) return null;
      return storeNames;
      //return ModelMapper.StoreToViewModelStore(storeTask);
    }
    [HttpGet("products")]
    public List<Product> getProducts()
    {
      string sql = "SELECT * FROM Products";
      //List<Product> result = new List<Product>();
      List<Product> result = new List<Product>();

      SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
      SqlCommand cmd = new SqlCommand(sql, connection);
      using (SqlCommand command = new SqlCommand(sql, connection))
      {
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Product tempProduct = new Product();
            tempProduct.ProductId = reader.GetInt32(0);
            tempProduct.ProductName = reader.GetString(1);
            tempProduct.ProductDescription = reader.GetString(2);
            tempProduct.ProductPrice = reader.GetDecimal(3);

            result.Add(tempProduct);//.ToString();
            //result.Add();
          }
        }
      }
      return result;
    }

    [HttpGet("orders/{storeName}")]
    public List<Order> orders(string storeName)
    {
      string sql = $"SELECT * FROM StoreOrders WHERE storeName = '{storeName}'";
      //List<Product> result = new List<Product>();
      List<Order> result = new List<Order>();

      SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
      SqlCommand cmd = new SqlCommand(sql, connection);
      using (SqlCommand command = new SqlCommand(sql, connection))
      {
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Order tempOrder = new Order();
            tempOrder.storeid = reader.GetInt32(0);
            tempOrder.storeName = reader.GetString(1);
            tempOrder.orderid = reader.GetString(2);
            tempOrder.customerId = reader.GetInt32(3);
            tempOrder.orderDate = reader.GetDateTime(4);
            tempOrder.productId = reader.GetInt32(5);

            result.Add(tempOrder);//.ToString();
            //result.Add();
          }
        }
      }
      return result;
    }

    [HttpGet("pastOrders/{customerId}")]
    public List<Order> pastOrders(string customerId)
    {
      _logger.LogInformation(customerId);
      string sql = $"SELECT * FROM StoreOrders WHERE customerId = '{customerId}'";
      //List<Product> result = new List<Product>();
      List<Order> result = new List<Order>();

      SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
      SqlCommand cmd = new SqlCommand(sql, connection);
      using (SqlCommand command = new SqlCommand(sql, connection))
      {
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            Order tempOrder = new Order();
            tempOrder.storeid = reader.GetInt32(0);
            tempOrder.storeName = reader.GetString(1);
            tempOrder.orderid = reader.GetString(2);
            tempOrder.customerId = reader.GetInt32(3);
            tempOrder.orderDate = reader.GetDateTime(4);
            tempOrder.productId = reader.GetInt32(5);

            result.Add(tempOrder);//.ToString();
            //result.Add();
          }
        }
      }
      return result;
    }

    [HttpGet("saveOrder/{storeID}/{storeName}/{orderID}/{cusomterID}/{productID}")]
    public async void saveOrder(string storeID, string storeName, string orderID, string customerID, string productID)
    {
      Console.WriteLine(customerID);
      string sql = $"INSERT INTO StoreOrders (storeId, storeName, orderId, customerId, orderDate, productId) VALUES (@storeId,@storeName,@orderID,@customerId,@orderDate,@productID)";
      SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
      SqlCommand cmd = new SqlCommand(sql, connection);
      if (customerID == null) customerID = "45";
      cmd.Parameters.AddWithValue("@storeId", storeID);
      cmd.Parameters.AddWithValue("@storeName", storeName);
      cmd.Parameters.AddWithValue("@orderId", orderID);
      cmd.Parameters.AddWithValue("@customerId", customerID);
      cmd.Parameters.AddWithValue("@orderDate", DateTime.Now);
      cmd.Parameters.AddWithValue("@productId", productID);
      try
      {
        connection.Open();
        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine("Records Inserted Successfully");
      }
      catch (SqlException e)
      {
        Console.WriteLine("Error Generated. Details: " + e.ToString());
      }
      finally
      {
        connection.Close();
        //
        Console.ReadKey();
      }
      //ViewModelCustomer c = new ViewModelCustomer() { Fname = fname, Lname = lname };
      //send fname and lname into a method of the business layer to check the Db fo that guy/gal;
      ViewModelCustomer c1 = new ViewModelCustomer();// await _customerrepo.RegisterCustomerAsync(c);
      //int c2 = await _context.Database.ExecuteSqlRawAsync("INSERT INTO StoreOrders (storeId, storeName, orderId, CustomerId, orderDate, productId) VALUES ({0},{1},{2},{3},{4},{5})", storeID, storeName, orderID, customerID, DateTime.Now, productID);// default is NULL

    }


  }//EoC
}// EoN
