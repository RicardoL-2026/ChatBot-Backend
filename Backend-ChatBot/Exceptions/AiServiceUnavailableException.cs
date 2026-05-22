namespace Backend_ChatBot.Exceptions
{
    public class AiServiceUnavailableException : Exception
    {
        public AiServiceUnavailableException(string message) : base(message) { }
    }
}