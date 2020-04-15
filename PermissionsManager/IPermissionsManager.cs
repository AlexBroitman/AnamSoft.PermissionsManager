using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    public interface IPermissionsManager<TSubject, TObject, TRole>
    {
        IReadOnlyCollection<TRole> GetRoles(TSubject subj, TObject obj);

        void SetRoles(TSubject subj, TObject obj, IReadOnlyCollection<TRole> roles);

        bool AddRole(TSubject subj, TObject obj, TRole role);

        bool AddRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        bool RemoveRole(TSubject subj, TObject obj, TRole role);

        bool RemoveRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        bool RemoveAllSubjectRoles(TSubject subj);

        bool RemoveAllObjectRoles(TObject obj);

        bool RemoveAllRoles(TSubject subj, TObject obj);

        IReadOnlyCollection<TRole> this[TSubject subj, TObject obj] { get; set; }

        bool HasRole(TSubject subj, TObject obj, TRole role);

        bool HasAllRoles(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        bool HasAnyRole(TSubject subj, TObject obj, IEnumerable<TRole> roles);

        void Clear();
    }
}