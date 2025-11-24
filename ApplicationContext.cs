using Microsoft.EntityFrameworkCore;
using System;

public class ApplicationContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    public ApplicationContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=Qwertyasd111;Database=postgres");

        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().ToTable("patient");
        modelBuilder.Entity<Doctor>().ToTable("doctor");
        modelBuilder.Entity<Hospital>().ToTable("hospital");
        modelBuilder.Entity<Appointment>().ToTable("appointment");
    }
}