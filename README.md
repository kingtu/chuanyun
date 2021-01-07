氚云快速存取框架，让你用Visual Studio 2019写氚云代码。

使用例子：
//获取订单表架构
var scm = new Schema(this.Engine, me.CurrentPostValue, "订单表");
//获取订单表中的符合条件的订单。“订单编号”为控件显示的名称，“OrderNum”为数据库中的值。
scm.ClearFilter()
   .AndFilter(scm.Columns["订单编号"], "=", OrderNum)
   .AndFilter(scm.Columns["产品编号"], "=", ProductNum)
   .GetFirst(true);
var Quantity = scm.Cell("订单数量");    //取出本条记录订单数量的值。
scm.Cell("订单数量",Quantity + 10);    //将订单数量加十后保存。
scm.Update(true);  //更新到数据库。
