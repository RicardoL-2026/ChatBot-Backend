using AutoMapper;
using Backend_ChatBot.Data;
using Backend_ChatBot.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_ChatBot.Controllers
{
    [Route("/api/Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public MessageController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MessageDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MessageDTO>>>> GetMessage()
        {
            var message = await _db.Message.ToListAsync();
            var dtoResponseMessage = _mapper.Map<List<MessageDTO>>(message);
            var response = ApiResponse<IEnumerable<MessageDTO>>.Ok(dtoResponseMessage, "Message retrieved succesfully");
            return Ok(response);
        }

        [HttpDelete("{conversationId:int}/messages")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteMessagesByConversation(int conversationId)
        {
            try
            {
                var conversationExists = await _db.Conversation
                    .AnyAsync(c => c.Id == conversationId);

                if (!conversationExists)
                {
                    return NotFound(ApiResponse<object>.NotFound(
                        $"Conversation with ID {conversationId} was not found"
                    ));
                }

                var messages = await _db.Message
                    .Where(m => m.ConversacionID == conversationId)
                    .ToListAsync();

                if (!messages.Any())
                {
                    return Ok(ApiResponse<object>.NoContent("No Messages to delete."));
                }

                _db.Message.RemoveRange(messages);
                await _db.SaveChangesAsync();
                var response = ApiResponse<object>.NoContent("Conversation deleted succesfully");

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(
                    500,
                    $"An error occurred while deleting messages: {ex.Message}",
                    ex.Message
                );

                return StatusCode(500, errorResponse);
            }
        }
    }
}
