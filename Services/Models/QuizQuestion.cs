namespace TheTruthHurtsMe.Services.Models;

public class QuizQuestion
{
    public required string Question { get; set; }
    public required string Prompt { get; set; }
    public required List<string> Choices { get; set; }
}