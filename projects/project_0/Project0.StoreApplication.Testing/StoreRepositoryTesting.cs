using Xunit;
using Project0.StoreApplication.Storage.Repositories;
using Project0.StoreApplication.Storage;
using Project0.StoreApplication.Domain;
using Project0.StoreApplication.Client;

namespace Project0.StoreApplication.Testing
{
  public class StoreRepositoryTests
  {
    [Fact]
    public void Test_StoreCollection()
    {
      // arrange = instance of the entity to test
      var sut = StoreRepository.Instance;

      // act = execute sut for data
      var actual = sut.Stores;

      // assert
      Assert.NotNull(actual);
    }

    [Fact]
    public void Test_InputValidation()
    {
      var num = "1";

      Assert.True(1 == Program.validateInput(1, 3, num));
    }

    [Fact]
    public void Test_CustomerNames()
    {
      Assert.True(Program._customerRepository.Customers[0].name == "John Doe");
      Assert.True(Program._customerRepository.Customers[1].name == "Batman");
      Assert.True(Program._customerRepository.Customers[2].name == "Spider-man");
    }
    [Fact]
    public void Test_CustomerOrders()
    {
      Assert.True(Program._customerRepository.Customers[0].Orders.Count >= 1);
      Assert.True(Program._customerRepository.Customers[1].Orders.Count >= 1);
      Assert.True(Program._customerRepository.Customers[2].Orders.Count >= 1);
    }

    [Fact]
    public void Test_StoreNames()
    {
      Assert.True(Program._storeRepository.Stores[0].name == "Pizza Factory");
      Assert.True(Program._storeRepository.Stores[1].name == "Little Ceasar's");
      Assert.True(Program._storeRepository.Stores[2].name == "Domnio's");
    }
    [Fact]
    public void Test_StoreOrders()
    {
      Assert.True(Program._storeRepository.Stores[0].orders.Count >= 3);
      Assert.True(Program._storeRepository.Stores[1].orders.Count >= 2);
      Assert.True(Program._storeRepository.Stores[2].orders.Count >= 1);
    }
  }
}