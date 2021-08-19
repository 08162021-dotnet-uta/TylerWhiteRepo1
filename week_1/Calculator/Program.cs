using System;

namespace Calculator
{
  class Program
  {
    static void Main(string[] args)
    {
      calculator();
    }

    public static void calculator()
    {
      string calcOperator = "+";
      int input1 = 0;
      int input2 = 0;
      int result = 0;
      print("Enter your First Number for calculation.");
      input1 = validateIntInput();
      print("Enter your Second Number for calculation.");
      input2 = validateIntInput();
      print("Enter either '+' for Addition, '-' For Subtraction, '*' For Multiplication, or '/' For Division");
      calcOperator = validateOperationInput(Console.ReadLine());
      result = calculate(input1, input2, calcOperator);
      print("");
      if (result != -1) print("Your Result is: " + result.ToString());
    }

    private static int calculate(int num1, int num2, string mathOp)
    {
      int rValue = 0;
      switch (mathOp)
      {
        case "+":
          rValue = Add(num1, num2);
          break;
        case "-":
          rValue = Subtract(num1, num2);
          break;
        case "*":
          rValue = Multiplication(num1, num2);
          break;
        case "/":
          rValue = Division(num1, num2);
          break;
      }
      return rValue;
    }

    private static int validateIntInput()
    {
      bool validInput = false;
      int Output = 0;
      do
      {
        validInput = Int32.TryParse(Console.ReadLine(), out Output);
        if (validInput == false) print("Invalid Input. Please Enter a number.");
      }
      while (validInput == false);
      return Output;
    }

    private static string validateOperationInput(string Output)
    {
      bool validInput = false;
      do
      {
        if (Output == "+" || Output == "-" || Output == "*" || Output == "/")
        {
          validInput = true;
          break;
        }
        else print("Invalid Input. Please Enter an operator.");
        Output = Console.ReadLine();
      }
      while (validInput == false);
      return Output;
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
      int rValue = 0;
      try
      {
        rValue = num1 / num2;
      }
      catch (DivideByZeroException)
      {
        print("Divide by Zero Error. Becareful with your inputs");
        rValue = -1;
      }
      return rValue;
    }
  }
}
