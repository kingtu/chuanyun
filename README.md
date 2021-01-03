氚云快速存取框架，让你用Visual Studio 2019写氚云代码。

使用例子：
protected void GetProductObjectID(Schema me, SubmitSmartFormResponse response)
    {
       var pdata = this.Request["Data"] + string.Empty;
       var pro = this.Deserialize<Product>(pdata);
 
       var scm = new Schema(this.Engine, me.CurrentPostValue, "产品参数表");
       scm.ClearFilter()
           .AndFilter(scm.Columns["订单号"], "=", pro.OrderNum)
           .AndFilter(scm.Columns["规格号"], "=", pro.SpecNum)
           .GetFirst(true);
       pro.ObjectID = scm.Cell("ObjectId");
       return;
    }
