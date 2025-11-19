using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace WellWork.Application;

public class GroqLLMService : ILLMService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GroqLLMService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["Groq:ApiKey"]
            ?? throw new Exception("Groq ApiKey não configurada.");
    }

    public async Task<LLMResult> GenerateMessageForCheckInAsync(
        string mood,
        string energy,
        string? notes)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        var prompt =
            $"O usuário fez um check-in com os seguintes dados:\n" +
            $"- Humor: {mood}\n" +
            $"- Energia: {energy}\n" +
            $"- Observações: {notes}\n\n" +
            "Gere uma mensagem empática, curta (máximo 250 caracteres), motivacional e adequada ao estado emocional. " +
            "Além disso, gere também um nível de confiança entre 0 e 1 baseado na clareza e assertividade do estado emocional. " +
            "Retorne APENAS um JSON no formato: {\"message\":\"...\", \"confidence\":0.xx}";

        var body = new
        {
            model = "llama-3.1-8b-instant",   // 🔥 Modelo suportado
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var json = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(
            "https://api.groq.com/openai/v1/chat/completions",
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao chamar Groq API ({response.StatusCode}): {err}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseContent);

        var content = doc
            .RootElement.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (content is null)
            throw new Exception("IA retornou conteúdo vazio.");

        // Agora o content é um JSON dentro da string retornada
        using var msgDoc = JsonDocument.Parse(content);

        var message = msgDoc.RootElement.GetProperty("message").GetString()
            ?? throw new Exception("Campo 'message' não encontrado no JSON retornado.");

        var confidence = msgDoc.RootElement.GetProperty("confidence").GetDecimal();

        return new LLMResult(message, confidence);
    }
}