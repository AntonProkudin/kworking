namespace kworkingApi.Entities;
public class KworkingContext : DbContext
{
    public KworkingContext(DbContextOptions<KworkingContext> options) : base(options)
    { }

    public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
    public virtual DbSet<TblUserFriend> TblUserFriends { get; set; } = null!;
    public virtual DbSet<TblMessage> TblMessages { get; set; } = null!;
    public virtual DbSet<TblOrder> TblOrders { get; set; } = null!;
}
