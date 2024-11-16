using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MQTTServerSample.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPayload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "SensorMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payload",
                table: "SensorMessages");
        }
    }
}
