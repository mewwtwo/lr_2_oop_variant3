using System.Xml.Linq;

namespace Hurto.Core;

public class LinqSearchStrategy : IXmlSearchStrategy
{
    public string Name => "LINQ to XML";

    public List<Student> Search(string path, string attr, string value)
    {
        var doc = XDocument.Load(path);

        var query = from s in doc.Descendants("student")
                    where (string?)s.Attribute(attr) == value
                    select s;

        var result = new List<Student>();
        foreach (var s in query)
        {
            var student = new Student();
            foreach (var a in s.Attributes())
                student.Attributes[a.Name.LocalName] = a.Value;

            foreach (var r in s.Elements("residence"))
                student.AddResidence((string?)r.Attribute("room"),
                    (string?)r.Attribute("checkIn"), (string?)r.Attribute("checkOut"));

            result.Add(student);
        }
        return result;
    }
}
