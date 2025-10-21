using System;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateOnly DayOfBirth { get; set; }
    public string Phone { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Ім'я: {Name}, Прізвище: {Surname}, ДН: {DayOfBirth}, Тел: {Phone}";
    }
}

public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Назва: {Name}, Адреса: {Address}";
    }
}

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Specialization { get; set; }
    public int HospitalId { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Ім'я: {Name} {Surname}, Спец: {Specialization}, ID Лікарні: {HospitalId}";
    }
}

public class Appointment
{
    public int Id { get; set; }
    public DateOnly Data { get; set; } 
    public TimeOnly Hour { get; set; } 
    public string Diagnosis { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Дата: {Data} {Hour}, Діагноз: {Diagnosis}, ID Пацієнта: {PatientId}, ID Лікаря: {DoctorId}";
    }
}