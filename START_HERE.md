# 🚀 Начните здесь - Миграция на PostgreSQL

## 📋 Что случилось?

При открытии раздела "Закрытие МЛ" возникают ошибки HTTP 500, потому что база данных не создана или недоступна.

## ✅ Решение

Мигрировать на PostgreSQL - бесплатную, кроссплатформенную и надежную СУБД.

## 📚 Какой документ читать?

### Для нетерпеливых (10 минут)
👉 **`POSTGRESQL_QUICKSTART.md`** - 6 шагов для быстрого старта

### Для методичных (30 минут)
👉 **`MIGRATION_CHECKLIST.md`** - Подробный чек-лист с галочками

### Для дотошных (1 час)
👉 **`database/SETUP_POSTGRESQL.md`** - Полная инструкция со всеми деталями

### Для понимания общей картины
👉 **`MIGRATION_SUMMARY.md`** - Что сделано и что нужно сделать

## 🎯 Быстрый старт (6 шагов)

### 1️⃣ Установить PostgreSQL
```bash
# Windows: скачать с https://www.postgresql.org/download/windows/
# Linux: sudo apt install postgresql
# macOS: brew install postgresql@15
```

### 2️⃣ Создать БД и загрузить данные
```bash
psql -U postgres -c "CREATE DATABASE otk_db;"
cd path/to/WebAPI_OTK
psql -U postgres -d otk_db -f database/schema.sql
psql -U postgres -d otk_db -f database/data.sql
```

### 3️⃣ Установить Npgsql
```bash
cd WebAPI_OTK
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0
```

### 4️⃣ Обновить Program.cs
Заменить `UseSqlServer` на `UseNpgsql`
(см. `database/PROGRAM_CS_CHANGES.md`)

### 5️⃣ Настроить пароль
В `appsettings.json` заменить `your_password` на реальный

### 6️⃣ Запустить
```bash
dotnet run
```

Открыть: http://localhost:5000

## 📁 Структура документации

```
WebAPI_OTK/
├── START_HERE.md                    ← ВЫ ЗДЕСЬ
├── POSTGRESQL_QUICKSTART.md         ← Быстрый старт (10 мин)
├── MIGRATION_CHECKLIST.md           ← Чек-лист (30 мин)
├── MIGRATION_SUMMARY.md             ← Резюме
├── database/
│   ├── schema.sql                   ← SQL: создание схемы
│   ├── data.sql                     ← SQL: тестовые данные
│   ├── SETUP_POSTGRESQL.md          ← Подробная инструкция (1 час)
│   ├── PROGRAM_CS_CHANGES.md        ← Изменения в коде
│   └── README.md                    ← О базе данных
├── FIX_500_ERROR.md                 ← Устранение ошибок
├── DEBUGGING.md                     ← Отладка
└── README.md                        ← Общая документация

.kiro/specs/postgresql-migration/
├── requirements.md                  ← Спецификация
└── implementation.md                ← Детали реализации
```

## 🎁 Что уже готово

✅ SQL скрипты для PostgreSQL (schema.sql, data.sql)
✅ Тестовые данные (80+ записей)
✅ Подробная документация (5 файлов)
✅ Обновленная конфигурация (appsettings.json)
✅ Инструкции по изменению кода (Program.cs)
✅ Чек-лист для проверки
✅ Устранение типичных проблем

## ⏱️ Сколько времени займет?

- **Установка PostgreSQL**: 5-10 минут
- **Создание БД и загрузка данных**: 2-3 минуты
- **Установка пакета и изменение кода**: 3-5 минут
- **Настройка и запуск**: 2 минуты

**Итого: 15-20 минут**

## 🆘 Если что-то пошло не так

### Ошибка: "password authentication failed"
→ Проверьте пароль в `appsettings.json`

### Ошибка: "database does not exist"
→ Выполните: `psql -U postgres -c "CREATE DATABASE otk_db;"`

### Ошибка: "could not connect to server"
→ Убедитесь, что PostgreSQL запущен

### Другие ошибки
→ См. `database/SETUP_POSTGRESQL.md` раздел "Устранение проблем"

## 🎓 Что вы получите

После миграции:
- ✅ Система работает без ошибок HTTP 500
- ✅ Все модули функционируют корректно
- ✅ Справочники загружаются быстро
- ✅ Тестовые данные для демонстрации
- ✅ Кроссплатформенная СУБД
- ✅ Бесплатное решение

## 🚦 Следующий шаг

Выберите свой путь:

**Путь 1: Быстрый** (для опытных)
→ Откройте `POSTGRESQL_QUICKSTART.md`

**Путь 2: Методичный** (для внимательных)
→ Откройте `MIGRATION_CHECKLIST.md`

**Путь 3: Подробный** (для изучающих)
→ Откройте `database/SETUP_POSTGRESQL.md`

## 💡 Совет

Если вы впервые работаете с PostgreSQL, начните с `MIGRATION_CHECKLIST.md` - там есть галочки для отметки выполненных шагов.

## 📞 Поддержка

Все ответы на вопросы есть в документации:
- Установка → `database/SETUP_POSTGRESQL.md`
- Изменение кода → `database/PROGRAM_CS_CHANGES.md`
- Ошибки → `FIX_500_ERROR.md`
- Отладка → `DEBUGGING.md`

---

**Готовы начать? Откройте `POSTGRESQL_QUICKSTART.md` или `MIGRATION_CHECKLIST.md`** 🚀
