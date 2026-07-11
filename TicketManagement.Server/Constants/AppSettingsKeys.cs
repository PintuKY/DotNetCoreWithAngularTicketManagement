namespace TicketManagement.Server.Constants
{
    public static class AppSettingsKeys
    {
        public const string JwtKey = "JWT_KEY";
        public const string JwtIssuer = "JWT_ISSUER";
        public const string JwtAudience = "JWT_AUDIENCE";
        public const string JwtDuration = "JWT_DURATION";
        //var key = _configuration[AppSettingsKeys.JwtKey];
        //var key = _configuration["JWT_KEY"];
    }
}
