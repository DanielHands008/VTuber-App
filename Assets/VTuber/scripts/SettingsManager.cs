using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    public string settingsFolder = "settings";

    public void saveFile(string settingsFile, string json)
    {
        if (!Directory.Exists(settingsFolder))
            Directory.CreateDirectory(settingsFolder);
        File.WriteAllText(settingsFolder + "/" + settingsFile + ".json", json);
    }

    public string loadFile(string settingsFile)
    {
        if (!Directory.Exists(settingsFolder))
        {
            Debug.LogError("Settings folder (" + settingsFolder + ") dosn't exist.");
            return "";
        }
        if (!File.Exists(settingsFolder + "/" + settingsFile + ".json"))
        {
            Debug.LogError("Settings file (" + settingsFile + ") dosn't exist.");
            return "";
        }
        return File.ReadAllText(settingsFolder + "/" + settingsFile + ".json");

    }
}
