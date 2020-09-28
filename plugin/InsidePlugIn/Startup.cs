using Microsoft.Extensions.DependencyInjection;
using vng.core.Interfaces;
using vng.core.Interfaces.Notification;
using vng.plugin.insideplugin.Notifications;
using vng.plugin.insideplugin.Workflows;
using vng.plugincore.Interfaces;

namespace vng.plugin.insideplugin
{
    public class Startup : IPlugin, IConfigureServices
    {
        public string Name => "InsidePlugin";

        public void ConfigureServices(IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services.AddTransient<IEntityNotification, InsideAddressNewNotification>();
            mvcBuilder.Services.AddTransient<IDataWorkflowAfterSave, AddressAfterSaveInsideWorkflow>();
        }
    }
}
