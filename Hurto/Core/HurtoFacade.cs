using System.Text;
using System.Xml;

namespace Hurto.Core;

public class HurtoFacade
{
    private readonly Dictionary<string, IXmlSearchStrategy> _strategies = new IXmlSearchStrategy[]
    {
        new SaxSearchStrategy(), new DomSearchStrategy(), new LinqSearchStrategy()
    }.ToDictionary(s => s.Name);

    private readonly StudentSearcher _searcher;
    private Dictionary<string, List<string>> _catalog = new();

    public string? FilePath { get; private set; }

    public HurtoFacade() => _searcher = new StudentSearcher(_strategies.Values.First());

    public void LoadFile(string path)
    {
        _catalog = XmlAttributeCatalog.Load(path);
        FilePath = path;
    }

    public IEnumerable<string> GetAttributeNames() => _catalog.Keys;

    public IEnumerable<string> GetValues(string attr) => _catalog[attr];

    public string Search(string method, string attr, string value)
    {
        _searcher.SetStrategy(_strategies[method]);
        var students = _searcher.Search(FilePath!, attr, value);

        var sb = new StringBuilder($"Метод: {method}. Запит: {attr} = {value}\nЗнайдено: {students.Count}\n\n");
        foreach (var s in students)
        {
            sb.AppendLine(string.Join("; ", s.Attributes.Select(a => $"{a.Key}={a.Value}")));
            foreach (var r in s.Residences) sb.AppendLine("    " + r);
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public string ToHtml(XmlReader xsl, string attr, string value)
        => HtmlTransformer.Transform(FilePath!, xsl, attr, value);
}
