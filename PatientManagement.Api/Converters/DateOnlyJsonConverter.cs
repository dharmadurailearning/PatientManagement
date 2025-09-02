using System.Text.Json.Serialization;


namespace PatientManagement.Api.Converters
{
    public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        => DateOnly.Parse(reader.GetString()!);
        public override void Write(System.Text.Json.Utf8JsonWriter writer, DateOnly value, System.Text.Json.JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
    }
}