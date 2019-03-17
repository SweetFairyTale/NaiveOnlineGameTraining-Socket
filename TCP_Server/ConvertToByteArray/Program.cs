using System;
using System.Text;


namespace ConvertToByteArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int temp = 100;
            byte[] data = BitConverter.GetBytes(temp);
            foreach(byte b in data)
            {
                Console.Write(b + ":");
            }
            Console.ReadKey();
        }
    }
}
