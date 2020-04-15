namespace AnamSoft.PermissionsManager.Tests
{
    internal class FakeSubject
    {
        public int Id { get; set; }

        public override string ToString() => Id.ToString();
    }
}