namespace PMnHRD1.App.Models;

public interface IQuestion
{
    string Text { get; set; }
    string[]? Answers { get; set; }
}
