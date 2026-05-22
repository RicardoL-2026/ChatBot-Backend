namespace Backend_ChatBot.Services
{
    public class ChatService
    {
        private readonly KnowledgeService _knowledgeService;
        private readonly GeminiService _geminiService;


        public ChatService( KnowledgeService knowledgeService, GeminiService geminiService)
        {
            _knowledgeService = knowledgeService;
            _geminiService = geminiService;
        }

        public async Task<string> AskAsync(string question)
        {
            var context = await _knowledgeService.GetContextAsync();
            return await _geminiService.AskWithContextAsync(
                question,
                context
            );
        }
    }
}
