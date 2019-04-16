using System;
using System.Text;

//一个测试客户端数据发送方案的类
namespace ConvertToByteArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int temp = 100;
            byte[] data = BitConverter.GetBytes(temp);  //BitConverter根据值类型转换，保证不同数据所占字节相同.
            
            //Encoding.UTF8.GetBytes(char[] chars)
            //该方法只能按字符(引用)处理值，转换长度与具体数据类型无关(如:"10","100").
            //此时不利于标准化数据长度并解决分包粘包问题.

            foreach (byte b in data)
            {
                Console.Write(b + ":");
            }
            Console.ReadKey();
        }
    }
}
