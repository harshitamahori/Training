using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Custom_Plugins
{
    public class Create_Trial:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity customer = (Entity)context.InputParameters["Target"];

                if (customer.LogicalName != "cr371_customer_p") return;

                // Get customer details
                string customerName = customer.Contains("cr371_customername") ? customer.GetAttributeValue<string>("cr371_customername") : "Unknown";
                string phoneNumber = customer.Contains("cr371_phonenumber") ? customer.GetAttributeValue<string>("cr371_phonenumber") : "N/A";

                // Create a default order for this new customer
                Entity order = new Entity("cr371_order_p");
                order["cr371_ordername"] = "First Order for " + customerName;
                order["cr371_orderamount"] = new Money(500); // Default order amount
                order["cr371_orderdate"] = DateTime.Now;
                order["cr371_customerid"] = new EntityReference("cr371_customer_p", customer.Id); // Linking order to customer

                // Save the order
                Guid orderId = service.Create(order);
            }
        }
    }
}
