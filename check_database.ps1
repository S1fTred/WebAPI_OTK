# Скрипт для проверки структуры базы данных SQLite

$dbPath = "WebAPI_OTK/otk.db"

if (-not (Test-Path $dbPath)) {
    Write-Host "❌ База данных не найдена: $dbPath" -ForegroundColor Red
    Write-Host "Запустите сервер из Rider IDE, чтобы создать базу данных." -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ База данных найдена: $dbPath" -ForegroundColor Green
Write-Host ""

# Проверяем структуру таблицы Операция_МЛ
Write-Host "Проверка структуры таблицы Операция_МЛ:" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$query = "PRAGMA table_info(Операция_МЛ);"
$result = sqlite3 $dbPath $query 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Ошибка при выполнении запроса. Убедитесь, что sqlite3 установлен." -ForegroundColor Red
    Write-Host "Установите sqlite3: winget install SQLite.SQLite" -ForegroundColor Yellow
    exit 1
}

Write-Host $result
Write-Host ""

# Проверяем наличие новых полей
$hasДатаВыдачи = $result -match "ДатаВыдачи"
$hasДатаИсполнения = $result -match "ДатаИсполнения"
$hasДатаЗакрытия = $result -match "ДатаЗакрытия"

Write-Host "Проверка новых полей:" -ForegroundColor Cyan
Write-Host "=====================" -ForegroundColor Cyan

if ($hasДатаВыдачи) {
    Write-Host "✓ ДатаВыдачи - найдено" -ForegroundColor Green
} else {
    Write-Host "❌ ДатаВыдачи - НЕ найдено" -ForegroundColor Red
}

if ($hasДатаИсполнения) {
    Write-Host "✓ ДатаИсполнения - найдено" -ForegroundColor Green
} else {
    Write-Host "❌ ДатаИсполнения - НЕ найдено" -ForegroundColor Red
}

if ($hasДатаЗакрытия) {
    Write-Host "✓ ДатаЗакрытия - найдено" -ForegroundColor Green
} else {
    Write-Host "❌ ДатаЗакрытия - НЕ найдено" -ForegroundColor Red
}

Write-Host ""

# Проверяем количество записей
Write-Host "Статистика данных:" -ForegroundColor Cyan
Write-Host "==================" -ForegroundColor Cyan

$countQuery = "SELECT COUNT(*) FROM Операция_МЛ;"
$count = sqlite3 $dbPath $countQuery

Write-Host "Всего операций в БД: $count" -ForegroundColor Yellow

# Проверяем операции с заполненными новыми полями
$filledQuery = "SELECT COUNT(*) FROM Операция_МЛ WHERE ДатаВыдачи IS NOT NULL;"
$filledCount = sqlite3 $dbPath $filledQuery

Write-Host "Операций с заполненной ДатаВыдачи: $filledCount" -ForegroundColor Yellow

# Проверяем количество МЛ
$mlQuery = "SELECT COUNT(*) FROM МЛ;"
$mlCount = sqlite3 $dbPath $mlQuery

Write-Host "Всего маршрутных листов: $mlCount" -ForegroundColor Yellow

# Проверяем количество сотрудников
$empQuery = "SELECT COUNT(*) FROM Сотрудник;"
$empCount = sqlite3 $dbPath $empQuery

Write-Host "Всего сотрудников: $empCount" -ForegroundColor Yellow

Write-Host ""

if ($hasДатаВыдачи -and $hasДатаИсполнения -and $hasДатаЗакрытия) {
    Write-Host "✓ Все проверки пройдены успешно!" -ForegroundColor Green
} else {
    Write-Host "❌ Обнаружены проблемы с структурой базы данных" -ForegroundColor Red
}
