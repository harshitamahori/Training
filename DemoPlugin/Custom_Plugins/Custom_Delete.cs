using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoPlugin.Custom_Plugins
{
    public class Custom_Delete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("Plugin Execution Started");

            if (context.MessageName.ToLower() != "delete")
                return;

            if (!context.PreEntityImages.Contains("PreImage") || !context.PreEntityImages["PreImage"].Contains("cr371_customerid"))
            {
                tracingService.Trace("Pre-Image not found");
                return;
            }

            Entity preImage = context.PreEntityImages["PreImage"];
            string customerStringId = preImage.GetAttributeValue<string>("cr371_customerid");
            tracingService.Trace("Customer ID from Pre-Image: " + customerStringId);

            // Fetch GUID from Dataverse
            QueryExpression query = new QueryExpression("cr371_customer_p")
            {
                ColumnSet = new ColumnSet("cr371_customerid"),
                Criteria = { Conditions = { new ConditionExpression("cr371_customerid", ConditionOperator.Equal, customerStringId) } }
            };
            EntityCollection customers = service.RetrieveMultiple(query);

            if (customers.Entities.Count == 0)
            {
                tracingService.Trace("Customer GUID not found");
                return;
            }

            Guid customerGuid = customers.Entities[0].Id;
            tracingService.Trace("Fetched Customer GUID: " + customerGuid);

            // Fetch and Store Related Orders Before Deletion
            QueryExpression orderQuery = new QueryExpression("cr371_order_p")
            {
                ColumnSet = new ColumnSet("cr371_orderid"),
                Criteria = { Conditions = { new ConditionExpression("cr371_customerid", ConditionOperator.Equal, customerGuid) } }
            };
            EntityCollection relatedOrders = service.RetrieveMultiple(orderQuery);

            if (relatedOrders.Entities.Count == 0)
            {
                tracingService.Trace("No related orders found for the customer.");
                return;
            }

            List<Guid> orderIds = relatedOrders.Entities.Select(o => o.Id).ToList();
            tracingService.Trace("Stored " + orderIds.Count + " orders for deletion.");

            // Now that we have stored the order IDs, the system can proceed with deleting the customer.
            tracingService.Trace("Customer deletion will proceed now.");

            // Delete Related Orders After Customer is Deleted
            foreach (var orderId in orderIds)
            {
                service.Delete("cr371_order_p", orderId);
                tracingService.Trace("Deleted Order ID: " + orderId);
            }
        }
    }
}
