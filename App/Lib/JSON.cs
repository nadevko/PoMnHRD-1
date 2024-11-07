using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Lib;

public class Helpers
{
    public required JsonSerializerOptions Options { get; set; }

    public string GetPropertyName(string property) =>
        Options.PropertyNamingPolicy == null
            ? property
            : Options.PropertyNamingPolicy.ConvertName(property);

    public bool GetProperty<T>(JsonElement root, string property, out T? result)
    {
        var output = root.TryGetProperty(GetPropertyName(property), out var elem);
        result = output ? elem.Deserialize<T>(Options) : default;
        return output;
    }

    public T GetProperty<T>(JsonElement root, string property, T fallback) =>
        GetProperty<T>(root, property, out var result) ? result ?? fallback : fallback;

    public T GetProperty<T>(JsonElement root, string property) =>
        (GetProperty<T>(root, property, out var result) && result != null)
            ? result
            : throw new JsonException($"{property} property is missing.");
}

public class TestConverter : JsonConverter<Test>
{
    private Helpers? Helpers;

    public override Test? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        Helpers = new Helpers { Options = options };
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        if (!root.TryGetProperty(Helpers.GetPropertyName("Type"), out var elem))
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
    private Helpers? Helpers;

    public override TestCosts? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        Helpers = new Helpers { Options = options };
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        var test = new TestCosts()
        {
            Name = Helpers.GetProperty<string>(root, "Name"),
            Description = Helpers.GetProperty<string>(root, "Description"),
            Topic = Helpers.GetProperty<uint>(root, "Topic"),
            Externals = Helpers.GetProperty(root, "Externals", new List<string>())!,
            Answers = Helpers.GetProperty<string[]>(root, "Answers"),
            Costs = Helpers.GetProperty<int[]>(root, "Costs"),
        };
        if (
            !root.TryGetProperty(Helpers.GetPropertyName("Questions"), out var array)
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
            !root.TryGetProperty(Helpers.GetPropertyName("Results"), out array)
            || array.GetArrayLength() == 0
        )
            throw new JsonException("Results is missing or null.");
        foreach (var elem in array.EnumerateArray())
            test.Results.Push(
                new ResultCosts
                {
                    Text = Helpers.GetProperty<string>(elem, "Text"),
                    From = Helpers.GetProperty(elem, "From", int.MinValue),
                    To = Helpers.GetProperty(elem, "To", int.MaxValue),
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
                Text = Helpers!.GetProperty<string>(elem, "Text"),
                Costs = Helpers.GetProperty<int[]?>(elem, "Costs", null),
                Answers = Helpers.GetProperty<string[]?>(elem, "Answers", null),
            },
            _ => throw new JsonException("Question must be a string or object."),
        };

    public override void Write(
        Utf8JsonWriter writer,
        TestCosts value,
        JsonSerializerOptions options
    ) => JsonSerializer.Serialize(writer, value, options);
}
