namespace PrivateWiki
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