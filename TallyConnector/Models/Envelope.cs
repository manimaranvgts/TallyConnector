﻿namespace TallyConnector.Models;

[XmlRoot(ElementName = "ENVELOPE")]
public class Envelope<T> : TallyXmlJson where T : TallyXmlJson, ITallyObject
{
    public Envelope()
    {
    }

    public Envelope(T ObjecttoExport, StaticVariables staticVariables)
    {
        Body = new();
        Header = new(Request: RequestTye.Import, Type: HType.Data, ID: "All Masters");
        Body.Desc.StaticVariables = staticVariables;
        Body.Data.Message.Objects.Add(ObjecttoExport);
    }
    public Envelope(List<T> ObjectstoExport, StaticVariables staticVariables)
    {
        Body = new();
        Header = new(Request: RequestTye.Import, Type: HType.Data, ID: "All Masters");
        Body.Desc.StaticVariables = staticVariables;
        Body.Data.Message.Objects.AddRange(ObjectstoExport);
    }
   

    [XmlElement(ElementName = "HEADER")]
    public Header Header { get; set; }

    [XmlElement(ElementName = "BODY")]
    public Body<T> Body { get; set; } = new();


    public new string GetXML(XmlAttributeOverrides attrOverrides = null)
    {
        //Gets Root attribute of ReturnObject
        XmlRootAttribute RootAttribute = (XmlRootAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(XmlRootAttribute));
        //ElementName of ReturnObject will match with TallyType
        string TallyType = RootAttribute.ElementName;

        //Adding xmlelement name according to RootElement name of ReturnObject
        attrOverrides ??= new();
        XmlAttributes attrs = new();
        attrs.XmlElements.Add(new(TallyType));
        attrOverrides.Add(typeof(Message<T>), "Objects", attrs);
        return base.GetXML(attrOverrides);
    }
}

[XmlRoot(ElementName = "BODY")]
public class Body<T>
{
    [XmlElement(ElementName = "DESC")]
    public Description Desc { get; set; } = new();

    [XmlElement(ElementName = "DATA")]
    public Data<T> Data { get; set; } = new();
}

[XmlRoot(ElementName = "DATA")]
public class Data<T>
{
    [XmlElement(ElementName = "TALLYMESSAGE")]
    public Message<T> Message { get; set; } = new();

    [XmlElement(ElementName = "COLLECTION")]
    public Colllection<T> Collection { get; set; }

}

[XmlRoot(ElementName = "COLLECTION")]
public class Colllection<T>
{
    public List<T> Objects { get; set; }
}

[XmlRoot(ElementName = "TALLYMESSAGE")]
public class Message<T>
{

    public List<T> Objects { get; set; } = new();
}


[XmlRoot(ElementName = "HEADER")]
public class Header
{
    public Header(RequestTye Request, HType Type, string ID)
    {
        this._request = Request;
        this._type = Type;
        this._Id = ID;
    }
    public Header() { }
    private int _version = 1;
    private RequestTye _request;
    private HType _type;
    private string _Id;
    [XmlElement(ElementName = "VERSION")]
    public int Version { get { return _version; } set { _version = value; } }

    [XmlElement(ElementName = "TALLYREQUEST")]
    public RequestTye Request { get { return _request; } set { _request = value; } }

    [XmlElement(ElementName = "TYPE")]
    public HType Type { get { return _type; } set { _type = value; } }

    [XmlElement(ElementName = "ID")]
    public string ID { get { return _Id; } set { _Id = value; } }
}


[XmlRoot(ElementName = "DESC")]
public class Description
{
    [XmlElement(ElementName = "STATICVARIABLES")]
    public StaticVariables StaticVariables { get; set; } = new();

}

[XmlRoot(ElementName = "STATICVARIABLES")]
public class StaticVariables : TallyBaseObject
{
    private string _ExportFormat;

    public StaticVariables()
    {

    }

    [XmlElement(ElementName = "SVEXPORTFORMAT")]
    public string SVExportFormat { get { return _ExportFormat; } set { _ExportFormat = $"$$SysName:{value}"; } }

    [XmlElement(ElementName = "SVCURRENTCOMPANY")]
    public string SVCompany { get; set; }

    [XmlIgnore]
    public string SVFromDate { get { return SVFrom.Text; } set { SVFrom = new(); SVFrom.Text = value; } }

    [XmlElement(ElementName = "SVFROMDATE")]
    public SVFrom SVFrom { get; set; }

    [XmlIgnore]
    public string SVToDate { get { return SVTo.Text; } set { SVTo = new(); SVTo.Text = value; } }

    [XmlElement(ElementName = "SVTODATE")]
    public SVTo SVTo { get; set; }

    [XmlElement(ElementName = "SVViewName")]
    public VoucherViewType ViewName { get; set; }

    [XmlElement(ElementName = "EXPLODEFLAG")]
    public string ExplodeFlag { get; set; }


}
[XmlRoot(ElementName = "SVFROMDATE")]
public class SVFrom
{
    [XmlAttribute(AttributeName = "TYPE")]
    public string Type { get; set; } = "Date";

    [XmlText]
    public string Text { get; set; }
}
[XmlRoot(ElementName = "SVTODATE")]
public class SVTo
{
    [XmlAttribute(AttributeName = "TYPE")]
    public string Type { get; set; } = "Date";

    [XmlText]
    public string Text { get; set; }
}

public enum RequestTye
{
    [XmlEnum(Name = "EXPORT")]
    Export,
    [XmlEnum(Name = "IMPORT")]
    Import
}

public enum HType
{
    [XmlEnum(Name = "OBJECT")]
    Object,
    [XmlEnum(Name = "COLLECTION")]
    Collection,
    [XmlEnum(Name = "DATA")]
    Data
}