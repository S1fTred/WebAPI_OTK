# Простой скрипт для проверки базы данных без sqlite3

$dbPath = "WebAPI_OTK/otk.db"

Write-Host "Проверка базы данных OTK" -ForegroundColor Cyan
Write-Host "=========================" -ForegroundColor Cyan
Write-Host ""

if (-not (Test-Path $dbPath)) {
    Write-Host "❌ База данных НЕ найдена: $dbPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "Действия:" -ForegroundColor Yellow
    Write-Host "1. Запустите сервер из Rider IDE" -ForegroundColor Yellow
    Write-Host "2. Дождитесь сообщения о запуске сервера" -ForegroundColor Yellow
    Write-Host "3. Остановите сервер" -ForegroundColor Yellow
    Write-Host "4. Запустите этот скрипт снова" -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ База данных найдена: $dbPath" -ForegroundColor Green

# Проверяем размер файла
$fileInfo = Get-Item $dbPath
$sizeKB = [math]::Round($fileInfo.Length / 1KB, 2)
$sizeMB = [math]::Round($fileInfo.Length / 1MB, 2)

Write-Host "  Размер: $sizeKB KB ($sizeMB MB)" -ForegroundColor Gray
Write-Host "  Создан: $($fileInfo.CreationTime)" -ForegroundColor Gray
Write-Host "  Изменен: $($fileInfo.LastWriteTime)" -ForegroundColor Gray
Write-Host ""

# Проверяем вспомогательные файлы SQLite
$shmPath = "WebAPI_OTK/otk.db-shm"
$walPath = "WebAPI_OTK/otk.db-wal"

if (Test-Path $shmPath) {
    Write-Host "✓ Файл otk.db-shm найден (shared memory)" -ForegroundColor Green
} else {
    Write-Host "  Файл otk.db-shm не найден (это нормально)" -ForegroundColor Gray
}

if (Test-Path $walPath) {
    Write-Host "✓ Файл otk.db-wal найден (write-ahead log)" -ForegroundColor Green
} else {
    Write-Host "  Файл otk.db-wal не найден (это нормально)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Проверка завершена!" -ForegroundColor Cyan
Write-Host ""
Write-Host "Для детальной проверки структуры таблиц:" -ForegroundColor Yellow
Write-Host "1. Установите DB Browser for SQLite: https://sqlitebrowser.org/" -ForegroundColor Yellow
Write-Host "2. Откройте файл WebAPI_OTK/otk.db" -ForegroundColor Yellow
Write-Host "3. Перейдите на вкладку 'Database Structure'" -ForegroundColor Yellow
Write-Host "4. Найдите таблицу 'Операция_МЛ' и проверьте наличие полей:" -ForegroundColor Yellow
Write-Host "   - ДатаВыдачи" -ForegroundColor Yellow
Write-Host "   - ДатаИсполнения" -ForegroundColor Yellow
Write-Host "   - ДатаЗакрытия" -ForegroundColor Yellow
