using System.Text.Json;
using TheTruthHurtsMe.Services.Models;

namespace TheTruthHurtsMe.Services;

public class QuizService
{
    public List<QuizQuestion> Questions { get; private set; }

    public QuizService()
    {
        Questions = new List<QuizQuestion>();
    }
    
    public async Task LoadQuizAsync()
    {
        using var fs = File.OpenRead("./quiz.json");

        var questions = await JsonSerializer.DeserializeAsync<List<QuizQuestion>>(fs);
        if (questions is null)
        {
            throw new Exception("Failed to find quiz.json");
        }
        
        Questions.Clear();
        Questions.AddRange(questions);
    }
}