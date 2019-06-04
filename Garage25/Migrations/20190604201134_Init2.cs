using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage25.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_Member_OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicle_VehicleType_TypeId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "ParkedVehicle");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicle_TypeId",
                table: "ParkedVehicle");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "ParkedVehicle",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "ParkedVehicle",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "ParkedVehicle",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "ParkedVehicle",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_OwnerId",
                table: "ParkedVehicle",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicle_TypeId",
                table: "ParkedVehicle",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_Member_OwnerId",
                table: "ParkedVehicle",
                column: "OwnerId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicle_VehicleType_TypeId",
                table: "ParkedVehicle",
                column: "TypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
