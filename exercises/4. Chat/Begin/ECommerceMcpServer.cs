namespace Chat;

public class ECommerceMcpServer(Cart cart)
{
    private readonly Cart _cart = cart;

    [Description("Computes the price of socks, returning a value in dollars")]
    public static float GetPrice([Description("The number of pairs of socks to calculate price for")] int count)
        => Cart.GetPrice(count);

    [Description("Adds the specified number of pairs of socks to the cart")]
    public void AddSocksToCart([Description("The number of pairs to add")] int numPairs)
        => _cart.AddSocksToCart(numPairs);

    [Description("Removes the specified number of pairs of socks from the cart")]
    public void RemoveSocksFromCart([Description("The number of pairs to remove")] int numPairs)
        => _cart.RemoveSocksFromCart(numPairs);

    [Description("Gets the current cart contents")]
    public object GetCartStatus() => new
    {
        totalItems = _cart.NumPairsOfSocks,
        totalPrice = Cart.GetPrice(_cart.NumPairsOfSocks),
        currency = "USD"
    };
}
