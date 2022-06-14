using H3.Organization;

namespace H3
{
    public interface IOrganization
    {
        Company Company { get; set; }

        OrgRole[] GetAllRoles();
        string[] GetChildren(string dianzhang, UnitType user, bool v, State active);
        User GetUnit(string userId);
        OrgRole[] GetUserRoles(string item, bool v);
        Unit GetParentUnit(string userId);
    }
}