using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Custom_Plugins
{
    public class DeleteOrder : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

            tracingService.Trace("Custom_Delete Plugin Execution Started.");

            if (context.MessageName.ToLower() != "delete" || context.PrimaryEntityName != "cr371_customer_p")
            {
                tracingService.Trace("Plugin exited: Not a delete event for cr371_customer_p.");
                return;
            }

            Guid customerId = context.PrimaryEntityId;
            tracingService.Trace("Deleting orders related to customer ID: " + customerId);

            // Delete associated orders
            DeleteOrdersByCustomer(service, customerId, tracingService);

            // Delete orders with null customer lookup
            DeleteOrdersWithNullCustomer(service, tracingService);

            tracingService.Trace("Custom_Delete Plugin Execution Completed.");
        }

        private void DeleteOrdersByCustomer(IOrganizationService service, Guid customerId, ITracingService tracingService)
        {
            tracingService.Trace("Fetching orders for customer ID: " + customerId);
            QueryExpression query = new QueryExpression("cr371_order_p")
            {
                ColumnSet = new ColumnSet("cr371_orderid"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("cr371_customerid", ConditionOperator.Equal, customerId)
                    }
                }
            };

            EntityCollection relatedOrders = service.RetrieveMultiple(query);
            tracingService.Trace("Found " + relatedOrders.Entities.Count + " orders to delete.");

            foreach (Entity order in relatedOrders.Entities)
            {
                tracingService.Trace("Deleting order ID: " + order.Id);
                service.Delete("cr371_order_p", order.Id);
            }
        }

        private void DeleteOrdersWithNullCustomer(IOrganizationService service, ITracingService tracingService)
        {
            tracingService.Trace("Fetching orders with null customer lookup.");
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

            EntityCollection nullCustomerOrders = service.RetrieveMultiple(query);
            tracingService.Trace("Found " + nullCustomerOrders.Entities.Count + " orders with null customer lookup to delete.");

            foreach (Entity order in nullCustomerOrders.Entities)
            {
                tracingService.Trace("Deleting order ID: " + order.Id);
                service.Delete("cr371_order_p", order.Id);
            }
        }
    }
}
