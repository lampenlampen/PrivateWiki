namespace PrivateWiki.Services.GlobalNotificationService
{
	public interface IGlobalNotificationManager
	{
		void ShowNotImplementedNotification();
		void ShowOperationFinishedNotification();
		void ShowPageLockedNotification();
		void ShowCreatePageInSystemNamespaceNotAllowedNotification();
		void ShowPageExistsNotificationOnUIThread();
	}
}