using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Text;
using System.Threading.Tasks;

public class DatabaseModel
{
    private readonly string _connectionString =
        "Host=localhost;Username=postgres;Password=Qwertyasd111;Database=postgres";

    private async Task<NpgsqlConnection> GetConnectionAsync()
    {
        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        return conn;
    }

    public async Task AddPatientAsync(Patient patient)
    {
        await using var conn = await GetConnectionAsync();
        var sql = "INSERT INTO patient (name, surname, \"day of birth\", phone) " +
          "VALUES (@name, @surname, @dob, @phone)";
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("name", patient.Name);
        cmd.Parameters.AddWithValue("surname", patient.Surname);
        cmd.Parameters.AddWithValue("dob", patient.DayOfBirth);
        cmd.Parameters.AddWithValue("phone", patient.Phone);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<string> AddDoctorAsync(Doctor doctor)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "INSERT INTO doctor (name, surname, specialization, hospital_id) " +
                      "VALUES (@name, @surname, @spec, @h_id)";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("name", doctor.Name);
            cmd.Parameters.AddWithValue("surname", doctor.Surname);
            cmd.Parameters.AddWithValue("spec", doctor.Specialization);
            cmd.Parameters.AddWithValue("h_id", doctor.HospitalId);

            await cmd.ExecuteNonQueryAsync();
            return "Лікаря успішно додано.";
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return "ПОМИЛКА: Неможливо додати лікаря. " +
                   "Переконайтеся, що лікарня з ID = " + doctor.HospitalId + " існує.";
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
            await using var conn = await GetConnectionAsync();
            var sql = "INSERT INTO appointment (data, hour, diagnosis, patient_id, doctor_id) " +
                      "VALUES (@data, @hour, @diag, @p_id, @d_id)";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("data", app.Data);
            cmd.Parameters.AddWithValue("hour", app.Hour);
            cmd.Parameters.AddWithValue("diag", app.Diagnosis);
            cmd.Parameters.AddWithValue("p_id", app.PatientId);
            cmd.Parameters.AddWithValue("d_id", app.DoctorId);

            await cmd.ExecuteNonQueryAsync();
            return "Прийом успішно додано.";
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return "ПОМИЛКА: Неможливо додати прийом. " +
                   "Переконайтеся, що пацієнт та лікар з вказаними ID існують.";
        }
    }

    public async Task<string> AddHospitalAsync(Hospital hospital)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "INSERT INTO hospital (name, address) VALUES (@name, @address)";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("name", hospital.Name);
            cmd.Parameters.AddWithValue("address", hospital.Address);

            await cmd.ExecuteNonQueryAsync();
            return "Лікарню успішно додано.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<string> DeletePatientAsync(int patientId)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "DELETE FROM patient WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", patientId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                return "Пацієнта успішно видалено.";
            }
            else
            {
                return "ПОМИЛКА: Пацієнта з ID = " + patientId + " не знайдено.";
            }
        }
        catch (PostgresException ex) when (ex.SqlState == "23503") 
        {
            return "ПОМИЛКА: Неможливо видалити пацієнта, оскільки за ним закріплені 'прийоми'.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> DeleteDoctorAsync(int doctorId)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "DELETE FROM doctor WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", doctorId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                return "Лікаря успішно видалено.";
            }
            else
            {
                return "ПОМИЛКА: Лікаря з ID = " + doctorId + " не знайдено.";
            }
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return "ПОМИЛКА: Неможливо видалити лікаря, оскільки за ним закріплені 'прийоми'.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> DeleteAppointmentAsync(int appointmentId)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "DELETE FROM appointment WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", appointmentId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                return "Прийом успішно видалено.";
            }
            else
            {
                return "ПОМИЛКА: Прийом з ID = " + appointmentId + " не знайдено.";
            }
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<string> DeleteHospitalAsync(int hospitalId)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "DELETE FROM hospital WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", hospitalId);
            await cmd.ExecuteNonQueryAsync();
            return "Лікарню успішно видалено.";
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return "ПОМИЛКА: Неможливо видалити лікарню, оскільки за нею закріплені лікарі.";
        }
        catch (Exception ex)
        {
            return $"Загальна помилка: {ex.Message}";
        }
    }

    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        var patients = new List<Patient>();
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, name, surname, \"day of birth\", phone FROM patient ORDER BY id";
        await using var cmd = new NpgsqlCommand(sql, conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            patients.Add(new Patient
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("name")),
                Surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("surname")),
                DayOfBirth = reader.IsDBNull(reader.GetOrdinal("day of birth")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("day of birth"))),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("phone"))
            });
        }
        return patients;
    }

    public async Task<List<Doctor>> GetAllDoctorsAsync()
    {
        var doctors = new List<Doctor>();
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, name, surname, specialization, hospital_id FROM doctor ORDER BY id";
        await using var cmd = new NpgsqlCommand(sql, conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            doctors.Add(new Doctor
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("name")),
                Surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("surname")),
                Specialization = reader.IsDBNull(reader.GetOrdinal("specialization")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("specialization")),
                HospitalId = reader.GetInt32(reader.GetOrdinal("hospital_id"))
            });
        }
        return doctors;
    }

    public async Task<List<Hospital>> GetAllHospitalsAsync()
    {
        var hospitals = new List<Hospital>();
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, name, address FROM hospital ORDER BY id";
        await using var cmd = new NpgsqlCommand(sql, conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            hospitals.Add(new Hospital
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("name")),
                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("address"))
            });
        }
        return hospitals;
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync()
    {
        var appointments = new List<Appointment>();
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, data, hour, diagnosis, patient_id, doctor_id FROM appointment ORDER BY id";
        await using var cmd = new NpgsqlCommand(sql, conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            appointments.Add(new Appointment
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Data = reader.IsDBNull(reader.GetOrdinal("data")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data"))),
                Hour = reader.IsDBNull(reader.GetOrdinal("hour")) ? TimeOnly.MinValue : TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("hour"))),
                Diagnosis = reader.IsDBNull(reader.GetOrdinal("diagnosis")) ? "[Немає даних]" : reader.GetString(reader.GetOrdinal("diagnosis")),
                PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id"))
            });
        }
        return appointments;
    }

    public async Task<string> UpdatePatientAsync(Patient patient)
    {
        try
        {
            await using var conn = await GetConnectionAsync();
            var sql = "UPDATE patient SET name = @name, surname = @surname, \"day of birth\" = @dob, phone = @phone " +
                      "WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("name", patient.Name);
            cmd.Parameters.AddWithValue("surname", patient.Surname);
            cmd.Parameters.AddWithValue("dob", patient.DayOfBirth);
            cmd.Parameters.AddWithValue("phone", patient.Phone);
            cmd.Parameters.AddWithValue("id", patient.Id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0 ? "Дані пацієнта оновлено." : "ПОМИЛКА: Пацієнта не знайдено.";
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
            await using var conn = await GetConnectionAsync();
            var sql = "UPDATE doctor SET name = @name, surname = @surname, specialization = @spec, hospital_id = @h_id " +
                      "WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("name", doctor.Name);
            cmd.Parameters.AddWithValue("surname", doctor.Surname);
            cmd.Parameters.AddWithValue("spec", doctor.Specialization);
            cmd.Parameters.AddWithValue("h_id", doctor.HospitalId);
            cmd.Parameters.AddWithValue("id", doctor.Id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0 ? "Дані лікаря оновлено." : "ПОМИЛКА: Лікаря не знайдено.";
        }
        catch (PostgresException ex) when (ex.SqlState == "23503") 
        {
            return "ПОМИЛКА: Неможливо оновити. Лікарня з ID = " + doctor.HospitalId + " не існує.";
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
            await using var conn = await GetConnectionAsync();
            var sql = "UPDATE hospital SET name = @name, address = @address WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("name", hospital.Name);
            cmd.Parameters.AddWithValue("address", hospital.Address);
            cmd.Parameters.AddWithValue("id", hospital.Id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0 ? "Дані лікарні оновлено." : "ПОМИЛКА: Лікарню не знайдено.";
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
            await using var conn = await GetConnectionAsync();
            var sql = "UPDATE appointment SET data = @data, hour = @hour, diagnosis = @diag, " +
                      "patient_id = @p_id, doctor_id = @d_id WHERE id = @id";
            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("data", app.Data);
            cmd.Parameters.AddWithValue("hour", app.Hour);
            cmd.Parameters.AddWithValue("diag", app.Diagnosis);
            cmd.Parameters.AddWithValue("p_id", app.PatientId);
            cmd.Parameters.AddWithValue("d_id", app.DoctorId);
            cmd.Parameters.AddWithValue("id", app.Id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0 ? "Дані прийому оновлено." : "ПОМИЛКА: Прийом не знайдено.";
        }
        catch (PostgresException ex) when (ex.SqlState == "23503") 
        {
            return "ПОМИЛКА: Неможливо оновити. Переконайтеся, що пацієнт та лікар існують.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: {ex.Message}";
        }
    }

    public async Task<Patient> GetPatientByIdAsync(int id)
    {
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, name, surname, \"day of birth\", phone FROM patient WHERE id = @id";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Patient
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString(reader.GetOrdinal("name")),
                Surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? "" : reader.GetString(reader.GetOrdinal("surname")),
                DayOfBirth = reader.IsDBNull(reader.GetOrdinal("day of birth")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("day of birth"))),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString(reader.GetOrdinal("phone"))
            };
        }
        return null; 
    }

    public async Task<Doctor> GetDoctorByIdAsync(int id)
    {
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, name, surname, specialization, hospital_id FROM doctor WHERE id = @id";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Doctor
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString(reader.GetOrdinal("name")),
                Surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? "" : reader.GetString(reader.GetOrdinal("surname")),
                Specialization = reader.IsDBNull(reader.GetOrdinal("specialization")) ? "" : reader.GetString(reader.GetOrdinal("specialization")),
                HospitalId = reader.GetInt32(reader.GetOrdinal("hospital_id"))
            };
        }
        return null;
    }

    public async Task<Hospital> GetHospitalByIdAsync(int id)
    {
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, name, address FROM hospital WHERE id = @id";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Hospital
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString(reader.GetOrdinal("name")),
                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address"))
            };
        }
        return null;
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id)
    {
        await using var conn = await GetConnectionAsync();
        var sql = "SELECT id, data, hour, diagnosis, patient_id, doctor_id FROM appointment WHERE id = @id";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Appointment
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Data = reader.IsDBNull(reader.GetOrdinal("data")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data"))),
                Hour = reader.IsDBNull(reader.GetOrdinal("hour")) ? TimeOnly.MinValue : TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("hour"))),
                Diagnosis = reader.IsDBNull(reader.GetOrdinal("diagnosis")) ? "" : reader.GetString(reader.GetOrdinal("diagnosis")),
                PatientId = reader.GetInt32(reader.GetOrdinal("patient_id")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id"))
            };
        }
        return null;
    }

    public async Task<string> GenerateRandomDataAsync(int count)
    {
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
            await using var conn = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.CommandTimeout = 300; // 5
            var sw = Stopwatch.StartNew();
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
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
            await using var conn = await GetConnectionAsync();

            var sql = @"
            TRUNCATE hospital RESTART IDENTITY CASCADE;
            TRUNCATE patient RESTART IDENTITY CASCADE;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();

            return "Успіх! Усі дані з таблиць видалено, лічильники ID скинуто до 1.";
        }
        catch (Exception ex)
        {
            return $"ПОМИЛКА: Не вдалося очистити базу даних. {ex.Message}";
        }
    }
    public async Task<(List<string> results, long timeMs)> SearchPatientsByDoctorAndDateAsync(
        string doctorSurnamePattern, DateOnly startDate, DateOnly endDate)
    {
        var results = new List<string>();
        var sw = Stopwatch.StartNew();

        var sql = $@"
        SELECT 
            p.name, p.surname, p.phone,
            d.surname AS doctor_surname,
            COUNT(a.id) AS appointment_count 
        FROM patient p
        JOIN appointment a ON p.id = a.patient_id
        JOIN doctor d ON a.doctor_id = d.id
        WHERE 
            d.surname ILIKE @surnamePattern AND
            a.data BETWEEN @startDate AND @endDate
        GROUP BY 
            p.id, p.name, p.surname, p.phone, d.surname 
        ORDER BY 
            appointment_count DESC;
    ";

        await using var conn = await GetConnectionAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("surnamePattern", $"%{doctorSurnamePattern}%");
        cmd.Parameters.AddWithValue("startDate", startDate);
        cmd.Parameters.AddWithValue("endDate", endDate);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(
                $"Пацієнт: {reader["name"]} {reader["surname"]} (Тел: {reader["phone"]}), " +
                $"Лікар: {reader["doctor_surname"]}, " +
                $"Кількість прийомів: {reader["appointment_count"]}"
            );
        }

        sw.Stop();
        return (results, sw.ElapsedMilliseconds);
    }

    public async Task<(List<string> results, long timeMs)> SearchDoctorStatisticsAsync(
        string specializationPattern, DateOnly startDate, DateOnly endDate)
    {
        var results = new List<string>();
        var sw = Stopwatch.StartNew(); 

        var sql = $@"
        SELECT 
            d.name, 
            d.surname, 
            d.specialization,
            COUNT(a.id) AS appointment_count 
        FROM doctor d
        LEFT JOIN appointment a ON d.id = a.doctor_id
        WHERE 
            d.specialization ILIKE @specPattern AND 
            a.data BETWEEN @startDate AND @endDate 
        GROUP BY 
            d.id, d.name, d.surname, d.specialization 
        ORDER BY 
            appointment_count DESC;
    ";

        await using var conn = await GetConnectionAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("specPattern", $"%{specializationPattern}%");
        cmd.Parameters.AddWithValue("startDate", startDate);
        cmd.Parameters.AddWithValue("endDate", endDate);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(
                $"Лікар: {reader["name"]} {reader["surname"]} ({reader["specialization"]}), " +
                $"Кількість прийомів: {reader["appointment_count"]}"
            );
        }

        sw.Stop();
        return (results, sw.ElapsedMilliseconds); 
    }
    public async Task<(List<string> results, long timeMs)> SearchHospitalStatisticsAsync(
        string addressPattern, string diagnosisPattern)
    {
        var results = new List<string>();
        var sw = Stopwatch.StartNew(); 

        var sql = $@"
        SELECT 
            h.name, 
            h.address,
            COUNT(DISTINCT a.patient_id) AS patient_count 
        FROM hospital h
        JOIN doctor d ON h.id = d.hospital_id
        JOIN appointment a ON d.id = a.doctor_id
        WHERE 
            h.address ILIKE @addressPattern AND 
            a.diagnosis ILIKE @diagnosisPattern 
        GROUP BY 
            h.id, h.name, h.address 
        ORDER BY 
            patient_count DESC;
    ";

        await using var conn = await GetConnectionAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("addressPattern", $"%{addressPattern}%");
        cmd.Parameters.AddWithValue("diagnosisPattern", $"%{diagnosisPattern}%");

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(
                $"Лікарня: {reader["name"]} ({reader["address"]}), " +
                $"Кількість унікальних пацієнтів: {reader["patient_count"]}"
            );
        }

        sw.Stop();
        return (results, sw.ElapsedMilliseconds); 
    }
}