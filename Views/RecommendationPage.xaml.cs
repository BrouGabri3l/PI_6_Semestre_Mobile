using projeto_pi.ViewModels;

namespace projeto_pi.Views;

public partial class RecommendationPage : ContentPage
{
	public RecommendationPage()
	{
		InitializeComponent();

        BindingContext = new RecommendationViewModel();
    }
}