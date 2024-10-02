using System.Text;

using OpenAI.Chat;

namespace TheTruthHurtsMe.Services;

public class GPTService
{
    public string SystemMessage { get; set; }
    
    private ChatClient _chatClient;

    public GPTService(string apiKey, string model = "gpt-3.5-turbo")
    {
        SystemMessage = "You are a helpful AI assistant.";
        _chatClient = new ChatClient(model: model, apiKey: apiKey);
    }
    
    public async Task<string> PromptAsync(string prompt)
    {
        var result = await _chatClient.CompleteChatAsync($"{SystemMessage} {prompt}");
        var completion = result.Value;

        var sb = new StringBuilder();
        foreach (var content in completion.Content)
        {
            sb.Append(content.Text);
        }

        return sb.ToString();
    }
}