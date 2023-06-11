using Backend.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAL;

public class BackendDbContext : DbContext
{
    public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options){}
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SportObjectEntity> SportObjects { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<RatingEntity> Ratings { get; set; }
    public DbSet<ReportEntity> Reports { get; set; }
    public DbSet<SlotEntity> Slots { get; set; }
}