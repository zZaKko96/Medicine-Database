using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Patient
{
    [Key]
    [Column("id")] 
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("surname")]
    public string Surname { get; set; }

    [Column("day of birth")] 
    public DateOnly DayOfBirth { get; set; }

    [Column("phone")]
    public string Phone { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public override string ToString()
    {
        return $"ID: {Id}, Ім'я: {Name}, Прізвище: {Surname}, ДН: {DayOfBirth}, Тел: {Phone}";
    }
}

public class Hospital
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("address")]
    public string Address { get; set; }
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public override string ToString()
    {
        return $"ID: {Id}, Назва: {Name}, Адреса: {Address}";
    }
}

public class Doctor
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("surname")]
    public string Surname { get; set; }

    [Column("specialization")]
    public string Specialization { get; set; }

    [Column("hospital_id")] 
    public int HospitalId { get; set; }

    [ForeignKey("HospitalId")]
    public virtual Hospital Hospital { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public override string ToString()
    {
        return $"ID: {Id}, Ім'я: {Name} {Surname}, Спец: {Specialization}, ID Лікарні: {HospitalId}";
    }
}

public class Appointment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("data")]
    public DateOnly Data { get; set; }

    [Column("hour")]
    public TimeOnly Hour { get; set; }

    [Column("diagnosis")]
    public string Diagnosis { get; set; }

    [Column("patient_id")]
    public int PatientId { get; set; }

    [Column("doctor_id")]
    public int DoctorId { get; set; }

    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }

    [ForeignKey("DoctorId")]
    public virtual Doctor Doctor { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Дата: {Data} {Hour}, Діагноз: {Diagnosis}, ID Пацієнта: {PatientId}, ID Лікаря: {DoctorId}";
    }
}