using System;
using System.Collections.Generic;
using Project0.StoreApplication.Domain.Models;
using Serilog;
using Project0.StoreApplication.Storage.Repositories;
using Project0.StoreApplication.Storage.Adapters;

namespace Project0.StoreApplication.Client
{

  class Program
  {
    private static readonly StoreRepository _storeRepository = StoreRepository.Instance;
    private static CustomerRepository _customerRepository = CustomerRepository.Instance;
    private static readonly FileAdapter fa = new FileAdapter();
    static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
      mainMenu();
      //storeApp();
    }

    private static void storeApp(Customer customer)
    {
      Order currentOrder = new Order();
      Store currentStore = new Store();

      bool readyForPurchese = false;
      int selection;

      currentStore = SelectStore();
      storeMenu(ref currentOrder, currentStore);
      while (readyForPurchese == false)
      {
        print("what you like to do?\n1: Complete Order\n2: Add Another Product\n3: View Order\n4: Cancel order");
        selection = int.Parse(Console.ReadLine());
        switch (selection)
        {
          case 1:
            readyForPurchese = true;
            break;
          case 2:
            storeMenu(ref currentOrder, currentStore);
            break;
          case 3:
            printOrder(currentOrder);
            break;
          case 4:
            currentOrder = new Order();
            customerMenu(customer);
            break;
        }
      }
      print($"Thank you for Shopping at {currentStore.name}");
      print($"Your total price is {currentOrder.getTotalPrice()}");
      customer.Orders.Add(currentOrder);
      currentStore.orders.Add(currentOrder);
      saveCustomersData(currentStore, customer);
      saveStoresData(currentStore, currentOrder);
      mainMenu();
    }

    private static void saveStoresData(Store currentStore, Order order)
    {
      List<Store> stores = _storeRepository.Stores;
      int index = 0;
      for (int i = 0; i < stores.Count - 1; i++)
      {
        if (stores[i].name == currentStore.name)
        {
          index = i;
          stores[i] = currentStore;
        }
      }
      if (stores[index].orders == null) stores[index].orders = new List<Order>();
      stores[index].orders.Add(order);
      fa.WriteToFile(stores, @"/home/tylerwhite/Revature/training_code/data/stores.xml");
    }

    private static void saveCustomersData(Store currentStore, Customer customer)
    {
      List<Order> tempOrders = new List<Order>();
      foreach (var order in currentStore.orders)
      {
        tempOrders.Add(order);
      }
      foreach (var store in _storeRepository.Stores)
      {
        if (store.name != currentStore.name)
        {
          foreach (var order in store.orders)
          {
            tempOrders.Add(order);
          }
        }
      }
      for (int i = 0; i < _customerRepository.Customers.Count - 1; i++)
      {
        if (_customerRepository.Customers[i].name == customer.name) _customerRepository.Customers[i] = customer;
      }
      fa.WriteToFile(_customerRepository.Customers, @"/home/tylerwhite/Revature/training_code/data/customers.xml");
    }

    private static void printOrder(Order order)
    {
      print($"\nStore:\n{order.storeName}\nCustomer:{order.customerName}\n");
      for (int i = 0; i < order.Products.Count; i++)
      {
        Console.WriteLine($"{order.Products[i].name} : ${order.Products[i].price}");
      }
      print("");
    }

    private static void mainMenu()
    {
      print("Welcome to Stores R Us! \nPlease Select your customer profile:");
      int selection = printAllCustomers();
      if (selection == _customerRepository.Customers.Count) storeEmployeeMenu();
      else
      {
        customerMenu(_customerRepository.Customers[selection]);
      }
    }

    private static void storeEmployeeMenu()
    {
      print("1: Print all Store Location Orders\n2: Print a Single Store's Orders");
      int selection = int.Parse(Console.ReadLine());
      if (selection == 1)
      {
        foreach (Store store in _storeRepository.Stores)
        {
          print(store.name);
          foreach (Order ord in store.orders)
          {
            printOrder(ord);
          }
        }
      }
      else
      {
        print("Select Which Store you'd like to view all orders of.");
        Store specificStore = SelectStore();
        print(specificStore.name);
        foreach (Order ord in specificStore.orders)
        {
          printOrder(ord);
        }
      }
      mainMenu();
    }

    private static void storeMenu(ref Order currentOrder, Store store)
    {
      print($"Welcome to {store.name}");
      print("Here's a list of all Products for Sale: \n");
      printAllProducts(store.products);
      print("What would you like to purchese today?");
      int selectIndex = int.Parse(Console.ReadLine()) - 1;
      Product selection = store.products[selectIndex];
      currentOrder.Add(selection);
      print($"Adding \"{selection.name} : {selection.price}\" to your cart");
    }

    private static void customerMenu(Customer customer)
    {
      print($"Welcome {customer.name}!\nWhat would you like to do?\n");
      print("1: Make new purchese\n2: View All previous orders");
      int selection = int.Parse(Console.ReadLine());
      if (selection == 1) storeApp(customer);
      else if (selection == 2)
      {
        if (customer.Orders.Count == 0) print("No Previous Orders");
        foreach (Order ord in customer.Orders)
        {
          printOrder(ord);
        }
        mainMenu();
      }
    }
    private static int printAllCustomers()
    {
      var customerList = _customerRepository.Customers;
      int iter = 1;

      foreach (var customer in customerList)
      {
        Console.WriteLine(iter + ": " + customer.ToString());
        iter++;
      }
      print($"{iter}: Store Orders");
      return (int.Parse(Console.ReadLine())) - 1;
    }
    private static void printAllStoreLocations()
    {
      var storeLocations = _storeRepository.Stores;
      int iter = 1;

      foreach (var store in storeLocations)
      {
        Console.WriteLine(iter + ": " + store.ToString());
        iter++;
      }
    }
    private static void print(String Output)
    {
      Console.WriteLine(Output);
    }

    private static void printAllProducts(List<Product> Output)
    {
      int iter = 1;
      foreach (Product product in Output)
      {
        print($"{iter} : {product.name} : $ {product.price}");
        iter++;
      }
    }

    static Store SelectStore()
    {
      Console.WriteLine("Select a Store: ");
      printAllStoreLocations();
      return _storeRepository.Stores[int.Parse(Console.ReadLine()) - 1];
    }
  }
}
