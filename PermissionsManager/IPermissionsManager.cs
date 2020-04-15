using System;
using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    /// <summary>
    /// Interface for generic PermissionsManager
    /// </summary>
    /// <typeparam name="TSubject">A type of permissions subject (User, UserGroup, Profile, Resource, etc).</typeparam>
    /// <typeparam name="TObject">A type of permissions object (File, WorkItem, etc).</typeparam>
    /// <typeparam name="TRole">A type of role</typeparam>
    public interface IPermissionsManager<TSubject, TObject, TRole>
    {
        /// <summary>
        /// Gets all roles that <paramref name="subj"/> has on <paramref name="obj"/>. Same as <see cref="this[TSubject, TObject]"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="IReadOnlyCollection{T}"/> that contains all roles that <paramref name="subj"/> has on <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> is <see langword="null"/>.</exception>
        IReadOnlyCollection<TRole> GetRoles(TSubject subj, TObject obj);

        /// <summary>
        /// Sets direct roles for <paramref name="subj"/> on <paramref name="obj"/>.
        /// Previous direct roles will be removed.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="roles">The roles.</param>        
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="roles"/> are <see langword="null"/>.</exception>
        void SetRoles(TSubject subj, TObject obj, IReadOnlyCollection<TRole> roles);

        /// <summary>
        /// Adds new <paramref name="role"/> for subject <paramref name="subj"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="role">The role to add.</param>        
        /// <returns><see langword="true"/> if the role was added; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="role"/> are <see langword="null"/>.</exception>
        bool AddRole(TSubject subj, TObject obj, TRole role);

        /// <summary>
        /// Adds new <paramref name="roles"/> for subject <paramref name="subj"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="roles">The roles to add.</param>        
        /// <returns><see langword="true"/> if at least one role was added; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="roles"/> are <see langword="null"/>.</exception>
        bool AddRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        /// <summary>
        /// Removes <paramref name="role"/> from subject <paramref name="subj"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="role">The role to remove.</param>        
        /// <returns><see langword="true"/> if the role was remmoved; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="role"/> are <see langword="null"/>.</exception>
        bool RemoveRole(TSubject subj, TObject obj, TRole role);

        /// <summary>
        /// Removes <paramref name="roles"/> from subject <paramref name="subj"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="roles">The roles to remove.</param>        
        /// <returns><see langword="true"/> if at least one role was removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="roles"/> are <see langword="null"/>.</exception>
        bool RemoveRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        /// <summary>
        /// Removes all roles from subject <paramref name="subj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <returns><see langword="true"/> if at roles were removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> is <see langword="null"/>.</exception>
        bool RemoveAllSubjectRoles(TSubject subj);

        /// <summary>
        /// Removes all roles on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><see langword="true"/> if roles were removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null"/>.</exception>
        bool RemoveAllObjectRoles(TObject obj);

        /// <summary>
        /// Removes all roles that subject <paramref name="subj"/> has on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns><see langword="true"/> if roles were removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> are <see langword="null"/>.</exception>
        bool RemoveAllRoles(TSubject subj, TObject obj);

        /// <summary>
        /// Gets all roles that <paramref name="subj"/> has on <paramref name="obj"/>. Same as <see cref="GetRoles(TSubject, TObject)"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <returns><see cref="IReadOnlyCollection{T}"/> that contains all roles that <paramref name="subj"/> has on <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> is <see langword="null"/>.</exception>
        IReadOnlyCollection<TRole> this[TSubject subj, TObject obj] { get; set; }

        /// <summary>
        /// Determines if the subject <paramref name="subj"/> has the <paramref name="role"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="role">The role to test.</param>
        /// <returns><see langword="true"/> if <paramref name="subj"/> has <paramref name="role"/> on <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="role"/> are <see langword="null"/>.</exception>
        bool HasRole(TSubject subj, TObject obj, TRole role);

        /// <summary>
        /// Determines if the subject <paramref name="subj"/> has all specified <paramref name="roles"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="roles">The roles to test.</param>
        /// <returns><see langword="true"/> if <paramref name="subj"/> has all <paramref name="roles"/> on <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="roles"/> are <see langword="null"/>.</exception>
        bool HasAllRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        /// <summary>
        /// Determines if the subject <paramref name="subj"/> has at least one of specified <paramref name="roles"/> on object <paramref name="obj"/>.
        /// </summary>
        /// <param name="subj">The subject.</param>
        /// <param name="obj">The object.</param>
        /// <param name="roles">The roles to test.</param>
        /// <returns><see langword="true"/> if <paramref name="subj"/> has at least one of <paramref name="roles"/> on <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subj"/> or <paramref name="obj"/> or <paramref name="roles"/> are <see langword="null"/>.</exception>
        bool HasAnyRole(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        /// <summary>
        /// Clears the <see cref="IPermissionsManager{TSubject, TObject, TRole}"/>.
        /// </summary>
        void Clear();
    }
}