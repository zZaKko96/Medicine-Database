using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class DatabaseModel
{

    public async Task AddPatientAsync(Patient patient)
    {
        using var db = new ApplicationContext();
        db.Patients.Add(patient);
        await db.SaveChangesAsync();
    }

    public async Task<string> AddDoctorAsync(Doctor doctor)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Doctors.Add(doctor);
            await db.SaveChangesAsync();
            return "Лікаря успішно додано.";
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            return "ПОМИЛКА: Неможливо додати лікаря. Переконайтеся, що лікарня з таким ID існує.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> AddAppointmentAsync(Appointment app)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Appointments.Add(app);
            await db.SaveChangesAsync();
            return "Прийом успішно додано.";
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            return "ПОМИЛКА: Неможливо додати прийом. Переконайтеся, що пацієнт та лікар існують.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> AddHospitalAsync(Hospital hospital)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Hospitals.Add(hospital);
            await db.SaveChangesAsync();
            return "Лікарню успішно додано.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        using var db = new ApplicationContext();
        return await db.Patients.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<List<Doctor>> GetAllDoctorsAsync()
    {
        using var db = new ApplicationContext();
        return await db.Doctors.OrderBy(d => d.Id).ToListAsync();
    }

    public async Task<List<Hospital>> GetAllHospitalsAsync()
    {
        using var db = new ApplicationContext();
        return await db.Hospitals.OrderBy(h => h.Id).ToListAsync();
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync()
    {
        using var db = new ApplicationContext();
        return await db.Appointments.OrderBy(a => a.Id).ToListAsync();
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        using var db = new ApplicationContext();
        return await db.Patients.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Doctor?> GetDoctorByIdAsync(int id)
    {
        using var db = new ApplicationContext();
        return await db.Doctors.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Hospital?> GetHospitalByIdAsync(int id)
    {
        using var db = new ApplicationContext();
        return await db.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int id)
    {
        using var db = new ApplicationContext();
        return await db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<string> UpdatePatientAsync(Patient patient)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Patients.Update(patient);
            int rows = await db.SaveChangesAsync();
            return rows > 0 ? "Дані пацієнта оновлено." : "ПОМИЛКА: Пацієнта не знайдено або дані не змінились.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<string> UpdateDoctorAsync(Doctor doctor)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Doctors.Update(doctor);
            await db.SaveChangesAsync();
            return "Дані лікаря оновлено.";
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            return $"ПОМИЛКА: Неможливо оновити. Лікарня з ID = {doctor.HospitalId} не існує.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<string> UpdateHospitalAsync(Hospital hospital)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Hospitals.Update(hospital);
            await db.SaveChangesAsync();
            return "Дані лікарні оновлено.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<string> UpdateAppointmentAsync(Appointment app)
    {
        try
        {
            using var db = new ApplicationContext();
            db.Appointments.Update(app);
            await db.SaveChangesAsync();
            return "Дані прийому оновлено.";
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            return "ПОМИЛКА: Неможливо оновити. Переконайтеся, що пацієнт та лікар існують.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<string> DeletePatientAsync(int id)
    {
        try
        {
            using var db = new ApplicationContext();
            int rows = await db.Patients.Where(p => p.Id == id).ExecuteDeleteAsync();
            return rows > 0 ? "Пацієнта успішно видалено." : $"ПОМИЛКА: Пацієнта з ID = {id} не знайдено.";
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return "ПОМИЛКА: Неможливо видалити пацієнта, оскільки за ним закріплені 'прийоми'.";
        }
        catch (Exception ex) when (IsForeignKeyViolation(ex)) // EF Core обгортка
        {
            return "ПОМИЛКА: Неможливо видалити пацієнта, оскільки за ним закріплені 'прийоми'.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> DeleteDoctorAsync(int id)
    {
        try
        {
            using var db = new ApplicationContext();
            int rows = await db.Doctors.Where(d => d.Id == id).ExecuteDeleteAsync();
            return rows > 0 ? "Лікаря успішно видалено." : $"ПОМИЛКА: Лікаря з ID = {id} не знайдено.";
        }
        catch (Exception ex) when (IsForeignKeyViolation(ex))
        {
            return "ПОМИЛКА: Неможливо видалити лікаря, оскільки за ним закріплені 'прийоми'.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> DeleteAppointmentAsync(int id)
    {
        try
        {
            using var db = new ApplicationContext();
            int rows = await db.Appointments.Where(a => a.Id == id).ExecuteDeleteAsync();
            return rows > 0 ? "Прийом успішно видалено." : $"ПОМИЛКА: Прийом з ID = {id} не знайдено.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> DeleteHospitalAsync(int id)
    {
        try
        {
            using var db = new ApplicationContext();
            int rows = await db.Hospitals.Where(h => h.Id == id).ExecuteDeleteAsync();
            return rows > 0 ? "Лікарню успішно видалено." : "ПОМИЛКА: Лікарню не знайдено.";
        }
        catch (Exception ex) when (IsForeignKeyViolation(ex))
        {
            return "ПОМИЛКА: Неможливо видалити лікарню, оскільки за нею закріплені лікарі.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<(List<string> results, long timeMs)> SearchPatientsByDoctorAndDateAsync(
        string surnamePattern, DateOnly startDate, DateOnly endDate)
    {
        using var db = new ApplicationContext();
        var sw = Stopwatch.StartNew();

        var query = from p in db.Patients
                    join a in db.Appointments on p.Id equals a.PatientId
                    join d in db.Doctors on a.DoctorId equals d.Id
                    where EF.Functions.ILike(d.Surname, $"%{surnamePattern}%")
                          && a.Data >= startDate && a.Data <= endDate
                    group new { p, d } by new { p.Id, p.Name, p.Surname, p.Phone, DoctorSurname = d.Surname } into g
                    orderby g.Count() descending
                    select new
                    {
                        g.Key.Name,
                        g.Key.Surname,
                        g.Key.Phone,
                        g.Key.DoctorSurname,
                        Count = g.Count()
                    };

        var data = await query.ToListAsync();

        var results = data.Select(x =>
            $"Пацієнт: {x.Name} {x.Surname} (Тел: {x.Phone}), Лікар: {x.DoctorSurname}, Кількість прийомів: {x.Count}"
        ).ToList();

        sw.Stop();
        return (results, sw.ElapsedMilliseconds);
    }

    public async Task<(List<string> results, long timeMs)> SearchDoctorStatisticsAsync(
        string specPattern, DateOnly startDate, DateOnly endDate)
    {
        using var db = new ApplicationContext();
        var sw = Stopwatch.StartNew();

        var query = from d in db.Doctors
                    join a in db.Appointments on d.Id equals a.DoctorId into docApps
                    from a in docApps.DefaultIfEmpty() // LEFT JOIN
                    where EF.Functions.ILike(d.Specialization, $"%{specPattern}%")
                          && (a == null || (a.Data >= startDate && a.Data <= endDate))
                    group a by new { d.Id, d.Name, d.Surname, d.Specialization } into g
                    orderby g.Count(x => x != null) descending
                    select new
                    {
                        g.Key.Name,
                        g.Key.Surname,
                        g.Key.Specialization,
                        Count = g.Count(x => x != null)
                    };

        var data = await query.ToListAsync();

        var results = data.Select(x =>
            $"Лікар: {x.Name} {x.Surname} ({x.Specialization}), Кількість прийомів: {x.Count}"
        ).ToList();

        sw.Stop();
        return (results, sw.ElapsedMilliseconds);
    }

    public async Task<(List<string> results, long timeMs)> SearchHospitalStatisticsAsync(
        string addressPattern, string diagnosisPattern)
    {
        using var db = new ApplicationContext();
        var sw = Stopwatch.StartNew();

        var query = from h in db.Hospitals
                    join d in db.Doctors on h.Id equals d.HospitalId
                    join a in db.Appointments on d.Id equals a.DoctorId
                    where EF.Functions.ILike(h.Address, $"%{addressPattern}%")
                          && EF.Functions.ILike(a.Diagnosis, $"%{diagnosisPattern}%")
                    group a by new { h.Id, h.Name, h.Address } into g
                    orderby g.Select(x => x.PatientId).Distinct().Count() descending
                    select new
                    {
                        g.Key.Name,
                        g.Key.Address,
                        UniquePatients = g.Select(x => x.PatientId).Distinct().Count()
                    };

        var data = await query.ToListAsync();

        var results = data.Select(x =>
            $"Лікарня: {x.Name} ({x.Address}), Кількість унікальних пацієнтів: {x.UniquePatients}"
        ).ToList();

        sw.Stop();
        return (results, sw.ElapsedMilliseconds);
    }

    public async Task<string> GenerateRandomDataAsync(int count)
    {
        using var db = new ApplicationContext();
        var sw = Stopwatch.StartNew();

        var sql = $@"
        WITH generated_patients AS (
            INSERT INTO patient (name, surname, ""day of birth"", phone)
            SELECT 
                (ARRAY['Іван', 'Петро', 'Олександр', 'Сергій', 'Андрій', 'Дмитро', 'Марія', 'Анна', 'Олена', 'Тетяна', 'Вікторія', 'Юлія'])[floor(random() * 12 + 1)::int],
                (ARRAY['Мельник', 'Шевченко', 'Коваленко', 'Бондаренко', 'Ткаченко', 'Кравченко', 'Олійник', 'Петренко', 'Іванов', 'Сидоренко', 'Романюк', 'Косарук'])[floor(random() * 12 + 1)::int],
                DATE '1950-01-01' + (RANDOM() * (365 * 70))::integer,
                (ARRAY['050', '095', '066', '067'])[floor(random() * 4 + 1)::int] || LPAD((RANDOM() * 9999999)::int::text, 7, '0')
            FROM generate_series(1, {count}) AS s(id)
            RETURNING id
        ),
        generated_hospitals AS (
            INSERT INTO hospital (name, address)
            SELECT
                (ARRAY['Київська', 'Львівська', 'Луцька', 'Харківська', 'Одеська', 'Дніпровська', 'Волинська'])[floor(random() * 7 + 1)::int]
                || ' ' || 
                (ARRAY['районна', 'обласна', 'міська'])[floor(random() * 3 + 1)::int]
                || ' лікарня',
                (ARRAY['вулиця', 'проспект', 'бульвар'])[floor(random() * 3 + 1)::int]
                || ' ' ||
                (ARRAY['Шевченка', 'Лесі Українки', 'Франка', 'Грушевського', 'Перемоги', 'Миру', 'Соборності'])[floor(random() * 7 + 1)::int]
                || ', ' ||
                (floor(random() * 200 + 1)::int)::text
            FROM generate_series(1, {count / 10 + 1}) AS s(id)
            RETURNING id
        ),
        generated_doctors AS (
            INSERT INTO doctor (name, surname, specialization, hospital_id)
            SELECT
                (ARRAY['Олег', 'Ігор', 'Володимир', 'Максим', 'Назар', 'Тарас', 'Ольга', 'Ірина', 'Наталія', 'Світлана'])[floor(random() * 10 + 1)::int],
                (ARRAY['Петров', 'Іваненко', 'Захаров', 'Павлюк', 'Лисенко', 'Гончарук', 'Попова', 'Давидова', 'Ковальчук', 'Мартинюк'])[floor(random() * 10 + 1)::int],
                (ARRAY['Терапевт', 'Хірург', 'Проктолог', 'Офтальмолог', 'Кардіолог', 'Невролог', 'Педіатр'])[floor(random() * 7 + 1)::int],
                h_rand.id
            FROM generate_series(1, {count / 5 + 1}) AS s(id)

            CROSS JOIN LATERAL (
                SELECT id FROM generated_hospitals
                WHERE s.id > 0  
                ORDER BY random() LIMIT 1
            ) AS h_rand
            RETURNING id
        )
        INSERT INTO appointment (data, hour, diagnosis, patient_id, doctor_id)
        SELECT
            DATE '2024-01-01' + (RANDOM() * 700)::integer,
            MAKE_TIME(floor(random() * 10 + 8)::int, floor(random() * 60)::int, floor(random() * 60)::double precision),
            (ARRAY['Здоровий', 'ГРВІ', 'Перелом', 'Вивих', 'Мігрень', 'Ангіна', 'Гастрит', 'Отруєння'])[floor(random() * 8 + 1)::int],
            p_rand.id,
            d_rand.id
        FROM generate_series(1, {count}) AS s(id)

        CROSS JOIN LATERAL (
            SELECT id FROM generated_patients
            WHERE s.id > 0 
            ORDER BY random() LIMIT 1
        ) AS p_rand

        CROSS JOIN LATERAL (
            SELECT id FROM generated_doctors
            WHERE s.id > 0  
            ORDER BY random() LIMIT 1
        ) AS d_rand;
        ";

        try
        {
            await db.Database.ExecuteSqlRawAsync(sql);
            sw.Stop();
            return $"Успішно згенеровано дані. Загальний час: {sw.ElapsedMilliseconds} мс.";
        }
        catch (Exception ex)
        {
            return $"Помилка генерації: {ex.Message}";
        }
    }

    public async Task<string> ClearAllDataAsync()
    {
        try
        {
            using var db = new ApplicationContext();
            await db.Database.ExecuteSqlRawAsync(@"
                TRUNCATE hospital RESTART IDENTITY CASCADE;
                TRUNCATE patient RESTART IDENTITY CASCADE;
            ");
            return "Успіх! Усі дані з таблиць видалено, лічильники ID скинуто до 1.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: Не вдалося очистити базу даних. {ex.Message}";
        }
    }

    private bool IsForeignKeyViolation(Exception ex)
    {
        var inner = ex.InnerException as PostgresException;
        return inner != null && inner.SqlState == "23503";
    }
}