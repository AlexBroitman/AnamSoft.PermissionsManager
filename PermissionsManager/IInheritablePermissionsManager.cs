using System;
using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    /// <summary>
    /// Interface for generic InheritablePermissionsManager
    /// </summary>
    /// <typeparam name="TSubject">A type of permissions subject (User, UserGroup, Profile, Resource, etc).</typeparam>
    /// <typeparam name="TObject">A type of permissions object (File, WorkItem, etc).</typeparam>
    /// <typeparam name="TRole">A type of role</typeparam>
    public interface IInheritablePermissionsManager<TSubject, TObject, TRole> : IPermissionsManager<TSubject, TObject, TRole>
    {
        /// <summary>
        /// Adds permissions inheritance for subjects from <paramref name="origin"/> to <paramref name="inheritor"/>.
        /// </summary>
        /// <param name="inheritor">The inheritor.</param>
        /// <param name="origin">The origin.</param>
        /// <returns><see langword="true"/> if the inheritance was added; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="inheritor"/> or <paramref name="origin"/> are <see langword="null"/>.</exception>
        bool AddSubjectInheritance(TSubject inheritor, TSubject origin);

        /// <summary>
        /// Removes permissions inheritance for subjects from <paramref name="origin"/> to <paramref name="inheritor"/>.
        /// </summary>
        /// <param name="inheritor">The inheritor.</param>
        /// <param name="origin">The origin.</param>
        /// <returns><see langword="true"/> if the inheritance was removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="inheritor"/> or <paramref name="origin"/> are <see langword="null"/>.</exception>
        bool RemoveSubjectInheritance(TSubject inheritor, TSubject origin);

        /// <summary>
        /// Adds permissions inheritance for objects from <paramref name="origin"/> to <paramref name="inheritor"/>.
        /// </summary>
        /// <param name="inheritor">The inheritor.</param>
        /// <param name="origin">The origin.</param>
        /// <returns><see langword="true"/> if the inheritance was added; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="inheritor"/> or <paramref name="origin"/> are <see langword="null"/>.</exception>
        bool AddObjectInheritance(TObject inheritor, TObject origin);

        /// <summary>
        /// Removes permissions inheritance for objects from <paramref name="origin"/> to <paramref name="inheritor"/>.
        /// </summary>
        /// <param name="inheritor">The inheritor.</param>
        /// <param name="origin">The origin.</param>
        /// <returns><see langword="true"/> if the inheritance was removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="inheritor"/> or <paramref name="origin"/> are <see langword="null"/>.</exception>
        bool RemoveObjectInheritance(TObject inheritor, TObject origin);

        /// <summary>
        /// Determines if subject <paramref name="inheritor"/> inherits permissions from other subject <paramref name="origin"/>.
        /// </summary>
        /// <param name="inheritor">The inheritor.</param>
        /// <param name="origin">The origin.</param>
        /// <returns><see langword="true"/> if the <paramref name="inheritor"/> inherits permissions from <paramref name="origin"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="inheritor"/> or <paramref name="origin"/> are <see langword="null"/>.</exception>
        bool IsSubjectInherits(TSubject inheritor, TSubject origin);

        /// <summary>
        /// Determines if object <paramref name="inheritor"/> inherits permissions from other object <paramref name="origin"/>.
        /// </summary>
        /// <param name="inheritor">The inheritor.</param>
        /// <param name="origin">The origin.</param>
        /// <returns><see langword="true"/> if the <paramref name="inheritor"/> inherits permissions from <paramref name="origin"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="inheritor"/> or <paramref name="origin"/> are <see langword="null"/>.</exception>
        bool IsObjectInherits(TObject inheritor, TObject origin);

        /// <summary>
        /// Gets direct roles that <paramref name="subj"/> has on <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="IReadOnlyCollection{T}"/> that contains direct roles that <paramref name="subj"/> has on <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> are <see langword="null"/>.</exception>
        IReadOnlyCollection<TRole> GetDirectRoles(TSubject subj, TObject obj);
    }
}