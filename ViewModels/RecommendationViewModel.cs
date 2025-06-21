using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projeto_pi.Models;

namespace projeto_pi.ViewModels;
public class RecommendationViewModel
{
    public ObservableCollection<RecommendationModel> Recommendations { get; } = new();

    public RecommendationViewModel()
    {
        LoadSampleData();
    }

    void LoadSampleData()
    {
        Recommendations.Add(new RecommendationModel
        {
            Title = "Game 1",
            Description = "Descrição do jogo 1",
            Image = "bem_vindo.png"
        });
        Recommendations.Add(new RecommendationModel
        {
            Title = "Game 2",
            Description = "Descrição do jogo 2",
            Image = "splash.png"
        });
    }
}