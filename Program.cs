using System;
using System.Text.Json;

namespace QuickTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var errorModel = AuthenticationErrorModel.CreateWithDefaultMessage();
                Console.WriteLine(JsonSerializer.Serialize(errorModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }
    }
}