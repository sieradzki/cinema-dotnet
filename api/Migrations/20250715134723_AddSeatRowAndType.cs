using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatRowAndType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_RoomId_Number",
                table: "Seats");

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "Seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Seats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_RoomId_Row_Number",
                table: "Seats",
                columns: new[] { "RoomId", "Row", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_RoomId_Row_Number",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_RoomId_Number",
                table: "Seats",
                columns: new[] { "RoomId", "Number" },
                unique: true);
        }
    }
}
