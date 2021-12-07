using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSwith15
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin...");
            EWSServerwith15 eWSServerwith14 = new EWSServerwith15();
            eWSServerwith14.ReceiveEmail();
            Console.WriteLine("finished.");
            Console.ReadLine();
        }
    }
}
