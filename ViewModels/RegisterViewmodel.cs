using projeto_pi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
namespace projeto_pi.ViewModels;


public class RegisterViewModel : INotifyPropertyChanged
{
    readonly AuthService _authService;

    string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    string _email = string.Empty;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    string _password = string.Empty;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public Command RegisterCommand { get; }

    public RegisterViewModel() : this(MauiProgram.Services.GetRequiredService<AuthService>()) { }

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
        RegisterCommand = new Command(async () => await RegisterAsync());
    }

    async Task RegisterAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            await _authService.RegisterAsync(Name, Email, Password);
            IsBusy = false;
            await Snackbar.Make("Cadastro realizado com sucesso").Show();
            await Application.Current!.MainPage!.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            IsBusy = false;
            await Snackbar.Make($"Erro ao cadastrar: {ex.Message}").Show();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
