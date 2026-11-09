using System.Xml;

namespace Hurto.Core;

public class DomSearchStrategy : IXmlSearchStrategy
{
    public string Name => "DOM";

    public List<Student> Search(string path, string attr, string value)
    {
        var doc = new XmlDocument();
        doc.Load(path);

        string literal = value.Contains('\'') ? $"\"{value}\"" : $"'{value}'";
        string query = $"//student[@{attr}={literal}]";

        var result = new List<Student>();
        foreach (XmlNode node in doc.SelectNodes(query)!)
        {
            var student = new Student();
            foreach (XmlAttribute a in node.Attributes!)
                student.Attributes[a.Name] = a.Value;

            foreach (XmlNode r in node.SelectNodes("residence")!)
                student.AddResidence(r.Attributes!["room"]?.Value,
                    r.Attributes["checkIn"]?.Value, r.Attributes["checkOut"]?.Value);

            result.Add(student);
        }
        return result;
    }
}
