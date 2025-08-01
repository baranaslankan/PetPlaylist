using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetPlaylist.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntensityLevel",
                table: "PetBehaviors");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PetBehaviors");

            migrationBuilder.DropColumn(
                name: "ObservedBy",
                table: "PetBehaviors");

            migrationBuilder.DropColumn(
                name: "ObservedDate",
                table: "PetBehaviors");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Behaviors");

            migrationBuilder.DropColumn(
                name: "EffectivenessRating",
                table: "BehaviorPlaylists");

            migrationBuilder.DropColumn(
                name: "RecommendedBy",
                table: "BehaviorPlaylists");

            migrationBuilder.DropColumn(
                name: "UsageNotes",
                table: "BehaviorPlaylists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntensityLevel",
                table: "PetBehaviors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PetBehaviors",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservedBy",
                table: "PetBehaviors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ObservedDate",
                table: "PetBehaviors",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Behaviors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EffectivenessRating",
                table: "BehaviorPlaylists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedBy",
                table: "BehaviorPlaylists",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsageNotes",
                table: "BehaviorPlaylists",
                type: "TEXT",
                nullable: true);
        }
    }
}
