using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project0.StoreApplication.Domain.Models;
using Project0.StoreApplication.Storage.Adapters;

namespace Project0.StoreApplication.Storage.Repositories
{
  public class StoreRepository
  {
    private readonly string _path = @"/home/tylerwhite/Revature/training_code/data/stores.xml";

    private static readonly DataAdapter _da = new DataAdapter();
    public List<Store> Stores { get; set; }
    public StoreRepository()
    {
      var fileAdapter = new FileAdapter();

      if (fileAdapter.ReadFromFile<List<Store>>(_path) == null)
      {
        fileAdapter.WriteToFile(new List<Store>()
        {
          new Store() {name = "Little Ceasar's"},
          new Store() {name = "Pizza Factory"},
          new Store() {name = "Domnio's"}
        }, _path);
      }

      Stores = fileAdapter.ReadFromFile<List<Store>>(_path);
      foreach (Store store in Stores)
      {
        //store.products = _da.Products.FromSqlRaw("select p.name,p.price from Products p").ToList();
      }
    }
    private static StoreRepository _storeRepository;

    public static StoreRepository Instance
    {
      get
      {
        if (_storeRepository == null)
        {
          _storeRepository = new StoreRepository();
        }

        return _storeRepository;
      }
    }
  }
}