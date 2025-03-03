using System;
using System.CodeDom;
using Microsoft.Xrm.Sdk;

namespace DemoPlugin.Plugins
{
    public class Pre_Validation_Fax:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                try
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "account") return;

                    if (!entity.Attributes.Contains("fax"))
                    {
                        throw new InvalidPluginExecutionException("Fax number is required before creating account");
                    }
                }
                catch(Exception ex)
                {
                    throw new InvalidPluginExecutionException($"Pre-validation Error: {ex.Message}");

                }
            }
            
        }
    }
}
