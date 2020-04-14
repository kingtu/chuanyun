
namespace H3
{
    public interface IEngine
    {
        BizObjectManager BizObjectManager { get; set; }
        WorkflowInstanceManager WorkflowInstanceManager { get; set; }
        Interactor Interactor { get; set; }
        WorkflowTemplateManager WorkflowTemplateManager { get; set; }
    }
}