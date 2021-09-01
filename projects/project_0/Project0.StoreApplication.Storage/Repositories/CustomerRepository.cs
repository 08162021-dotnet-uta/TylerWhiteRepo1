using System;
using System.Collections.Generic;
using Project0.StoreApplication.Domain.Abstracts;
using Project0.StoreApplication.Domain.Models;
using Project0.StoreApplication.Storage.Adapters;

namespace Project0.StoreApplication.Storage.Repositories
{
  public class CustomerRepository
  {
    private const string _path = @"/home/tylerwhite/Revature/training_code/data/customers.xml";
    public List<Customer> Customers { get; set; }
    public CustomerRepository()
    {
      var fileAdapter = new FileAdapter();

      if (fileAdapter.ReadFromFile<List<Customer>>(_path) == null)
      {
        fileAdapter.WriteToFile(new List<Customer>()
        {
          new Customer() {name = "John Doe"},
          new Customer() {name = "Batman"},
          new Customer() {name = "Spiderman"}
        }, _path);
      }

      Customers = fileAdapter.ReadFromFile<List<Customer>>(_path);
    }
    private static CustomerRepository _customerRepository;

    public static CustomerRepository Instance
    {
      get
      {
        if (_customerRepository == null)
        {
          _customerRepository = new CustomerRepository();
        }

        return _customerRepository;
      }
    }
  }
}