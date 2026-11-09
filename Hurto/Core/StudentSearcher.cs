namespace Hurto.Core;

public class StudentSearcher
{
    private IXmlSearchStrategy _strategy;

    public StudentSearcher(IXmlSearchStrategy strategy) => _strategy = strategy;

    public void SetStrategy(IXmlSearchStrategy strategy) => _strategy = strategy;

    public List<Student> Search(string path, string attr, string value)
        => _strategy.Search(path, attr, value);
}
