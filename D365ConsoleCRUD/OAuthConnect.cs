using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365ConsoleCRUD
{
    public class OAuthConnect
    {
        public CrmServiceClient ConnectWithOAuth()
        {
            Console.WriteLine("connecting to D365 Server...");
            string authType = "OAuth";
            string userName = "harshitam@llcscg.com";
            string password = "Symbiotic1234";
            string url = "https://org5b5b5b82.crm.dynamics.com";
            string appId = "a7349124-1c4c-4753-8e11-8921d5edb8c0";
            string reDirectURI = "https://localhost";
            string loginPrompt = "Auto";
            string ConnectionString = string.Format("AuthType={0};Username={1};Password={2};Url={3};AppId={4};RedirectUri={5};LoginPrompt={6}", authType, userName, password, url, appId, reDirectURI, loginPrompt);
            Console.WriteLine("Connection String" + ConnectionString);

            CrmServiceClient svc = new CrmServiceClient(ConnectionString);
            return svc;

        }

        public void PerformCRUD(CrmServiceClient svc)
        {
            //CREATE
            var myContact = new Entity("contact");
            myContact.Attributes["lastname"] = "Mahori";
            myContact.Attributes["firstname"] = "Harshita";
            myContact.Attributes["jobtitle"] = "Consultant";
            Guid RecordId = svc.Create(myContact);
            Console.WriteLine("Contact craete with ID -" + RecordId);

            //RETRIEVE
            Entity contact = svc.Retrieve("contact", new Guid(""), new Microsoft.Xrm.Sdk.Query.ColumnSet("firstname", "lastname"));
            Console.WriteLine("Contact lastname si - " + contact.Attributes["lastname"]);

            //Retrieve Multiple Record
            QueryExpression qe = new QueryExpression("contact");
            qe.ColumnSet = new ColumnSet("firstname", "lastname");
            EntityCollection ec = svc.RetrieveMultiple(qe);

            for(int i=0;i< ec.Entities.Count; i++)
            {
                if (ec.Entities[i].Attributes.ContainsKey("firstname"))
                {
                    Console.WriteLine(ec.Entities[i].Attributes["firstname"]);
                }
            }
            Console.WriteLine("Retrieved all Contacts...");

            //UPDATE
            Entity entContact = new Entity("contact");
            entContact.Id = RecordId;
            entContact.Attributes["lastname"] = "Facts";
            svc.Update(entContact);
            Console.WriteLine("Contact lastname updated");

            //DELETE
            svc.Delete("contact", RecordId);

            //Execute
            Entity acc = new Entity("account");
            acc["name"] = "Soft";
            var createRequest = new CreateRequest()
            {
                Target = acc
            };
            svc.Execute(createRequest);

            //Execute Multiple
            //Create Request object
            var Request = new ExecuteMultipleRequest()
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses=true
                }
            };
            //add entity object
            Entity acc1 = new Entity("account");
            acc1["name"] = "Soft1";
            Entity acc2 = new Entity("account");
            acc1["name"] = "Soft2";


            var createRequest1 = new CreateRequest()
            {
                Target = acc1
            };

            var createRequest2 = new CreateRequest()
            {
                Target = acc2
            };
            Request.Requests.Add(createRequest1);
            Request.Requests.Add(createRequest2);
            var response = (ExecuteMultipleResponse)svc.Execute(Request);

        }
    }
}
