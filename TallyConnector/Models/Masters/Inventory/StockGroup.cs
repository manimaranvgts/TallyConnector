﻿namespace TallyConnector.Models.Masters.Inventory;

[XmlRoot(ElementName = "STOCKGROUP")]
[XmlType(AnonymousType = true)]
public class StockGroup : BasicTallyObject, ITallyObject
{
    public StockGroup()
    {
        BaseUnit = "";
        LanguageNameList = new();
    }


    [XmlAttribute(AttributeName = "NAME")]
    [JsonIgnore]
    public string OldName { get; set; }

    private string name;

    [XmlElement(ElementName = "NAME")]
    [Required]
    public string Name
    {
        get
        {
            name = (name == null || name == string.Empty) ? OldName : name;
            return name;
        }
        set => name = value;
    }

    [XmlElement(ElementName = "PARENT")]
    public string Parent { get; set; }


    [XmlElement(ElementName = "ISADDABLE")]
    public string IsAddable { get; set; }  //Should Quantities of Items be Added

    [XmlElement(ElementName = "GSTAPPLICABLE")]
    public string GSTApplicability { get; set; }

    [XmlElement(ElementName = "BASEUNITS")]
    public string BaseUnit { get; set; }


    [XmlIgnore]
    public string Alias { get; set; }

    [JsonIgnore]
    [XmlElement(ElementName = "LANGUAGENAME.LIST")]
    [TDLCollection(CollectionName = "LanguageName")]
    public List<LanguageNameList> LanguageNameList { get; set; }
    /// <summary>
    /// Accepted Values //Create, Alter, Delete
    /// </summary>
    [XmlAttribute(AttributeName = "Action")]
    public string Action { get; set; }


    public void CreateNamesList()
    {
        if (LanguageNameList.Count == 0)
        {
            LanguageNameList.Add(new LanguageNameList());
            LanguageNameList[0].NameList.NAMES.Add(Name);

        }
        if (Alias != null && Alias != string.Empty)
        {
            LanguageNameList[0].LanguageAlias = Alias;
        }
    }
    public new string GetXML(XmlAttributeOverrides attrOverrides = null)
    {
        CreateNamesList();
        return base.GetXML(attrOverrides);
    }

    public new void PrepareForExport()
    {
        if (Parent != null && Parent.Contains("Primary"))
        {
            Parent = null;
        }
        CreateNamesList();
    }
}
