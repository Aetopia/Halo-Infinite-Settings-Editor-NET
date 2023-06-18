using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class SpecControlSettings
{
    private string path = "";
    public Dictionary<string, Dictionary<string, object>> jsonObject;
    private JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

    public SpecControlSettings()
    {
        string
        localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA"),
        pathSteam = $"{localAppData}\\HaloInfinite\\Settings\\SpecControlSettings.json",
        pathUwp = $"{localAppData}\\Packages\\Microsoft.254428597CFE2_8wekyb3d8bbwe\\LocalCache\\Local\\HaloInfinite\\Settings\\SpecControlSettings.json";

        if (File.Exists(pathSteam))
            this.path = pathSteam;
        else if (File.Exists(pathUwp))
            this.path = pathUwp;
    }
    public void Read() { this.jsonObject = javaScriptSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(File.ReadAllText(this.path)); }

    public void Write() { File.WriteAllText(this.path, javaScriptSerializer.Serialize(this.jsonObject)); }
}