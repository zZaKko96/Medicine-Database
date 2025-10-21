# Розрахунково-графічна робота з дисципліни «Бази Даних»

**Тема:** «Створення додатку бази даних, орієнтованого на взаємодію з СУБД PostgreSQL»

**Виконав:** студент групи КВ-32, Косарук Захар 
**GitHub:** `https://github.com/zZaKko96/Medicine-Database.git` 

## 1. Про проєкт

Цей проєкт є консольним додатком на C# (.NET), який реалізує CRUD-операції (Create, Read, Update, Delete), складні пошукові запити та генерацію даних для бази даних у PostgreSQL.

Додаток розроблено за архітектурним шаблоном **MVC (Model-View-Controller)**:
* **Model:** Відповідає за всю логіку взаємодії з базою даних (чисті SQL-запити через Npgsql).
* **View:** Відповідає за відображення інформації в консолі (меню, списки, повідомлення).
* **Controller:** Керує потоком програми, приймає ввід користувача та зв'язує Model і View.

## 2. Структура Бази Даних

**Тема БД:** «Медична система для збереження даних пацієнтів»

### Сутності та їх атрибути

* **Пацієнт** – сутність для запису даних пацієнтів (ім’я, прізвище, дата народження, контактний телефон).
* **Лікар** – сутність для запису даних лікарів (ім’я, прізвище, спеціалізація, лікарня).
* **Прийом** – сутність для запису існуючих прийомів між пацієнтом і лікарем (дата, час, діагноз, id пацієнта, id лікаря).
* **Лікарня** – сутність для запису даних про лікарні (назва, адреса).

### ER-діаграма (Сутність-Зв'язок)

`![ER-діаграма](db_scheme.png)`

### Реляційна схема (Фізична модель)

`![Реляційна схема](db_scheme_pgadmin4.png)`

**Опис таблиць:**

* `public.patient`
    * `id` (integer, PK, AI)
    * `name` (character varying(20))
    * `surname` (character varying(20))
    * `"day of birth"` (date)
    * `phone` (character varying(10))
* `public.hospital`
    * `id` (integer, PK, AI)
    * `name` (character varying(40))
    * `address` (character varying(100))
* `public.doctor`
    * `id` (integer, PK, AI)
    * `name` (character varying(20))
    * `surname` (character varying(20))
    * `specialization` (character varying(100))
    * `hospital_id` (integer, FK -> hospital.id)
* `public.appointment`
    * `id` (integer, PK, AI)
    * `data` (date)
    * `hour` (time without time zone)
    * `diagnosis` (character varying(50))
    * `patient_id` (integer, FK -> patient.id)
    * `doctor_id` (integer, FK -> doctor.id)


## 3. Технології

* **Мова:** C#
* **Платформа:** .NET 8
* **База даних:** PostgreSQL
* **Бібліотека для БД:** Npgsql