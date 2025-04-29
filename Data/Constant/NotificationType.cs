namespace Firebase_Auth.Data.Constant;
/// <summary>
/// Enum representing the recipient type for notifications.
/// </summary>
public enum NotificationRecipientType
{
    /// <summary>
    /// Notification is intended for a specific user.
    /// </summary>
    SpecificUser = 0,

    /// <summary>
    /// Notification is intended for a general audience.
    /// </summary>
    General = 1
}