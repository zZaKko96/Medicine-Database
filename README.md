# Медична система (Бази Даних)

**Дисципліна:** «Бази Даних та засоби управління»
**Виконав:** студент групи КВ-32, Косарук Захар  
**Лабораторна робота №2:** «Засоби оптимізації роботи СУБД PostgreSQL»

## 1. Про проєкт

Цей проєкт є консольним додатком на C# (.NET), який демонструє роботу з базою даних PostgreSQL. Проєкт еволюціонував від використання "чистого" SQL до повноцінної ORM.

Додаток розроблено за архітектурним шаблоном **MVC (Model-View-Controller)**:
* **Model:** Реалізована через **Entity Framework Core**. Відповідає за взаємодію з БД через класи-сутності (ORM), а також містить методи для генерації масових даних.
* **View:** Відповідає за відображення інформації в консолі (меню, списки, повідомлення).
* **Controller:** Керує потоком програми, обробляє ввід користувача.

## 2. Функціонал (Лабораторна №2)

У цій версії додано засоби оптимізації та контролю цілісності:

1.  **ORM (Object-Relational Mapping):** * Перехід на **Entity Framework Core 8**.
    * Реалізація CRUD-операцій через об'єкти C# замість SQL-рядків.
2.  **Індекси (Optimization):**
    * Досліджено та впроваджено індекси типів **Hash** (для точного пошуку) та **GIN** (для текстового пошуку `ILIKE` та агрегації).
3.  **Тригери (Automation & Security):**
    * Реалізовано тригер `BEFORE UPDATE/DELETE`.
    * **Аудит:** Автоматичне збереження старої версії запису в таблицю `patient_log` перед зміною.
    * **Бізнес-логіка:** Заборона видалення записів у певні дні тижня.
4.  **Транзакції:**
    * Демонстрація рівнів ізоляції: `READ COMMITTED`, `REPEATABLE READ`, `SERIALIZABLE`.

## 3. Структура Бази Даних

**Тема:** «Медична система для збереження даних пацієнтів»

### Опис таблиць:

* `public.patient` (Основна таблиця пацієнтів)
    * `id` (PK), `name`, `surname`, `day of birth`, `phone`
* `public.hospital` (Лікарні)
    * `id` (PK), `name`, `address`
* `public.doctor` (Лікарі)
    * `id` (PK), `name`, `surname`, `specialization`
    * `hospital_id` (FK -> hospital.id)
* `public.appointment` (Прийоми - зв'язуюча таблиця)
    * `id` (PK), `data`, `hour`, `diagnosis`
    * `patient_id` (FK -> patient.id)
    * `doctor_id` (FK -> doctor.id)
* **`public.patient_log`** (Нова таблиця для аудиту)
    * `log_id` (PK), `operation_type`, `operation_time`, `user_name`
    * Зберігає копію полів пацієнта (`old_name`, `old_phone` тощо) перед зміною.

### Діаграми
`![ER-діаграма](db_scheme.png)`  

## 4. Технології

* **Мова:** C# (.NET 8)
* **СУБД:** PostgreSQL 16/17
* **ORM:** Entity Framework Core (Microsoft.EntityFrameworkCore)
* **Провайдер БД:** Npgsql.EntityFrameworkCore.PostgreSQL
* **IDE:** Visual Studio 2022 / pgAdmin 4
