using System;
using System.Collections.Generic;

public class ConsoleView
{
    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("--- МЕДИЧНА СИСТЕМА ---");
        Console.WriteLine("");
        Console.WriteLine("1. Додати");
        Console.WriteLine("2. Видалити");
        Console.WriteLine("3. Переглянути");
        Console.WriteLine("4. Редагувати");
        Console.WriteLine("");
        
        Console.WriteLine("5. Запустити генерацію 'рандомних' даних");
        Console.WriteLine("6. Очистити усю базу даних");
        Console.WriteLine("");
        Console.WriteLine("7. Пошук пацієнтів за прізвищем лікаря та датою");
        Console.WriteLine("8. Вивести статистику по лікарях");
        Console.WriteLine("9. Вивести статистику по лікарнях");
        Console.WriteLine("");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");
    }

    public void ShowAddMenu()
    {
        Console.Clear();
        Console.WriteLine("1. Додати пацієнта");
        Console.WriteLine("2. Додати лікаря");
        Console.WriteLine("3. Додати прийом");
        Console.WriteLine("4. Додати лікарню");
        Console.WriteLine("");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");
    }

    public void ShowDeleteMenu()
    {
        Console.Clear();
        Console.WriteLine("1. Видалити пацієнта");
        Console.WriteLine("2. Видалити лікаря");
        Console.WriteLine("3. Видалити прийом");
        Console.WriteLine("4. Видалити лікарню");
        Console.WriteLine("");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");
    }

    public void ShowShowMenu()
    {
        Console.Clear();
        Console.WriteLine("1. Переглянути таблицю Пацієнт");
        Console.WriteLine("2. Переглянути таблицю Лікар");
        Console.WriteLine("3. Переглянути таблицю Прийом");
        Console.WriteLine("4. Переглянути таблицю Лікарня");
        Console.WriteLine("");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");
    }

    public void ShowEditMenu()
    {
        Console.Clear();
        Console.WriteLine("1. Редагувати таблицю Пацієнт");
        Console.WriteLine("2. Редагувати таблицю Лікар");
        Console.WriteLine("3. Редагувати таблицю Прийом");
        Console.WriteLine("4. Редагувати таблицю Лікарня");
        Console.WriteLine("");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");
    }

    public void ShowMessage(string message, bool isError = false)
    {
        if (isError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        Console.WriteLine("Натисніть Enter для продовження...");
        Console.ReadLine();
    }

    public string GetInput(string prompt)
    {
        if (!string.IsNullOrEmpty(prompt))
        {
            Console.Write($"{prompt}: ");
        }
        return Console.ReadLine();
    }

    public int GetIntInput(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write($"{prompt}: ");
                return Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                ShowMessage("Будь ласка, введіть коректне число.", true);
            }
        }
    }

    public DateOnly GetDateInput(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write($"{prompt} (у форматі РРРР-ММ-ДД): ");
                return DateOnly.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                ShowMessage("Будь ласка, введіть коректну дату (РРРР-ММ-ДД).", true);
            }
        }
    }
    public void ShowSearchResults(string title, List<string> results, long timeMs)
    {
        Console.Clear();
        Console.WriteLine($"{title} (знайдено {results.Count} записів за {timeMs} мс)");
        Console.WriteLine(new string('-', title.Length));

        if (results.Count == 0)
        {
            Console.WriteLine("\nНічого не знайдено.");
        }
        else
        {
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        Console.WriteLine("\nНатисніть Enter для продовження...");
        Console.ReadLine();
    }

    public void ShowList<T>(string title, List<T> items)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine(new string('-', title.Length)); 

        if (items.Count == 0)
        {
            Console.WriteLine("\nСписок порожній.");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
        }

        Console.WriteLine("\nНатисніть Enter для продовження...");
        Console.ReadLine();
    }

    public string GetInputWithDefault(string prompt, string defaultValue)
    {
        Console.Write($"{prompt} (поточне: {defaultValue}): ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return defaultValue;
        }
        return input; 
    }
}

