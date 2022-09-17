namespace ITWitor.Models
{
    public class Form
    {
        public string? Name { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public string? ConfirmText { get; set; }
        public List<DataField>? DataFields { get; set; }
        public List<DataFieldsGroup>? DataFieldsGroups { get; set; }
        public Form(string? action, string? controller)
        {
            Action = action;
            Controller = controller;
        }
        public Form(string? action, string? controller, List<DataField> dataFields)
        {
            Action = action;
            Controller = controller;
            DataFields = dataFields;
        }
        public Form(string? action, string? controller, List<DataField> dataFields, string? name)
        {
            Action = action;
            Controller = controller;
            DataFields = dataFields;
            Name = name;
        }
    }
}
