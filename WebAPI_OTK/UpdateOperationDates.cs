namespace WebAPI_OTK
{
    public static class UpdateOperationDates
    {
        public static void UpdateDates(Model1 context)
        {
            // Проверяем, нужно ли обновление (если хотя бы одна операция уже имеет ДатаВыдачи, значит обновление уже было)
            var hasUpdatedDates = context.Операция_МЛ.Any(o => o.ДатаВыдачи != null);
            if (hasUpdatedDates)
            {
                Console.WriteLine("Даты операций уже обновлены");
                return;
            }

            // Получаем все операции
            var operations = context.Операция_МЛ.ToList();
            
            foreach (var op in operations)
            {
                // Дата Выдачи = за 1 день до ДатаНачала (плановая дата начала)
                op.ДатаВыдачи = op.ДатаНачала.AddDays(-1);
                
                // Дата Исполнения = ДатаОкончания (фактическая дата завершения)
                op.ДатаИсполнения = op.ДатаОкончания;
                
                // Дата Закрытия = за 3 дня после ДатаНачала (дедлайн)
                op.ДатаЗакрытия = op.ДатаНачала.AddDays(3);
            }
            
            context.SaveChanges();
            Console.WriteLine($"Обновлено {operations.Count} операций с новыми датами");
        }
    }
}
