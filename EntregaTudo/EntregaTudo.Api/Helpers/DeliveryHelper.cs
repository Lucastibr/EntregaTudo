namespace EntregaTudo.Api.Helpers;

public static class DeliveryHelper
{
    private static readonly Random Random = new();

    public static string GenerateDeliveryCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}