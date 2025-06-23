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
    int _gamesPage = 1;
    bool _gamesHasNext = true;
    bool _loadingGames;
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
            CurrentIndex = 0;
            MainThread.BeginInvokeOnMainThread(() =>
                OnPropertyChanged(nameof(CurrentQuestion)));
            await LoadGameOptionsAsync();
        }
        catch (Exception ex)
        {
            await Snackbar.Make($"Erro ao buscar perguntas: {ex.Message}").Show();
        }
        IsBusy = false;
    }

    async Task LoadGameOptionsAsync()
    {
        if (_loadingGames || !_gamesHasNext) return;
        _loadingGames = true;
        try
        {
            var question = Questions.FirstOrDefault(q => q.Tag == "FAVORITE_GAMES");
            if (question != null)
            {
                var result = await _quizService.GetGameOptionsAsync(_gamesPage);
                foreach (var opt in result.Options)
                    question.Options.Add(opt);
                _gamesPage++;
                _gamesHasNext = result.HasNextPage;
            }
        }
        catch (Exception ex)
        {
            await Snackbar.Make($"Erro ao buscar jogos: {ex.Message}").Show();
        }
        _loadingGames = false;
    }

    async Task NextAsync()
    {
        if (CurrentQuestion == null) return;
        var selected = CurrentQuestion.Options.Count(o => o.IsSelected);
        if (selected < CurrentQuestion.MinimumSelections) return;
        if (CurrentIndex < Questions.Count - 1)
        {
            CurrentIndex++;
            return;
        }

        var answers = new Dictionary<string, object>();
        foreach (var q in Questions)
        {
            var values = q.Options
                .Where(o => o.IsSelected)
                .Select(o =>
                    q.Tag == "FAVORITE_GAMES"
                        ? (object)int.Parse(o.Value)
                        : o.Value)
                .ToList();
            answers[q.Tag] = values;
        }
        try
        {
            var profile = await _quizService.SubmitQuizAsync(answers);
            var recommendations = await _quizService.GetRecommendationsAsync();
            await Snackbar.Make("Quiz enviado com sucesso").Show();
            await Application.Current!.MainPage!.Navigation.PushAsync(new Views.RecommendationPage(recommendations));
        }
        catch (Exception ex)
        {
            await Snackbar.Make($"Erro ao enviar quiz: {ex.Message}").Show();
        }

    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public async Task LoadMoreOptionsAsync()
    {
        if (CurrentQuestion?.Tag == "FAVORITE_GAMES")
            await LoadGameOptionsAsync();
    }
}