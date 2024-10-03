using Microsoft.AspNetCore.Components;

namespace TheTruthHurtsMe.Components.Pages;

public partial class BehavioralQuiz : ComponentBase
{
    private bool _quizStarted;
    
    private void StartQuiz()
    {
        ResetQuiz();
        _quizStarted = true;
    }

    private void ResetQuiz()
    {
        
    }
}