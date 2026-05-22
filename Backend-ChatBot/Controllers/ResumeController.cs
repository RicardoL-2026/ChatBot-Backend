using AutoMapper;
using Backend_ChatBot.Data;
using Backend_ChatBot.Entities;
using Backend_ChatBot.Models.Request.Resume;
using Backend_ChatBot.Models.Response;
using Backend_ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_ChatBot.Controllers
{
    [Route("/api/Resume")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResumeTextExtractorService _resumeTextExtractorService;
        public ResumeController(AppDbContext db, IMapper mapper, ResumeTextExtractorService resumeTEService) { 
            _db = db; 
            _mapper = mapper; 
            _resumeTextExtractorService = resumeTEService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<ResumeDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ResumeDTO>>> CreateResume([FromForm] ResumeCreateDTO resumeDTO)
        {
            try
            {
                if (resumeDTO == null) { return BadRequest(ApiResponse<object>.BadRequest("Resume data is required")); }

                using var memoryStream = new MemoryStream();
                await resumeDTO.FileContent.CopyToAsync(memoryStream);

                var fileBytes = memoryStream.ToArray();

                var extractedText = await _resumeTextExtractorService.ExtractTextAsync(
                    fileBytes,
                    resumeDTO.FileContent.ContentType
                );

                var resume = new Resume
                {
                    FileName = resumeDTO.FileName,
                    ContentType = resumeDTO.FileContent.ContentType,
                    Size = (int)resumeDTO.FileContent.Length,
                    FileContent = memoryStream.ToArray(),
                    CreatedAt = DateTime.UtcNow,
                    ExtractedText = extractedText
                };

                await _db.Resume.AddAsync(resume);
                await _db.SaveChangesAsync();

                var resumeResponseDTO = _mapper.Map<ResumeDTO>(resume);

                return CreatedAtAction(
                    nameof(CreateResume),
                    new { id = resume.Id },
                    ApiResponse<ResumeDTO>.CreatedAt(
                        resumeResponseDTO,
                        "New Resume added successfully"
                    )
                );
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error ocurred while creating the Resume: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<ResumeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ResumeDTO>>> UpdatedResume( int id, [FromForm] ResumeUpdateDTO resumeDTO)
        {
            try
            {
                if (resumeDTO == null)
                    return BadRequest(ApiResponse<object>.BadRequest("Resume data is required"));

                if (id != resumeDTO.ID)
                    return BadRequest(ApiResponse<object>.BadRequest("Resume ID in URL does not match Resume ID in request body"));

                var existingResume = await _db.Resume.FirstOrDefaultAsync(r => r.Id == id);

                if (existingResume == null)
                    return NotFound(ApiResponse<object>.NotFound($"Resume with ID {id} was not found"));

                if (!string.IsNullOrWhiteSpace(resumeDTO.FileName))
                {
                    existingResume.FileName = resumeDTO.FileName;
                }

                if (resumeDTO.FileContent != null)
                {
                    using var memoryStream = new MemoryStream();
                    await resumeDTO.FileContent.CopyToAsync(memoryStream);

                    var fileBytes = memoryStream.ToArray();

                    var extractedText = await _resumeTextExtractorService.ExtractTextAsync(
                        fileBytes,
                        resumeDTO.FileContent.ContentType
                    );

                    existingResume.ContentType = resumeDTO.FileContent.ContentType;
                    existingResume.Size = (int)resumeDTO.FileContent.Length;
                    existingResume.FileContent = fileBytes;
                    existingResume.ExtractedText = extractedText;
                }

                await _db.SaveChangesAsync();

                var responseDto = _mapper.Map<ResumeDTO>(existingResume);

                return Ok(ApiResponse<ResumeDTO>.Ok(
                    responseDto,
                    "Resume updated successfully"
                ));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(
                    500,
                    $"An error occurred while updating the Resume: {ex.Message}",
                    ex.Message
                );

                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<object>>> DeletedResume(int id)
        {

            try
            {
                var existingResume = await _db.Resume.FirstOrDefaultAsync(u => u.Id == id);
                if (existingResume == null) { return NotFound(ApiResponse<object>.NotFound($"Resume with ID {id} was not found")); };

                _db.Remove(existingResume);
                await _db.SaveChangesAsync();
                var response = ApiResponse<object>.NoContent("Resume deleted succesfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error ocurred while deleting the Resume: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
