using System;

namespace HelloWorld
{
  class Program
  {

    // Build a simple calculator using 5 methods: Add, Multiply, Subtract, Divide, Print
    static void Main(string[] args)
    {
      calculator();
    }

    public static void calculator()
    {
      int Input1;
      int Input2;
      int result = 0;
      string calcOperator = "+";

      print("Enter your First Number for calculation.");
      Input1 = Int32.Parse(Console.ReadLine());
      print("Enter your Second Number for calculation.");
      Input2 = Int32.Parse(Console.ReadLine());
      print("Enter either '+' for Addition, '-' For Subtraction, '*' For Multiplication, or '/' For Division");
      calcOperator = Console.ReadLine();

      switch (calcOperator)
      {
        case "+":
          result = Add(Input1, Input2);
          break;
        case "-":
          result = Subtract(Input1, Input2);
          break;
        case "*":
          result = Multiplication(Input1, Input2);
          break;
        case "/":
          result = Division(Input1, Input2);
          break;
      }
      print("");
      print("Your Result is: " + result.ToString());
    }

    private static void print(string output)
    {
      Console.WriteLine(output);
    }

    private static int Add(int num1, int num2)
    {
      return num1 + num2;
    }
    private static int Subtract(int num1, int num2)
    {
      return num1 - num2;
    }
    private static int Multiplication(int num1, int num2)
    {
      return num1 * num2;
    }
    private static int Division(int num1, int num2)
    {
      return num1 / num2;
    }
  }
}
