using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using projeto_pi.Models;
using projeto_pi.Services;
using CommunityToolkit.Maui.Alerts;

namespace projeto_pi.ViewModels;

public class QuizViewModel : INotifyPropertyChanged
{
    int _currentIndex;
    bool _isBusy;

    public ObservableCollection<QuestionModel> Questions { get; } = new();

    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (_currentIndex != value)
            {
                _currentIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }
    }

    public QuestionModel? CurrentQuestion =>
        CurrentIndex < Questions.Count ? Questions[CurrentIndex] : null;

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public Command NextCommand { get; }

    readonly QuizService _quizService;

    public QuizViewModel() : this(MauiProgram.Services.GetRequiredService<QuizService>()) { }

    public QuizViewModel(QuizService quizService)
    {
        _quizService = quizService;
        NextCommand = new Command(async () => await NextAsync());
        _ = LoadQuestionsAsync();
    }

    async Task LoadQuestionsAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _quizService.GetQuestionsAsync();
            Questions.Clear();
            foreach (var q in list)
                Questions.Add(q);
        }
        catch (Exception ex)
        {
            await Snackbar.Make($"Erro ao buscar perguntas: {ex.Message}").Show();
        }
        IsBusy = false;
    }

    async Task NextAsync()
    {
        if (CurrentQuestion == null) return;
        var selected = CurrentQuestion.Options.Count(o => o.IsSelected);
        if (selected < CurrentQuestion.MinimumSelections) return;
        if (CurrentIndex < Questions.Count - 1)
        {
            CurrentIndex++;
        }
        else
        {
            var answers = new Dictionary<string, object>();
            foreach (var q in Questions)
            {
                var values = q.Options.Where(o => o.IsSelected).Select(o => (object)o.Value).ToList();
                answers[q.Tag] = values;
            }
            try
            {
                await _quizService.SubmitQuizAsync(answers);
                await Snackbar.Make("Quiz enviado com sucesso").Show();
                await Application.Current!.MainPage!.Navigation.PushAsync(new Views.RecommendationPage());
            }
            catch (Exception ex)
            {
                await Snackbar.Make($"Erro ao enviar quiz: {ex.Message}").Show();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}