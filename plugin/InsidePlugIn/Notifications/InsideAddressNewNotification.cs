using vng.core.Enums;
using vng.core.Helpers;
using vng.core.Interfaces;
using vng.core.Interfaces.Notification;
using vng.core.Models.Notification;

namespace vng.plugin.insideplugin.Notifications
{
    public class InsideAddressNewNotification : IEntityNotification
    {
        public EntityNotificationModel CheckNotification(IDataBase data, NotificationDataOperation dataOperation)
        {        
            if (dataOperation == NotificationDataOperation.create
                && data.GetDictData().TryFindKeyAndGetValue("Adressherkunft", out string adressherkunft)
                && !string.IsNullOrEmpty(adressherkunft)
                && adressherkunft == "Inside")
            {
                return new EntityNotificationModel()
                {
                    MessageObject = new
                    {
                        Info = "Plugin"
                    }
                };
            }
            return null;
        }
    }
}
