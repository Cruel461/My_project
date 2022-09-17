namespace ITWitor.Models
{
    public class DataFieldsGroup
    {
        public List<DataField>? DataFields { get; set; }
    }

    public class DataField
    {
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
        public decimal? Row { get; set; }

        public DataField(string? name, string? label, string? type, string? value, decimal? row)
        {
            Name = name;
            Label = label;
            Type = type;
            Value = value;
            Row = row;
        }

        public DataField(string? name, string? type, decimal? row = 1)
        {
            Name = name;
            Type = type;
            Row = row;
        }
    }
}
