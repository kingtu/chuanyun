using System;
/// <summary>
/// 生产制造流程,设备工时系数表子表
/// </summary>
public class EquipmentTimeCoefficientSubtabulation
{
    H3.DataModel.BizObject inner;
    public static readonly string TableCode = "D001419Fbb7854d117af4bba8eff4de46d128f63";
    public EquipmentTimeCoefficientSubtabulation() { }
    public EquipmentTimeCoefficientSubtabulation(H3.DataModel.BizObject bizObject)
    {
        inner = bizObject;
    }
    private string _DataTitle = "Name";
    /// <summary>
    /// 数据标题
    /// </summary>
    public object DataTitle
    {
        get { return inner[_DataTitle]; }
        set { inner[_DataTitle] = value; }
    }
    private string _DoubleRollingManHourCoefficient = "F0000008";
    /// <summary>
    /// 双轧工时系数
    /// </summary>
    public Object DoubleRollingManHourCoefficient
    {
        get { return inner[_DoubleRollingManHourCoefficient]; }
        set { inner[_DoubleRollingManHourCoefficient] = value; }
    }
    private string _UpperApertureLimit = "F0000011";
    /// <summary>
    /// 孔径上限
    /// </summary>
    public Object UpperApertureLimit
    {
        get { return inner[_UpperApertureLimit]; }
        set { inner[_UpperApertureLimit] = value; }
    }
    private string _LowerOuterDiameterLimit = "F0000013";
    /// <summary>
    /// 外径下限
    /// </summary>
    public Object LowerOuterDiameterLimit
    {
        get { return inner[_LowerOuterDiameterLimit]; }
        set { inner[_LowerOuterDiameterLimit] = value; }
    }
    private string _Objectid = "ObjectId";
    /// <summary>
    /// ObjectId
    /// </summary>
    public Object Objectid
    {
        get { return inner[_Objectid]; }
        set { inner[_Objectid] = value; }
    }
    private string _SingleRollingManHourCoefficient = "F0000007";
    /// <summary>
    /// 单轧工时系数
    /// </summary>
    public Object SingleRollingManHourCoefficient
    {
        get { return inner[_SingleRollingManHourCoefficient]; }
        set { inner[_SingleRollingManHourCoefficient] = value; }
    }
    private string _LowerApertureLimit = "F0000010";
    /// <summary>
    /// 孔径下限
    /// </summary>
    public Object LowerApertureLimit
    {
        get { return inner[_LowerApertureLimit]; }
        set { inner[_LowerApertureLimit] = value; }
    }
    private string _UpperOuterDiameterLimit = "F0000012";
    /// <summary>
    /// 外径上限
    /// </summary>
    public Object UpperOuterDiameterLimit
    {
        get { return inner[_UpperOuterDiameterLimit]; }
        set { inner[_UpperOuterDiameterLimit] = value; }
    }
    private string _EquipmentType = "F0000004";
    /// <summary>
    /// 设备类型
    /// </summary>
    public Object EquipmentType
    {
        get { return inner[_EquipmentType]; }
        set { inner[_EquipmentType] = value; }
    }
    private string _Parentobjectid = "ParentObjectId";
    /// <summary>
    /// ParentObjectId
    /// </summary>
    public object Parentobjectid
    {
        get { return inner[_Parentobjectid]; }
        set { inner[_Parentobjectid] = value; }
    }

    public void Update(bool Effective = true)
    {
        if (Effective)
        {
            inner.Status = H3.DataModel.BizObjectStatus.Effective;
        }
        inner.Update();
    }

    public void Remove()
    {
        if (inner != null)
        {
            inner.Remove();
        }
    }
}

