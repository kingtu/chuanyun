
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;

public class D001419Sgx7flbvwu9r0u3hail6512uq4 : H3.SmartForm.SmartFormController
{
    public D001419Sgx7flbvwu9r0u3hail6512uq4(H3.SmartForm.SmartFormRequest request) : base(request)
    {

    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        Init(response);
        base.OnLoad(response);

    }



    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //提交
        if (actionName == "Submit")
        {
            base.OnSubmit(actionName, postValue, response);
            //发起异常
            string strInitiateException = this.Request.BizObject[RoughCast.InitiateException] + string.Empty;
            if (strInitiateException == "是")
            {
                //异常工步
                AbnormalStep();
            }
        }
    }
    /**
           * --Author: zzx
           * 关于发起异常之后各个节点进行的操作。
           * 
           */
    protected void AbnormalStep()
    {
        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, RoughCast.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity100" && strActivityCode != "Activity101")
        {
            //设置异常权限
            this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
            //创建发起异常的日志
            logObjectID = ExceptionLog.CreateLog(RoughCast.ID, RoughCast.CurrentWorkStep, RoughCast.CurrentOperation,
            RoughCast.ExceptionCategory, RoughCast.ExceptionDescription, this.Request.BizObject, this.Engine);
            exceptionBo[RoughCast.ObjectIDForUpdateTheExceptionLog] = logObjectID;
            exceptionBo.Update();
        }
        //确认调整意见
        if (strActivityCode == "Activity100")
        {
            //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
            ExceptionLog.UpdateLog(RoughCast.ID, RoughCast.CurrentWorkStep, RoughCast.ExceptionCategory,
            RoughCast.ExceptionDescription, this.Request.BizObject, exceptionBo[RoughCast.ObjectIDForUpdateTheExceptionLog] + string.Empty, this.Engine);
        }
        //审批确认
        if (strActivityCode == "Activity101")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[RoughCast.InitiateException] = "否";
            //异常描述赋值
            exceptionBo[RoughCast.ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[RoughCast.ExceptionCategory] = "安全异常";
            exceptionBo.Update();
        }
    }


    //质检结论值的由来（可删前端有写）
    // protected void Result(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    // {
    //     var Code = this.Request.ActivityCode;
    //     //理化结果
    //     if(Code == "Activity61")
    //     {
    //         var inspectionResults = this.Request.BizObject[RoughCast.InspectionResults] + string.Empty;
    //         this.Request.BizObject[RoughCast.QualityInspectionConclusion] = inspectionResults;
    //     }
    //     //待理化结果
    //     if(Code == "Activity262")
    //     {
    //         //理化结果
    //         var unqualified = this.Request.BizObject[RoughCast.PhysicalAndChemicalResults] + string.Empty;
    //         this.Request.BizObject[RoughCast.QualityInspectionConclusion] = unqualified;
    //     }

    // }
    protected void Init(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {

            H3.DataModel.BizObject thisObj = this.Request.BizObject;
            H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);

            if (thisObj != null)
            {
                string bizid = thisObj.ObjectId;

                string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_D001419Sgx7flbvwu9r0u3hail6512uq4  a left join H_WorkItem b on a.objectid = b.BizObjectId  where (b.ActivityCode = 'Activity104') and b.BizObjectId = '{0}'", bizid);
                DataTable data = this.Engine.Query.QueryTable(command, null);
                //机加工耗时计算
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    if (data.Rows[0]["utime"] + string.Empty != string.Empty)
                    {
                        string utimestr = data.Rows[0]["utime"] + string.Empty;
                        double utime = double.Parse(utimestr) / 10000000 / 60;
                        thisObj[RoughCast.ActualProcessingTime] = utime;
                    }
                }

                //当前工序
                if (thisObj[RoughCast.CurrentOperation] + string.Empty == string.Empty)
                {           //当前工序
                    thisObj[RoughCast.CurrentOperation] = "毛坯";
                }

                //产品类别更新
                if (thisObj[RoughCast.ProductCategory] + string.Empty == string.Empty)
                {
                    string orderSpec = thisObj[RoughCast.OrderSpecificationNumber] + string.Empty; //订单规格号
                    string mysql = string.Format("Select ObjectId,F0000004 From i_D0014196b62f7decd924e1e8713025dc6a39aa5 Where F0000073 = '{0}'", orderSpec);
                    DataTable typeData = this.Engine.Query.QueryTable(mysql, null);
                    if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
                    {
                        thisObj[RoughCast.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty; //产品参数表
                        thisObj[RoughCast.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty; //产品类型
                    }
                }



                if (this.Request.WorkflowInstance.IsUnfinished)
                {
                    //获取父流程实例对象
                    H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);

                    var parentId = instance != null ? instance.BizObjectId : "";
                    //获取父流程业务对象
                    H3.DataModel.BizObject parentObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Sq0biizim9l50i2rl6kgbpo3u4", parentId, false);
                    //读取父流程数据Id
                    string planId = parentObj != null ? parentObj[ProcessFlow.OperationSchedule] + string.Empty : string.Empty;
                    //获取工序计划业务对象
                    H3.DataModel.BizObject planObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Szlywopbivyrv1d64301ta5xv4", planId, false);
                    //读取《工序计划表》中单件忽略理化结果设置
                    string lhConfig = planObj != null ? planObj[ABCDProcessPlan.SinglePieceIgnorePhysicochemicalResults] + string.Empty : string.Empty;
                    //读取规格表数据Id
                    string sizeId = planObj != null ? planObj[ABCDProcessPlan.OrderSpecificationTable] + string.Empty : string.Empty;
                    //获取规格表数据对象
                    H3.DataModel.BizObject sizeFormData = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, "D001419Skniz33124ryujrhb4hry7md21", sizeId, false);
                    //从规格表中读取试样尺寸
                    string specimanSize = sizeFormData != null ? sizeFormData[OrderSpecification.SampleSize] + string.Empty : string.Empty;
                    //赋值试样尺寸
                    thisObj[RoughCast.SampleType] = specimanSize;

                    //工艺配置表
                    string mySql = string.Format("Select * from i_D0014194755c7eecbe9410c84cf6640d9cb147b");
                    DataTable jzForm = this.Engine.Query.QueryTable(mySql, null);
                    //加载"工艺配置表"中“是否开户制样流程”的值
                    thisObj[RoughCast.WhetherToStartTheSamplePreparationProcess] = jzForm.Rows[0][ProcessConfig.GlobalOpenSamplePreparationProcess] + string.Empty == "1" ? "是" : "否";

                    //质量配置表
                    string sql = string.Format("Select * from i_D0014198feb957936e040648d486b034af96597");
                    DataTable qcForm = this.Engine.Query.QueryTable(sql, null);

                    if (qcForm.Rows[0][QAConfig.PriorityLevelphysicochemical] + string.Empty == "配置表")
                    {           //是否忽略理化结果                  //全局忽略理化结果
                        thisObj[RoughCast.WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = qcForm.Rows[0]["F0000004"] + string.Empty;
                    }

                    if (qcForm.Rows[0][QAConfig.PriorityLevelphysicochemical] + string.Empty == "计划表")
                    {
                        if (lhConfig != "")
                        {           //是否忽略理化结果    //单件忽略理化结果
                            thisObj[RoughCast.WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = lhConfig;
                        }
                        else
                        {          //是否忽略理化结果                   //全局忽略理化结果
                            thisObj[RoughCast.WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = qcForm.Rows[0]["F0000004"] + string.Empty;
                        }
                    }
                }
                //更新本表单
                thisObj.Update();
            }

            try
            {       //同步数据至实时制造情况
                //DataSync.instance.MPSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }

        }

    }

}


