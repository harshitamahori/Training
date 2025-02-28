using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DemoPlugin.Plugins
{
    public class SampleUpdate:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);
            if(context.PrimaryEntityName == "account")
            {
                if (context.Depth > 1)
                    return;
                Entity accountRecord = service.Retrieve("account", context.PrimaryEntityId, new ColumnSet("paymenttermscode", "address1_shippingmethodcode"));
                int shippingTerms = accountRecord.Contains("address1_shippingmethodcode") ?accountRecord.GetAttributeValue<OptionSetValue>("address1_shippingmethodcode").Value : 2;
                int paymentTerms = accountRecord.Contains("paymenttermscode") ?accountRecord.GetAttributeValue<OptionSetValue>("paymenttermscode").Value : 2;

                //new entity as account . Why? - If use accountRecord doent work as retrieve only 2 columns craete 2 rows
                Entity accountToUpdate = new Entity("account");
                accountToUpdate.Id = context.PrimaryEntityId;
                if (shippingTerms == 1 && paymentTerms == 1)
                {
                    accountToUpdate["revenue"] = new Money(30);
                }

                else
                {
                    accountToUpdate["revenue"] = new Money(100);
                }

                service.Update(accountToUpdate);

            }
        }
    }
}
