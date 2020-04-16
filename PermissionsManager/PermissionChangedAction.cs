namespace AnamSoft.PermissionsManager
{
    /// <summary>
    /// Describes the action that caused a <see cref="INotifyPermissionChanged{TSubject, TObject, TRole}.PermissionChanged"/> event.
    /// </summary>
    public enum PermissionChangedAction
    {
        /// <summary>
        /// A permissions were added.
        /// </summary>
        Add,
        /// <summary>
        /// A permissions were removed.
        /// </summary>
        Remove,
        /// <summary>
        /// A permissions were reset (cleared).
        /// </summary>
        Reset
    }
}