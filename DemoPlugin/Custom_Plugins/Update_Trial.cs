using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Custom_Plugins
{
    public class Update_Trial : IPlugin
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

                // Retrieve Customer Type
                Entity customerRecord = service.Retrieve("cr371_customer_p", context.PrimaryEntityId, new ColumnSet("cr371_customertype"));
                int customerType = customerRecord.Contains("cr371_customertype") ? customerRecord.GetAttributeValue<OptionSetValue>("cr371_customertype").Value : 0;

                // Update Customer Card Limit
                Entity customerToUpdate = new Entity("cr371_customer_p");
                customerToUpdate.Id = context.PrimaryEntityId;

                if (customerType == 852020000) // Basic
                {
                    customerToUpdate["cr371_cardlimit"] = new Money(5000);
                }
                else // Premium
                {
                    customerToUpdate["cr371_cardlimit"] = new Money(10000);
                }

                service.Update(customerToUpdate);

                // Fetch updated customer type to ensure correct discount application
                Entity customerRecordNew = service.Retrieve("cr371_customer_p", context.PrimaryEntityId, new ColumnSet("cr371_customertype"));
                int customerTypeNew = customerRecordNew.Contains("cr371_customertype") ? customerRecordNew.GetAttributeValue<OptionSetValue>("cr371_customertype").Value : 0;

                // Retrieve related orders and update discount based on customer type
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

                EntityCollection relatedOrders = service.RetrieveMultiple(orderQuery);

                foreach (Entity order in relatedOrders.Entities)
                {
                    Entity orderToUpdate = new Entity("cr371_order_p");
                    orderToUpdate.Id = order.Id;

                    // Apply discount based on customer type
                    if (customerTypeNew == 852020000) // Basic customer
                    {
                        orderToUpdate["cr371_discountm"] = new Money(10);
                    }
                    else if (customerTypeNew == 852020001) // Premium customer
                    {
                        orderToUpdate["cr371_discountm"] = new Money(20);
                    }

                    service.Update(orderToUpdate);
                }
            }
        }
    }
}
