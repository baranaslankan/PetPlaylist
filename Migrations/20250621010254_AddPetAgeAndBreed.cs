using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetPlaylist.Migrations
{
    /// <inheritdoc />
    public partial class AddPetAgeAndBreed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Room",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Owners",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Owners",
                newName: "FullName");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Owners",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Bookings",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Owners",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Owners",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Bookings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
