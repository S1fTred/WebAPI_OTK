using Microsoft.EntityFrameworkCore;
using WebAPI_OTK;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Используем SQLite вместо SQL Server
builder.Services.AddDbContext<Model1>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем поддержку статических файлов (HTML, CSS, JS)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Инициализация БД при первом запуске
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Model1>();
    
    // Удаляем и пересоздаем БД для применения изменений схемы
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    
    DatabaseInitializer.Initialize(context);
    
    // Добавляем операции к дополнительным МЛ
    AddOperationsToML.AddOperations(context);
    
    // Обновляем существующие операции с новыми полями дат
    UpdateOperationDates.UpdateDates(context);
}

app.Run();