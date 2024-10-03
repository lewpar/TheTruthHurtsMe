using System.Text;

using OpenAI.Chat;

namespace TheTruthHurtsMe.Services;

public class GPTService
{
    private ChatClient _chatClient;

    public GPTService(string apiKey, string model = "gpt-3.5-turbo")
    {
        _chatClient = new ChatClient(model: model, apiKey: apiKey);
    }
    
    public async Task<string> PromptAsync(string prompt, string systemMessage = "You are a helpful AI assistant.")
    {
        var result = await _chatClient.CompleteChatAsync($"{systemMessage} {prompt}");
        var completion = result.Value;

        var sb = new StringBuilder();
        foreach (var content in completion.Content)
        {
            sb.Append(content.Text);
        }

        return sb.ToString();
    }
}