using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KulinarstvoASP.Migrations
{
    /// <inheritdoc />
    public partial class PrepravkaRelacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceptSastojci_Sastojci_SastojakId",
                table: "ReceptSastojci");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceptSastojci_Sastojci_SastojakId",
                table: "ReceptSastojci",
                column: "SastojakId",
                principalTable: "Sastojci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceptSastojci_Sastojci_SastojakId",
                table: "ReceptSastojci");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceptSastojci_Sastojci_SastojakId",
                table: "ReceptSastojci",
                column: "SastojakId",
                principalTable: "Sastojci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
