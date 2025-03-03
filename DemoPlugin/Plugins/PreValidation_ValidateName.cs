using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace DemoPlugin.Plugins
{
    public class PreValidation_ValidateName:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                if(entity.LogicalName=="account" && entity.Attributes.Contains("name"))
                {
                    string name = entity["name"].ToString();

                    if(Regex.IsMatch(name, @"\d"))
                    {
                        throw new InvalidPluginExecutionException("Account name cannot contain number");
                    }
                }
            }
        }
    }
}
