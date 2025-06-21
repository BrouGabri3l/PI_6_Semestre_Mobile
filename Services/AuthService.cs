using projeto_pi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projeto_pi.Services;

public class AuthService
{
    private readonly ApiService _api;
    private UserModel? _user;

    public string? Token => _user?.Token;
    public UserModel? User => _user;

    public AuthService(ApiService api)
    {
        _api = api;
    }

    public async Task<UserModel?> LoginAsync(string email, string password)
    {
        var data = new { email, password };
        var user = await _api.PostAsync<UserModel>("/login", data);
        _user = user;
        return _user;
    }

    public async Task RegisterAsync(string name, string email, string password)
    {
        var data = new { name, email, password };
        await _api.PostAsync("/sign-in", data);
    }
}