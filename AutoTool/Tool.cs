
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using H3;
using H3.DataModel;

public class Tools
{
    public class Log
    {
        internal static string ErrorLog(IEngine engine, BizObject me, Exception ex, string activityCode, string userName)
        {
            throw new NotImplementedException();
        }

        internal static string ErrorLog(IEngine engine, object p, Exception ex, string activityCode)
        {
            throw new NotImplementedException();
        }
    }

    public class Filter
    {

        /*
       * -- Author:fubin
       * 构建符合条件的And过滤器
       * @param filter 需要构建的过滤器
       * @param componentCode 控件编码
       * @param operators 操作符
       * @param value 控件值
       */
        public static void And(H3.Data.Filter.Filter filter, string componentCode, H3.Data.ComparisonOperatorType operators, object value, bool isColumn = false)
        {
            H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();    //构造And匹配器
            andMatcher.Add(new H3.Data.Filter.ItemMatcher(componentCode, operators, value, isColumn)); //添加查询条件
            if (filter.Matcher != null)
            {
                andMatcher.Add(filter.Matcher);
            }
            filter.Matcher = andMatcher;
        }

        /*
        * -- Author:fubin
        * 构建符合条件的Or过滤器
        * @param filter 需要构建的过滤器
        * @param componentCode 控件编码
        * @param operators 操作符
        * @param value 控件值
        */
        public static void Or(H3.Data.Filter.Filter filter, string componentCode, H3.Data.ComparisonOperatorType operators, object value, bool isColumn = false)
        {
            H3.Data.Filter.Or orMatcher = new H3.Data.Filter.Or();    //构造And匹配器
            orMatcher.Add(new H3.Data.Filter.ItemMatcher(componentCode, operators, value)); //添加查询条件
            if (filter.Matcher != null)
            {
                orMatcher.Add(filter.Matcher);
            }
            filter.Matcher = orMatcher;
        }
    }
    public class BizOperation
    {
        /*
         * -- Author:fubin
         * 加载业务对象
         * @param engine 编译引擎
         * @param schemacode 表单编码
         * @param bizObjectId 业务对象Id
         */
        public static H3.DataModel.BizObject Load(H3.IEngine engine, string schemaCode, string bizObjectId)
        {
            H3.DataModel.BizObject bizObject = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, engine, schemaCode, bizObjectId, false);
            return bizObject;
        }

        /*
         * -- Author:zlm
         * 创建新的业务对象
         * @param engine 编译引擎
         * @param schemacode 表单编码
         */
        public static H3.DataModel.BizObject New(H3.IEngine engine, string schemaCode)
        {
            H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema(schemaCode);
            H3.DataModel.BizObject bizObject = new H3.DataModel.BizObject(engine, schema, H3.Organization.User.SystemUserId);
            return bizObject;
        }

        /*
         * -- Author:fubin
         * 根据过滤器筛选符合条件的所有数据
         * @param engine 编译引擎
         * @param schemacode 表单编码
         * @param filter 构建完成的过滤器
         */
        public static H3.DataModel.BizObject[] GetList(H3.IEngine engine, string schemacode, H3.Data.Filter.Filter filter)
        {
            H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema(schemacode);
            H3.DataModel.BizObject[] bizObjects = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
            return bizObjects;
        }

        /*
         * -- Author:mc
         * 根据过滤器筛选符合条件的首条数据
         * @param engine 编译引擎
         * @param schemacode 表单编码
         * @param filter 构建完成的过滤器
         */
        public static H3.DataModel.BizObject GetFirst(H3.IEngine engine, string schemacode, H3.Data.Filter.Filter filter)
        {
            H3.DataModel.BizObject[] bizObjects = null;
            try
            {
                H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema(schemacode);
                bizObjects = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);


            }

            catch (Exception e)
            {
                var ex = e;
            }
            if (bizObjects != null && bizObjects.Length > 0)
                return bizObjects[0];
            else
                return null;
        }


        /*
         * -- Author:fubin
         * 后端通过主表业务对象给子表增加一行数据，默认在最后增加一行
         * @param engine 编译引擎
         * @param masterObject 主表对象
         * @param childSchemaCode 子表结构编码
         * @param childBizObject  子表业务对象
         * @param row 指定子表中插入的位置下标
         */
        public static void AddChildBizObject(H3.IEngine engine, H3.DataModel.BizObject masterObject, string childSchemaCode, H3.DataModel.BizObject childBizObject, int row = -1)
        {
            //获取子表结构体对象
            H3.DataModel.BizObjectSchema childSchema = masterObject.Schema.GetChildSchema(childSchemaCode);

            //定义新的子表数据集合
            List<H3.DataModel.BizObject> newChildBoList = new List<H3.DataModel.BizObject>();

            //获取子表内已有数据
            H3.DataModel.BizObject[] childBoArray = (H3.DataModel.BizObject[])masterObject[childSchemaCode];

            if (childBoArray != null && childBoArray.Length > 0)
            {
                foreach (H3.DataModel.BizObject itemBo in childBoArray)
                {
                    //将子表内已有数据循环添加到新的子表数据集合里
                    newChildBoList.Add(itemBo);
                }
            }

            //将子表业务对象添加到子表数据最后一行或指定行
            if (row == -1)
            {
                newChildBoList.Add(childBizObject);
            }
            else
            {
                newChildBoList.Insert(row - 1, childBizObject);
            }

            //将新的子表数据集合赋值到子表控件
            masterObject[childSchemaCode] = newChildBoList.ToArray();

            //修改主表业务对象，系统会自动识别出上面子表数据被修改了，执行完Update方法，新的子表数据就会被保存到数据库
            masterObject.Update();

        }

        /*
         * -- Author:fubin
         * 后端通过主表业务对象给子表删除一行数据，默认删除最后一行
         * @param engine 编译引擎
         * @param masterObject 主表对象
         * @param childSchemaCode 子表结构编码
         * @param row 指定子表中要删除的位置下标
         */
        public static void DeleteChildBizObject(H3.IEngine engine, H3.DataModel.BizObject masterObject, string childSchemaCode, int row = -1)
        {
            //获取子表结构体对象
            H3.DataModel.BizObjectSchema childSchema = masterObject.Schema.GetChildSchema(childSchemaCode);

            //定义新的子表数据集合
            List<H3.DataModel.BizObject> newChildBoList = new List<H3.DataModel.BizObject>();

            //获取子表内已有数据
            H3.DataModel.BizObject[] childBoArray = (H3.DataModel.BizObject[])masterObject[childSchemaCode];

            if (childBoArray != null && childBoArray.Length > 0)
            {
                for (int i = 0; i < childBoArray.Length; i++)
                {   //删除尾行
                    if (row == -1)
                    {
                        if (i != childBoArray.Length - 1)
                        {
                            //将子表内已有数据循环添加到新的子表数据集合里
                            newChildBoList.Add(childBoArray[i]);
                        }
                    }
                    else
                    {   //删除指定行
                        if (i != row - 1)
                        {
                            //将子表内已有数据循环添加到新的子表数据集合里
                            newChildBoList.Add(childBoArray[i]);
                        }
                    }
                }
            }

            //将新的子表数据集合赋值到子表控件
            masterObject[childSchemaCode] = newChildBoList.ToArray();
            //修改主表业务对象，系统会自动识别出上面子表数据被修改了，执行完Update方法，新的子表数据就会被保存到数据库
            masterObject.Update();
        }

        /*
         * -- Author:fubin
         * 填充业务对象中的空数据，避免仪表盘展示的数据不全
         * @param OneBizObject 业务对象
         */
        public static void FillNullActivex(H3.DataModel.BizObject OneBizObject)
        {
            foreach (H3.DataModel.PropertySchema activex in OneBizObject.Schema.Properties)
            {
                try
                {
                    OneBizObject[activex.Name] = OneBizObject[activex.Name] == null ? string.Empty : OneBizObject[activex.Name] + string.Empty;
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }

    public class Role
    {
        /*
        * -- Author:zlm
        * 取的拥有指定角色的的所有人员
        * @param engine    编译引擎 
        * @param roleName  角色名称   
        */
        public static string[] GetUsers(H3.IEngine engine, string roleName)
        {
            string Dianzhang = "";//角色ID
            H3.Organization.OrgRole[] allRoles = engine.Organization.GetAllRoles(); //获取所有角色
            foreach (H3.Organization.OrgRole role in allRoles)
            {
                if (role.Name == roleName) { Dianzhang = role.ObjectId; }
            }
            string[] Renyuans = engine.Organization.GetChildren(Dianzhang, H3.Organization.UnitType.User, true, H3.Organization.State.Active); //获取RoleName(角色)的所有成员ID
            return Renyuans;
        }

        /*
         * -- Author:zlm
         * 获取相对与流程当前步骤的参与者
         * @param request 当前对象的Request 
         * @param step 当前节点的前几步，或后几步   
         */
        public static string[] Participants(H3.SmartForm.SmartFormRequest request, int step)
        {
            H3.Workflow.Instance.Token t = request.WorkflowInstance.GetRunningToken(request.ActivityCode);

            H3.Workflow.Instance.Token preTokens = request.WorkflowInstance.Tokens[t.PreTokens[0] + step];

            return preTokens.Participants;

        }
        //
        /*
        * -- Author:zlm
        * 根据用户Id 取得用户所在部门的名称
        * @param engine 编译引擎 
        * @param userId 用户Id     
        */
        public static string GetDepartmentName(H3.IEngine engine, string userId)
        {
            H3.Organization.User employee = engine.Organization.GetUnit(userId) as H3.Organization.User;
            if (employee == null) { return null; }
            return employee.DepartmentName;
        }

    }

    public class WorkFlow
    {
        /*
          * -- Author:zlm
          * 调整节点参与者
          * @param engine 编译引擎 
          * @param workflowInstandId 工作流实例的ID
          * @param worker 人员ID
          */
        public static void AdjustParticipant(H3.IEngine engine, string WorkflowInstanceId, string[] worker)
        {

            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(WorkflowInstanceId).GetLastToken();
            string activityCode = tok.Activity;
            H3.Workflow.Messages.AdjustParticipantMessage AdjustMessage = new H3.Workflow.Messages.AdjustParticipantMessage(WorkflowInstanceId, activityCode, worker);
            engine.WorkflowInstanceManager.SendMessage(AdjustMessage);

        }



        /*
     * -- Author:zzx
     * 开启流程
     * @param engine 编译引擎
     * @param refBizObject 业务对象
     * @param userId 发起人
     * @param flag 是否提交流程操作，默认true为提交
     */
        public static void StartWorkflow(H3.IEngine engine, H3.DataModel.BizObject refBizObject, string userId = null , bool flag = true)
        {
            string instanceId = System.Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(refBizObject.WorkflowInstanceId))
            {
                refBizObject.WorkflowInstanceId = instanceId;
                refBizObject.Update();
            }

            H3.Workflow.Instance.WorkflowInstance wfInstance = engine.WorkflowInstanceManager.GetWorkflowInstance(refBizObject.WorkflowInstanceId);

            if (wfInstance == null)
            {
                //启动流程
                string workItemID = string.Empty;
                string errorMsg = string.Empty;
                H3.Workflow.Template.WorkflowTemplate wfTemp = engine.WorkflowTemplateManager.GetDefaultWorkflow(refBizObject.Schema.SchemaCode);

                engine.Interactor.OriginateInstance(userId, refBizObject.Schema.SchemaCode,

                    wfTemp.WorkflowVersion, refBizObject.ObjectId, refBizObject.WorkflowInstanceId, H3.Workflow.WorkItem.AccessMethod.Web,

                    flag, string.Empty, true, out workItemID, out errorMsg);

            }//第七个参数 false/true 为是否提交流程操作
        }
    }

    public class BusinessExcepiton : Exception
    {
        internal static void ErrorLog(IEngine engine, string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}


/*
* -- Author:zlm
* 异常类 
*/
public class Dexception : Exception
{
    public Exception Eexception;
    public string Message = "";
    public int ErrorNo;
    public string StepName = "";
    public string ProcessName = "";

    public Dexception(Exception ex, int errNo, string msg)
    {


        Eexception = ex;
        Message = msg;
        ErrorNo = errNo;


    }
    public Dexception(Exception ex, int errNo, string msg, string stepName)
    {


        Eexception = ex;
        ErrorNo = errNo;
        Message = msg;
        StepName = stepName;


    }

    public void LogError(H3.IEngine engine)
    {

        string exceptionStr = "";
        string content = "";

        if (Eexception != null)
        {
            exceptionStr = Eexception.ToString();


        }
        content = ProcessName + "/" + StepName + "：" + Message;

        //D00141958b5367f26f9464a85d20ce0aed83767
        H3.DataModel.BizObjectSchema bugBizobjectSchema = engine.BizObjectManager.GetPublishedSchema("D00141958b5367f26f9464a85d20ce0aed83767");
        H3.DataModel.BizObject newBizObject = new H3.DataModel.BizObject(engine, bugBizobjectSchema, H3.Organization.User.SystemUserId);

        //用户名
        newBizObject["F0000028"] = H3.Organization.User.SystemUserId;
        //故障等级
        newBizObject["F0000005"] = "3";
        //标题
        newBizObject["F0000002"] = "系统自动记录";
        //故障内容
        newBizObject["F0000023"] = content;

        newBizObject["F0000029"] = exceptionStr;


        newBizObject.Status = H3.DataModel.BizObjectStatus.Effective;
        newBizObject.Create();



    }


}
