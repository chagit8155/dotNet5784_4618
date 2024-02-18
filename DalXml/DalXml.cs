namespace Dal;
using DalApi;
using System.Data.Common;
using System.Xml.Linq;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    private DalXml() { }
    public DateTime? StartProjectDate { get; set; }

  
    public DateTime? EndProjectDate {  get; set; }

    private static readonly string s_data_config_xml = "data-config";
   
    public DateTime? GetStartProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml).Element("StartProjectDate")!;
        if (root.Value == "") 
            return null;
        return DateTime.Parse(root.Value);  
    } 
    public DateTime? GetEndProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml).Element("EndProjectDate")!;
        if (root.Value == "")
            return null;
        return DateTime.Parse(root.Value);
    }
    public DateTime? SetStartProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
        root.Element("StartProjectDate")!.Value = EndProjectDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, s_data_config_xml);
        return StartProjectDate;

    }


    public DateTime? SetEndProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
        root.Element("EndProjectDate")!.Value = EndProjectDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, s_data_config_xml);
        return EndProjectDate;
    }
    public void resetTimeLine()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("StartProjectDate")!.Value = "";
        root.Element("EndProjectDate")!.Value = "";
        XMLTools.SaveListToXMLElement(root, "data-config");
    }
}
