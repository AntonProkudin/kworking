namespace kworkingApi.Entities;

public class TblOrder
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }

}
