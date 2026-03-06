# Настройка PostgreSQL для системы ОТК

## Обзор
Данная инструкция описывает процесс установки и настройки PostgreSQL для работы с системой ОТК.

## Требования
- PostgreSQL 12+ (рекомендуется 15+)
- .NET 8.0 SDK
- Доступ к командной строке (PowerShell/CMD для Windows, Terminal для Linux/Mac)

## Шаг 1: Установка PostgreSQL

### Windows
1. Скачайте установщик PostgreSQL с официального сайта: https://www.postgresql.org/download/windows/
2. Запустите установщик и следуйте инструкциям
3. Запомните пароль для пользователя `postgres`, который вы укажете при установке
4. Убедитесь, что установлены компоненты:
   - PostgreSQL Server
   - pgAdmin 4 (графический интерфейс)
   - Command Line Tools

### Linux (Ubuntu/Debian)
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

### macOS
```bash
brew install postgresql@15
brew services start postgresql@15
```

## Шаг 2: Создание базы данных и пользователя

### Вариант 1: Через psql (командная строка)

1. Подключитесь к PostgreSQL:
```bash
# Windows
psql -U postgres

# Linux/Mac
sudo -u postgres psql
```

2. Создайте базу данных:
```sql
CREATE DATABASE otk_db;
```

3. Создайте пользователя для приложения (опционально, для безопасности):
```sql
CREATE USER otk_user WITH PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE otk_db TO otk_user;
```

4. Подключитесь к созданной базе данных:
```sql
\c otk_db
```

5. Предоставьте права на схему public:
```sql
GRANT ALL ON SCHEMA public TO otk_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO otk_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO otk_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO otk_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO otk_user;
```

6. Выйдите из psql:
```sql
\q
```

### Вариант 2: Через pgAdmin 4 (графический интерфейс)

1. Запустите pgAdmin 4
2. Подключитесь к серверу PostgreSQL (localhost)
3. Правой кнопкой на "Databases" → "Create" → "Database"
4. Укажите имя: `otk_db`
5. Нажмите "Save"

## Шаг 3: Выполнение SQL скриптов

### Вариант 1: Через psql

1. Перейдите в директорию проекта:
```bash
cd path/to/WebAPI_OTK
```

2. Выполните скрипт создания схемы:
```bash
# Windows
psql -U postgres -d otk_db -f database/schema.sql

# Linux/Mac
sudo -u postgres psql -d otk_db -f database/schema.sql

# Если создали отдельного пользователя
psql -U otk_user -d otk_db -f database/schema.sql
```

3. Выполните скрипт загрузки тестовых данных:
```bash
# Windows
psql -U postgres -d otk_db -f database/data.sql

# Linux/Mac
sudo -u postgres psql -d otk_db -f database/data.sql

# Если создали отдельного пользователя
psql -U otk_user -d otk_db -f database/data.sql
```

### Вариант 2: Через pgAdmin 4

1. В pgAdmin 4 откройте Query Tool (правой кнопкой на базе данных `otk_db` → "Query Tool")
2. Откройте файл `database/schema.sql` (File → Open)
3. Нажмите кнопку "Execute" (F5)
4. Откройте файл `database/data.sql`
5. Нажмите кнопку "Execute" (F5)

## Шаг 4: Установка Npgsql для Entity Framework Core

1. Откройте терминал в директории проекта `WebAPI_OTK`
2. Установите пакет Npgsql:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
```

## Шаг 5: Настройка строки подключения

1. Откройте файл `WebAPI_OTK/appsettings.json`
2. Замените строку подключения SQL Server на PostgreSQL:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=otk_db;Username=postgres;Password=your_password"
  }
}
```

### Параметры строки подключения:
- `Host` - адрес сервера PostgreSQL (обычно `localhost`)
- `Port` - порт PostgreSQL (по умолчанию `5432`)
- `Database` - имя базы данных (`otk_db`)
- `Username` - имя пользователя (`postgres` или `otk_user`)
- `Password` - пароль пользователя

### Использование переменных окружения (рекомендуется для production)

Вместо хранения пароля в `appsettings.json`, используйте переменные окружения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=otk_db;Username=postgres;Password=${DB_PASSWORD}"
  }
}
```

Установите переменную окружения:
```bash
# Windows (PowerShell)
$env:DB_PASSWORD="your_password"

# Windows (CMD)
set DB_PASSWORD=your_password

# Linux/Mac
export DB_PASSWORD="your_password"
```

## Шаг 6: Обновление Program.cs

1. Откройте файл `WebAPI_OTK/Program.cs`
2. Найдите строку с `UseSqlServer` и замените на `UseNpgsql`:

```csharp
// Было:
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Стало:
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

3. Добавьте using директиву в начало файла (если её нет):
```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

## Шаг 7: Проверка подключения

1. Запустите приложение:
```bash
cd WebAPI_OTK
dotnet run
```

2. Откройте браузер и перейдите по адресу: http://localhost:5000
3. Попробуйте открыть раздел "Закрытие МЛ" - ошибок быть не должно
4. Проверьте загрузку справочников в других разделах

## Шаг 8: Проверка данных в базе

### Через psql:
```bash
psql -U postgres -d otk_db

# Проверка таблиц
\dt

# Проверка данных
SELECT COUNT(*) FROM "Сотрудник";
SELECT COUNT(*) FROM "МЛ";
SELECT COUNT(*) FROM "Операция_МЛ";

# Выход
\q
```

### Через pgAdmin 4:
1. Откройте pgAdmin 4
2. Разверните дерево: Servers → PostgreSQL → Databases → otk_db → Schemas → public → Tables
3. Правой кнопкой на таблице → "View/Edit Data" → "All Rows"

## Устранение проблем

### Ошибка: "password authentication failed"
- Проверьте правильность пароля в строке подключения
- Убедитесь, что пользователь существует: `\du` в psql

### Ошибка: "database does not exist"
- Убедитесь, что база данных создана: `\l` в psql
- Создайте базу данных: `CREATE DATABASE otk_db;`

### Ошибка: "could not connect to server"
- Убедитесь, что PostgreSQL запущен:
  - Windows: проверьте службу "postgresql-x64-15" в Services
  - Linux: `sudo systemctl status postgresql`
  - Mac: `brew services list`

### Ошибка: "permission denied for schema public"
- Выполните команды из Шага 2 для предоставления прав пользователю

### Ошибка: "relation does not exist"
- Убедитесь, что выполнен скрипт `schema.sql`
- Проверьте список таблиц: `\dt` в psql

## Дополнительные ресурсы

- Официальная документация PostgreSQL: https://www.postgresql.org/docs/
- Документация Npgsql: https://www.npgsql.org/doc/
- Entity Framework Core с PostgreSQL: https://www.npgsql.org/efcore/

## Миграция с SQL Server

Если вы ранее использовали SQL Server, см. файл `MIGRATION_GUIDE.md` для подробной инструкции по миграции.

## Резервное копирование

Для создания резервной копии базы данных:
```bash
pg_dump -U postgres -d otk_db -f backup.sql
```

Для восстановления из резервной копии:
```bash
psql -U postgres -d otk_db -f backup.sql
```
