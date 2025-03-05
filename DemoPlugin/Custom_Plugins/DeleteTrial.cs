using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Custom_Plugins
{
    public class DeleteTrial : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            if (context.MessageName.ToLower() != "delete" || context.PrimaryEntityName != "cr371_customer_p")
                return;

            Guid customerId = context.PrimaryEntityId;

            // Delete orders where customer_id is NULL but were linked to this customer
            DeleteOrders(service);
        }

        private void DeleteOrders(IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("cr371_order_p")
            {
                ColumnSet = new ColumnSet("cr371_orderid"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("cr371_customerid", ConditionOperator.Null)
                    }
                }
            };

            EntityCollection orphanedOrders = service.RetrieveMultiple(query);

            foreach (Entity order in orphanedOrders.Entities)
            {
                service.Delete("cr371_order_p", order.Id);
            }
        }
    }
}
