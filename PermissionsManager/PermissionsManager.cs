using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnamSoft.PermissionsManager
{
    public class PermissionsManager<TSubject, TObject, TRole> : IPermissionsManager<TSubject, TObject, TRole>
    {
        private readonly Dictionary<TSubject, Dictionary<TObject, HashSet<TRole>>> _storage = new Dictionary<TSubject, Dictionary<TObject, HashSet<TRole>>>();

        public RoleCollection GetRoles(TSubject subj, TObject obj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));

            return new RoleCollection(GetRolesHashSet(subj, obj));
        }

        IReadOnlyCollection<TRole> IPermissionsManager<TSubject, TObject, TRole>.GetRoles(TSubject subj, TObject obj) => GetRoles(subj, obj);

        public void SetRoles(TSubject subj, TObject obj, IReadOnlyCollection<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            if (_storage.TryGetValue(subj, out var allSubjRoles))
            {
                if (roles.Count == 0)
                    allSubjRoles.Remove(obj);
                else
                    allSubjRoles[obj] = new HashSet<TRole>(roles);
            }
            else if (roles.Count > 0)
                _storage[subj] = new Dictionary<TObject, HashSet<TRole>> { [obj] = new HashSet<TRole>(roles) };
        }

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

        public bool AddRole(TSubject subj, TObject obj, TRole role)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (role is null) throw new ArgumentNullException(nameof(role));

            if (_storage.TryGetValue(subj, out var allSubjRoles))
            {
                if (allSubjRoles.TryGetValue(obj, out var existingRoles))
                    return existingRoles.Add(role);

                allSubjRoles.Add(obj, new HashSet<TRole> { role });
            }
            else
                _storage.Add(subj, new Dictionary<TObject, HashSet<TRole>> { [obj] = new HashSet<TRole> { role } });
            return true;
        }

        public bool AddRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            if (_storage.TryGetValue(subj, out var allSubjRoles))
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

                _storage.Add(subj, new Dictionary<TObject, HashSet<TRole>> { [obj] = newRoles });
                return true;
            }
        }

        public bool RemoveRole(TSubject subj, TObject obj, TRole role)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (role is null) throw new ArgumentNullException(nameof(role));

            return _storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.TryGetValue(obj, out var roles) && roles.Remove(role);
        }

        public bool RemoveRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            return _storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.TryGetValue(obj, out var existingRoles)
                && roles.Aggregate(false, (res, role) => res | existingRoles.Remove(role));
        }

        public bool RemoveAllSubjectRoles(TSubject subj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            return _storage.Remove(subj);
        }

        public bool RemoveAllObjectRoles(TObject obj)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return _storage.Values.Aggregate(false, (res, roles) => res | roles.Remove(obj));
        }

        public bool RemoveAllRoles(TSubject subj, TObject obj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return _storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.Remove(obj);
        }

        public bool HasRole(TSubject subj, TObject obj, TRole role)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (role is null) throw new ArgumentNullException(nameof(role));

            return GetRolesHashSet(subj, obj).Contains(role);
        }

        public bool HasAllRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            return GetRolesHashSet(subj, obj).IsSupersetOf(roles);
        }

        public bool HasAnyRole(TSubject subj, TObject obj, IEnumerable<TRole> roles)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (roles is null) throw new ArgumentNullException(nameof(roles));

            return GetRolesHashSet(subj, obj).Overlaps(roles);
        }

        public virtual void Clear() => _storage.Clear();

        protected virtual HashSet<TRole> GetRolesHashSet(TSubject subj, TObject obj)
            => _storage.TryGetValue(subj, out var allSubjRoles) && allSubjRoles.TryGetValue(obj, out var roles) ? roles : new HashSet<TRole>();

        public sealed class RoleCollection : IReadOnlyCollection<TRole>
        {
            public static RoleCollection Empty = new RoleCollection(new TRole[0]);

            private readonly ICollection<TRole> _roles;

            public RoleCollection(ICollection<TRole> roles)
            {
                _roles = roles;
            }

            public int Count => _roles.Count;

            public IEnumerator<TRole> GetEnumerator() => _roles.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
