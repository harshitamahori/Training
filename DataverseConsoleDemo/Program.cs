using System;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

class Program
{
    
    private static string dataverseUrl = "https://org5b5b5b82.crm.dynamics.com";  
    private static string username = "harshitam@llcscg.com";  
    private static string password = "Symbiotic1234";  

    static async Task Main(string[] args)
    {
        Console.WriteLine(" Authenticating with Username & Password...");

        string connectionString = $"AuthType=OAuth; Url={dataverseUrl};" +
                                  $"Username={username}; Password={password};" +
                                  $"AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;" +  // Microsoft default App ID
                                  $"RedirectUri=http://localhost; LoginPrompt=Auto;";

        using (var serviceClient = new ServiceClient(connectionString))
        {
            if (serviceClient.IsReady)
            {
                Console.WriteLine(" Successfully connected to Dataverse!");
                Console.WriteLine(" Retrieving records from Dataverse...");

                RetrieveRecords(serviceClient);
                CreateRecord(serviceClient);
                UpdateRecord(serviceClient);
                DeleteRecord(serviceClient);
            }
            else
            {
                Console.WriteLine(" Failed to connect to Dataverse.");
            }
        }
    }

    static void RetrieveRecords(ServiceClient serviceClient)
    {
        string tableName = "account";  

        QueryExpression query = new QueryExpression(tableName)
        {
            ColumnSet = new ColumnSet("name", "accountnumber")  
        };

        EntityCollection results = serviceClient.RetrieveMultiple(query);

        Console.WriteLine($" Total Records Found: {results.Entities.Count}");
        foreach (Entity entity in results.Entities)
        {
            Console.WriteLine($" Name: {entity.GetAttributeValue<string>("name")}, " +
                              $"Account Number: {entity.GetAttributeValue<string>("accountnumber")}");
        }
    }

    static Guid CreateRecord(ServiceClient serviceClient)
    {
        Console.WriteLine(" Creating a new record...");

        Entity newEntity = new Entity("account");
        newEntity["name"] = "Test Account";  
        newEntity["accountnumber"] = "ACC123456";
        newEntity["fax"] = "1234567890";

        Guid newRecordId = serviceClient.Create(newEntity);
        Console.WriteLine($" Created Record ID: {newRecordId}\n");
        return newRecordId;
    }
    static void UpdateRecord(ServiceClient serviceClient)
    {
        Guid recordId = new Guid("958c4a3a-c9f2-ef11-be20-0022481c528c"); 

        Console.WriteLine($" Updating record with ID: {recordId}");

        Entity updateEntity = new Entity("account");
        updateEntity.Id = recordId; // Assign the existing record GUID

       
        updateEntity["name"] = "Updated Test Account";
        updateEntity["fax"] = "987-654-3210"; 

        serviceClient.Update(updateEntity);
        Console.WriteLine(" Record Updated Successfully!");
    }
    static void DeleteRecord(ServiceClient serviceClient)
    {
        Guid recordId = new Guid("32adf780-14f6-ef11-be1f-7c1e526798cd"); 

        Console.WriteLine($" Deleting record with ID: {recordId}");

        serviceClient.Delete("account", recordId); // Delete the record

        Console.WriteLine(" Record Deleted Successfully!");
    }
}
