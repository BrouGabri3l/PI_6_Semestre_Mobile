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

    public RecommendationViewModel() { }

    public RecommendationViewModel(IEnumerable<RecommendationModel> recommendations)
    {
        foreach (var rec in recommendations)
            Recommendations.Add(rec);
    }
}