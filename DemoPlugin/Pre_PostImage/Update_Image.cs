using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Pre_PostImage
{
    public class Update_Image : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain execution context
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("Plugin Execution Started.");
            tracingService.Trace($"Primary Entity: {context.PrimaryEntityName}");
            tracingService.Trace($"Depth: {context.Depth}");

            if (context.PrimaryEntityName == "cr371_customer_p")
            {
                if (context.Depth > 1)
                {
                    tracingService.Trace("Exiting plugin execution due to depth check.");
                    return;
                }

                // Get Pre-Image (Old Values)
                int oldCustomerType = 0;
                if (context.PreEntityImages.Contains("PreImage") && context.PreEntityImages["PreImage"] is Entity preImage)
                {
                    oldCustomerType = preImage.Contains("cr371_customertype") ? preImage.GetAttributeValue<OptionSetValue>("cr371_customertype").Value : 0;
                    tracingService.Trace($"Old Customer Type: {oldCustomerType}");
                }

                // Get Post-Image (New Values)
                int newCustomerType = 0;
                if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity postImage)
                {
                    newCustomerType = postImage.Contains("cr371_customertype") ? postImage.GetAttributeValue<OptionSetValue>("cr371_customertype").Value : 0;
                    tracingService.Trace($"New Customer Type: {newCustomerType}");
                }

                // Update Customer Card Limit
                Entity customerToUpdate = new Entity("cr371_customer_p") { Id = context.PrimaryEntityId };

                if (newCustomerType == 852020000) // Basic
                {
                    customerToUpdate["cr371_cardlimit"] = new Money(5000);
                    tracingService.Trace("Updated card limit to 5000 for Basic customer.");
                }
                else // Premium
                {
                    customerToUpdate["cr371_cardlimit"] = new Money(10000);
                    tracingService.Trace("Updated card limit to 10000 for Premium customer.");
                }

                service.Update(customerToUpdate);
                tracingService.Trace("Customer record updated successfully.");

                // Retrieve related orders and update discount based on new customer type
                QueryExpression orderQuery = new QueryExpression("cr371_order_p")
                {
                    ColumnSet = new ColumnSet("cr371_orderid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("cr371_customerid", ConditionOperator.Equal, context.PrimaryEntityId)
                        }
                    }
                };

                tracingService.Trace("Fetching related orders for customer.");
                EntityCollection relatedOrders = service.RetrieveMultiple(orderQuery);
                tracingService.Trace($"Total related orders found: {relatedOrders.Entities.Count}");

                foreach (Entity order in relatedOrders.Entities)
                {
                    tracingService.Trace($"Updating order ID: {order.Id}");
                    Entity orderToUpdate = new Entity("cr371_order_p") { Id = order.Id };

                    if (newCustomerType == 852020000) // Basic customer
                    {
                        orderToUpdate["cr371_discountm"] = new Money(10);
                        tracingService.Trace("Applied discount of 10 for Basic customer.");
                    }
                    else if (newCustomerType == 852020001) // Premium customer
                    {
                        orderToUpdate["cr371_discountm"] = new Money(20);
                        tracingService.Trace("Applied discount of 20 for Premium customer.");
                    }

                    service.Update(orderToUpdate);
                    tracingService.Trace($"Order ID: {order.Id} updated successfully.");
                }

                tracingService.Trace("Plugin Execution Completed.");
            }
        }
    }
}
