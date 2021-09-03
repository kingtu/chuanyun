using H3;
using H3.DataModel;
using System;

internal class TaskRecorder
{
    private IEngine engine;
    private BizObject bizObject;

    public TaskRecorder(IEngine engine, BizObject bizObject)
    {
        this.engine = engine;
        this.bizObject = bizObject;
    }

    internal object TaskRecord(string v, BizObject bizObject)
    {
        throw new NotImplementedException();
    }
}