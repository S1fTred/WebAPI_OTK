# Изменения в Program.cs для PostgreSQL

## Что нужно изменить

В файле `WebAPI_OTK/Program.cs` необходимо заменить провайдер базы данных с SQL Server на PostgreSQL.

## Шаг 1: Добавить using директиву

В начале файла `Program.cs` добавьте (если её нет):

```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

Полный список using директив должен выглядеть примерно так:

```csharp
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK;
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

## Шаг 2: Заменить UseSqlServer на UseNpgsql

Найдите строку с `UseSqlServer` (обычно в середине файла):

### Было:
```csharp
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Стало:
```csharp
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Полный пример Program.cs

Вот как может выглядеть ваш `Program.cs` после изменений:

```csharp
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Настройка Entity Framework с PostgreSQL
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка CORS (если требуется)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Раздача статических файлов
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Fallback для SPA
app.MapFallbackToFile("index.html");

app.Run();
```

## Проверка изменений

После внесения изменений:

1. Сохраните файл `Program.cs`
2. Убедитесь, что установлен пакет Npgsql:
   ```bash
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
   ```
3. Проверьте, что нет ошибок компиляции:
   ```bash
   dotnet build
   ```
4. Запустите приложение:
   ```bash
   dotnet run
   ```

## Возможные ошибки

### Ошибка: "The name 'UseNpgsql' does not exist"
**Решение**: Установите пакет Npgsql:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
```

### Ошибка: "The type or namespace name 'Npgsql' could not be found"
**Решение**: Добавьте using директиву в начало файла:
```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

### Ошибка при запуске: "Connection refused"
**Решение**: Убедитесь, что PostgreSQL запущен и строка подключения в `appsettings.json` правильная.

## Откат изменений (если нужно вернуться к SQL Server)

Если нужно вернуться к SQL Server:

1. Верните `UseSqlServer`:
   ```csharp
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
   ```

2. Измените строку подключения в `appsettings.json`:
   ```json
   "DefaultConnection": "Server=USER-PC\\SQLEXPRESS;Database=OTK;Integrated Security=true;Encrypt=True;TrustServerCertificate=True;"
   ```

## Дополнительные настройки (опционально)

### Логирование SQL запросов

Для отладки можно включить логирование SQL запросов:

```csharp
builder.Services.AddDbContext<OTKContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); // Только для разработки!
    options.LogTo(Console.WriteLine, LogLevel.Information);
});
```

### Настройка таймаута команд

Если запросы выполняются долго:

```csharp
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.CommandTimeout(60) // 60 секунд
    ));
```

### Настройка пула подключений

Для оптимизации производительности:

```csharp
builder.Services.AddDbContext<OTKContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => 
        {
            npgsqlOptions.CommandTimeout(30);
            npgsqlOptions.EnableRetryOnFailure(3); // 3 попытки при ошибке
        }
    ));
```

## Следующие шаги

После обновления `Program.cs`:
1. Запустите приложение: `dotnet run`
2. Откройте браузер: http://localhost:5000
3. Проверьте работу всех модулей
4. Убедитесь, что нет ошибок HTTP 500

Если возникнут проблемы, см. `database/SETUP_POSTGRESQL.md` раздел "Устранение проблем".
