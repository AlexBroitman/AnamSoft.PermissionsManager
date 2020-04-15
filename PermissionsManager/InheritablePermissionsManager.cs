using AnamSoft.ReadOnlySet;
using AnamSoft.DependencyGraph;
using System;
using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    public class InheritablePermissionsManager<TSubject, TObject, TRole> : PermissionsManager<TSubject, TObject, TRole>, IInheritablePermissionsManager<TSubject, TObject, TRole>
    {
        private readonly DependencyGraph<TSubject> _subjInheritance = new DependencyGraph<TSubject>();
        private readonly DependencyGraph<TObject> _objInheritance = new DependencyGraph<TObject>();

        #region IInheritablePermissionsManager
        public bool AddSubjectInheritance(TSubject inheritor, TSubject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _subjInheritance.AddDependency(inheritor, origin);
        }

        public bool RemoveSubjectInheritance(TSubject inheritor, TSubject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _subjInheritance.RemoveDependency(inheritor, origin);
        }

        public bool AddObjectInheritance(TObject inheritor, TObject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _objInheritance.AddDependency(inheritor, origin);
        }

        public bool RemoveObjectInheritance(TObject inheritor, TObject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));
            return _objInheritance.RemoveDependency(inheritor, origin);
        }

        public bool IsSubjectInherits(TSubject inheritor, TSubject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _subjInheritance.IsDepends(inheritor, origin);
        }

        public bool IsObjectInherits(TObject inheritor, TObject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _objInheritance.IsDepends(inheritor, origin);
        }

        public IReadOnlyCollection<TRole> GetDirectRoles(TSubject subj, TObject obj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));

            return new RoleCollection(base.GetRolesHashSet(subj, obj));
        }
        #endregion

        #region Overrides
        protected override HashSet<TRole> GetRolesHashSet(TSubject subj, TObject obj)
        {
            // get direct roles
            var roles = base.GetRolesHashSet(subj, obj);

            var originSubjs = _subjInheritance.GetDirectDependencies(subj);
            var originObjs = _objInheritance.GetDirectDependencies(obj);

            if (originSubjs.IsEmpty && originObjs.IsEmpty)
                return roles;

            roles = new HashSet<TRole>(roles, roles.Comparer);

            if (!originSubjs.IsEmpty)
                AddInheritedRolesFromSubjects(roles, subj, obj, originSubjs);

            if (!originObjs.IsEmpty)
                AddInheritedRolesFromObjects(roles, subj, obj, originObjs);

            return roles;
        }

        private void AddInheritedRolesFromSubjects(HashSet<TRole> roles, TSubject subj, TObject obj, IReadOnlySet<TSubject> originSubjs)
        {
            var processedSubjects = new HashSet<TSubject> { subj };
            var subjStack = new Stack<TSubject>(originSubjs);
            while (subjStack.Count > 0)
            {
                var originSubj = subjStack.Pop();
                if (!processedSubjects.Contains(originSubj))
                {
                    var originRoles = base.GetRolesHashSet(originSubj, obj);
                    roles.UnionWith(originRoles);

                    foreach (var subOriginSubj in _subjInheritance.GetDirectDependencies(originSubj))
                        subjStack.Push(subOriginSubj);

                    processedSubjects.Add(originSubj);
                }
            }
        }

        private void AddInheritedRolesFromObjects(HashSet<TRole> roles, TSubject subj, TObject obj, IReadOnlySet<TObject> originObjs)
        {
            var processedObjects = new HashSet<TObject> { obj };
            var objStack = new Stack<TObject>(originObjs);
            while (objStack.Count > 0)
            {
                var originObj = objStack.Pop();
                if (!processedObjects.Contains(originObj))
                {
                    var originRoles = base.GetRolesHashSet(subj, originObj);
                    roles.UnionWith(originRoles);

                    foreach (var subOriginObj in _objInheritance.GetDirectDependencies(originObj))
                        objStack.Push(subOriginObj);

                    processedObjects.Add(originObj);
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            _subjInheritance.Clear();
            _objInheritance.Clear();
        }
        #endregion
    }
}
