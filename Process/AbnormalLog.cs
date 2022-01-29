using H3;
using H3.DataModel;
using System;

internal class AbnormalLog
{
    /// <summary>
    /// 创建日志
    /// </summary>
    /// <param name="iD">产品ID</param>
    /// <param name="currentWorkStep">当前工步</param>
    /// <param name="currentOperation"></param>
    /// <param name="exceptionCategory"></param>
    /// <param name="exceptionDescription"></param>
    /// <param name="bizObject"></param>
    /// <param name="engine"></param>
    /// <returns></returns>
    internal static string CreateLog(string iD, string currentWorkStep, string currentOperation, string exceptionCategory, string exceptionDescription, BizObject bizObject, IEngine engine)
    {
        throw new NotImplementedException();
    }

    internal static void UpdateLog(string iD, string currentWorkStep, string exceptionCategory, string exceptionDescription, BizObject bizObject, string v, IEngine engine)
    {
        throw new NotImplementedException();
    }
}