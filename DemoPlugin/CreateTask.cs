using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//namespace for d365 interaction
using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk;

namespace DemoPlugin
{
    public class CreateTask:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            //Obtain Eecution context from service provider
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if(context.InputParameters.Contains("Target")&& context.InputParameters["Target"] is Entity)
            {
                //obtain target entity from input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                //If not, plugin wasn't register  correctly
                if (entity.LogicalName != "account")
                    return;
                try
                {
                    //create a task entity to foloowup with the account customer in 7 days .
                    Entity followup = new Entity("task");
                    followup["subject"] = "Send e-mail to new customer";
                    followup["description"] = "Follow with the customer";
                    followup["scheduledstart"] = DateTime.Now.AddDays(7);
                    followup["scheduledend"] = DateTime.Now.AddDays(7);
                    followup["category"] = context.PrimaryEntityName;

                    //Refer to the acoount in task entity.
                    if (context.OutputParameters.Contains("id"))
                    {
                        //lookup
                        Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                        string regardingobjectidType = "account";

                        followup["regardingobjectid"] = new EntityReference(regardingobjectidType, regardingobjectid);

                    }
                        //Obtain the organisation service reference
                        IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                        IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                        service.Create(followup);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                        throw new InvalidPluginExecutionException("An error occured in followup plugin ",ex);
                    }
            }
                
            else
            {
                return;
            }
        }
    }
}
