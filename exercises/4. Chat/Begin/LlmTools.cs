namespace Chat;

public static class LlmTools
{
    public static async Task ExtractAndDisplayDetails<T>(
        IChatClient chatClient,
        JsonSerializerOptions? jsonSerializerOptions = null,
        params string[] inputs)
    {
        var jsonShape = JsonTools.GenerateJsonShape<T>();
        var systemMessage = $$"""
            Extract information from the following text.
            Respond in JSON with the following shape:
            {{jsonShape}}
            """;

        foreach (var inputText in inputs)
        {
            var response = await chatClient.GetResponseAsync<T>(
            [
                new(ChatRole.System, systemMessage),
                new(ChatRole.User, inputText)
            ]);

            if (response.TryGetResult(out var info))
            {
                Console.WriteLine(JsonSerializer.Serialize(info, options: jsonSerializerOptions));
            }
            else
            {
                Console.WriteLine("Response was not in the expected format.");
            }
        }
    }
}
