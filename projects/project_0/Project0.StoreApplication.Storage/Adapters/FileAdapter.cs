using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Project0.StoreApplication.Domain.Models;

namespace Project0.StoreApplication.Storage.Adapters
{
  public class FileAdapter
  {
    public F ReadFromFile<F>(string path) where F : class
    {
      // open file
      var file = new StreamReader(path);
      // serialize object
      var xml = new XmlSerializer(typeof(F));
      // read from file
      var result = xml.Deserialize(file) as F;
      // return data
      return result;
    }

    public void WriteToFile<F>(List<F> stores, string path) where F : class
    {
      // file path
      //var path = @"/home/tylerwhite/Revature/training_code/data/project_0.xml";
      // open file
      var file = new StreamWriter(path);
      // serialize object
      var xml = new XmlSerializer(typeof(List<F>));
      // write to file
      xml.Serialize(file, stores);
    }

    public void UseReadFile()
    {
      ReadFromFile<Store>("path");
    }
  }
}