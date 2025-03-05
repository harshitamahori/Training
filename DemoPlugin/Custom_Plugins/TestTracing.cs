using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
namespace DemoPlugin.Custom_Plugins
{
    public class TestTracing:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingService.Trace("Plugin Execution Started");

            
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            
            if (context.PrimaryEntityName == "cr371_customer_p" && context.MessageName.ToLower() == "delete")
            {
                tracingService.Trace("Deleting Customer record...");

                //throw new InvalidPluginExecutionException("Test Exception: Customer deletion is being tracked.");
            }
            tracingService.Trace("Task Completed");
        }
    }
}
