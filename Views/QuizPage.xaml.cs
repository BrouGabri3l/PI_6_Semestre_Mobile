using projeto_pi.ViewModels;

namespace projeto_pi.Views;

public partial class QuizPage : ContentPage
{
	public QuizPage()
	{
		InitializeComponent();
        BindingContext = new QuizViewModel();
    }

    async void OnRemainingItemsThresholdReached(object sender, EventArgs e)
    {
        if (BindingContext is QuizViewModel vm)
            await vm.LoadMoreOptionsAsync();
    }
}