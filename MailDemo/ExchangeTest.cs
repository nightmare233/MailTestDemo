using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
namespace MailDemo
{
    public class ExchangeTest
    {
        public static void TestExchange()
        {
            DateTime date = new DateTime(2014, 01, 01);
            int count = 2000;
            ExchangeEmailServer exchangeEmailServer = new ExchangeEmailServer();
            exchangeEmailServer.Test();
            //exchangeEmailServer.ReceiveEmail();
            //exchangeEmailServer.GetOneEmail();
            //exchangeEmailServer.MoveArchive(date, count);
            //exchangeEmailServer.ReceiveEmailByFindItems(date, count);
            //exchangeEmailServer.GetMailsFromArchive(date, count); 
            exchangeEmailServer.CountEmailsNumber();
        }

        public static void Archive()
        {
            //Console.WriteLine("please input the year of begin date (example: 2017):");
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine("please input the month of begin date (example: 10):");
            //int month = int.Parse(Console.ReadLine());
            //Console.WriteLine("please input the day of begin date (example: 23):");
            //int day = int.Parse(Console.ReadLine());
            //DateTime date = new DateTime(year, month, day);
            DateTime date = new DateTime(2017, 12, 4);
            int count = 1000;
            int days = 6;
            ExchangeEmailServer exchangeEmailServer = new ExchangeEmailServer();


            while (date < new DateTime(2018, 4, 30))
            {
                Console.WriteLine(date.ToShortDateString());
                exchangeEmailServer.MoveAfterDateArchive(date, count, days);
                date = date.AddDays(days); 
            }
        }
    }
}