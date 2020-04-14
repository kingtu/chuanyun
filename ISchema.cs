using H3;
using H3.DataModel;

public interface ISchema
{
    Schema AndFilter(string filed, string Operator, string value);
    string Cell(string columnName);
    string Cell(string columnName, BizObject row);
    void Cell(string columnName, string value);
    void Cell(string columnName, string value, BizObject row);
    object CellAny(string columnName);
    Schema ClearFilter();
    void Copy(Schema srcSchema);
    void CopyPostValue();
    void Create();
    void Create(bool Effective);
    BizObjectSchema Get();
    BizObjectSchema Get(IEngine Engine, string appID, string tableID);
    BizObject GetFirst();
    BizObject GetFirst(bool setCurrentRow);
    BizObject[] GetList();
    void GetNew();
    BizObject GetRow(string BizObjectId);
    BizObject[] GetRows(string[] objs);
    Schema OrFilter(string filed, string Operator, string value);
    string PostValue(string columnName);
    string PostValue(string columnName, SmartForm.SmartFormPostValue postValue);
    void Remove();
    bool RunActivity(string currentActivityCode, string nextActivityCode);
    bool StartNewActivity();
    void Update();
    void Update(bool Effective);
}