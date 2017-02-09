using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OpenPriceConfig.Data.Migrations
{
    public partial class configurators2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Child");

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConfiguratorID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DescriptionLocaleID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OptionTag = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Option_Configurator_ConfiguratorID",
                        column: x => x.ConfiguratorID,
                        principalTable: "Configurator",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Option_Locale_DescriptionLocaleID",
                        column: x => x.DescriptionLocaleID,
                        principalTable: "Locale",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BracketPricing",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ForFloorNumber = table.Column<int>(nullable: false),
                    OptionID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BracketPricing", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BracketPricing_Option_OptionID",
                        column: x => x.OptionID,
                        principalTable: "Option",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "FloorsNumber",
                table: "Configurator",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Locale",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BracketPricing_OptionID",
                table: "BracketPricing",
                column: "OptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Option_ConfiguratorID",
                table: "Option",
                column: "ConfiguratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Option_DescriptionLocaleID",
                table: "Option",
                column: "DescriptionLocaleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FloorsNumber",
                table: "Configurator");

            migrationBuilder.DropTable(
                name: "BracketPricing");

            migrationBuilder.DropTable(
                name: "Option");

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

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Locale",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Child_ConfiguratorID",
                table: "Child",
                column: "ConfiguratorID");
        }
    }
}
