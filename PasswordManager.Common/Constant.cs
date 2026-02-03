namespace PasswordManager.Common
{
    public class Constant
    {
        public class Api
        {
            public const string Version = "v1";
            public const string BaseUrl = "api/" + Version;
        }

        public class Database
        {
            public const string DbName = "PasswordManagerDb";
            public const string DbConnectionName = "PasswordManagerDbConnection";
        }

        public class Authentication
        {
            public const string Scheme = "PasswordManagerScheme";
            public const string AntiforgeryTokeName = "PM-XSR-TOKEN";
            public class Jwt
            {
                public const string Key = "PMJWT:Key";
                public const string Issuer = "PMJWT:Issuer";
                public const string Audience = "PMJWT:Audience";
                public const int ExpireInHours = 2;
                public const int RefreshExpireInDays = 7;
            }
        }        
    }
}
