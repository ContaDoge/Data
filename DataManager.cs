using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _textFile;
    private string _xmlGroupMember;
    private string _jsonGroupMember;

    private List<GroupMember> groupMembers = new List<GroupMember>
    {
    new GroupMember("Mark", 20020422, "Green"),
    new GroupMember("Marcus", 20020729, "Red"),
    new GroupMember("Gilah", 20010918, "Blue"),
    new GroupMember("Oscar", 20000304, "Blue"),
    new GroupMember("Tristan", 19991215, "Yellow"),
    new GroupMember("Oscar D", 19980105, "Purple"),
    new GroupMember("Alexander", 19970206, "Green")
    };
    private string _jsonWeapons;

    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    // i got the xml data to save but i can't get the json data to save
    void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);

        _textFile = _dataPath + "Save_Data.txt";
        _jsonWeapons = _dataPath + "WeaponJSON.json";
        _xmlGroupMember = _dataPath + "GroupMembers.xml";
        _jsonGroupMember = _dataPath + "GroupMembers.json";
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Data Manager initialized..";
        Debug.Log(_state);

        //FilesystemInfo();
        NewDirectory();
        //DeleteDirectory();
        //NewTextFile();
        //UpdateTextFile();
        //ReadFromFile(_textFile);
        //WriteToStream(_streamingTextFile);
        //ReadFromFile(_streamingTextFile);
        //WriteToXML(_xmlLevelProgress);
        //ReadFromStream(_xmlLevelProgress);
        //SerializeXML();
        //DeserializeXML();
        CheckJsonFile();
        Debug.Log("Group Member Count: " + groupMembers.Count);
        SerializeJSON();
        DeserializeJSON();
    }

    public void FilesystemInfo()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}",
        Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }


    public void NewDirectory()
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists...");
            return;
        }

        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }

    public void DeleteDirectory()
    {
        if (!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist or has already been deleted...");
            return;
        }

        Directory.Delete(_dataPath, true);
        Debug.Log("Directory successfully deleted!");
    }

    public void NewTextFile()
    {
        if (File.Exists(_textFile))
        {
            Debug.Log("File already exists...");
            return;
        }

        File.WriteAllText(_textFile, "<SAVE DATA>\n");
        Debug.Log("New file created!");
    }

    public void UpdateTextFile()
    {
        if (!File.Exists(_textFile))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        File.AppendAllText(_textFile, $"Game started: {DateTime.Now}\n");
        Debug.Log("File updated successfully!");
    }

    public void ReadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        Debug.Log(File.ReadAllText(filename));
    }

    public void DeleteFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist or has already been deleted...");
            return;
        }

        File.Delete(_textFile);
        Debug.Log("File successfully deleted!");
    }

    public void WriteToStream(string filename)
    {
        if (!File.Exists(filename))
        {
            StreamWriter newStream = File.CreateText(filename);
            newStream.WriteLine("<Save Data> for HERO BORN \n");
            newStream.Close();

            Debug.Log("New file created with StreamWriter!");
        }

        StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.WriteLine("Game ended: " + DateTime.Now);
        streamWriter.Close();

        Debug.Log("File contents updated with StreamWriter!");
    }

    public void ReadFromStream(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        StreamReader streamReader = new StreamReader(filename);
        Debug.Log(streamReader.ReadToEnd());
    }

    public void WriteToXML(string filename)
    {
        if (!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("level_progress");

            for (int i = 1; i < 5; i++)
            {
                xmlWriter.WriteElementString("level", "Level-" + i);
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
        }
    }

    public void SerializeXML()
    {
        var xmlSerializer = new XmlSerializer(typeof(List<GroupMember>));

        using (FileStream stream = File.Create(_xmlGroupMember))
        {
            xmlSerializer.Serialize(stream, groupMembers);
        }
    }

    public void DeserializeXML()
    {
        if (File.Exists(_xmlGroupMember))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<GroupMember>));

            using (FileStream stream = File.OpenRead(_xmlGroupMember))
            {
                var members = (List<GroupMember>)xmlSerializer.Deserialize(stream);
                foreach (var member in members)
                {
                    Debug.LogFormat("Name: {0}, Date of Birth: {1}, Color: {2}", member.Name, member.DateOfBirth, member.Color);
                }
            }
        }
    }

    public void SerializeJSON()
    {
        try
        {
            if (groupMembers != null && groupMembers.Count > 0)
            {
                Debug.Log("Serializing JSON data...");
                // Serialize the groupMembers list into JSON data
                string jsonString = JsonUtility.ToJson(groupMembers, true);
                Debug.Log("Serialized JSON data: " + jsonString); // Log serialized JSON data
                File.WriteAllText(_jsonGroupMember, jsonString);
                Debug.Log("JSON data saved to: " + _jsonGroupMember);
            }
            else
            {
                Debug.LogError("Cannot serialize JSON: groupMembers list is null or empty.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error during JSON serialization: " + e.Message);
        }
    }

    public void DeserializeJSON()
    {
        if (File.Exists(_jsonGroupMember))
        {
            using (StreamReader stream = new StreamReader(_jsonGroupMember))
            {
                var jsonString = stream.ReadToEnd();
                Debug.Log("JSON String: " + jsonString); // Log JSON string for debugging

                if (!string.IsNullOrEmpty(jsonString))
                {
                    var members = JsonUtility.FromJson<List<GroupMember>>(jsonString);

                    if (members != null)
                    {
                        foreach (var member in members)
                        {
                            Debug.LogFormat("Name: {0}, Date of Birth: {1}, Color: {2}", member.Name, member.DateOfBirth, member.Color);
                        }
                    }
                    else
                    {
                        Debug.Log("Deserialized list is null.");
                    }
                }
                else
                {
                    Debug.Log("JSON file is empty.");
                }
            }
        }
        else
        {
            Debug.Log("JSON file does not exist.");
        }
    }
    private void CheckJsonFile()
    {
        if (File.Exists(_jsonGroupMember))
        {
            string jsonContents = File.ReadAllText(_jsonGroupMember);
            Debug.Log("JSON File Contents:\n" + jsonContents);
        }
        else
        {
            Debug.LogError("JSON file does not exist at path: " + _jsonGroupMember);
        }
    }
}