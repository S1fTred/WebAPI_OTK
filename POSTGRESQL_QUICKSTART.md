# PostgreSQL - Быстрый старт

## Краткая инструкция для устранения ошибок HTTP 500

Если при открытии раздела "Закрытие МЛ" возникают ошибки HTTP 500, это означает, что база данных не создана или недоступна. Следуйте этим шагам для быстрого решения проблемы.

## Шаг 1: Установка PostgreSQL

### Windows
1. Скачайте: https://www.postgresql.org/download/windows/
2. Запустите установщик
3. Запомните пароль для пользователя `postgres`

### Linux (Ubuntu/Debian)
```bash
sudo apt update && sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
```

### macOS
```bash
brew install postgresql@15
brew services start postgresql@15
```

## Шаг 2: Создание БД и загрузка данных

```bash
# Создать базу данных
psql -U postgres -c "CREATE DATABASE otk_db;"

# Перейти в директорию проекта
cd path/to/WebAPI_OTK

# Создать схему БД
psql -U postgres -d otk_db -f database/schema.sql

# Загрузить тестовые данные
psql -U postgres -d otk_db -f database/data.sql
```

## Шаг 3: Установка Npgsql пакета

```bash
cd WebAPI_OTK
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
```

## Шаг 4: Настройка подключения

Откройте `WebAPI_OTK/appsettings.json` и замените пароль:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=otk_db;Username=postgres;Password=ВАШ_ПАРОЛЬ"
  }
}
```

## Шаг 5: Обновление Program.cs

Откройте `WebAPI_OTK/Program.cs` и найдите строку с `UseSqlServer`:

```csharp
// Было:
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

// Стало:
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
```

Добавьте using в начало файла (если нет):
```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

## Шаг 6: Запуск приложения

```bash
cd WebAPI_OTK
dotnet run
```

Откройте браузер: http://localhost:5000

## Проверка

После запуска проверьте:
- ✅ Раздел "Закрытие МЛ" открывается без ошибок
- ✅ Справочники загружаются
- ✅ Нет ошибок HTTP 500 в консоли браузера

## Устранение проблем

### Ошибка: "password authentication failed"
Проверьте пароль в `appsettings.json`

### Ошибка: "database does not exist"
Выполните: `psql -U postgres -c "CREATE DATABASE otk_db;"`

### Ошибка: "could not connect to server"
Убедитесь, что PostgreSQL запущен:
- Windows: проверьте службу в Services
- Linux: `sudo systemctl status postgresql`
- Mac: `brew services list`

## Подробная документация

Для детальной информации см.:
- `database/SETUP_POSTGRESQL.md` - полная инструкция по настройке
- `README.md` - общая документация проекта
- `FIX_500_ERROR.md` - устранение ошибок HTTP 500
