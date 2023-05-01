using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rovio.Challenge.Matchmaking.Database.Migrations.Sqllite
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Servers_ServerId",
                table: "Sessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServerId",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Servers_ServerId",
                table: "Sessions",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Servers_ServerId",
                table: "Sessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServerId",
                table: "Sessions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Servers_ServerId",
                table: "Sessions",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id");
        }
    }
}
