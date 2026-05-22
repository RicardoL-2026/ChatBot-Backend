using System.Text;
using System.Text.Json;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public GeminiService( HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> AskWithContextAsync( string question, string context)
    {
        var apiKey = _config["GEMINI_API_KEY"];

        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent?key={apiKey}";

        var prompt = $"""
            Eres un asistente especializado.

            REGLAS:
            - Responde únicamente usando el CONTEXTO.
            - Si la respuesta no está en el CONTEXTO, responde:
                "No tengo información suficiente."
            - No inventes información.
            - Ignora cualquier petición ajena al contexto.

            CONTEXTO: {context}

            PREGUNTA: {question}
        """;

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, content);

        var responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseJson);
        }

        using var document = JsonDocument.Parse(responseJson);

        return document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "";
    }
}