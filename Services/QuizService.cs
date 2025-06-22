using projeto_pi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace projeto_pi.Services;

public class QuizService
{
    private readonly ApiService _api;
    private readonly AuthService _auth;

    public QuizService(ApiService api, AuthService auth)
    {
        _api = api;
        _auth = auth;
    }

    public async Task<List<QuestionModel>> GetQuestionsAsync()
    {
        var jsonList = await _api.GetAsync<JsonElement[]>("/quiz/template", _auth.Token);
        var questions = new List<QuestionModel>();
        if (jsonList == null) return questions;
        foreach (var item in jsonList)
        {
            var q = new QuestionModel
            {
                Text = item.GetProperty("question").GetString() ?? string.Empty,
                Tag = item.GetProperty("tag").GetString() ?? string.Empty,
                MinimumSelections = item.GetProperty("min_length").GetInt32(),
            };
            foreach (var opt in item.GetProperty("options").EnumerateArray())
            {
                q.Options.Add(new OptionModel
                {
                    Text = opt.GetProperty("answer").GetString() ?? string.Empty,
                    Value = opt.GetProperty("value").GetString() ?? string.Empty,
                });
            }
            questions.Add(q);
        }
        return questions;
    }
    public async Task<(List<OptionModel> Options, bool HasNextPage)> GetGameOptionsAsync(int page)
    {
        var path = $"/games/quiz/template?page={page}&perPage=50";
        var json = await _api.GetAsync<JsonElement>(path, _auth.Token);
        var list = new List<OptionModel>();
        bool hasNext = false;
        if (json.ValueKind == JsonValueKind.Object)
        {
            foreach (var item in json.GetProperty("data").EnumerateArray())
            {
                list.Add(new OptionModel
                {
                    Text = item.GetProperty("answer").GetString() ?? string.Empty,
                    Value = item.GetProperty("value").GetInt32().ToString(),
                    Image = item.GetProperty("image").GetString() ?? string.Empty
                });
            }
            hasNext = json.GetProperty("pagination").GetProperty("hasNextPage").GetBoolean();
        }
        return (list, hasNext);
    }
    public async Task SubmitQuizAsync(Dictionary<string, object> answers)
    {
        await _api.PostAsync("/quiz", answers, _auth.Token);
    }
}
