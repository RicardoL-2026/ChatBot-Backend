using AutoMapper;
using Backend_ChatBot.Data;
using Backend_ChatBot.Entities;
using Backend_ChatBot.Models.Request.Conversation;
using Backend_ChatBot.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_ChatBot.Controllers
{
    [Route("/api/Conversation")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public ConversationController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }


        //Conversasion General
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ConversationDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ConversationDTO>>>> GetConversation()
        {
            var conversation = await _db.Conversation.ToListAsync();
            var dtoResponseConversation = _mapper.Map<List<ConversationDTO>>(conversation);
            var response = ApiResponse<IEnumerable<ConversationDTO>>.Ok(dtoResponseConversation, "Conversations retrieved succesfully");
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ConversationDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ConversationDTO>>> CreateConversation(ConversationCreateDTO conversationDTO)
        {
            try
            {
                if (conversationDTO == null) { return BadRequest(ApiResponse<object>.BadRequest("Conversation data is required")); }

                Conversation conversation = _mapper.Map<Conversation>(conversationDTO);

                await _db.Conversation.AddAsync(conversation);
                await _db.SaveChangesAsync();
                var conversationResponseDTO = _mapper.Map<ConversationDTO>(conversation);
                return CreatedAtAction(
                    nameof(CreateConversation),
                    new { id = conversation.Id },
                    ApiResponse<ConversationDTO>.CreatedAt(conversationResponseDTO, "New Conversation added succesfully")
                );
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error ocurred while creating the Conversation: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<object>>> DeletedConversation(int id)
        {

            try
            {
                var existingConversation = await _db.Conversation.FirstOrDefaultAsync(u => u.Id == id);
                if (existingConversation == null) { return NotFound(ApiResponse<object>.NotFound($"Conversation with ID {id} was not found")); };

                _db.Remove(existingConversation);
                await _db.SaveChangesAsync();
                var response = ApiResponse<object>.NoContent("Conversation deleted succesfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error ocurred while Deleting the Conversation: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

    }
}
