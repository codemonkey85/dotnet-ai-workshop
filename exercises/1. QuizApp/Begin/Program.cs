using QuizApp.Components;
using Microsoft.Extensions.AI;
using Azure.AI.OpenAI;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

//  - Decide which LLM backend you're going to use, and register it in DI
//  - It can be any IChatClient implementation, for example AzureOpenAIClient or OllamaChatClient
//  - See instructions for sample code
// Note that AzureOpenAIClient works with both GitHub Models and Azure OpenAI endpoints

var useGitHubModels = builder.Configuration.GetValue("UseGitHubModels", false);
IChatClient innerChatClient;

if (useGitHubModels)
{
    const string gitHubAiConfigurationSection = "GitHubAI";

    var aiEndpoint = builder.Configuration[$"{gitHubAiConfigurationSection}:Endpoint"];
    if (string.IsNullOrEmpty(aiEndpoint))
    {
        throw new InvalidOperationException($"{gitHubAiConfigurationSection}:Endpoint configuration is required.");
    }

    var aiKey = builder.Configuration[$"{gitHubAiConfigurationSection}:Key"];
    if (string.IsNullOrEmpty(aiKey))
    {
        throw new InvalidOperationException($"{gitHubAiConfigurationSection}:Key configuration is required.");
    }

    innerChatClient = new AzureOpenAIClient(
        new Uri(aiEndpoint),
        new ApiKeyCredential(aiKey))
        .GetChatClient("gpt-4o-mini").AsIChatClient();
}
else
{
    innerChatClient = new OllamaSharp.OllamaApiClient(
        new Uri("http://localhost:11434"), "llama3.1");
}

builder.Services.AddChatClient(innerChatClient);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
