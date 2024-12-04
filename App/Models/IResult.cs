using System;

namespace PMnHRD1.App.Models;

public interface IResult
{
    string Text { get; set; }
    TimeSpan Time { get; set; }
    DateTime Date { get; set; }
    int Id { get; set; }
    string? Name { get; set; }
}
