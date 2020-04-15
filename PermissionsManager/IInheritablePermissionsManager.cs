using System.Collections.Generic;

namespace AnamSoft.PermissionsManager
{
    public interface IInheritablePermissionsManager<TSubject, TObject, TRole> : IPermissionsManager<TSubject, TObject, TRole>
    {
        IReadOnlyCollection<TRole> GetDirectRoles(TSubject subj, TObject obj);
        bool AddSubjectInheritance(TSubject inheritor, TSubject origin);
        bool RemoveSubjectInheritance(TSubject inheritor, TSubject origin);
        bool AddObjectInheritance(TObject inheritor, TObject origin);
        bool RemoveObjectInheritance(TObject inheritor, TObject origin);
        bool IsSubjectInherits(TSubject inheritor, TSubject origin);
        bool IsObjectInherits(TObject inheritor, TObject origin);
    }
}