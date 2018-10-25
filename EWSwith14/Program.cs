using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSwith14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin...");
            EWSServerwith14 eWSServerwith14 = new EWSServerwith14();
            eWSServerwith14.ReceiveEmail();
            Console.WriteLine("finished.");
            Console.ReadLine();
        }
    }
}
