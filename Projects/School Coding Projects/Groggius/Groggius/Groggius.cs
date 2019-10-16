using System;

namespace Groggius
{
    public class GroggiusNumbers
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Groggius is learning to add numbers. Enter two numbers for him to add.");

            int num1 = int.Parse(Console.ReadLine());
            int num2 = int.Parse(Console.ReadLine());

            Console.WriteLine("Groggius informs you that the answer is " + num1 + num2 + ".");
        }
    }
}
