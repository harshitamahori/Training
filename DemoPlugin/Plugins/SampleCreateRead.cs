using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk; // to implemebt IPlugin Interface
using Microsoft.Xrm.Sdk.Query; // used for retireve operation 

namespace DemoPlugin.Plugins
{
    public class SampleCreateRead:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider) // prvides a mechanism to retrieve a service object
        {
            //defines contexual info passed to plugin at runtime .Entity , field , schema name etc
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            //This represent the factory for creating IOrganisation service instance
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            //IOrganisationService provides the programmatic access to meta data and data for oraganisation
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "account") // provides data on execution pipleine (schema name )
            {
                Entity accountRecord = service.Retrieve("account", context.PrimaryEntityId, new ColumnSet("name", "telephone1"));
                string accountName = accountRecord.GetAttributeValue<string>("name");
                string phoneNumber = accountRecord.GetAttributeValue<string>("telephone1");

                Entity contactRecord = new Entity("contact"); // local record of type contat entity
                contactRecord["fullname"] = accountName; //assign values to this contact record
                contactRecord["telephone1"] = phoneNumber; //assign value to phone filed

                //Associate account record to contact record
                contactRecord["parentcustomerid"] = new EntityReference("account", context.PrimaryEntityId);
                contactRecord["accountrolecode"] = new OptionSetValue(2);
                contactRecord["creditlimit"] = new Money(100);
                contactRecord["lastonholdtime"] = new DateTime(2025, 01, 01);
                contactRecord["donotphone"] = true;
                contactRecord["numberofchildren"]= 0;

                Guid contactId = service.Create(contactRecord);




            }


        }
    }
}
