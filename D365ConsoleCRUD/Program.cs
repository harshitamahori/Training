using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365ConsoleCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            #region OAuth
            OAuthConnect oAuth = new OAuthConnect();
            CrmServiceClient svc = oAuth.ConnectWithOAuth();
            if (!svc.IsReady)
            {
                Console.WriteLine("CrmServiceClient is NOT ready.");
                Console.WriteLine($"Error: {svc.LastCrmError}");
                Console.WriteLine($"Extended Error: {svc.LastCrmException}");
            }
            /*if (svc.IsReady)
            {
                Console.WriteLine("Connected to  D365 Server...");
                oAuth.PerformCRUD(svc);
            }*/

            else
            {
                Console.WriteLine("Could not connect to D365 Server...");
            }
            #endregion

        }
    }
    
}
