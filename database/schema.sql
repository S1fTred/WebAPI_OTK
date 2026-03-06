-- ========================================
-- СХЕМА БАЗЫ ДАННЫХ ДЛЯ СИСТЕМЫ ОТК
-- PostgreSQL
-- ========================================

-- Удаление существующих таблиц (если есть)
DROP TABLE IF EXISTS "Зарплата" CASCADE;
DROP TABLE IF EXISTS "Операция_МЛ" CASCADE;
DROP TABLE IF EXISTS "МЛ" CASCADE;
DROP TABLE IF EXISTS "ПремиальныеКоэффициенты" CASCADE;
DROP TABLE IF EXISTS "ДСЕ" CASCADE;
DROP TABLE IF EXISTS "Изделие" CASCADE;
DROP TABLE IF EXISTS "Сотрудник" CASCADE;
DROP TABLE IF EXISTS "Должность" CASCADE;
DROP TABLE IF EXISTS "ТипОперации" CASCADE;
DROP TABLE IF EXISTS "Оборудование" CASCADE;

-- ========================================
-- СПРАВОЧНЫЕ ТАБЛИЦЫ
-- ========================================

-- Таблица: Должность
CREATE TABLE "Должность" (
    "ID" SERIAL PRIMARY KEY,
    "Наименование" VARCHAR(100) NOT NULL,
    "Код" VARCHAR(20)
);

COMMENT ON TABLE "Должность" IS 'Справочник должностей сотрудников';
COMMENT ON COLUMN "Должность"."Наименование" IS 'Название должности';
COMMENT ON COLUMN "Должность"."Код" IS 'Код должности';

-- Таблица: Изделие
CREATE TABLE "Изделие" (
    "ID" SERIAL PRIMARY KEY,
    "Наименование" VARCHAR(200) NOT NULL,
    "Описание" VARCHAR(1000),
    "Состояние" VARCHAR(50),
    "ДатаСоздания" DATE DEFAULT CURRENT_DATE
);

COMMENT ON TABLE "Изделие" IS 'Справочник изделий';
COMMENT ON COLUMN "Изделие"."Наименование" IS 'Название изделия';
COMMENT ON COLUMN "Изделие"."Описание" IS 'Описание изделия';
COMMENT ON COLUMN "Изделие"."Состояние" IS 'Состояние изделия (В разработке, В производстве, Снято с производства)';

-- Таблица: ТипОперации
CREATE TABLE "ТипОперации" (
    "ID" SERIAL PRIMARY KEY,
    "Наименование" VARCHAR(200) NOT NULL,
    "Описание" VARCHAR(500),
    "ДлительностьЧас" DECIMAL(10, 2),
    "КодОперации" VARCHAR(50)
);

COMMENT ON TABLE "ТипОперации" IS 'Справочник типов операций';
COMMENT ON COLUMN "ТипОперации"."Наименование" IS 'Название операции';
COMMENT ON COLUMN "ТипОперации"."ДлительностьЧас" IS 'Нормативная длительность в часах';

-- Таблица: Оборудование
CREATE TABLE "Оборудование" (
    "ID" SERIAL PRIMARY KEY,
    "Наименование" VARCHAR(200) NOT NULL,
    "ИнвентарныйНомер" VARCHAR(100),
    "Местоположение" VARCHAR(200),
    "Статус" VARCHAR(50),
    "ДатаПоследнегоОбслуживания" DATE
);

COMMENT ON TABLE "Оборудование" IS 'Справочник оборудования';
COMMENT ON COLUMN "Оборудование"."Статус" IS 'Статус оборудования (Работает, На обслуживании, Неисправно)';

-- ========================================
-- ОСНОВНЫЕ ТАБЛИЦЫ
-- ========================================

-- Таблица: Сотрудник
CREATE TABLE "Сотрудник" (
    "ID" SERIAL PRIMARY KEY,
    "ФИО" VARCHAR(200) NOT NULL,
    "ДолжностьID" INTEGER REFERENCES "Должность"("ID") ON DELETE SET NULL,
    "ТабельныйНомер" VARCHAR(50),
    "ДатаПриема" DATE
);

COMMENT ON TABLE "Сотрудник" IS 'Сотрудники предприятия';
COMMENT ON COLUMN "Сотрудник"."ФИО" IS 'Фамилия Имя Отчество';
COMMENT ON COLUMN "Сотрудник"."ТабельныйНомер" IS 'Табельный номер сотрудника';

-- Таблица: ДСЕ (Детали и Сборочные Единицы)
CREATE TABLE "ДСЕ" (
    "ID" SERIAL PRIMARY KEY,
    "Код" VARCHAR(100) NOT NULL UNIQUE,
    "Наименование" VARCHAR(200),
    "Чертеж" VARCHAR(500),
    "ИзделиеID" INTEGER REFERENCES "Изделие"("ID") ON DELETE CASCADE
);

COMMENT ON TABLE "ДСЕ" IS 'Детали и сборочные единицы';
COMMENT ON COLUMN "ДСЕ"."Код" IS 'Уникальный код ДСЕ';
COMMENT ON COLUMN "ДСЕ"."Чертеж" IS 'Номер чертежа';

-- Таблица: МЛ (Маршрутный лист)
CREATE TABLE "МЛ" (
    "ID" SERIAL PRIMARY KEY,
    "НомерМЛ" VARCHAR(50) NOT NULL,
    "ДатаСоздания" DATE DEFAULT CURRENT_DATE,
    "Закрыт" BOOLEAN DEFAULT FALSE,
    "СотрудникОТК" INTEGER REFERENCES "Сотрудник"("ID") ON DELETE SET NULL,
    "КоличествоОТК" INTEGER,
    "КоличествоБрак" INTEGER,
    "ИзделиеID" INTEGER NOT NULL REFERENCES "Изделие"("ID") ON DELETE CASCADE,
    "ДСЕID" INTEGER NOT NULL REFERENCES "ДСЕ"("ID") ON DELETE CASCADE
);

COMMENT ON TABLE "МЛ" IS 'Маршрутные листы';
COMMENT ON COLUMN "МЛ"."НомерМЛ" IS 'Номер маршрутного листа';
COMMENT ON COLUMN "МЛ"."Закрыт" IS 'Признак закрытия МЛ';
COMMENT ON COLUMN "МЛ"."СотрудникОТК" IS 'Сотрудник ОТК, закрывший МЛ';
COMMENT ON COLUMN "МЛ"."КоличествоОТК" IS 'Количество принятых ОТК';
COMMENT ON COLUMN "МЛ"."КоличествоБрак" IS 'Количество брака';

-- Таблица: Операция_МЛ
CREATE TABLE "Операция_МЛ" (
    "ID" SERIAL PRIMARY KEY,
    "МЛID" INTEGER NOT NULL REFERENCES "МЛ"("ID") ON DELETE CASCADE,
    "ТипОперацииID" INTEGER NOT NULL REFERENCES "ТипОперации"("ID") ON DELETE RESTRICT,
    "ОборудованиеID" INTEGER REFERENCES "Оборудование"("ID") ON DELETE SET NULL,
    "СотрудникID" INTEGER NOT NULL REFERENCES "Сотрудник"("ID") ON DELETE RESTRICT,
    "ДатаНачала" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "ДатаОкончания" TIMESTAMP,
    "ФактическаяДлительностьЧас" DECIMAL(10, 2),
    "Подразделение" VARCHAR(100),
    "Статус" VARCHAR(50) DEFAULT 'В работе',
    "Примечание" VARCHAR(500)
);

COMMENT ON TABLE "Операция_МЛ" IS 'Операции маршрутного листа';
COMMENT ON COLUMN "Операция_МЛ"."Статус" IS 'Статус операции (В работе, Завершена, Приостановлена)';
COMMENT ON COLUMN "Операция_МЛ"."ФактическаяДлительностьЧас" IS 'Фактическая длительность в часах';

-- Таблица: ПремиальныеКоэффициенты
CREATE TABLE "ПремиальныеКоэффициенты" (
    "ID" SERIAL PRIMARY KEY,
    "Наименование" VARCHAR(200) NOT NULL,
    "Коэффициент" DECIMAL(5, 2) NOT NULL CHECK ("Коэффициент" > 0),
    "ДатаНачала" DATE NOT NULL DEFAULT CURRENT_DATE,
    "ДатаОкончания" DATE,
    "ИзделиеID" INTEGER REFERENCES "Изделие"("ID") ON DELETE CASCADE,
    "ДСЕID" INTEGER REFERENCES "ДСЕ"("ID") ON DELETE CASCADE,
    "ТипОперацииID" INTEGER REFERENCES "ТипОперации"("ID") ON DELETE CASCADE,
    CONSTRAINT "chk_dates" CHECK ("ДатаОкончания" IS NULL OR "ДатаОкончания" >= "ДатаНачала")
);

COMMENT ON TABLE "ПремиальныеКоэффициенты" IS 'Премиальные коэффициенты для расчета зарплаты';
COMMENT ON COLUMN "ПремиальныеКоэффициенты"."Коэффициент" IS 'Коэффициент премирования (например, 1.5 = +50%)';
COMMENT ON COLUMN "ПремиальныеКоэффициенты"."ДатаНачала" IS 'Дата начала действия коэффициента';
COMMENT ON COLUMN "ПремиальныеКоэффициенты"."ДатаОкончания" IS 'Дата окончания действия (NULL = бессрочно)';

-- Таблица: Зарплата
CREATE TABLE "Зарплата" (
    "ID" SERIAL PRIMARY KEY,
    "ОперацияМЛID" INTEGER REFERENCES "Операция_МЛ"("ID") ON DELETE CASCADE,
    "СотрудникID" INTEGER REFERENCES "Сотрудник"("ID") ON DELETE CASCADE,
    "ЧасыОтработано" DECIMAL(10, 2),
    "СтавкаЧасовая" DECIMAL(10, 2),
    "СуммаОклад" DECIMAL(12, 2),
    "Премия" DECIMAL(12, 2),
    "ИтогоКВыплате" DECIMAL(12, 2),
    "Период" DATE
);

COMMENT ON TABLE "Зарплата" IS 'Расчет зарплаты сотрудников';
COMMENT ON COLUMN "Зарплата"."СтавкаЧасовая" IS 'Часовая ставка сотрудника';
COMMENT ON COLUMN "Зарплата"."СуммаОклад" IS 'Сумма оклада без премий';
COMMENT ON COLUMN "Зарплата"."Премия" IS 'Сумма премии';
COMMENT ON COLUMN "Зарплата"."ИтогоКВыплате" IS 'Итоговая сумма к выплате';

-- ========================================
-- ИНДЕКСЫ ДЛЯ ОПТИМИЗАЦИИ
-- ========================================

-- Индексы для частых запросов
CREATE INDEX "idx_ml_closed" ON "МЛ"("Закрыт");
CREATE INDEX "idx_ml_izdelie" ON "МЛ"("ИзделиеID");
CREATE INDEX "idx_ml_dse" ON "МЛ"("ДСЕID");
CREATE INDEX "idx_ml_date" ON "МЛ"("ДатаСоздания");

CREATE INDEX "idx_operation_ml" ON "Операция_МЛ"("МЛID");
CREATE INDEX "idx_operation_employee" ON "Операция_МЛ"("СотрудникID");
CREATE INDEX "idx_operation_status" ON "Операция_МЛ"("Статус");
CREATE INDEX "idx_operation_date" ON "Операция_МЛ"("ДатаНачала");

CREATE INDEX "idx_prem_active" ON "ПремиальныеКоэффициенты"("ДатаНачала", "ДатаОкончания");
CREATE INDEX "idx_prem_izdelie" ON "ПремиальныеКоэффициенты"("ИзделиеID");
CREATE INDEX "idx_prem_dse" ON "ПремиальныеКоэффициенты"("ДСЕID");

CREATE INDEX "idx_salary_employee" ON "Зарплата"("СотрудникID");
CREATE INDEX "idx_salary_period" ON "Зарплата"("Период");

CREATE INDEX "idx_dse_izdelie" ON "ДСЕ"("ИзделиеID");
CREATE INDEX "idx_employee_dolzhnost" ON "Сотрудник"("ДолжностьID");

-- ========================================
-- ПРЕДСТАВЛЕНИЯ (VIEWS)
-- ========================================

-- Представление: Активные премиальные коэффициенты
CREATE OR REPLACE VIEW "v_ActivePremKoef" AS
SELECT 
    pk.*,
    i."Наименование" AS "ИзделиеНаименование",
    d."Наименование" AS "ДСЕНаименование",
    t."Наименование" AS "ТипОперацииНаименование"
FROM "ПремиальныеКоэффициенты" pk
LEFT JOIN "Изделие" i ON pk."ИзделиеID" = i."ID"
LEFT JOIN "ДСЕ" d ON pk."ДСЕID" = d."ID"
LEFT JOIN "ТипОперации" t ON pk."ТипОперацииID" = t."ID"
WHERE pk."ДатаНачала" <= CURRENT_DATE 
  AND (pk."ДатаОкончания" IS NULL OR pk."ДатаОкончания" >= CURRENT_DATE);

COMMENT ON VIEW "v_ActivePremKoef" IS 'Активные премиальные коэффициенты на текущую дату';

-- Представление: Открытые МЛ с деталями
CREATE OR REPLACE VIEW "v_OpenML" AS
SELECT 
    ml.*,
    i."Наименование" AS "ИзделиеНаименование",
    d."Наименование" AS "ДСЕНаименование",
    d."Код" AS "ДСЕКод",
    s."ФИО" AS "СотрудникОТКФИО",
    COUNT(o."ID") AS "КоличествоОпераций",
    COUNT(CASE WHEN o."Статус" = 'Завершена' THEN 1 END) AS "ЗавершенныхОпераций"
FROM "МЛ" ml
INNER JOIN "Изделие" i ON ml."ИзделиеID" = i."ID"
INNER JOIN "ДСЕ" d ON ml."ДСЕID" = d."ID"
LEFT JOIN "Сотрудник" s ON ml."СотрудникОТК" = s."ID"
LEFT JOIN "Операция_МЛ" o ON ml."ID" = o."МЛID"
WHERE ml."Закрыт" = FALSE
GROUP BY ml."ID", i."Наименование", d."Наименование", d."Код", s."ФИО";

COMMENT ON VIEW "v_OpenML" IS 'Открытые маршрутные листы с подробной информацией';

-- ========================================
-- ФУНКЦИИ
-- ========================================

-- Функция: Расчет фактической длительности операции
CREATE OR REPLACE FUNCTION calculate_operation_duration()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW."ДатаОкончания" IS NOT NULL AND NEW."ДатаНачала" IS NOT NULL THEN
        NEW."ФактическаяДлительностьЧас" := 
            EXTRACT(EPOCH FROM (NEW."ДатаОкончания" - NEW."ДатаНачала")) / 3600.0;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Триггер для автоматического расчета длительности
CREATE TRIGGER trg_calculate_duration
BEFORE INSERT OR UPDATE ON "Операция_МЛ"
FOR EACH ROW
EXECUTE FUNCTION calculate_operation_duration();

COMMENT ON FUNCTION calculate_operation_duration() IS 'Автоматический расчет фактической длительности операции';

-- ========================================
-- ЗАВЕРШЕНИЕ
-- ========================================

-- Вывод информации о созданных объектах
DO $$
DECLARE
    table_count INTEGER;
    index_count INTEGER;
    view_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO table_count 
    FROM information_schema.tables 
    WHERE table_schema = 'public' AND table_type = 'BASE TABLE';
    
    SELECT COUNT(*) INTO index_count 
    FROM pg_indexes 
    WHERE schemaname = 'public';
    
    SELECT COUNT(*) INTO view_count 
    FROM information_schema.views 
    WHERE table_schema = 'public';
    
    RAISE NOTICE 'Схема базы данных создана успешно!';
    RAISE NOTICE 'Создано таблиц: %', table_count;
    RAISE NOTICE 'Создано индексов: %', index_count;
    RAISE NOTICE 'Создано представлений: %', view_count;
END $$;
