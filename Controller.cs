using System;
using System.Threading.Tasks;

public class MainController
{
    private readonly DatabaseModel _model;
    private readonly ConsoleView _view;

    public MainController(DatabaseModel model, ConsoleView view)
    {
        _model = model;
        _view = view;
    }

    public async Task RunAsync()
    {
        bool running = true;
        while (running)
        {
            _view.ShowMainMenu();
            string choice = _view.GetInput("");
            string nextChoice;

            try
            {
                switch (choice)
                {
                    case "1":
                        _view.ShowAddMenu();
                        nextChoice = _view.GetInput("");
                        try
                        {
                            switch (nextChoice)
                            {
                                case "1":
                                    await AddPatientAsync();
                                    break;
                                case "2":
                                    await AddDoctorAsync();
                                    break;
                                case "3":
                                    await AddAppointmentAsync();
                                    break;
                                case "4":
                                    await AddHospitalAsync();
                                    break;
                                case "0":
                                    break;
                                default:
                                    _view.ShowMessage("Невідома команда.", true);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _view.ShowMessage($"Критична помилка: {ex.Message}", true);
                        }
                        break;
                    case "2":
                        _view.ShowDeleteMenu();
                        nextChoice = _view.GetInput("");
                        try
                        {
                            switch (nextChoice)
                            {
                                case "1":
                                    await DeletePatientAsync();
                                    break;
                                case "2":
                                    await DeleteDoctorAsync();
                                    break;
                                case "3":
                                    await DeleteAppointmentAsync();
                                    break;
                                case "4":
                                    await DeleteHospitalAsync();
                                    break;
                                case "0":
                                    break;
                                default:
                                    _view.ShowMessage("Невідома команда.", true);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _view.ShowMessage($"Критична помилка: {ex.Message}", true);
                        }
                        break;
                    case "3":
                        _view.ShowShowMenu();
                        nextChoice = _view.GetInput("");
                        try
                        {
                            switch (nextChoice)
                            {
                                case "1":
                                    await ShowPatientAsync();
                                    break;
                                case "2":
                                    await ShowDoctorAsync();
                                    break;
                                case "3":
                                    await ShowAppointmentAsync();
                                    break;
                                case "4":
                                    await ShowHospitalAsync();
                                    break;
                                case "0":
                                    break;
                                default:
                                    _view.ShowMessage("Невідома команда.", true);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _view.ShowMessage($"Критична помилка: {ex.Message}", true);
                        }
                        break;
                    case "4":
                        _view.ShowEditMenu();
                        nextChoice = _view.GetInput("");
                        try
                        {
                            switch (nextChoice)
                            {
                                case "1":
                                    await EditPatientAsync();
                                    break;
                                case "2":
                                    await EditDoctorAsync();
                                    break;
                                case "3":
                                    await EditAppointmentAsync();
                                    break;
                                case "4":
                                    await EditHospitalAsync();
                                    break;
                                case "0":
                                    break;
                                default:
                                    _view.ShowMessage("Невідома команда.", true);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _view.ShowMessage($"Критична помилка: {ex.Message}", true);
                        }
                        break;
                    case "5":
                        await GenerateDataAsync();
                        break;
                    case "6":
                        await ClearDataAsync();
                        break;
                    case "7":
                        await SearchPatientsAsync();
                        break;
                    case "8":
                        await SearchDoctorStatisticsAsync();
                        break;
                    case "9":
                        await SearchHospitalStatisticsAsync();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        _view.ShowMessage("Невідома команда.", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Критична помилка: {ex.Message}", true);
            }
        }
    }

        
    private async Task AddPatientAsync()
    {
        _view.ShowMessage("--- Додавання нового пацієнта ---", false);
        var patient = new Patient
        {
            Name = _view.GetInput("Ім'я"),
            Surname = _view.GetInput("Прізвище"),
            DayOfBirth = _view.GetDateInput("Дата народження (РРРР-ММ-ДД)"),
            Phone = _view.GetInput("Телефон (10 цифр)")
        };

        await _model.AddPatientAsync(patient);
        _view.ShowMessage("Пацієнта успішно додано.", false);
    }

    private async Task AddDoctorAsync()
    {
        _view.ShowMessage("--- Додавання нового лікаря ---", false);
        var doctor = new Doctor
        {
            Name = _view.GetInput("Ім'я"),
            Surname = _view.GetInput("Прізвище"),
            Specialization = _view.GetInput("Спеціалізація"),
            HospitalId = _view.GetIntInput("ID Лікарні, до якої він належить")
        };
        string result = await _model.AddDoctorAsync(doctor);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task AddAppointmentAsync()
    {
        _view.ShowMessage("--- Додавання нового прийому ---", false);
        var app = new Appointment
        {
            Data = _view.GetDateInput("Дата (РРРР-ММ-ДД)"),
            Hour = TimeOnly.Parse(_view.GetInput("Час (ГГ:ХХ)")),
            Diagnosis = _view.GetInput("Діагноз"),
            PatientId = _view.GetIntInput("ID Пацієнта"),
            DoctorId = _view.GetIntInput("ID Лікаря")
        };
        string result = await _model.AddAppointmentAsync(app);

        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task AddHospitalAsync()
    {
        _view.ShowMessage("--- Додавання нової лікарні ---", false);
        var hospital = new Hospital
        {
            Name = _view.GetInput("Назва лікарні"),
            Address = _view.GetInput("Адреса лікарні")
        };
        string result = await _model.AddHospitalAsync(hospital);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task DeletePatientAsync()
    {
        _view.ShowMessage("--- Видалення пацієнта ---", false);
        int id = _view.GetIntInput("Введіть ID пацієнта для видалення");
        string result = await _model.DeletePatientAsync(id);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task DeleteDoctorAsync()
    {
        _view.ShowMessage("--- Видалення лікаря ---", false);
        int id = _view.GetIntInput("Введіть ID лікаря для видалення");
        string result = await _model.DeleteDoctorAsync(id);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task DeleteAppointmentAsync()
    {
        _view.ShowMessage("--- Видалення (скасування) прийому ---", false);
        int id = _view.GetIntInput("Введіть ID прийому для видалення");
        string result = await _model.DeleteAppointmentAsync(id);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task DeleteHospitalAsync()
    {
        _view.ShowMessage("--- Видалення лікарні ---", false);
        int id = _view.GetIntInput("Введіть ID лікарні для видалення");
        string result = await _model.DeleteHospitalAsync(id);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task ShowPatientAsync()
    {
        var patients = await _model.GetAllPatientsAsync();
        _view.ShowList("--- Список Пацієнтів ---", patients);
    }

    private async Task ShowDoctorAsync()
    {
        var doctors = await _model.GetAllDoctorsAsync();
        _view.ShowList("--- Список Лікарів ---", doctors);
    }

    private async Task ShowHospitalAsync()
    {
        var hospitals = await _model.GetAllHospitalsAsync();
        _view.ShowList("--- Список Лікарень ---", hospitals);
    }

    private async Task ShowAppointmentAsync()
    {
        var appointments = await _model.GetAllAppointmentsAsync();
        _view.ShowList("--- Список Прийомів ---", appointments);
    }

    private async Task EditPatientAsync()
    {
        _view.ShowMessage("--- Редагування пацієнта ---", false);
        int id = _view.GetIntInput("Введіть ID пацієнта для редагування");

        var patient = await _model.GetPatientByIdAsync(id);
        if (patient == null)
        {
            _view.ShowMessage("ПОМИЛКА: Пацієнта з таким ID не знайдено.", true);
            return;
        }

        _view.ShowMessage($"Редагування пацієнта: {patient.Name} {patient.Surname}", false);

        patient.Name = _view.GetInputWithDefault("Ім'я", patient.Name);
        patient.Surname = _view.GetInputWithDefault("Прізвище", patient.Surname);

        string newDobStr = _view.GetInputWithDefault("Дата народження (РРРР-ММ-ДД)", patient.DayOfBirth.ToString("yyyy-MM-dd"));
        patient.DayOfBirth = DateOnly.Parse(newDobStr);

        patient.Phone = _view.GetInputWithDefault("Телефон", patient.Phone);

        string result = await _model.UpdatePatientAsync(patient);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task EditDoctorAsync()
    {
        _view.ShowMessage("--- Редагування лікаря ---", false);
        int id = _view.GetIntInput("Введіть ID лікаря для редагування");

        var doctor = await _model.GetDoctorByIdAsync(id);
        if (doctor == null)
        {
            _view.ShowMessage("ПОМИЛКА: Лікаря з таким ID не знайдено.", true);
            return;
        }

        _view.ShowMessage($"Редагування лікаря: {doctor.Name} {doctor.Surname}", false);

        doctor.Name = _view.GetInputWithDefault("Ім'я", doctor.Name);
        doctor.Surname = _view.GetInputWithDefault("Прізвище", doctor.Surname);
        doctor.Specialization = _view.GetInputWithDefault("Спеціалізація", doctor.Specialization);

        string newHospIdStr = _view.GetInputWithDefault("ID Лікарні", doctor.HospitalId.ToString());
        doctor.HospitalId = int.Parse(newHospIdStr); 

        string result = await _model.UpdateDoctorAsync(doctor);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task EditHospitalAsync()
    {
        _view.ShowMessage("--- Редагування лікарні ---", false);
        int id = _view.GetIntInput("Введіть ID лікарні для редагування");

        var hospital = await _model.GetHospitalByIdAsync(id);
        if (hospital == null)
        {
            _view.ShowMessage("ПОМИЛКА: Лікарню з таким ID не знайдено.", true);
            return;
        }

        _view.ShowMessage($"Редагування лікарні: {hospital.Name}", false);

        hospital.Name = _view.GetInputWithDefault("Назва", hospital.Name);
        hospital.Address = _view.GetInputWithDefault("Адреса", hospital.Address);


        string result = await _model.UpdateHospitalAsync(hospital);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task EditAppointmentAsync()
    {
        _view.ShowMessage("--- Редагування прийому ---", false);
        int id = _view.GetIntInput("Введіть ID прийому для редагування");

        var app = await _model.GetAppointmentByIdAsync(id);
        if (app == null)
        {
            _view.ShowMessage("ПОМИЛКА: Прийом з таким ID не знайдено.", true);
            return;
        }

        _view.ShowMessage($"Редагування прийому ID: {app.Id}", false);

        string newDataStr = _view.GetInputWithDefault("Дата (РРРР-ММ-ДД)", app.Data.ToString("yyyy-MM-dd"));
        app.Data = DateOnly.Parse(newDataStr);

        string newTimeStr = _view.GetInputWithDefault("Час (ГГ:ХХ)", app.Hour.ToString("HH:mm"));
        app.Hour = TimeOnly.Parse(newTimeStr);

        app.Diagnosis = _view.GetInputWithDefault("Діагноз", app.Diagnosis);

        string newPatIdStr = _view.GetInputWithDefault("ID Пацієнта", app.PatientId.ToString());
        app.PatientId = int.Parse(newPatIdStr);

        string newDocIdStr = _view.GetInputWithDefault("ID Лікаря", app.DoctorId.ToString());
        app.DoctorId = int.Parse(newDocIdStr);

        string result = await _model.UpdateAppointmentAsync(app);
        _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
    }

    private async Task GenerateDataAsync()
    {
        _view.ShowMessage("--- Генерація даних ---", false);
        int count = _view.GetIntInput("Скільки пацієнтів/прийомів згенерувати?");
        _view.ShowMessage("Запускаю генерацію... Це може зайняти час.", false);
        string result = await _model.GenerateRandomDataAsync(count);
        _view.ShowMessage(result, result.StartsWith("Помилка"));
    }
    private async Task ClearDataAsync()
    {
        _view.ShowMessage("--- ПОВНЕ ОЧИЩЕННЯ БАЗИ ДАНИХ ---", true); 

        Console.WriteLine("ПОПЕРЕДЖЕННЯ: Ця дія НЕОБОРОТНО видалить ВСІ дані з таблиць");
        Console.WriteLine("patient, doctor, hospital, and appointment.");
        Console.WriteLine("Лічильники ID будуть скинуті до 1.");

        string confirmation = _view.GetInput("Для підтвердження, будь ласка, введіть 'ТАК' (великими літерами)");

        if (confirmation == "ТАК")
        {
            _view.ShowMessage("Виконую очищення...", false);
            string result = await _model.ClearAllDataAsync();
            _view.ShowMessage(result, result.StartsWith("ПОМИЛКА"));
        }
        else
        {
            _view.ShowMessage("Очищення скасовано.", false);
        }
    }

    private async Task SearchPatientsAsync()
    {
        _view.ShowMessage("--- Пошук пацієнтів ---", false);
        string surnamePattern = _view.GetInput("Введіть прізвище лікаря (або частину)").Trim();
        DateOnly startDate = _view.GetDateInput("Початкова дата (РРРР-ММ-ДД)");
        DateOnly endDate = _view.GetDateInput("Кінцева дата (РРРР-ММ-ДД)");

        var (results, timeMs) = await _model.SearchPatientsByDoctorAndDateAsync(surnamePattern, startDate, endDate);

        _view.ShowSearchResults("--- Пошук пацієнтів ---", results, timeMs);
        _view.ShowMessage("Пошук завершено.", false);
    }
    private async Task SearchDoctorStatisticsAsync()
    {
        _view.ShowMessage("--- Пошук: Статистика прийомів по лікарях ---", false);

        string specPattern = _view.GetInput("Введіть спеціалізацію (або частину, напр. 'хірург')").Trim();
        DateOnly startDate = _view.GetDateInput("Початкова дата (РРРР-ММ-ДД)");
        DateOnly endDate = _view.GetDateInput("Кінцева дата (РРРР-ММ-ДД)");

        var (results, timeMs) = await _model.SearchDoctorStatisticsAsync(specPattern, startDate, endDate);

        _view.ShowSearchResults("--- Статистика по лікарях ---", results, timeMs);
    }
    private async Task SearchHospitalStatisticsAsync()
    {
        _view.ShowMessage("--- Пошук: Статистика по лікарнях ---", false);

        string addressPattern = _view.GetInput("Введіть адресу лікарні (або частину, напр. 'Шевченка')").Trim();
        string diagnosisPattern = _view.GetInput("Введіть діагноз (або частину, напр. 'ГРВІ')").Trim();

        var (results, timeMs) = await _model.SearchHospitalStatisticsAsync(addressPattern, diagnosisPattern);

        _view.ShowSearchResults("--- Статистика по лікарнях ---", results, timeMs);
    }
}