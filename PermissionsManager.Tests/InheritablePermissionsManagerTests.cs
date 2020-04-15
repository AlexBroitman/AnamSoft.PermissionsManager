using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AnamSoft.PermissionsManager.Tests
{
    [TestClass]
    public class InheritablePermissionsManagerTests : PermissionsManagerTests
    {
        internal IInheritablePermissionsManager<FakeSubject, FakeObject, Role> _ipm;

        [TestInitialize]
        public override void Init()
        {
            _pm = _ipm = new InheritablePermissionsManager<FakeSubject, FakeObject, Role>();
        }

        [TestMethod]
        public void AddRemoveSubjectInheritanceTest()
        {
            var s1 = new FakeSubject { Id = 1 };
            var s2 = new FakeSubject { Id = 2 };
            var o = new FakeObject();
            var roles1 = new Role[] { Role.Manager, Role.Owner };
            var roles2 = new Role[] { Role.Editor };
            var allRoles = roles1.Concat(roles2).ToList();

            _pm.AddRoles(s1, o, roles1);
            _pm.AddRoles(s2, o, roles2);

            _ipm.AddSubjectInheritance(s1, s2);

            Assert.IsTrue(_pm.HasRole(s1, o, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s1, o, Role.Owner));
            Assert.IsTrue(_pm.HasRole(s1, o, Role.Editor));
            Assert.IsTrue(_pm.HasAllRoles(s1, o, allRoles));
            Assert.IsTrue(_pm.HasAnyRole(s1, o, new Role[] { Role.Manager }));
            CollectionAssert.AreEquivalent(allRoles, _pm.GetRoles(s1, o).ToList());

            _ipm.RemoveSubjectInheritance(s1, s2);

            Assert.IsTrue(_pm.HasRole(s1, o, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s1, o, Role.Owner));
            Assert.IsFalse(_pm.HasRole(s1, o, Role.Editor));
            Assert.IsFalse(_pm.HasAllRoles(s1, o, allRoles));
            CollectionAssert.AreEquivalent(roles1, _pm.GetRoles(s1, o).ToList());
        }

        [TestMethod]
        public void AddRemoveObjectInheritanceTest()
        {
            var s = new FakeSubject();
            var o1 = new FakeObject { Id = 1 };
            var o2 = new FakeObject { Id = 2 };
            var roles1 = new Role[] { Role.Manager, Role.Owner };
            var roles2 = new Role[] { Role.Editor };
            var allRoles = roles1.Concat(roles2).ToList();

            _pm.AddRoles(s, o1, roles1);
            _pm.AddRoles(s, o2, roles2);

            _ipm.AddObjectInheritance(o1, o2);

            Assert.IsTrue(_pm.HasRole(s, o1, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s, o1, Role.Owner));
            Assert.IsTrue(_pm.HasRole(s, o1, Role.Editor));
            Assert.IsTrue(_pm.HasAllRoles(s, o1, allRoles));
            Assert.IsTrue(_pm.HasAnyRole(s, o1, new Role[] { Role.Manager }));
            CollectionAssert.AreEquivalent(allRoles, _pm.GetRoles(s, o1).ToList());

            _ipm.RemoveObjectInheritance(o1, o2);

            Assert.IsTrue(_pm.HasRole(s, o1, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s, o1, Role.Owner));
            Assert.IsFalse(_pm.HasRole(s, o1, Role.Editor));
            Assert.IsFalse(_pm.HasAllRoles(s, o1, allRoles));
            CollectionAssert.AreEquivalent(roles1, _pm.GetRoles(s, o1).ToList());
        }

        [TestMethod]
        public void IsSubjectInheritsTest()
        {
            var s1 = new FakeSubject { Id = 1 };
            var s2 = new FakeSubject { Id = 2 };
            var s3 = new FakeSubject { Id = 3 };
            var o = new FakeObject();

            _pm.AddRole(s1, o, Role.Editor);
            _pm.AddRole(s2, o, Role.Manager);
            _pm.AddRole(s3, o, Role.Owner);

            _ipm.AddSubjectInheritance(s1, s2);
            _ipm.AddSubjectInheritance(s2, s3);

            Assert.IsTrue(_ipm.IsSubjectInherits(s1, s2));
            Assert.IsTrue(_ipm.IsSubjectInherits(s2, s3));
            Assert.IsTrue(_ipm.IsSubjectInherits(s1, s3));
            Assert.IsFalse(_ipm.IsSubjectInherits(s2, s1));
            Assert.IsFalse(_ipm.IsSubjectInherits(s3, s1));
            Assert.IsFalse(_ipm.IsSubjectInherits(s3, s2));

            _ipm.RemoveSubjectInheritance(s2, s3);

            Assert.IsTrue(_ipm.IsSubjectInherits(s1, s2));
            Assert.IsFalse(_ipm.IsSubjectInherits(s2, s3));
            Assert.IsFalse(_ipm.IsSubjectInherits(s1, s3));
            Assert.IsFalse(_ipm.IsSubjectInherits(s2, s1));
            Assert.IsFalse(_ipm.IsSubjectInherits(s3, s1));
            Assert.IsFalse(_ipm.IsSubjectInherits(s3, s2));
        }

        [TestMethod]
        public void IsObjectInheritsTest()
        {
            var s = new FakeSubject();
            var o1 = new FakeObject { Id = 1 };
            var o2 = new FakeObject { Id = 1 };
            var o3 = new FakeObject { Id = 1 };

            _pm.AddRole(s, o1, Role.Editor);
            _pm.AddRole(s, o2, Role.Manager);
            _pm.AddRole(s, o3, Role.Owner);

            _ipm.AddObjectInheritance(o1, o2);
            _ipm.AddObjectInheritance(o2, o3);

            Assert.IsTrue(_ipm.IsObjectInherits(o1, o2));
            Assert.IsTrue(_ipm.IsObjectInherits(o2, o3));
            Assert.IsTrue(_ipm.IsObjectInherits(o1, o3));
            Assert.IsFalse(_ipm.IsObjectInherits(o2, o1));
            Assert.IsFalse(_ipm.IsObjectInherits(o3, o1));
            Assert.IsFalse(_ipm.IsObjectInherits(o3, o2));

            _ipm.RemoveObjectInheritance(o2, o3);

            Assert.IsTrue(_ipm.IsObjectInherits(o1, o2));
            Assert.IsFalse(_ipm.IsObjectInherits(o2, o3));
            Assert.IsFalse(_ipm.IsObjectInherits(o1, o3));
            Assert.IsFalse(_ipm.IsObjectInherits(o2, o1));
            Assert.IsFalse(_ipm.IsObjectInherits(o3, o1));
            Assert.IsFalse(_ipm.IsObjectInherits(o3, o2));
        }
    }
}