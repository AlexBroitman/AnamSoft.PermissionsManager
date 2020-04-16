namespace AnamSoft.PermissionsManager
{
    /// <summary>
    /// Notifies listeners of permissions changes, such as when a roles are added, removed or cleared.
    /// </summary>
    public interface INotifyPermissionChanged<TSubject, TObject, TRole>
    {
        /// <summary>
        /// Occurs when the permission changed.
        /// </summary>
        event PermissionChangedEventHandler<TSubject, TObject, TRole> PermissionChanged;
    }

    /// <summary>
    /// Represents the method that handles the <see cref="INotifyPermissionChanged{TSubject, TObject, TRole}.PermissionChanged"/> event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void PermissionChangedEventHandler<TSubject, TObject, TRole>(object sender, PermissionChangedEventArgs<TSubject, TObject, TRole> e);
}