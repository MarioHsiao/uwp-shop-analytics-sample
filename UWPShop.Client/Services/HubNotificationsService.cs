using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Messaging;

using UWPShop.Client.Activation;

using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;
using Windows.ApplicationModel.Core;
using UWPShop.Client.ViewModels;
using Windows.UI.Core;

namespace UWPShop.Client.Services
{
    internal class HubNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public async void InitializeAsync()
        {
            // See more about adding push notifications to your Windows app
            // Documentation: https://docs.microsoft.com/azure/app-service-mobile/app-service-mobile-windows-store-dotnet-get-started-push

            // Use your Hub Name here
            var hubName = "eventshub";
            // Use your DefaultListenSharedAccessSignature here
            var accessSignature = AppConfig.PushKey;

            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var hub = new NotificationHub(hubName, accessSignature);
            var result = await hub.RegisterNativeAsync(channel.Uri);
            if (result.RegistrationId != null)
            {
                // RegistrationID let you know it was successful
            }

            // You can also send push notifications from Windows Developer Center targeting your app consumers
            // Documentation: https://docs.microsoft.com/windows/uwp/publish/send-push-notifications-to-your-apps-customers
            channel.PushNotificationReceived += Channel_PushNotificationReceived;
        }

        private async void Channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                await MainViewModel.Current.Initialize();
            });
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            // TODO UWPTemplates: Handle activation from toast notification,
            // for more info handling activation see
            // Documentation: https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/

            await Task.CompletedTask;
        }
    }
}
