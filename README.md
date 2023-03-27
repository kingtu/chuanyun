氚云快速存取框架，实现在Visual Studio 2022写氚云后端代码，并通过H3CodeComiter(另一个项目)将代码自动保存到氚云。
Visual Studio 2022写氚云后端代码的视频见Http://bilibili.com/Phoenixsoft/
使用例子：
//获取订单表架构
var scm = new Schema(this.Engine, postValue, "订单表");
//获取订单表中的符合条件的订单。“订单编号”为控件显示的名称，“OrderNum”为数据库中的值。
scm.ClearFilter()
   .And("订单编号", "=", OrderNum)
   .And("产品编号", "=", ProductNum)
   .GetFirst(true);
var Quantity = scm["订单数量"];    //取出本条记录订单数量的值。
scm["订单数量"]=Quantity + 10;    //将订单数量加十后保存。
scm.Update(true);  //更新到数据库。
Http://bilibili.com/Phoenixsoft/
