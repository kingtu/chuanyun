
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using H3;

public class Tools
{
    public class Filter
    {

        /// <summary>
        /// 构建符合条件的And过滤器       
        /// </summary>
        /// <param name="filter">需要构建的过滤器</param>
        /// <param name="componentCode">控件编码</param>
        /// <param name="operators">操作符</param>
        /// <param name="value">控件值</param>
        /// <param name="isColumn"></param>
        /// <Author>fubin</Author>
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
            H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema(schemacode);
            H3.DataModel.BizObject[] bizObjects = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
            return bizObjects[0];
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
         * @param activity 流程节点编码
         * @param worker 人员ID
         */
        public void AdjustParticipant(H3.IEngine engine, string workflowInstandId, string activity, string[] worker)
        {
            H3.Workflow.Messages.AdjustParticipantMessage AdjustMessage = new H3.Workflow.Messages.AdjustParticipantMessage(workflowInstandId, activity, worker);
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
        public static void StartWorkflow(H3.IEngine engine, H3.DataModel.BizObject refBizObject, string userId , bool flag = true)
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
}

