using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace QuickTest
{
    [JsonConverter(typeof(N2PJsonEnumStringConverter<AuthenticationErrorType>))]
    public enum AuthenticationErrorType
    {
        [EnumMember(Value = "authentication_error")]
        AuthenticationError = 1
    }
}