using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Lib;

public class TestConverter : JsonConverter<Test>
{
    public override Test? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        Types? type = null;
        if (root.TryGetProperty("Type", out var typeElem))
        {
            type = typeElem.ValueKind switch
            {
                JsonValueKind.String => Enum.TryParse(typeElem.GetString(), out Types testType)
                    ? testType
                    : throw new JsonException("Unknown test type."),
                JsonValueKind.Number => (Types)typeElem.GetUInt32(),
                _ => throw new JsonException("Test type must be a string or unsigned integer."),
            };
        }
        return (type ?? Types.Costs) switch
        {
            Types.Costs => JsonSerializer.Deserialize<TestCosts>(root.GetRawText(), options),
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
            Name =
                GetProperty<string?>(root, "Name", options, null)
                ?? throw new JsonException("Name property is missing."),
            Description =
                GetProperty<string?>(root, "Description", options, null)
                ?? throw new JsonException("Description property is missing."),
            Topic =
                GetProperty<uint?>(root, "Topic", options, null)
                ?? throw new JsonException("Topic property is missing."),
            Externals = GetProperty(root, "Externals", options, new List<string>()),
        };
        var defaultCosts = GetProperty<int[]>(root, "Costs", options, []);
        var defaultAnswers = GetProperty<string[]>(root, "Answers", options, []);
        if (
            !root.TryGetProperty("Questions", out var questionElems)
            || questionElems.GetArrayLength() == 0
        )
            throw new JsonException("Questions is missing or null.");
        foreach (var questionElem in questionElems.EnumerateArray())
        {
            var question = questionElem.ValueKind switch
            {
                JsonValueKind.String => new QuestionCosts()
                {
                    Text =
                        questionElem.GetString()
                        ?? throw new JsonException("Text property is missing or null."),
                    Costs = defaultCosts,
                    Answers = defaultAnswers,
                },
                JsonValueKind.Object => new QuestionCosts()
                {
                    Text =
                        GetProperty<string>(questionElem, "Text", options, null)
                        ?? throw new JsonException("Text property is missing or null."),
                    Costs = GetProperty(questionElem, "Costs", options, defaultCosts),
                    Answers = GetProperty(questionElem, "Answers", options, defaultAnswers),
                },
                _ => throw new JsonException("Question must be a string or object."),
            };
            if (question.Costs.Length != question.Answers.Length || question.Costs.Length == 0)
                throw new JsonException("Costs and Answers must have the same length.");
            test.Questions.Add(question);
        }
        if (
            !root.TryGetProperty("Results", out var resultElems)
            || resultElems.GetArrayLength() == 0
        )
            throw new JsonException("Results is missing or null.");
        foreach (var resultElem in resultElems.EnumerateArray())
            test.Results.Push(
                new ResultCosts
                {
                    Text =
                        GetProperty<string>(resultElem, "Text", options, null)
                        ?? throw new JsonException("Text property is missing or null."),
                    From = GetProperty(resultElem, "From", options, int.MinValue),
                    To = GetProperty(resultElem, "To", options, int.MaxValue),
                }
            );
        if (!TestCoverage(test.Results))
            throw new JsonException("Results not cover full range.");
        return test;
    }

    private static T GetProperty<T>(
        JsonElement root,
        string property,
        JsonSerializerOptions options,
        T? defaultValue
    ) =>
        root.TryGetProperty(property, out var elem)
            ? JsonSerializer.Deserialize<T>(elem.GetRawText(), options)
                ?? throw new JsonException($"{property} property is null.")
            : defaultValue ?? throw new JsonException($"{property} property is missing.");

    private static bool TestCoverage(Stack<ResultCosts> stack)
    {
        var ranges = stack.OrderBy(i => i.From).ThenByDescending(i => i.To);
        for (var i = 1; i < ranges.Count(); i++)
            if (ranges.ElementAt(i).From <= ranges.ElementAt(i - 1).To)
                return false;
        return true;
    }

    public override void Write(
        Utf8JsonWriter writer,
        TestCosts value,
        JsonSerializerOptions options
    ) => JsonSerializer.Serialize(writer, value, options);
}
