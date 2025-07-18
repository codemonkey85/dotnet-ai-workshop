namespace Chat;

internal class PropertyDetails
{
    public ListingType ListingType { get; set; }
    public required string Neighbourhood { get; set; }
    public int NumBedrooms { get; set; }
    public int Price { get; set; }
    public required string[] Amenities { get; set; }
    public required string TenWordSummary { get; set; }
    public required string ContactInfo { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum ListingType { Sale, Rental }
