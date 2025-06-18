using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using projeto_pi.Models;

namespace projeto_pi.ViewModels
{
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

        public QuizViewModel()
        {
            NextCommand = new Command(Next);
            LoadSampleData();
        }

        void LoadSampleData()
        {
            Questions.Add(new QuestionModel
            {
                Text = "Quais plataformas você prefere?",
                Options = new ObservableCollection<OptionModel>
            {
                new OptionModel { Text = "PC" },
                new OptionModel { Text = "Console" },
                new OptionModel { Text = "Mobile" },
            },
                MinimumSelections = 1
            });
            Questions.Add(new QuestionModel
            {
                Text = "Quais gêneros você gosta?",
                Options = new ObservableCollection<OptionModel>
            {
                new OptionModel { Text = "Ação" },
                new OptionModel { Text = "Esporte" },
                new OptionModel { Text = "Estratégia" },
            },
                MinimumSelections = 1
            });
        }

        void Next()
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
                Application.Current!.MainPage!.Navigation.PushAsync(new Views.RecommendationPage());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
