using projeto_pi.ViewModels;

namespace projeto_pi.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();
    }
    async void OnRegister(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}