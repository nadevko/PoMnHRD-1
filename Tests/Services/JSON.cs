using System.Runtime.CompilerServices;
using System.Text.Json;
using DeepEqual.Syntax;
using PMnHRD1.App.Services;
using PMnHRD1.App.Models;

namespace PMnHRD1.Tests.Services;

[TestClass]
public class JSON
{
    private static string CallerFilePath([CallerFilePath] string? callerFilePath = null) =>
        callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath));

    private static string GetSuitePath(string suite) =>
        Path.Combine(
            Directory.GetParent(CallerFilePath())!.Parent!.FullName,
            "Data",
            suite + ".json"
        );

    [TestInitialize]
    public void SetUp()
    {
        _options = JsonHelpers.Options;
    }

    private JsonSerializerOptions? _options;

    [TestMethod]
    [DynamicData(nameof(TestPairs), DynamicDataSourceType.Method)]
    public void Read_ValidJSON_WithoutExceptions(string path, Suite expected)
    {
        Assert.IsTrue(File.Exists(path = GetSuitePath(path)));
        var suite = File.ReadAllText(path);

        var actual = JsonSerializer.Deserialize<Suite>(suite, _options);

        if (expected.Tests != null)
            if (expected.Tests[0] != null)
                if (expected.Tests[0].Results.Count == 3)
                {
                    expected.Tests[0].Results.Pop();
                    expected.Tests[0].Results.Pop();
                }
        actual.WithDeepEqual(expected).Assert();
    }

    private static IEnumerable<object[]> TestPairs() =>
        [
            [
                "camelCase",
                new Suite
                {
                    Name = "Examples of tests",
                    Description = "Every suite and test must have all those fields",
                    Id = 1,
                    Externals = [],
                },
            ],
            [
                "costs",
                new Suite
                {
                    Name = "Examples of tests",
                    Description = "Every suite and test must have all those fields",
                    Id = 1,
                    Externals = ["Literature and web references of the suite"],
                    Tests =
                    [
                        new TestCosts
                        {
                            Name = "Test with results based on costs",
                            Description = "Result will be choosen by sum of answers costs",
                            Id = 1,
                            Externals = ["Literature and web references of the suite"],
                            Costs = [-2, -1, 1, 2],
                            Answers =
                            [
                                "First answers has first cost",
                                "User can choose only one answer",
                                "Answers will be written on buttons",
                                "Number of answers must be equal to number of costs",
                            ],
                            Questions =
                            [
                                new QuestionCosts { Text = "Use default costs and answers" },
                                new QuestionCosts { Text = "Override costs", Costs = [1, 2, 3, 4] },
                                new QuestionCosts
                                {
                                    Text = "Override answers",
                                    Answers = ["First", "Second", "Third", "Fourth"],
                                },
                                new QuestionCosts
                                {
                                    Text =
                                        "Override costs and answers. Now you can change number of them",
                                    Costs = [1, 2, 3],
                                    Answers = ["First", "Second", "Third"],
                                },
                                new QuestionCosts
                                {
                                    Text = "Eqivalent to question with Text field only",
                                },
                            ],
                            Results = new Stack<ResultCosts>(
                                [
                                    new ResultCosts
                                    {
                                        Text = "This result will be shown if from <= sum < to",
                                        From = -2,
                                        To = 0,
                                    },
                                    new ResultCosts
                                    {
                                        Text =
                                            "If range limit not passed, program assumes it is infinity: 0 <= sum < inf",
                                        From = 0,
                                        To = int.MaxValue,
                                    },
                                ]
                            ),
                        },
                    ],
                },
            ],
        ];
}
