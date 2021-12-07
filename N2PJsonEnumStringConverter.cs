using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable enable

namespace QuickTest
{
    public class N2PJsonEnumStringConverter<T> : JsonConverter<Object> where T: Enum
    {
        private static readonly Lazy<Dictionary<string,T>> EnumValueByJsonStringLazy = new Lazy<Dictionary<string,T>>(CreateMappingForRead);
        private static readonly Lazy<Dictionary<T,string>> JsonStringByEnumValueLazy = new Lazy<Dictionary<T,string>>(CreateMappingForWrite);
        private static readonly Type NullableType = typeof(Nullable<>).MakeGenericType(typeof(T));

        public override bool CanConvert(Type typeToConvert)
        {
            var isNullableEnum = IsNullableEnum(typeToConvert);
            var result =typeToConvert.IsEnum || isNullableEnum;
            return result;
        }

        private static bool IsNullableEnum(Type type)
        {
            return type.IsGenericType && 
                type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                type.GetGenericArguments().First().IsEnum;
        }

        public override object? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            string? s;
            if(reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Can't deserialize into {typeof(T).Name} enum, because JSON value is not a string.");
            }
            if((s = reader.GetString()) == null)
            {
                if (!typeToConvert.IsEnum)
                {
                    return default;
                }
                throw new JsonException($"Can't deserialize into {typeof(T).Name} enum, because JSON value is null.");
            }
            if(!EnumValueByJsonStringLazy.Value.TryGetValue(s, out var value))
            {
                throw new JsonException($"Unable to convert '{s}' to Enum type {typeToConvert}.");
            }
            if (!typeToConvert.IsEnum)
            {
                return (T)Activator.CreateInstance(NullableType, value)!;
            }
            return value;
        }

        public override void Write(
            Utf8JsonWriter writer,
            object value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(JsonStringByEnumValueLazy.Value.TryGetValue((T)value, out var jsonString) ? jsonString : null);
        }

        private static EnumMemberAttribute? GetEnumMemberAttribute(T value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var attribute = type.GetField(name ?? throw new InvalidOperationException())
                ?.GetCustomAttributes(false)
                .OfType<EnumMemberAttribute>()
                .SingleOrDefault();
            if (attribute == null)
            {
                throw new InvalidOperationException("EnumMemberAttribute is not defined for this field.");
            }
            return attribute;
        }

        private static Dictionary<string,T> CreateMappingForRead()
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>();
            var attributeValues = values.Select(value => new KeyValuePair<string,T>(GetEnumMemberAttribute(value)?.Value!, value));
            return attributeValues.ToDictionary(x => x.Key, x => x.Value);
        }

        private static Dictionary<T,string> CreateMappingForWrite()
        {
            return EnumValueByJsonStringLazy.Value.ToDictionary(x => x.Value, x=> x.Key);
        }
    }
}
