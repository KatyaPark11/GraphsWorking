using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public TMP_Dropdown DownloadDropdown;

    private const string directoryPath = "../";

    void Start()
    {
        DirectoryInfo dir = new(directoryPath);
        List<TMP_Dropdown.OptionData> options = new();
        foreach (FileInfo file in dir.GetFiles("*.json"))
            options.Add(new TMP_Dropdown.OptionData() { text = file.Name.Replace(".json", "") });
        DownloadDropdown.options = options;
    }
}