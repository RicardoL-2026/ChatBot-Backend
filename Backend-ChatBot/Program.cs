using Backend_ChatBot.Data;
using Backend_ChatBot.Entities;
using Backend_ChatBot.Models.Request.Conversation;
using Backend_ChatBot.Models.Request.Message;
using Backend_ChatBot.Models.Request.Resume;
using Backend_ChatBot.Models.Response;
using Backend_ChatBot.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 7, 0))
    )
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<Conversation, ConversationDTO>().ReverseMap();
    o.CreateMap<Conversation, ConversationCreateDTO>().ReverseMap();

    o.CreateMap<Message, MessageDTO>().ReverseMap();
    o.CreateMap<Message, MessageCreateDTO>().ReverseMap();

    o.CreateMap<Resume, ResumeDTO>().ReverseMap();
    o.CreateMap<Resume, ResumeCreateDTO>().ReverseMap();
    o.CreateMap<Resume, ResumeUpdateDTO>().ReverseMap();
    o.CreateMap<ResumeUpdateDTO, ResumeDTO>().ReverseMap();

});

builder.Services.AddScoped<KnowledgeService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<ResumeTextExtractorService>();
builder.Services.AddHttpClient<GeminiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // redicters to "/scalar" endpoint.
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
