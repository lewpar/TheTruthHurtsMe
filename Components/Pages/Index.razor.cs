using Microsoft.AspNetCore.Components;
using TheTruthHurtsMe.Services;

namespace TheTruthHurtsMe.Components.Pages;

public partial class Index : ComponentBase
{
    [Inject]
    public QuizService QuizService { get; set; } = default!;
    
    private int _questionCount;

    protected override void OnInitialized()
    {
        _questionCount = QuizService.Questions.Count;
    }
}