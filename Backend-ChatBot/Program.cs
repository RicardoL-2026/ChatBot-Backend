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

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://ricardol-2026.github.io/ChatBot-Frontend/"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure();
        }
    )
);

// Add services to the container.

builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

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
app.MapGet("/", () => "ChatBot Backend is running");

app.UseCors("FrontendPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    // redicters to "/scalar" endpoint.
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

app.Run();
