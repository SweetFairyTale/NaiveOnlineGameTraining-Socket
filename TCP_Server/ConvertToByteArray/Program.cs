/*
 * proj03 -- 测试客户端数据发送方案的项目
 * 在每条数据前加上固定4个字节(int32)的信息用于表示数据的长度
 * 读取数据流时参考此长度向后读取相应数据
 */
using System;
using System.Text;

namespace ConvertToByteArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int temp = 100;
            byte[] data = BitConverter.GetBytes(temp);  //BitConverter根据值类型转换，保证不同数据值所占字节相同.
            
            //Encoding.UTF8.GetBytes(char[] chars)
            //该方法只能按字符(引用)处理，转换出的长度与具体数据值无关(如:"10"->2byte,"100"->3byte).
            //此时不利于标准化数据长度并解决分包粘包问题.

            foreach (byte b in data)
            {
                Console.Write(b + ":");
            }
            Console.ReadKey();
        }
    }
}
