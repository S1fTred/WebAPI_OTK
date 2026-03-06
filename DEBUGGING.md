# Руководство по отладке ошибок

## Ошибка "Ошибка сервера" (HTTP 500) при загрузке справочников

### Описание
При открытии раздела "Закрытие МЛ" или других разделов возникает ошибка HTTP 500.

### Причины
1. База данных не создана
2. Миграции не применены
3. SQL Server не запущен
4. Неверная строка подключения
5. Таблицы не существуют

### Быстрое решение

**Шаг 1**: Проверьте SQL Server
```powershell
Get-Service -Name "MSSQL$SQLEXPRESS" | Select-Object Status
Start-Service -Name "MSSQL$SQLEXPRESS"  # Если остановлен
```

**Шаг 2**: Примените миграции
```bash
cd WebAPI_OTK
dotnet ef database update
```

**Шаг 3**: Запустите приложение
```bash
dotnet run
```

### Подробное решение

См. файл `FIX_500_ERROR.md` для детальных инструкций.

---

## Ошибка "Ошибка сервера" при загрузке справочников (УСТАРЕЛО)

### Шаг 1: Проверка запуска сервера

Убедитесь, что сервер запущен:

```bash
cd WebAPI_OTK
dotnet run
```

Сервер должен запуститься на `http://localhost:5000` или `https://localhost:5001`

### Шаг 2: Проверка подключения к базе данных

Проверьте строку подключения в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OTK_DB;Trusted_Connection=True;"
  }
}
```

Убедитесь, что SQL Server запущен:

```bash
# Для LocalDB
sqllocaldb start mssqllocaldb
sqllocaldb info mssqllocaldb
```

### Шаг 3: Применение миграций

Если база данных не создана или не обновлена:

```bash
cd WebAPI_OTK
dotnet ef database update
```

### Шаг 4: Тестирование API эндпоинтов

Откройте тестовую страницу в браузере:

```
http://localhost:5000/test-api.html
```

Нажмите на кнопки тестирования для каждого эндпоинта. Это покажет:
- Работает ли эндпоинт
- Какую ошибку возвращает сервер
- Есть ли данные в базе

### Шаг 5: Проверка логов сервера

Смотрите консоль, где запущен `dotnet run`. Там будут видны:
- HTTP запросы
- Ошибки базы данных
- Исключения в контроллерах

### Шаг 6: Проверка браузерной консоли

Откройте Developer Tools (F12) в браузере:
- Вкладка **Console** - ошибки JavaScript
- Вкладка **Network** - HTTP запросы и ответы

### Типичные проблемы и решения

#### Проблема: "Cannot connect to database"

**Решение:**
1. Проверьте, что SQL Server запущен
2. Проверьте строку подключения
3. Попробуйте пересоздать базу:
   ```bash
   dotnet ef database drop
   dotnet ef database update
   ```

#### Проблема: "Table does not exist"

**Решение:**
Примените миграции:
```bash
dotnet ef database update
```

#### Проблема: "No data returned" (пустые массивы)

**Решение:**
Это нормально, если база пустая. Добавьте тестовые данные через Swagger UI:
```
https://localhost:5001/swagger
```

Или через SQL Management Studio.

#### Проблема: "CORS error"

**Решение:**
Убедитесь, что вы открываете страницы через тот же origin, что и API:
- ✅ Правильно: `http://localhost:5000/pages/ml-closure.html`
- ❌ Неправильно: `file:///C:/path/to/ml-closure.html`

#### Проблема: "401 Unauthorized"

**Решение:**
Авторизация в проекте - заглушка. Просто войдите с любыми данными на главной странице.

### Шаг 7: Добавление тестовых данных

Если база пустая, добавьте тестовые данные через Swagger:

1. Откройте `https://localhost:5001/swagger`
2. Создайте изделие (POST /api/Product)
3. Создайте ДСЕ (POST /api/DCE)
4. Создайте сотрудника (если нужно)
5. Создайте МЛ и операции

### Полезные команды

```bash
# Просмотр логов Entity Framework
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run

# Проверка миграций
dotnet ef migrations list

# Создание новой миграции
dotnet ef migrations add <Name>

# Откат миграции
dotnet ef database update <PreviousMigration>

# Удаление базы данных
dotnet ef database drop

# Пересоздание базы
dotnet ef database drop
dotnet ef database update
```

### Контакты для поддержки

Если проблема не решается:
1. Проверьте логи сервера
2. Проверьте Network tab в браузере
3. Сделайте скриншот ошибки
4. Опишите шаги для воспроизведения
