using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Services;

public class Stats : IStats
{
    private static readonly string FilePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "stats.json"
    );

    private readonly ObservableCollection<IResult> data;

    public Stats()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            data = JsonSerializer.Deserialize<ObservableCollection<IResult>>(json, Data.Options)!;
            return;
        }
        data = [];
    }

    public ObservableCollection<IResult> Get() => data;

    public void Push(IResult result)
    {
        data.Add(result);
        var json = JsonSerializer.Serialize(data, Data.Options);
        File.WriteAllText(FilePath, json);
    }

    public void Reset(int id)
    {
        data.Where(result => result.Id == id).ToList().ForEach(item => data.Remove(item));
        File.WriteAllText(FilePath, JsonSerializer.Serialize(data, Data.Options));
    }
}
