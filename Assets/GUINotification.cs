using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NotificationType
{
	Error,
	Valid,
}

public class GUINotification : MonoBehaviour
{
	public Text			notifText;
	public Image		notifImage;
	public float		notifTimeout = 2;

	public static GUINotification	instance;

	static Dictionary< NotificationType, Color > notificationColors = new Dictionary<NotificationType, Color>()
	{
		{NotificationType.Error, new Color(.5f, 0, 0, 1)},
		{NotificationType.Valid, new Color(0, .5f, 0, 1)},
	};

	struct Notification
	{
		public string	text;
		public Color	color;

		public Notification(string text, NotificationType type)
		{
			this.text = text;
			this.color = notificationColors[type];
		}
	}
	
	Queue< Notification >		notifications = new Queue< Notification >();

	private void Awake()
	{
		instance = this;

		StartCoroutine(UpdateNotifications());
	}

	public void AddNotification(string notif, NotificationType type = NotificationType.Valid)
	{
		notifications.Enqueue(new Notification(notif, type));
	}

	IEnumerator UpdateNotifications()
	{
		while (true)
		{
	
			while (notifications.Count != 0)
			{
				notifImage.gameObject.SetActive(true);

				var notif = notifications.Dequeue();
	
				notifText.text = notif.text;
				notifImage.color = notif.color;
				
				yield return new WaitForSeconds(notifTimeout);
			}
	
			notifImage.gameObject.SetActive(false);

			yield return new WaitForEndOfFrame();
		}
	}
}
