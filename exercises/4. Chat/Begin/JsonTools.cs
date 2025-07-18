namespace Chat;

internal static class JsonTools
{
    public static string GenerateJsonShape<T>()
    {
        string GetJsonType(Type t)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;

            return t switch
            {
                var type when type.IsEnum => "string", // Will be serialized as a string due to JsonStringEnumConverter
                var type when type == typeof(string) => "string",
                var type when type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal) => "number",
                var type when type == typeof(bool) => "boolean",
                var type when type.IsArray => $"[{GetJsonType(type.GetElementType()!)}]",
                _ => "object"
            };
        }

        string GetEnumComment(Type t)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;
            return t.IsEnum
                ? $" // one of: {string.Join(", ", Enum.GetNames(t).Select(n => $"\"{n}\""))}"
                : string.Empty;
        }

        var type = typeof(T);

        var props = type.GetProperties();
        var lines = props.Select(p =>
        {
            var name = p.Name;
            var jsonType = GetJsonType(p.PropertyType);
            var comment = GetEnumComment(p.PropertyType);
            return $"  \"{name}\": {jsonType}{comment}";
        });

        return $"{{\n{string.Join(",\n", lines)}\n}}";
    }
}
