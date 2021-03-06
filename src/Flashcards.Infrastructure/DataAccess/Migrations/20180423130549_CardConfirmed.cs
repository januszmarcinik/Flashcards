﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Flashcards.Infrastructure.DataAccess.Migrations
{
    public partial class CardConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Cards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Cards");
        }
    }
}
