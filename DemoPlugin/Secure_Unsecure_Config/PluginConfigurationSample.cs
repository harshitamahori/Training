using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

using DemoPlugin.DataContract;
using DemoPlugin.Helper;

namespace DemoPlugin.Secure_Unsecure_Config
{
    public class PluginConfigurationSample:IPlugin
    {
        //class variables 
        public readonly string unsecureValue;
        public readonly string secureValue;
        public PluginConfigurationSample(string unsecureConfig, string secureConfig)//{"Name":"XYZ"}
        {
            if (String.IsNullOrWhiteSpace(unsecureConfig)) //if null or now value
            {
                unsecureValue = string.Empty;

            }
            else
            {
               Config data = JSONHelper.Deserialize<Config>(unsecureConfig);
                unsecureValue = data.Name;

            }
            if (string.IsNullOrWhiteSpace(secureConfig))
            {
                secureValue = string.Empty;
            }
            else
            {
                Config data = JSONHelper.Deserialize<Config>(secureConfig);
                secureValue = data.Name;
            }

        }
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                if (context.Depth > 1) return;
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
                {
                    IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = factory.CreateOrganizationService(context.InitiatingUserId);

                    EntityReference applicationEntityRef = context.InputParameters["Target"] as EntityReference;

                    Entity updateApplication = new Entity(applicationEntityRef.LogicalName);
                    updateApplication.Id = applicationEntityRef.Id;
                    updateApplication["cr371_customername"] = secureValue + " " + unsecureValue;
                    service.Update(updateApplication);
                }
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);

            }

        }
    }
}
