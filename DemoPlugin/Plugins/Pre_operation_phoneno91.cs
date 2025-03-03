using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DemoPlugin.Plugins
{
    public class Pre_operation_phoneno91: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    if(entity.LogicalName=="account" && entity.Attributes.Contains("telephone1") && entity.Attributes.Contains("address1_country"))
                    {
                        string phoneNumber = entity.Attributes["telephone1"].ToString();
                        string country = entity.Attributes["address1_country"].ToString().ToLower();

                        if(country=="india" && !phoneNumber.StartsWith("+91"))
                        {
                            entity["telephone1"] = "+91" + phoneNumber;

                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new InvalidPluginExecutionException("Error in Pre-operation Plugin: " + ex.Message);
                }
            }
        }
    }
}
