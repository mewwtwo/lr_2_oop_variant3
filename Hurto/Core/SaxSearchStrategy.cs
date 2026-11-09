using System.Xml;

namespace Hurto.Core;

public class SaxSearchStrategy : IXmlSearchStrategy
{
    public string Name => "SAX";

    public List<Student> Search(string path, string attr, string value)
    {
        var result = new List<Student>();
        Student? current = null;

        using var reader = XmlReader.Create(path);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "student")
            {
                current = new Student();
                while (reader.MoveToNextAttribute())
                    current.Attributes[reader.Name] = reader.Value;
            }
            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "residence")
            {
                current?.AddResidence(reader.GetAttribute("room"),
                    reader.GetAttribute("checkIn"), reader.GetAttribute("checkOut"));
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "student")
            {
                if (current?.Attributes.GetValueOrDefault(attr) == value)
                    result.Add(current);
                current = null;
            }
        }
        return result;
    }
}
