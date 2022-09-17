namespace ITWitor.Models
{
  public interface IContentBlock
  {
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public Page? Parent { get; set; }
    public BlockType BlockType { get; set; }
  }
}