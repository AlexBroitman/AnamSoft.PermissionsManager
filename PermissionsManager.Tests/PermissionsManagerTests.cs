using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AnamSoft.PermissionsManager.Tests
{
    [TestClass]
    public class PermissionsManagerTests
    {
        internal IPermissionsManager<FakeSubject, FakeObject, Role> _pm;

        [TestInitialize]
        public virtual void Init()
        {
            _pm = new PermissionsManager<FakeSubject, FakeObject, Role>();
        }

        [TestMethod]
        public void AddRole_HasRole_GetRoles_Test()
        {
            var s = new FakeSubject();
            var o = new FakeObject();

            var res = _pm.AddRole(s, o, Role.Manager);

            Assert.IsTrue(res);
            Assert.IsTrue(_pm.HasRole(s, o, Role.Manager));
            Assert.IsTrue(_pm.HasAllRoles(s, o, new Role[] { Role.Manager }));
            Assert.IsTrue(_pm.HasAnyRole(s, o, new Role[] { Role.Manager, Role.Owner }));
            CollectionAssert.AreEquivalent(new Role[] { Role.Manager }, _pm.GetRoles(s, o).ToList());
        }

        [TestMethod]
        public void AddRoles_HasAllRoles_HasAnyRole_GetRoles_Test()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner };

            var res = _pm.AddRoles(s, o, roles);

            Assert.IsTrue(res);
            Assert.IsTrue(_pm.HasRole(s, o, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s, o, Role.Owner));
            Assert.IsTrue(_pm.HasAllRoles(s, o, roles));
            Assert.IsTrue(_pm.HasAnyRole(s, o, new Role[] { Role.Manager }));
            CollectionAssert.AreEquivalent(roles, _pm.GetRoles(s, o).ToList());
        }

        [TestMethod]
        public void SetRolesTest()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner };

            _pm.SetRoles(s, o, roles);

            Assert.IsTrue(_pm.HasRole(s, o, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s, o, Role.Owner));
            Assert.IsTrue(_pm.HasAllRoles(s, o, roles));
            Assert.IsTrue(_pm.HasAnyRole(s, o, roles));
            CollectionAssert.AreEquivalent(roles, _pm.GetRoles(s, o).ToList());
        }

        [TestMethod]
        public void RemoveRoleTest()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner };
            _pm.SetRoles(s, o, roles);

            _pm.RemoveRole(s, o, Role.Manager);

            Assert.IsFalse(_pm.HasRole(s, o, Role.Manager));
            Assert.IsTrue(_pm.HasRole(s, o, Role.Owner));
            Assert.IsFalse(_pm.HasAllRoles(s, o, roles));
            Assert.IsTrue(_pm.HasAnyRole(s, o, roles));
            CollectionAssert.AreEquivalent(new Role[] { Role.Owner }, _pm.GetRoles(s, o).ToList());
        }

        [TestMethod]
        public void RemoveRolesTest()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner, Role.Viewer };
            _pm.SetRoles(s, o, roles);

            _pm.RemoveRoles(s, o, new Role[] { Role.Manager, Role.Owner });

            Assert.IsFalse(_pm.HasRole(s, o, Role.Manager));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Owner));
            Assert.IsTrue(_pm.HasRole(s, o, Role.Viewer));
            Assert.IsFalse(_pm.HasAllRoles(s, o, roles));
            Assert.IsTrue(_pm.HasAnyRole(s, o, roles));
            CollectionAssert.AreEquivalent(new Role[] { Role.Viewer }, _pm.GetRoles(s, o).ToList());
        }

        [TestMethod]
        public void RemoveAllSubjectRolesTest()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner, Role.Viewer };
            _pm.SetRoles(s, o, roles);

            _pm.RemoveAllSubjectRoles(s);

            Assert.IsFalse(_pm.HasRole(s, o, Role.Manager));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Owner));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Viewer));
            Assert.IsFalse(_pm.HasAllRoles(s, o, roles));
            Assert.IsFalse(_pm.HasAnyRole(s, o, roles));
            Assert.AreEqual(0, _pm.GetRoles(s, o).Count);
        }

        [TestMethod]
        public void RemoveAllObjectRolesTest()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner, Role.Viewer };
            _pm.SetRoles(s, o, roles);

            _pm.RemoveAllObjectRoles(o);

            Assert.IsFalse(_pm.HasRole(s, o, Role.Manager));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Owner));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Viewer));
            Assert.IsFalse(_pm.HasAllRoles(s, o, roles));
            Assert.IsFalse(_pm.HasAnyRole(s, o, roles));
            Assert.AreEqual(0, _pm.GetRoles(s, o).Count);
        }

        [TestMethod]
        public void RemoveAllRolesTest()
        {
            var s = new FakeSubject();
            var o = new FakeObject();
            var roles = new Role[] { Role.Manager, Role.Owner, Role.Viewer };
            _pm.SetRoles(s, o, roles);

            _pm.RemoveAllRoles(s, o);

            Assert.IsFalse(_pm.HasRole(s, o, Role.Manager));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Owner));
            Assert.IsFalse(_pm.HasRole(s, o, Role.Viewer));
            Assert.IsFalse(_pm.HasAllRoles(s, o, roles));
            Assert.IsFalse(_pm.HasAnyRole(s, o, roles));
            Assert.AreEqual(0, _pm.GetRoles(s, o).Count);
        }
    }
}