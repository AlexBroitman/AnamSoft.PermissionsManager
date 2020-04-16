using System;

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
        event EventHandler<PermissionChangedEventArgs<TSubject, TObject, TRole>> PermissionChanged;
    }
}

