using System;
using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    /// <inheritdoc/>
    /// <summary>
    /// Generic observable PermissionsManager that that provides notifications when roles get added, removed, or cleared.
    /// </summary>
    public class ObservablePermissionsManager<TSubject, TObject, TRole> : PermissionsManager<TSubject, TObject, TRole>, INotifyPermissionChanged<TSubject, TObject, TRole>
    {
        /// <summary>
        /// Initializes new instance of the <see cref="ObservablePermissionsManager{TSubject, TObject, TRole}"/> class.
        /// </summary>
        public ObservablePermissionsManager()
        {
        }

        /// <inheritdoc/>
        public event EventHandler<PermissionChangedEventArgs<TSubject, TObject, TRole>>? PermissionChanged;

        #region Overrides
        /// <summary>
        /// Raises the <see cref="PermissionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="ea">Arguments of the event being raised.</param>
        protected virtual void OnPermissionChanged(PermissionChangedEventArgs<TSubject, TObject, TRole> ea) => PermissionChanged?.Invoke(this, ea);

        /// <inheritdoc/>
        /// <summary>
        /// Adds a <paramref name="role"/> for the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        protected override bool AddRoleTrusted(TSubject subj, TObject obj, TRole role)
        {
            if (!base.AddRoleTrusted(subj, obj, role))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Add, subj, obj, newRoles: new[] { role }));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Adds <paramref name="roles"/> for the <paramref name="subj"/> on the <paramref name="obj"/> and notofies the subscribers.
        /// </summary>
        protected override bool AddRolesTrusted(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (!base.AddRolesTrusted(subj, obj, roles))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Add, subj, obj, newRoles: roles));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Sets <paramref name="roles"/> for the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        protected override void SetRolesTrusted(TSubject subj, TObject obj, IReadOnlyCollection<TRole> roles)
        {
            base.SetRolesTrusted(subj, obj, roles);
            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Reset, subj, obj, newRoles: roles));
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes a <paramref name="role"/> from the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        protected override bool RemoveRoleTrusted(TSubject subj, TObject obj, TRole role)
        {
            if (!base.RemoveRoleTrusted(subj, obj, role))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj, obj, oldRoles: new[] { role }));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes <paramref name="roles"/> from the <paramref name="subj"/> on the <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        protected override bool RemoveRolesTrusted(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (!base.RemoveRolesTrusted(subj, obj, roles))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj, obj, oldRoles: roles));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes all roles from the subject <paramref name="subj"/> and notifies the subscribers.
        /// </summary>
        protected override bool RemoveAllSubjectRolesTrusted(TSubject subj)
        {
            if (!base.RemoveAllSubjectRolesTrusted(subj))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj: subj));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes all roles that were on the object <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        protected override bool RemoveAllObjectRolesTrusted(TObject obj)
        {
            if (!base.RemoveAllObjectRolesTrusted(obj))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, obj: obj));
            return true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes all roles from the subject <paramref name="subj"/> on the object <paramref name="obj"/> and notifies the subscribers.
        /// </summary>
        protected override bool RemoveAllRolesTrusted(TSubject subj, TObject obj)
        {
            if (!base.RemoveAllRolesTrusted(subj, obj))
                return false;

            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Remove, subj, obj));
            return true;
        }

        /// <summary>
        /// Clears the <see cref="ObservablePermissionsManager{TSubject, TObject, TRole}"/> and notifies to subscribers.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            OnPermissionChanged(new PermissionChangedEventArgs<TSubject, TObject, TRole>(PermissionChangedAction.Reset));
        }
        #endregion
    }
}
