using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingRoomsSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing rooms to have default slots
            migrationBuilder.Sql(@"
                UPDATE PhongTaps 
                SET SlotToiDa = 20, SlotDaDangKy = 0 
                WHERE SlotToiDa = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reset slots to 0
            migrationBuilder.Sql(@"
                UPDATE PhongTaps 
                SET SlotToiDa = 0, SlotDaDangKy = 0
            ");
        }
    }
}
