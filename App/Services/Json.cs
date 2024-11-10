using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Services;

public class Json
{
    private static Json? _instance;
    public static Json Instance => _instance ??= new Json();
    public ObservableCollection<Suite> Suites { get; private set; }
    private Json() => Suites = new(JsonHelpers.LoadDirectory<Suite>("Data"));
}

public static class JsonHelpers
{
    internal static string GetPropertyName(string property) =>
        Options.PropertyNamingPolicy == null
            ? property
            : Options.PropertyNamingPolicy.ConvertName(property);

    internal static bool GetProperty<T>(JsonElement root, string property, out T? result)
    {
        var output = root.TryGetProperty(GetPropertyName(property), out var elem);
        result = output ? elem.Deserialize<T>(Options) : default;
        return output;
    }

    internal static T GetProperty<T>(JsonElement root, string property, T fallback) =>
        GetProperty<T>(root, property, out var result) ? result ?? fallback : fallback;

    internal static T GetProperty<T>(JsonElement root, string property) =>
        (GetProperty<T>(root, property, out var result) && result != null)
            ? result
            : throw new JsonException($"{property} property is missing.");

    internal static List<T> LoadDirectory<T>(string path) =>
        Directory.GetFiles(path, "*.json").AsParallel().Select(LoadFile<T>).ToList();

    internal static T LoadFile<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), Options)
        ?? throw new JsonException($"Failed to deserialize {path} JSON.");

    public static readonly JsonSerializerOptions Options =
        new()
        {
            Converters = { new TestConverter(), new TestCostsConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
}

public class TestConverter : JsonConverter<Test>
{
    public override Test? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        if (!root.TryGetProperty(JsonHelpers.GetPropertyName("Type"), out var elem))
            throw new JsonException("Type property is missing.");

        var type = elem.ValueKind switch
        {
            JsonValueKind.String when Enum.TryParse(elem.GetString(), out Types parsedType) =>
                parsedType,
            JsonValueKind.Number => (Types)elem.GetUInt32(),
            _ => throw new JsonException("Test type must be a string or unsigned integer."),
        };

        return type switch
        {
            Types.Costs => root.Deserialize<TestCosts>(options),
            _ => throw new JsonException("Unknown test type."),
        };
    }

    public override void Write(Utf8JsonWriter writer, Test value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, options);
}

public class TestCostsConverter : JsonConverter<TestCosts>
{
    public override TestCosts? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        var test = new TestCosts()
        {
            Name = JsonHelpers.GetProperty<string>(root, "Name"),
            Description = JsonHelpers.GetProperty<string>(root, "Description"),
            Id = JsonHelpers.GetProperty<uint>(root, "Id"),
            Externals = JsonHelpers.GetProperty(root, "Externals", new List<string>())!,
            Answers = JsonHelpers.GetProperty<string[]>(root, "Answers"),
            Costs = JsonHelpers.GetProperty<int[]>(root, "Costs"),
        };
        if (
            !root.TryGetProperty(JsonHelpers.GetPropertyName("Questions"), out var array)
            || array.GetArrayLength() == 0
        )
            throw new JsonException("Questions is missing or null.");
        foreach (var elem in array.EnumerateArray())
        {
            var question = ParseQuestion(elem);
            if (
                (question.Costs ?? test.Costs).Length != (question.Answers ?? test.Answers).Length
                || (question.Costs ?? test.Costs).Length == 0
            )
                throw new JsonException("Costs and Answers must have the same length.");
            test.Questions.Add(question);
        }
        if (
            !root.TryGetProperty(JsonHelpers.GetPropertyName("Results"), out array)
            || array.GetArrayLength() == 0
        )
            throw new JsonException("Results is missing or null.");
        foreach (var elem in array.EnumerateArray())
            test.Results.Push(
                new ResultCosts
                {
                    Text = JsonHelpers.GetProperty<string>(elem, "Text"),
                    From = JsonHelpers.GetProperty(elem, "From", int.MinValue),
                    To = JsonHelpers.GetProperty(elem, "To", int.MaxValue),
                }
            );
        return test;
    }

    private QuestionCosts ParseQuestion(JsonElement elem) =>
        elem.ValueKind switch
        {
            JsonValueKind.String => new QuestionCosts()
            {
                Text =
                    elem.Deserialize<string>()
                    ?? throw new JsonException("Question must be a string or object."),
            },
            JsonValueKind.Object => new QuestionCosts()
            {
                Text = JsonHelpers.GetProperty<string>(elem, "Text"),
                Costs = JsonHelpers.GetProperty<int[]?>(elem, "Costs", null),
                Answers = JsonHelpers.GetProperty<string[]?>(elem, "Answers", null),
            },
            _ => throw new JsonException("Question must be a string or object."),
        };

    public override void Write(
        Utf8JsonWriter writer,
        TestCosts value,
        JsonSerializerOptions options
    ) => JsonSerializer.Serialize(writer, value, options);
}
