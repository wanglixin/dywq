using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static List<int> temp = new List<int>();
        static void Main(string[] args)
        {
            var list = new List<int>(){
                1,2,3,4,5,6
            };





            var rest = Parallel.ForEach(list,  item =>
            {
                //await Task.Delay(1000);

                temp.Add(item);
                Console.WriteLine(item);
            });

            if (rest.IsCompleted)
            {
                Console.WriteLine(string.Join(",", temp));
            }
            Console.WriteLine("end");
            Console.ReadLine();
        }

        static async Task Add(int i)
        {
            await Task.Run(() =>
             {
                 temp.Add(i);
             });
        }


    }


}
