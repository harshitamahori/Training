using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Custom_Plugins
{
    public class Update_Trial: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain execution context
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.PrimaryEntityName == "cr371_customer_p")
            {
                if (context.Depth > 1)
                    return;
                Entity customerRecord = service.Retrieve("cr371_customer_p", context.PrimaryEntityId, new ColumnSet("cr371_customertype"));
                int customerType = customerRecord.Contains("cr371_customertype") ? customerRecord.GetAttributeValue<OptionSetValue>("cr371_customertype").Value : 0;

                //new entity as account . Why? - If use accountRecord doent work as retrieve only 2 columns craete 2 rows
                Entity customerToUpdate = new Entity("cr371_customer_p");
                customerToUpdate.Id = context.PrimaryEntityId;
                if (customerType == 852020000)
                {
                    customerToUpdate["cr371_cardlimit"] = new Money(5000);
                }
                else
                {
                    customerToUpdate["cr371_cardlimit"] = new Money(10000);
                }

                service.Update(customerToUpdate);

            }
        }
    }
}
