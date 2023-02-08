using System.Xml;
public class Kazakhstan
{
    public void fetch()
    {
        XmlDocument doc = new();
        doc.Load("Info.xml");
        XmlElement? xRoot = doc.DocumentElement;
        if (xRoot != null)
        {
            foreach (XmlElement xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "API":
                        API = xnode.InnerText;
                        break;
                    case "WelcomeMessage":
                        WelcomeMessage = xnode.InnerText;
                        break;
                    case "HelpMessage":
                        HelpMessage = xnode.InnerText;
                        break;
                    case "GeneralInfo":
                        GeneralInfo = xnode.InnerText;
                        break;
                    case "History":
                        History = xnode.InnerText;
                        break;
                    case "Geography":
                        Geography = xnode.InnerText;
                        break;
                    case "Culture":
                        Culture = xnode.InnerText;
                        break;
                    case "Economy":
                        Economy = xnode.InnerText;
                        break;
                    case "Regions":
                        Regions = new List<KazakhstanRegion>();
                        foreach (XmlElement Xmlregion in xnode)
                        {
                            XmlNode? attr = Xmlregion.Attributes?.GetNamedItem("name");
                            if (attr != null)
                            {
                                KazakhstanRegion region = new();
                                region.Name = attr.Value!=null ? attr.Value : "Unknown";
                                foreach (XmlElement XmlregionInfo in Xmlregion)
                                {
                                    switch (XmlregionInfo.Name)
                                    {
                                        case "GeneralInfo":
                                            region.GeneralInfo = XmlregionInfo.InnerText;
                                            break;
                                        case "Landmarks":
                                            region.Landmarks = new List<Landmark>();
                                            foreach (XmlElement XmlLandmark in XmlregionInfo)
                                            {
                                                XmlNode? attrLandmark = XmlLandmark.Attributes?.GetNamedItem("name");
                                                if (attrLandmark != null)
                                                {
                                                    Landmark landmark = new();
                                                    landmark.Name = attrLandmark.Value!=null ? attrLandmark.Value : "Unknown";
                                                    foreach (XmlElement XmlLandmarkInfo in XmlLandmark)
                                                    {
                                                        switch (XmlLandmarkInfo.Name)
                                                        {
                                                            case "Description":
                                                                landmark.Description = XmlLandmarkInfo.InnerText;
                                                                break;
                                                            case "Image":
                                                                landmark.Image = XmlLandmarkInfo.InnerText;
                                                                break;
                                                            case "Location":
                                                                landmark.Location = XmlLandmarkInfo.InnerText;
                                                                break;
                                                        }
                                                    }
                                                    region.Landmarks.Add(landmark);
                                                }
                                            }
                                            break;
                                    }
                                }
                                Regions.Add(region);
                            }
                        }
                        break;

                }
            }
        }
    }
    public string? API { get; set; }
    public string WelcomeMessage { get; set; } = "Failed to load WelcomeMessage";
    public string HelpMessage { get; set; } = "Failed to load HelpMessage";
    public string GeneralInfo { get; set; } = "Failed to load GeneralInfo";
    public string History { get; set; } = "Failed to load History";
    public string Geography { get; set; } = "Failed to load Geography";
    public string Culture { get; set; } = "Failed to load Culture";
    public string Economy { get; set; } = "Failed to load Economy";

    public List<KazakhstanRegion> Regions { get; set; } = new();

}
public class KazakhstanRegion
{
    public string Name { get; set; } = "Failed to load Name";
    public string GeneralInfo { get; set; } = "Failed to load GeneralInfo";
    public List<Landmark> Landmarks { get; set; } = new();

}

public class Landmark
{
    public string Name { get; set; } = "Failed to load Name";
    public string Description { get; set; } = "Failed to load Description";
    public string Image { get; set; } = "Failed to load Image";
    public string Location { get; set; } = "Failed to load Location";
}