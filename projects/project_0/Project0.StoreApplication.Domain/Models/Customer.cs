using System.Collections.Generic;

namespace Project0.StoreApplication.Domain.Models
{
  /// <summary>
  /// 
  /// </summary>
  public class Customer
  {
    public string name { get; set; }
    public List<Order> Orders { get; set; }

    public Customer()
    {
      Orders = new List<Order>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return $"{name} with {Orders.Count} Orders so far";
    }
  }
}