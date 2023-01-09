using Chuanyun.H3.Workflow.Instance;
using H3.Workflow;

public class Role
{
    public Role() { }
    public static string[] GetUsers(string RoleName, H3.IEngine Engine)
    {
        string Dianzhang = "";//角色ID
        H3.Organization.OrgRole[] allRoles = Engine.Organization.GetAllRoles(); //获取所有角色
        foreach (H3.Organization.OrgRole role in allRoles)
        {
            if (role.Name == RoleName) { Dianzhang = role.ObjectId; }
        }
        string[] Renyuans = Engine.Organization.GetChildren(Dianzhang, H3.Organization.UnitType.User, true, H3.Organization.State.Active); //获取RoleName(角色)的所有成员ID
        return Renyuans;
    }
    public static string[] Participants(H3.SmartForm.SmartFormRequest request, int step)
    {
        Token t = request.WorkflowInstance.GetRunningToken(request.ActivityCode);

        Token preTokens = request.WorkflowInstance.Tokens[t.PreTokens[0] + step];

        return preTokens.Participants;

    }
    public static string GetDepartmentName(H3.IEngine engine, string userId)
    {
        H3.Organization.User employee = engine.Organization.GetUnit(userId) as H3.Organization.User;
        if (employee == null) { return null; }
        //string s = $"bbc{userId}";
        return employee.DepartmentName;
    }

}
