using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class H3AppInvoke
{

    private static string EngineCode = "g44mm22g2z6reydxji9hobg14";

    private static string EngineSecret = "21jS5Gfhb4fJZL3AOWl7oMOoLHdKZtdVTYHbJJT0N70YYowWtVxgHQ==";

    private static string Successful = "Successful";

    private static string ErrorMessage = "ErrorMessage";

    public static H3.BizBus.BizStructure DoPost(H3.IEngine Engine, string appCode, string controller, string actionName, Dictionary<string, object> data, bool resultCheck = false)
    {

        H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
        return DoPost(Engine, appCode, controller, actionName, data, structureSchema, resultCheck);
    }

    public static H3.BizBus.BizStructure DoPost(H3.IEngine Engine, string appCode, string controller, string actionName, Dictionary<string, object> data, H3.BizBus.BizStructureSchema structureSchema, bool resultCheck = false)
    {
        structureSchema.Add(new H3.BizBus.ItemSchema(Successful, "", H3.Data.BizDataType.Bool, null));
        structureSchema.Add(new H3.BizBus.ItemSchema(ErrorMessage, "", H3.Data.BizDataType.String, null));

        Dictionary<string, string> headers = new Dictionary<string, string>();
        Dictionary<string, string> querys = new Dictionary<string, string>();
        headers.Add("EngineCode", EngineCode);
        headers.Add("EngineSecret", EngineSecret);

        data.Add("Controller", controller);
        data.Add("ActionName", actionName);
        data.Add("AppCode", appCode);
        data.Add(SecretKeyService.Key, new SecretKeyService().GetSecretKey());
        H3.BizBus.InvokeResult InResult = Engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
            "Cloud", "POST", "application/json;charset=UTF-8", headers, querys, data, structureSchema);

        H3.BizBus.BizStructure resultData = InResult.Data;
        if (resultCheck)
        {
            bool success = (bool)resultData[Successful];
            if (!success)
            {
                string errorMessage = (string)resultData[ErrorMessage];
                throw new Exception("调用氚云接口失败 -> controller:" + controller + "actionName:" + actionName + "-> " + errorMessage);
            }
        }

        return InResult.Data;
    }
}

public class SecretKeyService
{

    public static string Key = "shuanghuan-secretKey";

    private string SecretKey = "shuanghuan-security-2023";

    public string GetSecretKey()
    {
        return SecretKey;
    }

    public bool CheckSecretKey(H3.SmartForm.RestApiRequest request)
    {
        string secretKey = request.GetValue<string>(Key, string.Empty);
        return secretKey == SecretKey;
    }

}
