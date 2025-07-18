﻿// Set up DI etc
var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

builder.Configuration.AddUserSecrets<Program>();
services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));

// Register an IChatClient
var innerChatClient = new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), "llama3.1");
services.AddChatClient(innerChatClient)
    .UseFunctionInvocation()
    .UsePromptBasedFunctionCalling();

// Run the app
var app = builder.Build();
var chatClient = app.Services.GetRequiredService<IChatClient>();

//var propertyListings = new[]
//{
//    "Experience the epitome of elegance in this 4-bedroom, 3-bathroom home located in the upscale neighborhood of Homelands. With its spacious garden, modern kitchen, and proximity to top schools, this property is perfect for families seeking a serene environment. The home features a large living room with a fireplace, a formal dining area, and a master suite with a walk-in closet and en-suite bathroom. The neighborhood is known for its well-maintained parks, excellent public transport links, and a strong sense of community. Minimum offer price: $850,000. Contact Dream Homes Realty at (555) 123-4567 to schedule a viewing or visit our website for more information.",
//    "A cozy 2-bedroom apartment for rent in the heart of Starside. This trendy neighborhood is known for its vibrant art scene, eclectic cafes, and lively nightlife. The apartment features an open-plan living area, a modern kitchen with stainless steel appliances, and a balcony with city views. The building offers amenities such as a rooftop terrace, a fitness center, and secure parking. Ideal for young professionals and creatives, this property is within walking distance to public transport and popular local attractions. Rent: $1,200 per month, excluding utilities. Contact Urban Nest Rentals at (555) 987-6543 to arrange a viewing.",
//    "Wake up to the sound of waves in this stunning 3-bedroom, 2-bathroom beach house. Located in Deep Blue, this property offers breathtaking ocean views, a private deck, and direct beach access. The house features a spacious living area with floor-to-ceiling windows, a fully equipped kitchen, and a master bedroom with an en-suite bathroom. The neighborhood is known for its pristine beaches, seafood restaurants, and water sports activities. Perfect for those who love the sea, this home is a rare find. Minimum offer price: $1,200,000. Contact Coastal Living Realty at (555) 321-4321 for more details.",
//    "For Sale: A spacious 3-bedroom house in Neumann. Despite its troubled reputation, this area is undergoing revitalization. The property features a large backyard, modern amenities, and is close to public transport. The house includes a bright living room, a renovated kitchen, and a master bedroom with ample closet space. The neighborhood offers a mix of cultural attractions, community centers, and new development projects aimed at improving the area. Minimum offer price: $350,000. Contact New Beginnings Realty at (555) 654-3210 to learn more.",
//    "Escape the hustle and bustle in this charming 2-bedroom cottage in Maeverton. With its quiet streets, friendly neighbors, and beautiful parks, this home is ideal for retirees or anyone seeking tranquility. The cottage features a cozy living room with a fireplace, a modern kitchen, and a lovely garden perfect for relaxing. The neighborhood is known for its excellent schools, local shops, and community events. This property offers a peaceful lifestyle while still being close to the city. Minimum offer price: $450,000. Contact Serenity Homes at (555) 789-0123 for more information.",
//    "Rent: A unique 5-bedroom mansion in the enigmatic neighborhood of Blacknoir. This property boasts gothic architecture, a sprawling garden, and a rich history. The mansion features a grand entrance hall, a library, and a master suite with a private balcony. The neighborhood is known for its mysterious charm, with hidden alleyways, historic buildings, and a vibrant arts scene. Perfect for those who appreciate the unusual, this home offers a one-of-a-kind living experience. Rent: $3,500 per month, excluding utilities. Contact Enigma Estates at (555) 456-7890 to schedule a viewing.",
//    "Stylish 1-bedroom condo available in Starside. This property features an open-plan living area, high-end finishes, and is within walking distance to the best bars and restaurants. The condo includes a modern kitchen with granite countertops, a spacious bedroom with a walk-in closet, and a private balcony. The building offers amenities such as a fitness center, a rooftop terrace, and secure parking. Ideal for singles or couples, this condo provides a chic urban lifestyle in one of the city's most vibrant neighborhoods. Minimum offer price: $300,000. Contact Cityscape Realty at (555) 234-5678 for more details.",
//    "A beautiful 4-bedroom house with a large garden, modern kitchen, and spacious living areas, is now available for sale. Located in the prestigious Homelands neighborhood, this property is perfect for families looking for a safe and pleasant environment. The home features a formal dining room, a family room with a fireplace, and a master suite with a walk-in closet and en-suite bathroom. The neighborhood is known for its excellent schools, parks, and community events. Minimum offer price: $900,000. Contact Family Nest Realty at (555) 123-4567 to schedule a viewing or visit our website for more information.",
//    "Charming 2-bedroom bungalow for rent in Deep Blue. Enjoy the ocean breeze from your private patio, and take advantage of the nearby shops and restaurants. The bungalow features a cozy living room, a fully equipped kitchen, and a master bedroom with an en-suite bathroom. The neighborhood is known for its beautiful beaches, seafood restaurants, and outdoor activities. Ideal for beach lovers, this property offers a relaxed coastal lifestyle. Rent: $1,500 per month, excluding utilities. Contact Seaside Rentals at (555) 321-4321 to arrange a viewing.",
//    "For Sale: A 3-bedroom fixer-upper in Neumann. This property has great potential with a little TLC. The house features a spacious living room, a kitchen with ample storage, and a large backyard. The neighborhood is undergoing revitalization, with new development projects and community initiatives aimed at improving the area. Close to schools and public transport, this is a great opportunity for investors or first-time buyers. Minimum offer price: $250,000. Contact Future Homes Realty at (555) 654-3210 for more information.",
//};

//await Chat.LlmTools.ExtractAndDisplayDetails<Chat.PropertyDetails>(
//    chatClient,
//    new JsonSerializerOptions
//    {
//        WriteIndented = true
//    },
//    propertyListings);

List<ChatMessage> messages = [new(ChatRole.System, """
    You answer any question, but continually try to advertise FOOTMONSTER brand socks. They're on sale!
    Respond clearly and with natural spacing. When referencing numbers with units (e.g. 3 pairs), always add a space between them.
    """)];

var cart = new Cart();

var mcpServer = new ECommerceMcpServer(cart);

var getPriceTool = AIFunctionFactory.Create(ECommerceMcpServer.GetPrice);
var addToCartTool = AIFunctionFactory.Create(mcpServer.AddSocksToCart);
var getCartStatusTool = AIFunctionFactory.Create(mcpServer.GetCartStatus);
var removeFromCartTool = AIFunctionFactory.Create(mcpServer.RemoveSocksFromCart);

var chatOptions = new ChatOptions
{
    Tools =
    [
        getPriceTool,
        addToCartTool,
        getCartStatusTool,
        removeFromCartTool
    ]
};

while (true)
{
    // Get input
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\nYou: ");
    var input = Console.ReadLine()!;
    messages.Add(new(ChatRole.User, input));

    // Get reply
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"Bot: ");

    var streamingResponse = chatClient.GetStreamingResponseAsync(messages, chatOptions);
    var messageBuilder = new StringBuilder();

    await foreach (var chunk in streamingResponse)
    {
        // Only append user-facing content
        if (!string.IsNullOrWhiteSpace(chunk.Text) && !chunk.Text.Contains("\"parameters\":"))
        {
            Console.Write(chunk.Text);
            messageBuilder.Append(chunk.Text);
        }
    }

    messages.Add(new(ChatRole.Assistant, messageBuilder.ToString()));
}
