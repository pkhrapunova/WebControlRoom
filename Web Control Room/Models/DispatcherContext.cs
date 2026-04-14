using Microsoft.EntityFrameworkCore;


namespace WebControlRoom.Models
{
    public class DispatcherContext : DbContext
    {
        public DispatcherContext(DbContextOptions<DispatcherContext> options) : base(options) { }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<WeekDay> WeekDays { get; set; }
        public DbSet<ClassTime> ClassTimes { get; set; }
        public DbSet<RoomOccupancy> RoomOccupancies { get; set; }
        public DbSet<EmailConfirmCode> EmailConfirmCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailConfirmCode>(entity =>
            {
                entity.ToTable("КодПодтвержденияПочты");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.Email).HasColumnName("Почта");
                entity.Property(e => e.Code).HasColumnName("КодПодтверждения");
                entity.Property(e => e.ExpireAt).HasColumnName("ДатаИстечения");
            });
            // ---------------- Buildings ----------------
            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Корпус");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.Number).HasColumnName("Номер");
            });

            // ---------------- Faculties ----------------
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Факультет");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.Name).HasColumnName("Название");
                entity.Property(e => e.BuildingId).HasColumnName("КодКорпуса");

                entity.HasOne(f => f.Building)
                      .WithMany(b => b.Faculties)
                      .HasForeignKey(f => f.BuildingId);
            });

            // ---------------- RoomTypes ----------------
            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("Вид");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.TypeName).HasColumnName("ТипАудитории");
            });

            // ---------------- Rooms ----------------
            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Аудитория");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.RoomNumber).HasColumnName("НомерАудитории");
                entity.Property(e => e.Capacity).HasColumnName("Вместимость");
                entity.Property(e => e.RoomTypeId).HasColumnName("КодВида");
                entity.Property(e => e.BuildingId).HasColumnName("КодКорпуса");
                entity.Property(e => e.Multimedia).HasColumnName("Мультимедиа"); 

                entity.HasOne(r => r.RoomType)
                      .WithMany(rt => rt.Rooms)
                      .HasForeignKey(r => r.RoomTypeId);

                entity.HasOne(r => r.Building)
                      .WithMany(b => b.Rooms)
                      .HasForeignKey(r => r.BuildingId);
            });

            // ---------------- Groups ----------------
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Группа");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.Name).HasColumnName("Название");
                entity.Property(e => e.Specialty).HasColumnName("Специальность");
                entity.Property(e => e.Course).HasColumnName("Курс");
                entity.Property(e => e.SemesterStart).HasColumnName("ДатаНачалаСеместра");
                entity.Property(e => e.SemesterEnd).HasColumnName("ДатаОкончанияСеместра");
                entity.Property(e => e.SemesterNumber).HasColumnName("НомерСеместра");
                entity.Property(e => e.StudentCount).HasColumnName("Численность");
                entity.Property(e => e.FacultyId).HasColumnName("КодФакультета");

                entity.HasOne(g => g.Faculty)
                      .WithMany(f => f.Groups)
                      .HasForeignKey(g => g.FacultyId);
            });

            // ---------------- WeekDays ----------------
            modelBuilder.Entity<WeekDay>(entity =>
            {
                entity.ToTable("День Недели");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.Name).HasColumnName("Название");
            });

            // ---------------- ClassTimes ----------------
            modelBuilder.Entity<ClassTime>(entity =>
            {
                entity.ToTable("Пара");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.Number).HasColumnName("Номер");
                entity.Property(e => e.StartTime).HasColumnName("ВремяНачала");
                entity.Property(e => e.EndTime).HasColumnName("ВремяОкончания");
            });

            // ---------------- RoomOccupancies ----------------
            modelBuilder.Entity<RoomOccupancy>(entity =>
            {
                entity.ToTable("Занятость Аудитории");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Код");
                entity.Property(e => e.RoomId).HasColumnName("КодАудитории");
                entity.Property(e => e.GroupId).HasColumnName("КодГруппы");
                entity.Property(e => e.WeekDayId).HasColumnName("КодДня");
                entity.Property(e => e.ClassTimeId).HasColumnName("КодПары");
                entity.Property(e => e.StartDate).HasColumnName("ДатаНачала");
                entity.Property(e => e.EndDate).HasColumnName("ДатаОкончания");

                entity.HasOne(o => o.Room)
                      .WithMany(r => r.Occupancies)
                      .HasForeignKey(o => o.RoomId);

                entity.HasOne(o => o.Group)
                      .WithMany(g => g.Occupancies)
                      .HasForeignKey(o => o.GroupId);

                entity.HasOne(o => o.WeekDay)
                      .WithMany(w => w.Occupancies)
                      .HasForeignKey(o => o.WeekDayId);

                entity.HasOne(o => o.ClassTime)
                      .WithMany(c => c.Occupancies)
                      .HasForeignKey(o => o.ClassTimeId);
            });
        }
    }
}
