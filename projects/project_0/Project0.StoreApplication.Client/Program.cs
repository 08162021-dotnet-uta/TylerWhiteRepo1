using System;
using System.Linq;
using System.Collections.Generic;
using Project0.StoreApplication.Domain.Models;
using Serilog;
using Project0.StoreApplication.Storage.Repositories;
using Project0.StoreApplication.Storage.Adapters;

namespace Project0.StoreApplication.Client
{

  public class Program
  {
    public static readonly StoreRepository _storeRepository = StoreRepository.Instance;
    public static CustomerRepository _customerRepository = CustomerRepository.Instance;
    private const string _logFilePath = @"/home/tylerwhite/Revature/training_code/data/logs.txt";
    private static readonly FileAdapter fa = new FileAdapter();
    static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration().WriteTo.File(_logFilePath).CreateLogger();
      mainMenu();
    }
    ///<summary>
    /// Manages Current Order in progress
    ///</summary>
    public static void storeApp(Customer customer)
    {
      Log.Information("method: storeApp()");
      Order currentOrder = new Order();
      Store currentStore = new Store();
      currentOrder.customerName = customer.name;
      currentOrder.OrderDate = DateTime.Today;

      bool readyForPurchese = false;
      int selection;

      currentStore = SelectStore();
      currentOrder.storeName = currentStore.name;
      storeMenu(ref currentOrder, currentStore);
      while (readyForPurchese == false)
      {
        print("what you like to do?\n1: Complete Order\n2: Add Another Product\n3: View Order\n4: Cancel order");
        selection = validateInput(1, 4);
        Log.Information($"method: storeApp(): {selection}");
        switch (selection)
        {
          case 1:
            readyForPurchese = true;
            break;
          case 2:
            currentOrder = storeMenu(ref currentOrder, currentStore);
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
      Log.Information($"method: storeApp(): Completed Order");
      Log.Information($"method: storeApp(): Completed Order Price: {currentOrder.getTotalPrice()}");
      customer.Orders.Add(currentOrder);
      currentStore.orders.Add(currentOrder);
      saveCustomersData(currentStore, customer);
      saveStoresData(currentStore, currentOrder);
      mainMenu();
    }

    ///<summary>
    /// Saves Store Data to File after every purchese
    ///</summary>
    public static void saveStoresData(Store currentStore, Order order)
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

    ///<summary>
    /// Saves Custer Data to File after every purchese
    ///</summary>
    public static void saveCustomersData(Store currentStore, Customer customer)
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

    ///<summary>
    /// Prints Order in a Readyable fashion
    ///</summary>
    public static void printOrder(Order order)
    {
      print($"\nStore:{order.storeName}\nCustomer:{order.customerName}\n");
      print($"Order Date: {order.OrderDate.ToString("MM-dd-yyyy")}");
      for (int i = 0; i < order.Products.Count; i++)
      {
        Console.WriteLine($"{order.Products[i].name} : ${order.Products[i].price}");
      }
      print("Total Order Price: " + order.getTotalPrice().ToString());
    }

    ///<summary>
    /// Startup Menu To choose a customer profile or Enter Store menu to view store Orders
    ///</summary>
    public static void mainMenu()
    {
      print("Welcome to Stores R Us! \nPlease Select your customer profile:");
      int selection = printAllCustomers();
      Log.Information($"method: mainMenu(): {selection}");
      if (selection == _customerRepository.Customers.Count) storeEmployeeMenu();
      else
      {
        customerMenu(_customerRepository.Customers[selection]);
      }
    }

    ///<summary>
    /// Menu for Stores to view all previous orders and Decide how they are printed out.
    /// Printed out by Store, All Stores, Or by Timespan
    ///</summary>
    public static void storeEmployeeMenu()
    {
      print("1: Print all Store Location Orders\n2: Print a Single Store's Orders\n3: Print All Store Orders By Timespan");
      int selection = validateInput(1, 3);
      Log.Information($"method: storeEmployeeMenu(): {selection}");
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
      else if (selection == 2)
      {
        print("Select Which Store you'd like to view all orders of.");
        Store specificStore = SelectStore();
        Log.Information($"method: storeEmployeeMenu(): SpecificStore : {specificStore.name}");
        print(specificStore.name);
        foreach (Order ord in specificStore.orders)
        {
          printOrder(ord);
        }
      }
      else
      {
        List<Order> tempOrders = new List<Order>();
        foreach (Store store in _storeRepository.Stores)
        {
          foreach (Order ord in store.orders)
          {
            tempOrders.Add(ord);
          }
        }
        tempOrders = tempOrders.OrderBy(x => x.OrderDate).ToList();
        print("1: View by past Week\n2: View by past Month\n3: View By past Quarter");
        int select = validateInput(1, 3);
        Log.Information($"method: storeEmployeeMenu(): OrderBy : {select}");
        if (select == 1)
        {
          tempOrders = tempOrders.Where(t => t.OrderDate >= DateTime.Now.Date.Subtract(TimeSpan.FromDays(7))).ToList();
          foreach (Order ord in tempOrders)
          {
            printOrder(ord);
          }
        }
        else if (select == 2)
        {
          tempOrders = tempOrders.Where(t => t.OrderDate >= DateTime.Now.Date.Subtract(TimeSpan.FromDays(28))).ToList();
          foreach (Order ord in tempOrders)
          {
            printOrder(ord);
          }
        }
        else
        {
          tempOrders = tempOrders.Where(t => t.OrderDate >= DateTime.Now.Date.Subtract(TimeSpan.FromDays(84))).ToList();
          foreach (Order ord in tempOrders)
          {
            printOrder(ord);
          }
        }
      }
      mainMenu();
    }

    ///<summary>
    /// Menu for Purchesing a product from a store. Returns reference order to make sure data is saved
    ///</summary>
    public static Order storeMenu(ref Order currentOrder, Store store)
    {
      print($"Welcome to {store.name}");
      print("Here's a list of all Products for Sale: \n");
      printAllProducts(store.products);
      print("What would you like to purchese today?");
      int selectIndex = validateInput(1, store.products.Count) - 1;
      Product selection = store.products[selectIndex];
      if (currentOrder.Products == null) currentOrder.Products = new List<Product>();
      currentOrder.Add(selection);
      print($"Adding \"{selection.name} : {selection.price}\" to your cart");
      return currentOrder;
    }

    ///<summary>
    /// Main Start menu for customer to decide whether they want to view past orders or make a new order
    ///</summary>
    public static void customerMenu(Customer customer)
    {
      print($"Welcome {customer.name}!\nWhat would you like to do?\n");
      print("1: Make new purchese\n2: View All previous orders");
      int selection = validateInput(1, 2);
      Log.Information($"method: customerMenu(): {selection}");
      if (selection == 1)
      {
        storeApp(customer);
      }
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
    ///<summary>
    /// Prints all Customers in the Customer Repository
    ///</summary>
    public static int printAllCustomers()
    {
      var customerList = _customerRepository.Customers;
      int iter = 1;

      foreach (var customer in customerList)
      {
        Console.WriteLine(iter + ": " + customer.ToString());
        iter++;
      }
      print($"{iter}: Store Orders");
      return validateInput(1, iter) - 1;
    }

    ///<summary>
    /// Prints all Stores in Store Repository
    ///</summary>
    public static void printAllStoreLocations()
    {
      var storeLocations = _storeRepository.Stores;
      int iter = 1;

      foreach (var store in storeLocations)
      {
        Console.WriteLine(iter + ": " + store.ToString());
        iter++;
      }
    }

    ///<summary>
    /// Generic string print method to prevent Console. Write lines
    ///</summary>
    public static void print(String Output)
    {
      Console.WriteLine(Output);
    }

    ///<summary>
    /// Prints all Products in a Readable Fashion
    ///</summary>
    public static void printAllProducts(List<Product> Output)
    {
      int iter = 1;
      foreach (Product product in Output)
      {
        print($"{iter} : {product.name} : $ {product.price}");
        iter++;
      }
    }

    ///<summary>
    /// Validate input for a int based upon a range
    ///</summary>
    public static int validateInput(int minRange, int maxRange, string input = "-1")
    {
      bool valid = false;
      int validInput = 0;
      while (valid == false)
      {
        if (input != "-1")
        {
          validInput = int.Parse(input);
          valid = true;
        }
        else valid = Int32.TryParse(Console.ReadLine(), out validInput);

        if (valid == false) print($"Invalid Input. Please enter a number between {minRange} and {maxRange}");
      }
      return validInput;
    }

    ///<summary>
    /// Menu for Selecting a store before making a purchese
    ///</summary>
    public static Store SelectStore()
    {
      Console.WriteLine("Select a Store: ");
      printAllStoreLocations();
      return _storeRepository.Stores[validateInput(1, 3) - 1];
    }
  }
}
