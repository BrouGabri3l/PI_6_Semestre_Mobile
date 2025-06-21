using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace projeto_pi.Services;


public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string path, string? token = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            try
            {

                var jsonValue = JsonDocument.Parse(content);
                var message = jsonValue.RootElement.GetProperty("message").GetString();

                throw new Exception($"Erro {response.StatusCode}: {message}");
            }
            catch (JsonException)
            {

                throw new Exception($"Erro {response.StatusCode}: {content}");
            }
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public async Task<T?> PostAsync<T>(string path, object? data = null, string? token = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, path);
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (data != null)
        {
            var json = JsonSerializer.Serialize(data, _options);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            try
            {
                
                var json = JsonDocument.Parse(content);
                var message = json.RootElement.GetProperty("message").GetString();

                throw new Exception($"Erro {response.StatusCode}: {message}");
            }
            catch (JsonException)
            {
                
                throw new Exception($"Erro {response.StatusCode}: {content}");
            }
        }

        return JsonSerializer.Deserialize<T>(content, _options);
    }

    public async Task PostAsync(string path, object? data = null, string? token = null)
    {
        await PostAsync<object?>(path, data, token);
    }
}