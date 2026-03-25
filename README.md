# WebAPI_OTK

Система учета ОТК на ASP.NET Core 8 с REST API, SQLite и встроенным статическим frontend на HTML/CSS/JavaScript.

## Что есть в проекте

- Закрытие маршрутных листов с контролем завершения операций
- Учет операций по МЛ и расчет данных по операции
- Управление премиальными коэффициентами
- Каталог ДСЕ и изделий
- Отчеты и экспорт в Excel/PDF
- Расчет зарплаты и ведомость выработки

## Текущий стек

- Backend: ASP.NET Core 8, Entity Framework Core, SQLite
- Frontend: Vanilla JS, HTML, CSS
- Экспорт: ClosedXML, QuestPDF

## Актуальная структура

```text
WebAPI_OTK/
├─ WebAPI_OTK/
│  ├─ Controllers/
│  │  ├─ Directory/     # справочники и каталоги
│  │  ├─ Production/    # МЛ, операции, премиальные коэффициенты
│  │  └─ Reporting/     # отчеты, зарплата, экспорт
│  ├─ DTOs/
│  │  ├─ Directory/
│  │  ├─ Production/
│  │  └─ Reporting/
│  ├─ Data/
│  │  ├─ OtkDbContext.cs
│  │  └─ DatabaseSeeder.cs
│  ├─ Models/
│  │  ├─ Directory/
│  │  ├─ Production/
│  │  ├─ Reporting/
│  │  └─ System/
│  ├─ Migrations/
│  ├─ wwwroot/
│  │  ├─ css/
│  │  ├─ js/
│  │  ├─ pages/
│  │  └─ dev/           # вспомогательные тестовые страницы
│  ├─ Program.cs
│  └─ otk.db
├─ database/            # SQL-артефакты и legacy-данные
├─ docs/reference/      # внешние материалы и исходные референсы
├─ scripts/dev/         # вспомогательные dev-скрипты
└─ WebAPI_OTK.sln
```

## Запуск

1. Установить .NET 8 SDK.
2. Из корня репозитория выполнить:

```powershell
dotnet build WebAPI_OTK.sln
dotnet run --project WebAPI_OTK
```

3. Открыть приложение по адресу из launch profile или `ASPNETCORE_URLS`.

## Важно про базу данных

- Рабочая база по умолчанию: `WebAPI_OTK/otk.db`
- При старте приложения вызывается `DatabaseSeeder`, который пересоздает базу и заново загружает тестовые данные
- SQLite сейчас является фактическим runtime-провайдером
- SQL-файлы в папке `database/` сохранены как справочные и legacy-артефакты, но не используются текущим runtime автоматически

## Основные маршруты frontend

- `/index.html`
- `/pages/ml-closure.html`
- `/pages/prem-koef.html`
- `/pages/dce-catalog.html`

Dev-страницы для ручной проверки:

- `/dev/test-api.html`
- `/dev/test-export.html`
- `/dev/test-i18n.html`

## Ключевые ограничения текущей версии

- Авторизация пока демонстрационная
- `DatabaseSeeder` не предназначен для production-сценария с постоянным хранением данных
- В расчете зарплаты и отчете по выработке часть логики остается упрощенной

## Что было нормализовано

- Backend разложен по предметным зонам: `Directory`, `Production`, `Reporting`
- `Model1` переименован в `OtkDbContext`
- Тестовые страницы вынесены из корня `wwwroot` в `wwwroot/dev`
- Вспомогательные скрипты и внешние материалы вынесены из корня в `scripts/dev` и `docs/reference`
- Избыточная документация сведена к одному актуальному `README.md`

## Проверка после изменений

Базовая проверка:

```powershell
dotnet build WebAPI_OTK.sln --no-restore
```
