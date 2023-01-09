

using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class ExceptionCompoent
{
    private static string AppCode = "D001419Operate";  //实施运维的AppCode

    public static void Log(H3.IEngine engin, string message, string info = "")
    {
        Dictionary<string, object> returnData = new Dictionary<string, object>();
        returnData.Add(LogService.pageNameReq, "日志记录");
        returnData.Add(LogService.infoReq, info);
        returnData.Add(LogService.logReq, message);
        sendLog(engin, returnData);
    }

    public static void HandelSubmitSmartFormResponse(H3.SmartForm.RequestContext request, H3.IEngine engin, Exception ex, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (IsApplicationException(ex))
        {
            if (HasInnerException(ex) && ExceptionLevel.error == ex.InnerException.Message)
            {
                response.Errors.Add(ex.Message);
            }
            else
            {
                response.Infos.Add(ex.Message);
            }
        }
        else
        {
            response.Errors.Add(GetReturnMessage(engin, ex));
            Dictionary<string, object> returnData = GenerateReturnData(request, ex, response.IsMobile);
            sendLog(engin, returnData);
        }
    }

    public static void HandelLoadSmartFormResponse(H3.SmartForm.RequestContext request, H3.IEngine engin, Exception ex, H3.SmartForm.LoadSmartFormResponse response)
    {
        if (IsApplicationException(ex))
        {
            if (HasInnerException(ex) && ExceptionLevel.error == ex.InnerException.Message)
            {
                response.Errors.Add(ex.Message);
            }
            else
            {
                response.Infos.Add(ex.Message);
            }
        }
        else
        {
            response.Errors.Add(GetReturnMessage(engin, ex));
            Dictionary<string, object> returnData = GenerateReturnData(request, ex, response.IsMobile);
            sendLog(engin, returnData);
        }
    }

    public static void HandelSubmitListViewResponse(H3.SmartForm.RequestContext request, H3.IEngine engin, Exception ex, H3.SmartForm.SubmitListViewResponse response)
    {
        if (IsApplicationException(ex))
        {
            // if(HasInnerException(ex) && ExceptionLevel.error == ex.InnerException.Message)
            // {
            //     response.Errors.Add(ex.Message);
            // }
            // else 
            // {
            //     response.Infos.Add(ex.Message);
            // }
            response.Errors.Add(ex.Message);
        }
        else
        {
            response.Errors.Add(GetReturnMessage(engin, ex));
            Dictionary<string, object> returnData = GenerateReturnData(request, ex, response.IsMobile);
            sendLog(engin, returnData);
        }
    }

    public static void HandelLoadListViewResponse(H3.SmartForm.RequestContext request, H3.IEngine engin, Exception ex, H3.SmartForm.LoadListViewResponse response)
    {
        if (IsApplicationException(ex))
        {
            if (HasInnerException(ex) && ExceptionLevel.error == ex.InnerException.Message)
            {
                response.Errors.Add(ex.Message);
            }
            else
            {
                response.Infos.Add(ex.Message);
            }
        }
        else
        {
            response.Errors.Add(GetReturnMessage(engin, ex));
            Dictionary<string, object> returnData = GenerateReturnData(request, ex, response.IsMobile);
            sendLog(engin, returnData);
        }
    }

    private static void sendLog(H3.IEngine engin, Dictionary<string, object> data)
    {
        H3AppInvoke.DoPost(engin, AppCode, "LogController", "log", data);
    }

    private static Dictionary<string, object> GenerateReturnData(H3.SmartForm.RequestContext request, Exception ex, bool isMobile)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        string requestType = request.GetType().Name;
        if (requestType == "SmartFormRequest")
        {
            H3.SmartForm.SmartFormRequest smartFormRequest = (H3.SmartForm.SmartFormRequest)request;
            H3.DataModel.BizObject bizObject = smartFormRequest.BizObject;
            string info = string.Format("表单名称：{0} \n表单编码：{1} \n方法名称：{2} \n错误详情：{3} \n数据ID：{4} \n活动编码：{5} \n当前用户：{6} \n移动端：{7}",
                bizObject.Schema.DisplayName, bizObject.Schema.SchemaCode, ex.TargetSite, ex.Message, bizObject.ObjectId, smartFormRequest.ActivityCode, smartFormRequest.UserContext.User.FullName, isMobile);
            dictionary.Add(LogService.pageNameReq, bizObject.Schema.DisplayName);
            dictionary.Add(LogService.infoReq, info);
        }
        else
        {
            H3.SmartForm.ListViewRequest listViewRequest = (H3.SmartForm.ListViewRequest)request;
            string info = string.Format("表单名称：{0} \n表单编码：{1} \n方法名称：{2} \n列表场景：{3} \n错误详情：{4} \n当前用户：{5} \n移动端：{6}",
                listViewRequest.DisplayName, listViewRequest.SchemaCode, ex.TargetSite, listViewRequest.ListScene, ex.Message, listViewRequest.CurrentUserId, isMobile);
            dictionary.Add(LogService.pageNameReq, listViewRequest.DisplayName);
            dictionary.Add(LogService.infoReq, info);
        }

        Dictionary<string, object> requestLog = request.CreateLog();
        string actionName = requestLog.ContainsKey("Command") ? string.Empty + requestLog["Command"] : string.Empty;
        dictionary.Add(LogService.actionReq, actionName);
        dictionary.Add(LogService.logReq, ex.ToString());
        return dictionary;
    }



    private static bool IsApplicationException(Exception ex)
    {
        return ex.GetType().Name == "ApplicationException";
    }

    private static bool HasInnerException(Exception ex)
    {
        bool flag = false;
        object e = ex.InnerException;
        try
        {
            flag = e != null;
        }
        catch (Exception exp)
        {
        }
        return flag;
    }

    private static string GetReturnMessage(H3.IEngine engine, Exception ex)
    {
        string dutySchemaCode = "D0014195ff2553b780b496985130f66aa5b86b4";
        string dutyObjectId = "b77b4a41-6e36-4ba5-9340-9c145795e690";
        H3.DataModel.BizObject[] dutyArr = Tools.BizOperation.Load(engine, dutySchemaCode, dutyObjectId)["D001419Fd4b31e1c3ca943c49d67fe0d167be34e"] as H3.DataModel.BizObject[];
        string info = dutyArr[DateTime.Now.Day % dutyArr.Length]["Person"] + " : " + dutyArr[DateTime.Now.Day % dutyArr.Length]["Contact"];
        return string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
    }

}
public class ExceptionLevel
{
    public static string error = "1";
    public static string info = "2";
}

public class ThirdApiCode
{
    public static string LogForward = "LogForward";
}

public class LogService
{
    public static string pageNameReq = "pageName";
    public static string actionReq = "action";
    public static string logReq = "log";
    public static string infoReq = "info";
}