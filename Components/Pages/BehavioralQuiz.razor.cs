using System.Text;

using Microsoft.AspNetCore.Components;

using TheTruthHurtsMe.Services;
using TheTruthHurtsMe.Services.Models;

namespace TheTruthHurtsMe.Components.Pages;

public partial class BehavioralQuiz : ComponentBase
{
    [Inject]
    public QuizService QuizService { get; set; } = default!;
    
    [Inject]
    public GPTService GPTService { get; set; } = default!;

    private string? _error;
    private string? _analysis;
    
    private bool _quizStarted;
    private bool _quizCompleted;
    private bool _analysisComplete;
    private bool _isTypingAnalysis;

    private Queue<QuizQuestion>? _questions;
    private QuizQuestion? _currentQuestion;
    private Dictionary<QuizQuestion, string> _quizChoices = new Dictionary<QuizQuestion, string>();

    protected override void OnInitialized()
    {
        ResetQuiz();
    }

    private void StartQuiz()
    {
        ResetQuiz();

        _analysisComplete = false;
        _quizCompleted = false;
        _quizStarted = true;
    }

    private void ResetQuiz()
    {
        _questions = new Queue<QuizQuestion>();
        foreach (var question in QuizService.Questions)
        {
            _questions.Enqueue(question);
        }
        
        if (_questions.Count == 0)
        {
            _error = "There are no questions configured.";
            _quizStarted = false;
            return;
        }
        
        _quizChoices.Clear();
        _currentQuestion = _questions.Dequeue();
    }

    private async Task StoreChoiceAsync(QuizQuestion question, string choice)
    {
        _quizChoices.Add(question, choice);
        
        if (_questions is null ||
            _questions.Count == 0)
        {
            _currentQuestion = null;
            _quizCompleted = true;

            await AnalyzeResultsAsync();
            
            return;
        }
        
        _currentQuestion = _questions.Dequeue();
    }

    private async Task AnalyzeResultsAsync()
    {
        var sb = new StringBuilder();
        foreach (var choice in _quizChoices)
        {
            sb.AppendLine($"{choice.Key.Prompt}{choice.Value},");
        }
        
        var analysis = await GPTService.PromptAsync(sb.ToString(), "I want you to do a deep behavioral analysis of me. Do not repeat the information I am about to tell you and instead give me new information. Make sure to write [NEWLINE] when you are starting a new paragraph: ");
        _analysisComplete = true;

        await TypeResponseAsync(analysis.Replace("[NEWLINE]", "\n").Trim());
    }
    
    private async Task TypeResponseAsync(string response)
    {
        _isTypingAnalysis = true;
        
        int msPerCharacter = 20;
        foreach (var character in response)
        {
            _analysis += character;
            StateHasChanged();
            await Task.Delay(msPerCharacter);
        }
        
        _isTypingAnalysis = false;
    }
}