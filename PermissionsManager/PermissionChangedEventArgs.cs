using System;
using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
#nullable disable
    /// <summary>
    /// Provides data for the <see cref="INotifyPermissionChanged{TSubject, TObject, TRole}.PermissionChanged"/> event.
    /// </summary>
    public class PermissionChangedEventArgs<TSubject, TObject, TRole> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionChangedEventArgs{TSubject, TObject, TRole}"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="oldRoles">Removed roles.</param>
        /// <param name="newRoles">Added roles.</param>
        public PermissionChangedEventArgs(PermissionChangedAction action, TSubject subj = default, TObject obj = default, IEnumerable<TRole> oldRoles = null, IEnumerable<TRole> newRoles = null)
        {
            AssertArgs(action, subj, obj, oldRoles, newRoles);
            Action = action;
            Subject = subj;
            Object = obj;
            OldRoles = oldRoles;
            NewRoles = newRoles;
        }

        private static void AssertArgs(PermissionChangedAction action, TSubject subj, TObject obj, IEnumerable<TRole> oldRoles, IEnumerable<TRole> newRoles)
        {
            if (action == PermissionChangedAction.Add || action == PermissionChangedAction.Remove)
            {
                if (subj is null) throw new ArgumentNullException(nameof(subj));
                if (obj is null) throw new ArgumentNullException(nameof(subj));
                if (action == PermissionChangedAction.Add && newRoles is null)
                    throw new ArgumentNullException(nameof(newRoles));
                if (action == PermissionChangedAction.Remove && oldRoles is null)
                    throw new ArgumentNullException(nameof(oldRoles));
            }
        }

        /// <summary>
        /// Gets the action that caused the event.
        /// </summary>
        /// <value>A <see cref="PermissionChangedAction"/> value that describes the action that caused the event.</value>
        public PermissionChangedAction Action { get; set; }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public TSubject Subject { get; set; }

        /// <summary>
        /// Get the object.
        /// </summary>
        /// <value>The object.</value>
        public TObject Object { get; set; }

        /// <summary>
        /// Gets removed roles.
        /// </summary>
        /// <value>Removed roles.</value>
        public IEnumerable<TRole> OldRoles { get; set; }

        /// <summary>
        /// Gets added roles
        /// </summary>
        /// <value>Added roles.</value>
        public IEnumerable<TRole> NewRoles { get; set; }
    }
#nullable restore
}
