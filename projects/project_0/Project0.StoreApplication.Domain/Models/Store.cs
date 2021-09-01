using System.Collections.Generic;

namespace Project0.StoreApplication.Domain.Models
{
  public class Store
  {
    public string name { get; set; }

    public List<Product> products { get; set; }

    public List<Order> orders { get; set; }

    public override string ToString()
    {
      return name;
    }
  }
}