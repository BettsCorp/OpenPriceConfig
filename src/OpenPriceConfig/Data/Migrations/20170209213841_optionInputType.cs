using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using OpenPriceConfig.Models;

namespace OpenPriceConfig.Data.Migrations
{
    public partial class optionInputType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InputType",
                table: "Option",
                nullable: false,
                defaultValue: Option.InputTypes.Checkbox);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputType",
                table: "Option");
        }
    }
}
