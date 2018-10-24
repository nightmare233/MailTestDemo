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
            DateTime date = new DateTime(2013, 06, 16);
            int count = 500;
            ExchangeEmailServer exchangeEmailServer = new ExchangeEmailServer();
            //exchangeEmailServer.Test();
            //exchangeEmailServer.ReceiveEmail();
            //exchangeEmailServer.GetOneEmail();
            //exchangeEmailServer.MoveArchive(date, count);
            //exchangeEmailServer.ReceiveEmailByFindItems(date, count);
            //exchangeEmailServer.GetMailsFromArchive(date, count);

            while (date < new DateTime(2017, 12, 31))
            {
                exchangeEmailServer.MoveArchive(date, count);
                date = date.AddDays(3);
            }

        }
    }
}
