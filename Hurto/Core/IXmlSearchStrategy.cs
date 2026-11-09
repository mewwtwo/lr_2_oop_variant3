namespace Hurto.Core;

public interface IXmlSearchStrategy
{
    string Name { get; }

    List<Student> Search(string path, string attr, string value);
}
