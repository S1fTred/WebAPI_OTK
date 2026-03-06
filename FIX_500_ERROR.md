# Исправление ошибки HTTP 500 при загрузке справочников

## Проблема
При открытии раздела "Закрытие МЛ" возникает ошибка HTTP 500 при загрузке справочников (сотрудники и изделия).

## Причины
1. База данных не создана
2. Миграции не применены
3. SQL Server не запущен
4. Неверная строка подключения

## Решение

### Шаг 1: Проверка SQL Server

Проверьте, что SQL Server запущен:

```powershell
# Проверка статуса службы SQL Server
Get-Service -Name "MSSQL$SQLEXPRESS" | Select-Object Status, DisplayName

# Если служба остановлена, запустите её:
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### Шаг 2: Проверка строки подключения

Откройте `WebAPI_OTK/appsettings.json` и проверьте строку подключения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=USER-PC\\SQLEXPRESS;Database=OTK;Integrated Security=true;Encrypt=True;TrustServerCertificate=True;"
  }
}
```

Убедитесь, что:
- Имя сервера `USER-PC\\SQLEXPRESS` соответствует вашему серверу
- База данных `OTK` существует

### Шаг 3: Применение миграций

```bash
cd WebAPI_OTK
dotnet ef database update
```

Если миграции не найдены, создайте их:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Шаг 4: Проверка базы данных

Подключитесь к SQL Server и проверьте, что база данных создана:

```sql
-- Проверка существования базы данных
SELECT name FROM sys.databases WHERE name = 'OTK';

-- Проверка таблиц
USE OTK;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES;
```

### Шаг 5: Добавление тестовых данных

Если база пустая, выполните SQL скрипт:

```bash
# Из корня проекта
sqlcmd -S USER-PC\SQLEXPRESS -d OTK -i test-data.sql
```

Или через SQL Server Management Studio:
1. Откройте `test-data.sql`
2. Выполните скрипт

### Шаг 6: Запуск приложения

```bash
cd WebAPI_OTK
dotnet run
```

### Шаг 7: Проверка API

Откройте в браузере:
```
http://localhost:5000/test-api.html
```

Нажмите кнопки:
- "Test Employee API" - должен вернуть список сотрудников (может быть пустым)
- "Test Product API" - должен вернуть список изделий (может быть пустым)

Если возвращается пустой массив `[]` - это нормально, база просто пустая.
Если возвращается ошибка 500 - смотрите логи сервера в консоли.

## Что изменено в коде

### 1. Улучшена обработка ошибок в `ml-closure.js`

Теперь страница не падает, если не удалось загрузить справочники:

```javascript
async function loadReferenceData() {
    try {
        // Загрузка сотрудников
        try {
            employees = await employeeApi.getAll();
        } catch (error) {
            console.warn('Не удалось загрузить сотрудников:', error);
            employees = [];
        }
        
        // Загрузка изделий
        try {
            products = await productApi.getAll();
        } catch (error) {
            console.warn('Не удалось загрузить изделия:', error);
            products = [];
        }
        
        // Заполнение селектов
        populateProductSelect();
        populateEmployeeSelects();
        
        // Показываем предупреждение, если данных нет
        if (employees.length === 0 && products.length === 0) {
            showToast('База данных пуста. Добавьте данные через API или SQL.', 'warning');
        }
        
    } catch (error) {
        console.error('Критическая ошибка загрузки справочников:', error);
        showToast('Не удалось подключиться к серверу. Проверьте, что сервер запущен.', 'error');
    }
}
```

### 2. Улучшена обработка ошибок в `api.js`

Теперь не показываются множественные toast-уведомления:

```javascript
// Логируем ошибку, но не показываем toast автоматически
console.error('API Error:', errorMessage);
throw new Error(errorMessage);

// ...

// Проверка на ошибки сети - показываем toast только для критичных ошибок
if (error.name === 'TypeError' && error.message.includes('fetch')) {
    showToast('Ошибка подключения к серверу. Проверьте соединение.', 'error');
}
// Для остальных ошибок не показываем toast автоматически
```

## Проверка логов сервера

Если ошибка 500 продолжается, проверьте логи в консоли, где запущен `dotnet run`:

```
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (123ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [s].[ID], [s].[ФИО], ...
```

Ищите строки с `fail:` или `error:` - они покажут точную причину ошибки.

## Типичные ошибки и решения

### Ошибка: "Cannot open database 'OTK'"

**Решение**: База данных не создана. Выполните:
```bash
dotnet ef database update
```

### Ошибка: "A network-related or instance-specific error"

**Решение**: SQL Server не запущен. Запустите службу:
```powershell
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### Ошибка: "Login failed for user"

**Решение**: Проблема с аутентификацией. Проверьте строку подключения:
- Используйте `Integrated Security=true` для Windows Authentication
- Или укажите `User Id=sa;Password=yourpassword` для SQL Authentication

### Ошибка: "Invalid object name 'Сотрудник'"

**Решение**: Таблицы не созданы. Примените миграции:
```bash
dotnet ef database update
```

## Результат

После исправления:
- ✅ Страница "Закрытие МЛ" открывается без ошибок
- ✅ Если база пустая, показывается предупреждение (не ошибка)
- ✅ Если сервер недоступен, показывается понятное сообщение
- ✅ Не показываются множественные toast-уведомления
- ✅ Ошибки логируются в консоль для отладки

## Дополнительная помощь

Если проблема не решается:

1. Откройте Developer Tools (F12) → Console
2. Скопируйте все ошибки
3. Откройте консоль, где запущен `dotnet run`
4. Скопируйте логи сервера
5. Проверьте, что:
   - SQL Server запущен
   - База данных создана
   - Миграции применены
   - Строка подключения корректна
