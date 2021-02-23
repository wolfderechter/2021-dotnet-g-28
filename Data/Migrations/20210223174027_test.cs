using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2021_dotnet_g_28.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerNr = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyAdress = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CustomerInitDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerNr);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeNr = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adress = table.Column<string>(nullable: true),
                    DateInService = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    //userId = table.Column<string>(nullable: true)
                }//,
                //constraints: table =>
                //{
                //    table.PrimaryKey("PK_Employees", x => x.EmployeeNr);
                //    table.ForeignKey(
                //        name: "FK_Employees_AspNetUsers_userId",
                //        column: x => x.userId,
                //        principalTable: "AspNetUsers",
                //        principalColumn: "Id",
                //        onDelete: ReferentialAction.Restrict);
                //}
                );

            migrationBuilder.CreateTable(
                name: "ContactPerson",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CustomerNr = table.Column<int>(nullable: false),
                    CustomerNr1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPerson", x => x.Email);
                    table.ForeignKey(
                        name: "FK_ContactPerson_Customer_CustomerNr",
                        column: x => x.CustomerNr,
                        principalTable: "Customer",
                        principalColumn: "CustomerNr",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactPerson_Customer_CustomerNr1",
                        column: x => x.CustomerNr1,
                        principalTable: "Customer",
                        principalColumn: "CustomerNr",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CustomerNr = table.Column<int>(nullable: false),
                    CustomerNr1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Contract_Customer_CustomerNr",
                        column: x => x.CustomerNr,
                        principalTable: "Customer",
                        principalColumn: "CustomerNr",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Customer_CustomerNr1",
                        column: x => x.CustomerNr1,
                        principalTable: "Customer",
                        principalColumn: "CustomerNr",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactPerson_CustomerNr",
                table: "ContactPerson",
                column: "CustomerNr");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPerson_CustomerNr1",
                table: "ContactPerson",
                column: "CustomerNr1");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CustomerNr",
                table: "Contract",
                column: "CustomerNr");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CustomerNr1",
                table: "Contract",
                column: "CustomerNr1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employees_userId",
            //    table: "Employees",
            //    column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactPerson");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
