using AutoMapper;
using Backend_ChatBot.Data;
using Backend_ChatBot.Entities;
using Backend_ChatBot.Models.Request.Message;
using Backend_ChatBot.Models.Response;
using Backend_ChatBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_ChatBot.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ChatController(AppDbContext db, IMapper mapper, ChatService chatService) { 
            _db = db;
            _mapper = mapper; 
            _chatService = chatService; 
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ChatResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ChatResponseDTO>>> Ask(MessageCreateDTO messageDTO)
        {
            try
            {
                if (messageDTO == null)
                    return BadRequest(ApiResponse<object>.BadRequest("Message data is required"));

                var userMessage = _mapper.Map<Message>(messageDTO);
                userMessage.Type = "Request";
                userMessage.CreatedAt = DateTime.UtcNow;

                await _db.Message.AddAsync(userMessage);
                await _db.SaveChangesAsync();

                var answer = await _chatService.AskAsync(messageDTO.Messa);

                var chatResponse = new ChatResponseDTO
                {
                    Question = _mapper.Map<MessageDTO>(userMessage),
                    Answer = answer
                };

                return CreatedAtAction(
                    nameof(Ask),
                    new { id = userMessage.Id },
                    ApiResponse<ChatResponseDTO>.CreatedAt(
                        chatResponse,
                        "AI response generated successfully"
                    )
                );
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(
                    500,
                    $"An error occurred while creating the Message: {ex.Message}",
                    ex.Message
                );

                return StatusCode(500, errorResponse);
            }
        }
    }
}
    