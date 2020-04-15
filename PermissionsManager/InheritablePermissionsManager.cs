using AnamSoft.ReadOnlySet;
using AnamSoft.DependencyGraph;
using System;
using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    /// <summary>
    /// Generic InheritablePermissionsManager
    /// </summary>
    /// <inheritdoc/>
    public class InheritablePermissionsManager<TSubject, TObject, TRole> : PermissionsManager<TSubject, TObject, TRole>, IInheritablePermissionsManager<TSubject, TObject, TRole>
    {
        private readonly DependencyGraph<TSubject> _subjInheritance = new DependencyGraph<TSubject>();
        private readonly DependencyGraph<TObject> _objInheritance = new DependencyGraph<TObject>();

        #region IInheritablePermissionsManager
        /// <inheritdoc/>
        public bool AddSubjectInheritance(TSubject inheritor, TSubject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _subjInheritance.AddDependency(inheritor, origin);
        }

        /// <inheritdoc/>
        public bool RemoveSubjectInheritance(TSubject inheritor, TSubject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _subjInheritance.RemoveDependency(inheritor, origin);
        }

        /// <inheritdoc/>
        public bool AddObjectInheritance(TObject inheritor, TObject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _objInheritance.AddDependency(inheritor, origin);
        }

        /// <inheritdoc/>
        public bool RemoveObjectInheritance(TObject inheritor, TObject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));
            return _objInheritance.RemoveDependency(inheritor, origin);
        }

        /// <inheritdoc/>
        public bool IsSubjectInherits(TSubject inheritor, TSubject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _subjInheritance.IsDepends(inheritor, origin);
        }

        /// <inheritdoc/>
        public bool IsObjectInherits(TObject inheritor, TObject origin)
        {
            if (inheritor is null) throw new ArgumentNullException(nameof(inheritor));
            if (origin is null) throw new ArgumentNullException(nameof(origin));

            return _objInheritance.IsDepends(inheritor, origin);
        }

        /// <summary>
        /// Gets direct roles that <paramref name="subj"/> has on <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="IReadOnlyCollection{T}"/> that contains direct roles that <paramref name="subj"/> has on <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> are <see langword="null"/>.</exception>
        public RoleCollection GetDirectRoles(TSubject subj, TObject obj)
        {
            if (subj is null) throw new ArgumentNullException(nameof(subj));
            if (obj is null) throw new ArgumentNullException(nameof(obj));

            return new RoleCollection(base.GetDirectRolesHashSet(subj, obj));
        }

        IReadOnlyCollection<TRole> IInheritablePermissionsManager<TSubject, TObject, TRole>.GetDirectRoles(TSubject subj, TObject obj) => GetDirectRoles(subj, obj);
        #endregion

        #region Overrides
        /// <inheritdoc/>
        protected override HashSet<TRole> GetDirectRolesHashSet(TSubject subj, TObject obj)
        {
            // get direct roles
            var roles = base.GetDirectRolesHashSet(subj, obj);

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

        /// <summary>Clears the <see cref="InheritablePermissionsManager{TSubject, TObject, TRole}"/>.</summary>
        public override void Clear()
        {
            base.Clear();
            _subjInheritance.Clear();
            _objInheritance.Clear();
        }
        #endregion

        #region Private Methods
        private void AddInheritedRolesFromSubjects(HashSet<TRole> roles, TSubject subj, TObject obj, IReadOnlySet<TSubject> originSubjs)
        {
            var processedSubjects = new HashSet<TSubject> { subj };
            var subjStack = new Stack<TSubject>(originSubjs);
            while (subjStack.Count > 0)
            {
                var originSubj = subjStack.Pop();
                if (!processedSubjects.Contains(originSubj))
                {
                    var originRoles = base.GetDirectRolesHashSet(originSubj, obj);
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
                    var originRoles = base.GetDirectRolesHashSet(subj, originObj);
                    roles.UnionWith(originRoles);

                    foreach (var subOriginObj in _objInheritance.GetDirectDependencies(originObj))
                        objStack.Push(subOriginObj);

                    processedObjects.Add(originObj);
                }
            }
        }
        #endregion
    }
}
