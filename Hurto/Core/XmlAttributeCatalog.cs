using System.Xml.Linq;

namespace Hurto.Core;

public static class XmlAttributeCatalog
{
    public static Dictionary<string, List<string>> Load(string path)
        => XDocument.Load(path).Descendants("student")
            .SelectMany(s => s.Attributes())
            .GroupBy(a => a.Name.LocalName)
            .ToDictionary(g => g.Key, g => g.Select(a => a.Value).Distinct().Order().ToList());
}
