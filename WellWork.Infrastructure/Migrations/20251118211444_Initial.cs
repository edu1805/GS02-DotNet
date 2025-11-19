using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellWork.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false, defaultValueSql: "SYS_GUID()"),
                    USERNAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_WELLWORK_CHECKINS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false, defaultValueSql: "SYS_GUID()"),
                    USER_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    MOOD = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ENERGY = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    NOTES = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    CREATED_AT = table.Column<DateTimeOffset>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WELLWORK_CHECKINS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_WELLWORK_CHECKINS_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_WELLWORK_GENERATED_MESSAGES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false, defaultValueSql: "SYS_GUID()"),
                    CHECKIN_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    MESSAGE = table.Column<string>(type: "CLOB", nullable: false),
                    CONFIDENCE = table.Column<decimal>(type: "NUMBER(5,4)", nullable: false),
                    GENERATED_AT = table.Column<DateTimeOffset>(type: "TIMESTAMP WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WELLWORK_GENERATED_MESSAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_WELLWORK_GENERATED_MESSAGES_T_WELLWORK_CHECKINS_CHECKIN_ID",
                        column: x => x.CHECKIN_ID,
                        principalTable: "T_WELLWORK_CHECKINS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_WELLWORK_CHECKINS_USER_ID",
                table: "T_WELLWORK_CHECKINS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_WELLWORK_GENERATED_MESSAGES_CHECKIN_ID",
                table: "T_WELLWORK_GENERATED_MESSAGES",
                column: "CHECKIN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_USERNAME",
                table: "USERS",
                column: "USERNAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_WELLWORK_GENERATED_MESSAGES");

            migrationBuilder.DropTable(
                name: "T_WELLWORK_CHECKINS");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
