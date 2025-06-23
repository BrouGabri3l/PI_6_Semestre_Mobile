using projeto_pi.Models;
using projeto_pi.ViewModels;

namespace projeto_pi.Views;

public partial class RecommendationPage : ContentPage
{
    public RecommendationPage() : this(Array.Empty<RecommendationModel>()) { }

    public RecommendationPage(IEnumerable<RecommendationModel> recommendations)
    {
        InitializeComponent();
        BindingContext = new RecommendationViewModel(recommendations);
    }
}