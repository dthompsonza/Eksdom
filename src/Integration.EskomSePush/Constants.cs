namespace Eksdom.Client;

internal static class Constants
{
    public const string Api20Uri = "https://developer.sepush.co.za/business/2.0/";

    public const string ClientDescription = "Eksdom (github.com/dthompsonza/Eksdom)";

    public const int DefaultCacheHours = 2;

    public const int DefaultShortCacheMinutes = 5;

    public static class Resources
    {
        public const string Allowance = "api_allowance";
        public const string AreaInformation = "area";
        public const string Status = "status";
    }

    public static class Headers
    {
        public const string Token = "Token";

        public const string Client = "x-client-name";
    }

    public static class Testing
    {
        public const string Current = "current";

        public const string Future = "future";
    }

    public static class QueryParams
    {
        public const string Test = "test";

        public const string Id = "id";
    }
}
