using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OpenPriceConfig.Data.Migrations
{
    public partial class locales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Child",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConfiguratorID = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    SubType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Child", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Child_Configurator_ConfiguratorID",
                        column: x => x.ConfiguratorID,
                        principalTable: "Configurator",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locale",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Tag = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locale", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Child_ConfiguratorID",
                table: "Child",
                column: "ConfiguratorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Child");

            migrationBuilder.DropTable(
                name: "Locale");
        }
    }
}
