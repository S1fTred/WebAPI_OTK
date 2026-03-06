using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_OTK.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Должность",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Наименование = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Код = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Должность", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Изделие",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Наименование = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Описание = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Состояние = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ДатаСоздания = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Изделие", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Оборудование",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Наименование = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ИнвентарныйНомер = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Местоположение = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Статус = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ДатаПоследнегоОбслуживания = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Оборудование", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ТипОперации",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Наименование = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Описание = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ДлительностьЧас = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    КодОперации = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ТипОперации", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "sysdiagrams",
                columns: table => new
                {
                    diagram_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    principal_id = table.Column<int>(type: "int", nullable: false),
                    version = table.Column<int>(type: "int", nullable: true),
                    definition = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sysdiagrams", x => x.diagram_id);
                });

            migrationBuilder.CreateTable(
                name: "Сотрудник",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ФИО = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ДолжностьID = table.Column<int>(type: "int", nullable: true),
                    ТабельныйНомер = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ДатаПриема = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Сотрудник", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Сотрудник_Должность_ДолжностьID",
                        column: x => x.ДолжностьID,
                        principalTable: "Должность",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ДСЕ",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Код = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Наименование = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Чертеж = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ИзделиеID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ДСЕ", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ДСЕ_Изделие_ИзделиеID",
                        column: x => x.ИзделиеID,
                        principalTable: "Изделие",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "МЛ",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    НомерМЛ = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ДатаСоздания = table.Column<DateTime>(type: "date", nullable: true),
                    Закрыт = table.Column<bool>(type: "bit", nullable: true),
                    ДатаЗакрытия = table.Column<DateTime>(type: "date", nullable: true),
                    СотрудникОТК = table.Column<int>(type: "int", nullable: true),
                    КоличествоОТК = table.Column<int>(type: "int", nullable: true),
                    КоличествоБрак = table.Column<int>(type: "int", nullable: true),
                    ИзделиеID = table.Column<int>(type: "int", nullable: false),
                    ДСЕID = table.Column<int>(type: "int", nullable: false),
                    СотрудникID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_МЛ", x => x.ID);
                    table.ForeignKey(
                        name: "FK_МЛ_ДСЕ_ДСЕID",
                        column: x => x.ДСЕID,
                        principalTable: "ДСЕ",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_МЛ_Изделие_ИзделиеID",
                        column: x => x.ИзделиеID,
                        principalTable: "Изделие",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_МЛ_Сотрудник_СотрудникID",
                        column: x => x.СотрудникID,
                        principalTable: "Сотрудник",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_МЛ_Сотрудник_СотрудникОТК",
                        column: x => x.СотрудникОТК,
                        principalTable: "Сотрудник",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ПремиальныеКоэффициенты",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Наименование = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Коэффициент = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ДатаНачала = table.Column<DateTime>(type: "date", nullable: false),
                    ДатаОкончания = table.Column<DateTime>(type: "date", nullable: true),
                    ИзделиеID = table.Column<int>(type: "int", nullable: true),
                    ДСЕID = table.Column<int>(type: "int", nullable: true),
                    ТипОперацииID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ПремиальныеКоэффициенты", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ПремиальныеКоэффициенты_ДСЕ_ДСЕID",
                        column: x => x.ДСЕID,
                        principalTable: "ДСЕ",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ПремиальныеКоэффициенты_Изделие_ИзделиеID",
                        column: x => x.ИзделиеID,
                        principalTable: "Изделие",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ПремиальныеКоэффициенты_ТипОперации_ТипОперацииID",
                        column: x => x.ТипОперацииID,
                        principalTable: "ТипОперации",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Операция_МЛ",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    МЛID = table.Column<int>(type: "int", nullable: false),
                    ТипОперацииID = table.Column<int>(type: "int", nullable: false),
                    ОборудованиеID = table.Column<int>(type: "int", nullable: true),
                    СотрудникID = table.Column<int>(type: "int", nullable: false),
                    ДатаНачала = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ДатаОкончания = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ФактическаяДлительностьЧас = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Подразделение = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Статус = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Примечание = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Операция_МЛ", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Операция_МЛ_МЛ_МЛID",
                        column: x => x.МЛID,
                        principalTable: "МЛ",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Операция_МЛ_Оборудование_ОборудованиеID",
                        column: x => x.ОборудованиеID,
                        principalTable: "Оборудование",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Операция_МЛ_Сотрудник_СотрудникID",
                        column: x => x.СотрудникID,
                        principalTable: "Сотрудник",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Операция_МЛ_ТипОперации_ТипОперацииID",
                        column: x => x.ТипОперацииID,
                        principalTable: "ТипОперации",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Зарплата",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ОперацияМЛID = table.Column<int>(type: "int", nullable: true),
                    СотрудникID = table.Column<int>(type: "int", nullable: true),
                    ЧасыОтработано = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    СтавкаЧасовая = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    СуммаОклад = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Премия = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    ИтогоКВыплате = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Период = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Зарплата", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Зарплата_Операция_МЛ_ОперацияМЛID",
                        column: x => x.ОперацияМЛID,
                        principalTable: "Операция_МЛ",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Зарплата_Сотрудник_СотрудникID",
                        column: x => x.СотрудникID,
                        principalTable: "Сотрудник",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ДСЕ_ИзделиеID",
                table: "ДСЕ",
                column: "ИзделиеID");

            migrationBuilder.CreateIndex(
                name: "IX_Зарплата_ОперацияМЛID",
                table: "Зарплата",
                column: "ОперацияМЛID");

            migrationBuilder.CreateIndex(
                name: "IX_Зарплата_СотрудникID",
                table: "Зарплата",
                column: "СотрудникID");

            migrationBuilder.CreateIndex(
                name: "IX_МЛ_ДСЕID",
                table: "МЛ",
                column: "ДСЕID");

            migrationBuilder.CreateIndex(
                name: "IX_МЛ_ИзделиеID",
                table: "МЛ",
                column: "ИзделиеID");

            migrationBuilder.CreateIndex(
                name: "IX_МЛ_СотрудникОТК",
                table: "МЛ",
                column: "СотрудникОТК");

            migrationBuilder.CreateIndex(
                name: "IX_МЛ_СотрудникID",
                table: "МЛ",
                column: "СотрудникID");

            migrationBuilder.CreateIndex(
                name: "IX_Операция_МЛ_МЛID",
                table: "Операция_МЛ",
                column: "МЛID");

            migrationBuilder.CreateIndex(
                name: "IX_Операция_МЛ_ОборудованиеID",
                table: "Операция_МЛ",
                column: "ОборудованиеID");

            migrationBuilder.CreateIndex(
                name: "IX_Операция_МЛ_СотрудникID",
                table: "Операция_МЛ",
                column: "СотрудникID");

            migrationBuilder.CreateIndex(
                name: "IX_Операция_МЛ_ТипОперацииID",
                table: "Операция_МЛ",
                column: "ТипОперацииID");

            migrationBuilder.CreateIndex(
                name: "IX_ПремиальныеКоэффициенты_ДСЕID",
                table: "ПремиальныеКоэффициенты",
                column: "ДСЕID");

            migrationBuilder.CreateIndex(
                name: "IX_ПремиальныеКоэффициенты_ИзделиеID",
                table: "ПремиальныеКоэффициенты",
                column: "ИзделиеID");

            migrationBuilder.CreateIndex(
                name: "IX_ПремиальныеКоэффициенты_ТипОперацииID",
                table: "ПремиальныеКоэффициенты",
                column: "ТипОперацииID");

            migrationBuilder.CreateIndex(
                name: "IX_Сотрудник_ДолжностьID",
                table: "Сотрудник",
                column: "ДолжностьID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Зарплата");

            migrationBuilder.DropTable(
                name: "ПремиальныеКоэффициенты");

            migrationBuilder.DropTable(
                name: "sysdiagrams");

            migrationBuilder.DropTable(
                name: "Операция_МЛ");

            migrationBuilder.DropTable(
                name: "МЛ");

            migrationBuilder.DropTable(
                name: "Оборудование");

            migrationBuilder.DropTable(
                name: "ТипОперации");

            migrationBuilder.DropTable(
                name: "ДСЕ");

            migrationBuilder.DropTable(
                name: "Сотрудник");

            migrationBuilder.DropTable(
                name: "Изделие");

            migrationBuilder.DropTable(
                name: "Должность");
        }
    }
}
