using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnamSoft.PermissionsManager
{
    /// <summary>
    /// Generic PermissionsManager
    /// </summary>
    /// <typeparam name="TSubject">A type of permissions subject (User, UserGroup, Profile, Resource, etc).</typeparam>
    /// <typeparam name="TObject">A type of permissions object (File, WorkItem, etc).</typeparam>
    /// <typeparam name="TRole">A type of role</typeparam>
    public class PermissionsManager<TSubject, TObject, TRole> : IPermissionsManager<TSubject, TObject, TRole>
    {
        /// <summary>
        /// Internal permissions storage 
        /// </summary>
        protected readonly Dictionary<TSubject, Dictionary<TObject, HashSet<TRole>>> Storage;

        /// <summary>
        /// Initializes a new instance of <see cref="PermissionsManager{TSubject, TObject, TRole}"/> class that is empty.
        /// </summary>
        public PermissionsManager()
        {
            Storage = new Dictionary<TSubject, Dictionary<TObject, HashSet<TRole>>>();
        }

        /// <summary>
        /// Gets all roles that <paramref name="subj"/> has on <paramref name="obj"/>. Same as getter of <see cref="this[TSubject, TObject]"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="RoleCollection"/> that contains all roles that <paramref name="subj"/> has on <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> are <see langword="null"/>.</exception>
        /// <inheritdoc/>
        public RoleCollection GetRoles(TSubject subj, TObject obj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));

            return new RoleCollection(GetDirectRolesHashSet(subj, obj));
        }

        IReadOnlyCollection<TRole> IPermissionsManager<TSubject, TObject, TRole>.GetRoles(TSubject subj, TObject obj) => GetRoles(subj, obj);

        /// <inheritdoc/>
        public virtual void SetRoles(TSubject subj, TObject obj, IReadOnlyCollection<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            if (Storage.TryGetValue(subj, out var allSubjRoles))
            {
                if (roles.Count == 0)
                    allSubjRoles.Remove(obj);
                else
                    allSubjRoles[obj] = new HashSet<TRole>(roles);
            }
            else if (roles.Count > 0)
                Storage[subj] = new Dictionary<TObject, HashSet<TRole>> { [obj] = new HashSet<TRole>(roles) };
        }

        /// <inheritdoc/>
        public RoleCollection this[TSubject subj, TObject obj]
        {
            get => GetRoles(subj, obj);
            set => SetRoles(subj, obj, value);
        }

        IReadOnlyCollection<TRole> IPermissionsManager<TSubject, TObject, TRole>.this[TSubject subj, TObject obj]
        {
            get => GetRoles(subj, obj);
            set => SetRoles(subj, obj, value);
        }

        /// <inheritdoc/>
        public virtual bool AddRole(TSubject subj, TObject obj, TRole role)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (role is null) throw new ArgumentNullException(nameof(role));

            if (Storage.TryGetValue(subj, out var allSubjRoles))
            {
                if (allSubjRoles.TryGetValue(obj, out var existingRoles))
                    return existingRoles.Add(role);

                allSubjRoles.Add(obj, new HashSet<TRole> { role });
            }
            else
                Storage.Add(subj, new Dictionary<TObject, HashSet<TRole>> { [obj] = new HashSet<TRole> { role } });
            return true;
        }

        /// <inheritdoc/>
        public virtual bool AddRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            if (Storage.TryGetValue(subj, out var allSubjRoles))
            {
                if (allSubjRoles.TryGetValue(obj, out var existingRoles))
                    return roles.Aggregate(false, (res, role) => res | existingRoles.Add(role));

                var newRoles = new HashSet<TRole>(roles);
                if (newRoles.Count == 0)
                    return false;

                allSubjRoles.Add(obj, newRoles);
                return true;
            }
            else
            {
                var newRoles = new HashSet<TRole>(roles);
                if (newRoles.Count == 0)
                    return false;

                Storage.Add(subj, new Dictionary<TObject, HashSet<TRole>> { [obj] = newRoles });
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual bool RemoveRole(TSubject subj, TObject obj, TRole role)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (role is null) throw new ArgumentNullException(nameof(role));

            return Storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.TryGetValue(obj, out var roles) && roles.Remove(role);
        }

        /// <inheritdoc/>
        public virtual bool RemoveRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            return Storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.TryGetValue(obj, out var existingRoles)
                && roles.Aggregate(false, (res, role) => res | existingRoles.Remove(role));
        }

        /// <inheritdoc/>
        public virtual bool RemoveAllSubjectRoles(TSubject subj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            return Storage.Remove(subj);
        }

        /// <inheritdoc/>
        public virtual bool RemoveAllObjectRoles(TObject obj)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return Storage.Values.Aggregate(false, (res, roles) => res | roles.Remove(obj));
        }

        /// <inheritdoc/>
        public virtual bool RemoveAllRoles(TSubject subj, TObject obj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return Storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.Remove(obj);
        }

        /// <inheritdoc/>
        public bool HasRole(TSubject subj, TObject obj, TRole role)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (role is null) throw new ArgumentNullException(nameof(role));

            return GetDirectRolesHashSet(subj, obj).Contains(role);
        }

        /// <inheritdoc/>
        public bool HasAllRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            return GetDirectRolesHashSet(subj, obj).IsSupersetOf(roles);
        }

        /// <inheritdoc/>
        public bool HasAnyRole(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            return GetDirectRolesHashSet(subj, obj).Overlaps(roles);
        }

        /// <summary>
        /// Clears the <see cref="PermissionsManager{TSubject, TObject, TRole}"/>.
        /// </summary>
        public virtual void Clear() => Storage.Clear();

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> that contains all direct roles that subject <paramref name="subj"/> has on object <paramref name="obj"/>.
        /// </summary>
        /// <remarks>You should not modify this <see cref="HashSet{T}"/>.</remarks>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns>A <see cref="HashSet{T}"/> that contains all direct roles that subject <paramref name="subj"/> has on object <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> are <see langword="null"/>.</exception>
        protected virtual HashSet<TRole> GetDirectRolesHashSet(TSubject subj, TObject obj)
            => Storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.TryGetValue(obj, out var roles) ? roles : new HashSet<TRole>();

        /// <summary>
        /// Read-only collection of roles
        /// </summary>
        public sealed class RoleCollection : IReadOnlyCollection<TRole>
        {
            /// <summary>
            /// Empty <see cref="RoleCollection"/>.
            /// </summary>
            public static RoleCollection Empty = new RoleCollection(new TRole[0]);

            private readonly ICollection<TRole> _roles;

            /// <summary>
            /// Initializes new instance of <see cref="RoleCollection"/> that has all items from specified collection.
            /// </summary>
            /// <param name="roles"></param>
            public RoleCollection(ICollection<TRole> roles)
            {
                _roles = roles;
            }

            /// <summary>
            /// Returns number of roles in the <see cref="RoleCollection"/>.
            /// </summary>
            /// <value>The number of roles in the <see cref="RoleCollection"/>.</value>
            public int Count => _roles.Count;

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<TRole> GetEnumerator() => _roles.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
