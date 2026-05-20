using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Partpurja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSendCreditReminderFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing invoices default to true so reminders continue for already-overdue credit accounts.
            migrationBuilder.AddColumn<bool>(
                name: "SendCreditReminder",
                table: "SalesInvoices",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendCreditReminder",
                table: "SalesInvoices");
        }
    }
}
