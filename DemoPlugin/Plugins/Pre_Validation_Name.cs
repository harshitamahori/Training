using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DemoPlugin.Plugins
{
    public class Pre_Validation_Name:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                if (entity.LogicalName != "account") return;

                if (!entity.Attributes.Contains("name") || string.IsNullOrWhiteSpace(entity["name"].ToString()))
                {
                    throw new InvalidPluginExecutionException("Name is required");
                }

            }
        }
    }
}
