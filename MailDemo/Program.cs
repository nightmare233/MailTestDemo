using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MailDemo
{
    class Program
    {
       
        static void Main(string[] args)
        {
            MailBee.Global.LicenseKey = "MN110-6DA5A581A531A520A5FB8750BEA3-9ACF"; //2019-1-7 ~ 2-7 expired.
            try
            {
                Console.WriteLine("begin..."); 
                Pop3Server.ReceiveEmail();
                Console.WriteLine("done!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Console.ReadKey();
            }
        }

    }
}
