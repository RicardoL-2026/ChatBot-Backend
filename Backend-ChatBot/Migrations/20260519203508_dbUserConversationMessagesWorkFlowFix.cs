using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_ChatBot.Migrations
{
    /// <inheritdoc />
    public partial class dbUserConversationMessagesWorkFlowFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversacionID",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ConversacionID",
                table: "Message",
                column: "ConversacionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Conversation_ConversacionID",
                table: "Message",
                column: "ConversacionID",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Conversation_ConversacionID",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ConversacionID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ConversacionID",
                table: "Message");
        }
    }
}
