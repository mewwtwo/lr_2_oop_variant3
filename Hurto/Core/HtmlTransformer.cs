using System.Xml;
using System.Xml.Xsl;

namespace Hurto.Core;

public static class HtmlTransformer
{
    public static string Transform(string xmlPath, XmlReader xsl, string attr, string value)
    {
        var xslt = new XslCompiledTransform();
        xslt.Load(xsl);

        var args = new XsltArgumentList();
        args.AddParam("attr", "", attr);
        args.AddParam("value", "", value);

        using var writer = new StringWriter();
        using var reader = XmlReader.Create(xmlPath);
        xslt.Transform(reader, args, writer);
        return writer.ToString();
    }
}
