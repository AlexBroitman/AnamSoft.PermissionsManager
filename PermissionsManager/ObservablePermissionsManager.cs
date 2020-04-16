using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AnamSoft.PermissionsManager
{
    /// <inheritdoc/>
    /// <summary>
    /// Generic observable PermissionsManager that that provides notifications when roles get added, removed, or cleared.
    /// </summary>
    public class ObservablePermissionsManager<TSubject, TObject, TRole> : PermissionsManager<TSubject, TObject, TRole>, INotifyPermissionChanged<TSubject, TObject, TRole>
    {
        /// <inheritdoc/>
        public event PermissionChangedEventHandler<TSubject, TObject, TRole>? PermissionChanged;

        /// <summary>
        /// Initializes new instance of the <see cref="ObservablePermissionsManager{TSubject, TObject, TRole}"/> class.
        /// </summary>
        public ObservablePermissionsManager()
        {

        }

        /// <inheritdoc/>
        /// <summary>
        /// Adds a <paramref name="role"/> for the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        public override bool AddRole(TSubject subj, TObject obj, TRole role)
        {
            if (!base.AddRole(subj, obj, role))
                return false;

            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Add, subj, obj, newRoles: new[] { role }));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Adds <paramref name="roles"/> for the <paramref name="subj"/> on the <paramref name="obj"/> and notofies the subscribers.
        /// </summary>
        public override bool AddRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (!base.AddRoles(subj, obj, roles))
                return false;

            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Add, subj, obj, newRoles: roles));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Sets <paramref name="roles"/> for the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        public override void SetRoles(TSubject subj, TObject obj, IReadOnlyCollection<TRole> roles)
        {
            base.SetRoles(subj, obj, roles);
            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Reset, subj, obj, newRoles: roles));
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes a <paramref name="role"/> from the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        public override bool RemoveRole(TSubject subj, TObject obj, TRole role)
        {
            if (!base.RemoveRole(subj, obj, role))
                return false;

            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj, obj, oldRoles: new[] { role }));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes <paramref name="roles"/> from the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        public override bool RemoveRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (!base.RemoveRoles(subj, obj, roles))
                return false;
            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj, obj, oldRoles: roles));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes all roles from the subject <paramref name="subj"/> and notifies the subscribers.
        /// </summary>
        public override bool RemoveAllSubjectRoles(TSubject subj)
        {
            if (!base.RemoveAllSubjectRoles(subj))
                return false;
            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj: subj));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes all roles that were on the object <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        public override bool RemoveAllObjectRoles(TObject obj)
        {
            if (!base.RemoveAllObjectRoles(obj))
                return false;
            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, obj: obj));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes all roles from the subject <paramref name="subj"/> on the object <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        public override bool RemoveAllRoles(TSubject subj, TObject obj)
        {
            if (!base.RemoveAllRoles(subj, obj))
                return false;
            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj, obj));
            return true;
        }

        /// <summary>
        /// Clears the <see cref="ObservablePermissionsManager{TSubject, TObject, TRole}"/> and notifies to subscribers.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            PermissionChanged?.Invoke(this, new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Reset));
        }
    }
}
