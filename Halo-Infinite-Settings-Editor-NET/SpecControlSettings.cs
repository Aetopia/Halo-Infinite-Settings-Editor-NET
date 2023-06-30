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
        pathSteam = $"{localAppData}\\HaloInfinite\\Settings",
        pathUwp = $"{localAppData}\\Packages\\Microsoft.254428597CFE2_8wekyb3d8bbwe\\LocalCache\\Local\\HaloInfinite\\Settings";

        if (Directory.Exists(pathSteam))
            this.path = pathSteam;
        else if (Directory.Exists(pathUwp))
            this.path = pathUwp;
    }

    public void Read(bool specControlMPSettings = false)
    {
        string path = specControlMPSettings ? $"{this.path}\\SpecControlMPSettings.json" : $"{this.path}\\SpecControlSettings.json";
        this.jsonObject = javaScriptSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(File.ReadAllText(path));
    }

    public void Write(bool specControlMPSettings = false)
    {
        string path = specControlMPSettings ? $"{this.path}\\SpecControlMPSettings.json" : $"{this.path}\\SpecControlSettings.json";
        File.WriteAllText(path, javaScriptSerializer.Serialize(this.jsonObject));
    }
}