namespace Hurto.Core;

public class Student
{
    public Dictionary<string, string> Attributes { get; } = new();
    public List<string> Residences { get; } = new();

    public void AddResidence(string? room, string? checkIn, string? checkOut)
        => Residences.Add($"кімната {room}: {checkIn} - {checkOut}");
}
