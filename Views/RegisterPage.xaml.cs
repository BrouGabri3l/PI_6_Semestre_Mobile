using projeto_pi.ViewModels;

namespace projeto_pi.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterViewModel();
    }
}