# Руководство по миграции базы данных

## Текущие миграции

### RemoveDateClosedFromML
Удаляет атрибут `ДатаЗакрытия` из таблицы `МЛ`, так как дата закрытия теперь определяется датой окончания финальной операции.

## Применение миграций

### Вариант 1: Через командную строку

```bash
# Применить все миграции
dotnet ef database update

# Применить конкретную миграцию
dotnet ef database update RemoveDateClosedFromML

# Откатить миграцию
dotnet ef database update <предыдущая_миграция>

# Откатить все миграции
dotnet ef database update 0
```

### Вариант 2: Через Package Manager Console (Visual Studio)

```powershell
# Применить все миграции
Update-Database

# Применить конкретную миграцию
Update-Database -Migration RemoveDateClosedFromML

# Откатить миграцию
Update-Database -Migration <предыдущая_миграция>

# Откатить все миграции
Update-Database -Migration 0
```

## Создание новой миграции

### Через командную строку

```bash
dotnet ef migrations add <НазваниеМиграции>
```

### Через Package Manager Console

```powershell
Add-Migration <НазваниеМиграции>
```

## Проверка статуса миграций

```bash
# Список всех миграций
dotnet ef migrations list

# Проверка применённых миграций
dotnet ef migrations has-pending-model-changes
```

## Важные замечания

1. **Резервное копирование**: Всегда создавайте резервную копию базы данных перед применением миграций в production.

2. **Тестирование**: Протестируйте миграции на тестовой базе данных перед применением в production.

3. **Строка подключения**: Убедитесь, что строка подключения в `appsettings.json` указывает на правильную базу данных.

4. **Права доступа**: Убедитесь, что у пользователя БД есть права на изменение схемы.

## Устранение проблем

### Ошибка: "Unable to connect to database"

Проверьте:
- SQL Server запущен
- Строка подключения корректна
- Пользователь имеет права доступа

### Ошибка: "Migration already applied"

Миграция уже применена. Проверьте таблицу `__EFMigrationsHistory` в базе данных.

### Ошибка: "Pending model changes"

Модель изменилась после создания миграции. Создайте новую миграцию:

```bash
dotnet ef migrations add <НовоеИмя>
```

## Откат изменений

Если миграция вызвала проблемы:

1. Откатите миграцию:
```bash
dotnet ef database update <предыдущая_миграция>
```

2. Удалите файл миграции:
```bash
dotnet ef migrations remove
```

3. Восстановите из резервной копии (если необходимо)

## Скрипты миграций

Для генерации SQL скриптов миграций:

```bash
# Скрипт для всех миграций
dotnet ef migrations script

# Скрипт для конкретной миграции
dotnet ef migrations script <от_миграции> <до_миграции>

# Идемпотентный скрипт (можно применять многократно)
dotnet ef migrations script --idempotent

# Сохранить в файл
dotnet ef migrations script -o migration.sql
```

## Production deployment

Для production рекомендуется:

1. Сгенерировать SQL скрипт:
```bash
dotnet ef migrations script --idempotent -o migration.sql
```

2. Проверить скрипт вручную

3. Применить через SQL Management Studio или sqlcmd

4. Проверить результат

## Контакты

При возникновении проблем с миграциями обратитесь к администратору базы данных.
