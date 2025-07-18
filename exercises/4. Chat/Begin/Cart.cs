namespace Chat;

public class Cart
{
    public int NumPairsOfSocks { get; set; }

    private const float UnitPrice = 5.99F;

    [Description("Adds the specified number of pairs of socks to the cart")]
    public void AddSocksToCart(
        [Description("The number of pairs of socks to add to the cart")]
        int numPairs)
    {
        NumPairsOfSocks += numPairs;
    }

    [Description("Removes the specified number of pairs of socks from the cart")]
    public void RemoveSocksFromCart(
        [Description("The number of pairs of socks to remove from the cart")]
        int numPairs)
    {
        if (NumPairsOfSocks >= numPairs)
        {
            NumPairsOfSocks -= numPairs;
        }
    }

    [Description("Computes the price of socks, returning a value in dollars.")]
    public static float GetPrice(
        [Description("The number of pairs of socks to calculate price for")] int count)
        => count * UnitPrice;
}
