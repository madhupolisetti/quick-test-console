using System.Text.Json.Serialization;
using EnsureThat;

namespace QuickTest
{
    public partial class AuthenticationErrorModel : ErrorModel
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(N2PJsonEnumStringConverter<AuthenticationErrorType>))]
        public AuthenticationErrorType? Type { get; set; } = default!;
    }

    public partial class AuthenticationErrorModel
    {
        private AuthenticationErrorModel()
        {
            StatusCode = 401;
            Type = AuthenticationErrorType.AuthenticationError;
        }
        public static AuthenticationErrorModel CreateWithDefaultMessage()
        {
            return new AuthenticationErrorModel
            {
                Message = "Failed to authenticate the request. Make sure the token is passed and valid."
            };
        }

        public static AuthenticationErrorModel CreateWithCustomMessage(
            string message)
        {
            Ensure.String.IsNotNullOrWhiteSpace(message,
                nameof(message));
            return new AuthenticationErrorModel {Message = message};
        }
    }
}