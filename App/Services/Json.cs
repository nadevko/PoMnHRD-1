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

    private Json() => Suites = new(LoadDirectory<Suite>().OrderBy(x => x.Id));

    public static string GetPropertyName(string property) =>
        Options.PropertyNamingPolicy == null
            ? property
            : Options.PropertyNamingPolicy.ConvertName(property);

    public static bool GetProperty<T>(JsonElement root, string property, out T? result)
    {
        var output = root.TryGetProperty(GetPropertyName(property), out var elem);
        result = output ? elem.Deserialize<T>(Options) : default;
        return output;
    }

    public static T GetProperty<T>(JsonElement root, string property, T fallback) =>
        GetProperty<T>(root, property, out var result) ? result ?? fallback : fallback;

    public static T GetProperty<T>(JsonElement root, string property) =>
        (GetProperty<T>(root, property, out var result) && result != null)
            ? result
            : throw new JsonException($"{property} property is missing.");

    public static List<T> LoadDirectory<T>() =>
        Directory
            .GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"), "*.json")
            .AsParallel()
            .Select(LoadFile<T>)
            .ToList();

    public static T LoadFile<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), Options)
        ?? throw new JsonException($"Failed to deserialize {path} JSON.");

    public static readonly JsonSerializerOptions Options =
        new()
        {
            Converters = { new TestConverter(), new TestCostsConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
}

public class TestConverter : JsonConverter<ITest>
{
    public override ITest? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        if (!root.TryGetProperty(Json.GetPropertyName("Type"), out var elem))
            throw new JsonException("Type property is missing.");

        var type = elem.ValueKind switch
        {
            JsonValueKind.String when Enum.TryParse(elem.GetString(), out TestTypes parsedType) =>
                parsedType,
            JsonValueKind.Number => (TestTypes)elem.GetUInt32(),
            _ => throw new JsonException("Test type must be a string or unsigned integer."),
        };

        return type switch
        {
            TestTypes.Costs => root.Deserialize<TestCosts>(options),
            _ => throw new JsonException("Unknown test type."),
        };
    }

    public override void Write(Utf8JsonWriter writer, ITest value, JsonSerializerOptions options) =>
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
            Name = Json.GetProperty<string>(root, "Name"),
            Description = Json.GetProperty<string>(root, "Description"),
            Externals = Json.GetProperty(root, "Externals", new List<string>())!,
            Answers = Json.GetProperty<string[]>(root, "Answers"),
            Costs = Json.GetProperty<int[]>(root, "Costs"),
        };

        test.Questions =
            root.TryGetProperty(Json.GetPropertyName("Questions"), out var array)
            && array.GetArrayLength() > 0
                ? array
                    .EnumerateArray()
                    .Select(elem =>
                        elem.ValueKind switch
                        {
                            JsonValueKind.String => new QuestionCosts()
                            {
                                Text =
                                    elem.Deserialize<string>()
                                    ?? throw new JsonException(
                                        "Question must be a string or object."
                                    ),
                            },
                            JsonValueKind.Object => new QuestionCosts()
                            {
                                Text = Json.GetProperty<string>(elem, "Text"),
                                Costs = Json.GetProperty<int[]?>(elem, "Costs", null),
                                Answers = Json.GetProperty<string[]?>(elem, "Answers", null),
                            },
                            _ => throw new JsonException("Question must be a string or object."),
                        }
                    )
                    .Where(question =>
                        (question.Costs ?? test.Costs).Length
                            == (question.Answers ?? test.Answers).Length
                        && (question.Costs ?? test.Costs).Length > 0
                    )
                    .Cast<IQuestion>()
                    .ToList()
                : throw new JsonException("Questions is missing or null.");

        test.Results =
            root.TryGetProperty(Json.GetPropertyName("Results"), out array)
            && array.GetArrayLength() > 0
                ? new Stack<ResultCosts>(
                    array
                        .EnumerateArray()
                        .Select(elem => new ResultCosts
                        {
                            Text = Json.GetProperty<string>(elem, "Text"),
                            From = Json.GetProperty(elem, "From", int.MinValue),
                            To = Json.GetProperty(elem, "To", int.MaxValue),
                        })
                )
                : throw new JsonException("Results is missing or null.");

        return test;
    }

    public override void Write(
        Utf8JsonWriter writer,
        TestCosts value,
        JsonSerializerOptions options
    ) => JsonSerializer.Serialize(writer, value, options);
}
