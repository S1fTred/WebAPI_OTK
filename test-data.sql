-- Скрипт для добавления тестовых данных в базу OTK_DB
-- Запустите этот скрипт через SQL Server Management Studio или sqlcmd

USE OTK_DB;
GO

-- Проверка существования данных
IF NOT EXISTS (SELECT 1 FROM Изделие)
BEGIN
    PRINT 'Добавление тестовых изделий...';
    
    INSERT INTO Изделие (Наименование, Описание, Состояние, ДатаСоздания)
    VALUES 
        (N'Изделие А-100', N'Тестовое изделие для демонстрации', N'В производстве', GETDATE()),
        (N'Изделие Б-200', N'Второе тестовое изделие', N'В производстве', GETDATE()),
        (N'Изделие В-300', N'Третье тестовое изделие', N'Проектирование', GETDATE());
    
    PRINT 'Изделия добавлены.';
END
ELSE
BEGIN
    PRINT 'Изделия уже существуют, пропускаем...';
END
GO

-- Добавление ДСЕ
IF NOT EXISTS (SELECT 1 FROM ДСЕ)
BEGIN
    PRINT 'Добавление тестовых ДСЕ...';
    
    DECLARE @ИзделиеID1 INT = (SELECT TOP 1 ID FROM Изделие ORDER BY ID);
    DECLARE @ИзделиеID2 INT = (SELECT ID FROM Изделие ORDER BY ID OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY);
    
    INSERT INTO ДСЕ (Код, Наименование, Чертеж, ИзделиеID)
    VALUES 
        (N'ДСЕ-001', N'Корпус основной', N'ЧЕР-001', @ИзделиеID1),
        (N'ДСЕ-002', N'Крышка верхняя', N'ЧЕР-002', @ИзделиеID1),
        (N'ДСЕ-003', N'Вал приводной', N'ЧЕР-003', @ИзделиеID1),
        (N'ДСЕ-004', N'Шестерня ведущая', N'ЧЕР-004', @ИзделиеID2),
        (N'ДСЕ-005', N'Подшипник опорный', N'ЧЕР-005', @ИзделиеID2);
    
    PRINT 'ДСЕ добавлены.';
END
ELSE
BEGIN
    PRINT 'ДСЕ уже существуют, пропускаем...';
END
GO

-- Добавление должностей
IF NOT EXISTS (SELECT 1 FROM Должность)
BEGIN
    PRINT 'Добавление должностей...';
    
    INSERT INTO Должность (Наименование, Описание)
    VALUES 
        (N'Токарь', N'Токарные работы'),
        (N'Фрезеровщик', N'Фрезерные работы'),
        (N'Слесарь', N'Слесарные работы'),
        (N'Контролер ОТК', N'Контроль качества'),
        (N'Мастер участка', N'Руководство участком');
    
    PRINT 'Должности добавлены.';
END
ELSE
BEGIN
    PRINT 'Должности уже существуют, пропускаем...';
END
GO

-- Добавление сотрудников
IF NOT EXISTS (SELECT 1 FROM Сотрудник)
BEGIN
    PRINT 'Добавление тестовых сотрудников...';
    
    DECLARE @ДолжностьТокарь INT = (SELECT ID FROM Должность WHERE Наименование = N'Токарь');
    DECLARE @ДолжностьФрезеровщик INT = (SELECT ID FROM Должность WHERE Наименование = N'Фрезеровщик');
    DECLARE @ДолжностьСлесарь INT = (SELECT ID FROM Должность WHERE Наименование = N'Слесарь');
    DECLARE @ДолжностьОТК INT = (SELECT ID FROM Должность WHERE Наименование = N'Контролер ОТК');
    
    INSERT INTO Сотрудник (ФИО, ТабельныйНомер, ДатаПриема, ДолжностьID)
    VALUES 
        (N'Иванов Иван Иванович', N'001', DATEADD(YEAR, -5, GETDATE()), @ДолжностьТокарь),
        (N'Петров Петр Петрович', N'002', DATEADD(YEAR, -3, GETDATE()), @ДолжностьФрезеровщик),
        (N'Сидоров Сидор Сидорович', N'003', DATEADD(YEAR, -2, GETDATE()), @ДолжностьСлесарь),
        (N'Смирнова Анна Сергеевна', N'004', DATEADD(YEAR, -4, GETDATE()), @ДолжностьОТК),
        (N'Козлов Дмитрий Александрович', N'005', DATEADD(YEAR, -1, GETDATE()), @ДолжностьТокарь);
    
    PRINT 'Сотрудники добавлены.';
END
ELSE
BEGIN
    PRINT 'Сотрудники уже существуют, пропускаем...';
END
GO

-- Добавление типов операций
IF NOT EXISTS (SELECT 1 FROM ТипОперации)
BEGIN
    PRINT 'Добавление типов операций...';
    
    INSERT INTO ТипОперации (Наименование, Описание, ДлительностьЧас, КодОперации)
    VALUES 
        (N'Токарная обработка', N'Обработка на токарном станке', 2.5, N'ТОК-01'),
        (N'Фрезерная обработка', N'Обработка на фрезерном станке', 3.0, N'ФРЕ-01'),
        (N'Сверление', N'Сверление отверстий', 1.0, N'СВЕ-01'),
        (N'Шлифование', N'Шлифовальные работы', 2.0, N'ШЛИ-01'),
        (N'Контроль ОТК', N'Контроль качества', 0.5, N'ОТК-01');
    
    PRINT 'Типы операций добавлены.';
END
ELSE
BEGIN
    PRINT 'Типы операций уже существуют, пропускаем...';
END
GO

-- Добавление маршрутных листов
IF NOT EXISTS (SELECT 1 FROM МЛ)
BEGIN
    PRINT 'Добавление тестовых МЛ...';
    
    DECLARE @ИзделиеID INT = (SELECT TOP 1 ID FROM Изделие ORDER BY ID);
    DECLARE @ДСЕID INT = (SELECT TOP 1 ID FROM ДСЕ ORDER BY ID);
    
    INSERT INTO МЛ (НомерМЛ, ДатаСоздания, Закрыт, ИзделиеID, ДСЕID)
    VALUES 
        (N'МЛ-2026-001', DATEADD(DAY, -10, GETDATE()), 0, @ИзделиеID, @ДСЕID),
        (N'МЛ-2026-002', DATEADD(DAY, -5, GETDATE()), 0, @ИзделиеID, @ДСЕID + 1),
        (N'МЛ-2026-003', DATEADD(DAY, -15, GETDATE()), 1, @ИзделиеID, @ДСЕID);
    
    PRINT 'МЛ добавлены.';
END
ELSE
BEGIN
    PRINT 'МЛ уже существуют, пропускаем...';
END
GO

-- Добавление операций
IF NOT EXISTS (SELECT 1 FROM Операция_МЛ)
BEGIN
    PRINT 'Добавление тестовых операций...';
    
    DECLARE @МЛID INT = (SELECT TOP 1 ID FROM МЛ WHERE Закрыт = 0 ORDER BY ID);
    DECLARE @СотрудникID INT = (SELECT TOP 1 ID FROM Сотрудник ORDER BY ID);
    DECLARE @ТипОперацииID INT = (SELECT TOP 1 ID FROM ТипОперации ORDER BY ID);
    
    INSERT INTO Операция_МЛ (МЛID, ТипОперацииID, СотрудникID, ДатаНачала, ДатаОкончания, ФактическаяДлительностьЧас, Статус, Подразделение)
    VALUES 
        (@МЛID, @ТипОперацииID, @СотрудникID, DATEADD(DAY, -9, GETDATE()), DATEADD(DAY, -8, GETDATE()), 2.5, N'Завершена', N'Цех 1'),
        (@МЛID, @ТипОперацииID + 1, @СотрудникID + 1, DATEADD(DAY, -7, GETDATE()), NULL, NULL, N'В работе', N'Цех 1'),
        (@МЛID, @ТипОперацииID + 2, @СотрудникID + 2, DATEADD(DAY, -6, GETDATE()), NULL, NULL, N'В работе', N'Цех 2');
    
    PRINT 'Операции добавлены.';
END
ELSE
BEGIN
    PRINT 'Операции уже существуют, пропускаем...';
END
GO

-- Добавление премиальных коэффициентов
IF NOT EXISTS (SELECT 1 FROM ПремиальныеКоэффициенты)
BEGIN
    PRINT 'Добавление премиальных коэффициентов...';
    
    DECLARE @ИзделиеID INT = (SELECT TOP 1 ID FROM Изделие ORDER BY ID);
    DECLARE @ДСЕID INT = (SELECT TOP 1 ID FROM ДСЕ ORDER BY ID);
    DECLARE @ТипОперацииID INT = (SELECT TOP 1 ID FROM ТипОперации ORDER BY ID);
    
    INSERT INTO ПремиальныеКоэффициенты (Наименование, Коэффициент, ДатаНачала, ДатаОкончания, ИзделиеID, ДСЕID, ТипОперацииID)
    VALUES 
        (N'Повышенная сложность', 1.5, DATEADD(MONTH, -1, GETDATE()), NULL, @ИзделиеID, @ДСЕID, @ТипОперацииID),
        (N'Срочный заказ', 2.0, DATEADD(DAY, -7, GETDATE()), DATEADD(DAY, 7, GETDATE()), @ИзделиеID, NULL, NULL),
        (N'Высокая точность', 1.3, DATEADD(MONTH, -2, GETDATE()), NULL, NULL, NULL, @ТипОперацииID + 1);
    
    PRINT 'Премиальные коэффициенты добавлены.';
END
ELSE
BEGIN
    PRINT 'Премиальные коэффициенты уже существуют, пропускаем...';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Тестовые данные успешно добавлены!';
PRINT '========================================';
PRINT '';
PRINT 'Статистика:';
SELECT 'Изделия' AS Таблица, COUNT(*) AS Количество FROM Изделие
UNION ALL
SELECT 'ДСЕ', COUNT(*) FROM ДСЕ
UNION ALL
SELECT 'Должности', COUNT(*) FROM Должность
UNION ALL
SELECT 'Сотрудники', COUNT(*) FROM Сотрудник
UNION ALL
SELECT 'Типы операций', COUNT(*) FROM ТипОперации
UNION ALL
SELECT 'МЛ', COUNT(*) FROM МЛ
UNION ALL
SELECT 'Операции', COUNT(*) FROM Операция_МЛ
UNION ALL
SELECT 'Премиальные коэффициенты', COUNT(*) FROM ПремиальныеКоэффициенты;
GO
