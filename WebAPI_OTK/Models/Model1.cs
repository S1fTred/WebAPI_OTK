using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebAPI_OTK
{
    public partial class Model1 : DbContext
    {
        public Model1() : base()
        {
        }

        public Model1(DbContextOptions<Model1> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Получаем строку подключения из конфигурации
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Должность> Должность { get; set; }
        public virtual DbSet<ДСЕ> ДСЕ { get; set; }
        public virtual DbSet<Зарплата> Зарплата { get; set; }
        public virtual DbSet<Изделие> Изделие { get; set; }
        public virtual DbSet<МЛ> МЛ { get; set; }
        public virtual DbSet<Оборудование> Оборудование { get; set; }
        public virtual DbSet<Операция_МЛ> Операция_МЛ { get; set; }
        public virtual DbSet<ПремиальныеКоэффициенты> ПремиальныеКоэффициенты { get; set; }
        public virtual DbSet<Сотрудник> Сотрудник { get; set; }
        public virtual DbSet<ТипОперации> ТипОперации { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация ДСЕ -> МЛ
            modelBuilder.Entity<ДСЕ>()
                .HasMany(e => e.МЛ)
                .WithOne(e => e.ДСЕ)
                .HasForeignKey(e => e.ДСЕID)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация Зарплата
            modelBuilder.Entity<Зарплата>()
                .Property(e => e.ЧасыОтработано)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Зарплата>()
                .Property(e => e.СтавкаЧасовая)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Зарплата>()
                .Property(e => e.СуммаОклад)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Зарплата>()
                .Property(e => e.Премия)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Зарплата>()
                .Property(e => e.ИтогоКВыплате)
                .HasPrecision(10, 2);

            // Конфигурация Изделие -> МЛ
            modelBuilder.Entity<Изделие>()
                .HasMany(e => e.МЛ)
                .WithOne(e => e.Изделие)
                .HasForeignKey(e => e.ИзделиеID)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация МЛ -> Операция_МЛ
            modelBuilder.Entity<МЛ>()
                .HasMany(e => e.Операция_МЛ)
                .WithOne(e => e.МЛ)
                .HasForeignKey(e => e.МЛID)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация Операция_МЛ
            modelBuilder.Entity<Операция_МЛ>()
                .HasMany(e => e.Зарплата)
                .WithOne(e => e.Операция_МЛ)
                .HasForeignKey(e => e.ОперацияМЛID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Операция_МЛ>()
                .Property(e => e.НормаВремениЧас)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Операция_МЛ>()
                .Property(e => e.ЦенаЗаЧас)
                .HasPrecision(10, 2);

            // Конфигурация ПремиальныеКоэффициенты
            modelBuilder.Entity<ПремиальныеКоэффициенты>()
                .Property(e => e.Коэффициент)
                .HasPrecision(5, 2);

            // Конфигурация Сотрудник -> МЛ
            // ИСПРАВЛЕНО: Добавьте коллекцию МЛ в Сотрудник или уберите эту конфигурацию
            modelBuilder.Entity<Сотрудник>()
                .HasMany(e => e.Операция_МЛ)
                .WithOne(e => e.Сотрудник)
                .HasForeignKey(e => e.СотрудникID)
                .OnDelete(DeleteBehavior.Restrict);

            // Добавьте связь для МЛ
            modelBuilder.Entity<МЛ>()
                .HasOne(e => e.Сотрудник)
                .WithMany() // Если нет обратной коллекции
                .HasForeignKey(e => e.СотрудникОТК)
                .OnDelete(DeleteBehavior.SetNull);

            // Конфигурация ТипОперации
            modelBuilder.Entity<ТипОперации>()
                .Property(e => e.ДлительностьЧас)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ТипОперации>()
                .HasMany(e => e.Операция_МЛ)
                .WithOne(e => e.ТипОперации)
                .HasForeignKey(e => e.ТипОперацииID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}