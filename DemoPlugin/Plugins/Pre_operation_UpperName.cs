using System;
using Microsoft.Xrm.Sdk;

namespace DemoPlugin.Plugins
{
    public class Pre_operation_UpperName : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    if (entity.LogicalName == "account" && entity.Attributes.Contains("name"))
                    {
                        string accountName = entity.Attributes["name"].ToString();
                        entity.Attributes["name"] = accountName.ToUpper();

                        //to test rollback
                        //throw new InvalidPluginExecutionException("Simulated error to trigger rollback.");

                    }
                    
                   
                }
                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException("Error in Pre-Operation: " + ex.Message);
                }
            }
        } 
    }
}
