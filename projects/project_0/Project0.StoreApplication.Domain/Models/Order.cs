using System;
using System.Collections.Generic;

namespace Project0.StoreApplication.Domain.Models
{
  public class Order
  {
    public List<Product> Products { get; set; }
    public string storeName { get; set; }
    public DateTime OrderDate { get; set; }
    public string customerName { get; set; }
    private int itemLimit = 50;
    private double priceLimit = 500.0;

    public void Add(Product product)
    {
      if (Products.Count <= itemLimit)
      {
        if ((getTotalPrice() + product.price) < priceLimit)
        {
          Products.Add(product);
        }
        else Console.WriteLine($"Price is over the limit of ${priceLimit}. Please Complete purchese or cancel purchese.");
      }
      else Console.WriteLine($"Too Many Items in Cart. Max items is {itemLimit} Please Complete purchese or cancel purchese.");
    }
    public double getTotalPrice()
    {
      double totalPrice = 0.0;
      foreach (Product prod in Products)
      {
        totalPrice += prod.price;
      }
      return totalPrice;
    }

  }
}