using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _jsonGroupMember;
    private string _xmlGroupMember; 

    private List<GroupMember> groupMember = new List<GroupMember>
    {
        new GroupMember("Mark", 20020422, "Green"),
        new GroupMember("Marcus", 20020729, "Red"),
        new GroupMember("Gilah", 20010918, "Blue"),
        new GroupMember("Oscar", 20000304, "Blue"),
        new GroupMember("Tristan", 19991215, "Yellow"),
        new GroupMember("Oscar D", 19980105, "Purple"),
        new GroupMember("Alexander", 19970206, "Green")
    };

    private void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        _jsonGroupMember = _dataPath + "GroupMembers.json";
        _xmlGroupMember = _dataPath + "GroupMembers.xml";
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        NewDirectory();
        SerializeXML();
        DeserializeXML();
        SerializeJSON();
        DeserializeJSON();
    }

    public void NewDirectory()
    {
        if (!Directory.Exists(_dataPath))
        {
            Directory.CreateDirectory(_dataPath);
            Debug.Log("New directory created!");
        }
    }

    public void SerializeJSON()
    {
        string json = JsonUtility.ToJson(new SerializableGroupMemberList(groupMember));

        File.WriteAllText(_jsonGroupMember, json);
        Debug.Log("JSON data saved successfully!");
    }

    public void DeserializeJSON()
    {
        if (File.Exists(_jsonGroupMember))
        {
            string json = File.ReadAllText(_jsonGroupMember);
            SerializableGroupMemberList serializedList = JsonUtility.FromJson<SerializableGroupMemberList>(json);

            foreach (var member in serializedList.groupMembers)
            {
                Debug.LogFormat("Name: {0}, Date of Birth: {1}, Color: {2}", member.Name, member.DateOfBirth, member.Color);
            }
        }
    }
    public void SerializeXML()
    {
        var xmlSerializer = new XmlSerializer(typeof(List<GroupMember>));

        using (FileStream stream = File.Create(_xmlGroupMember))
        {
            xmlSerializer.Serialize(stream, groupMember);
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
}
[System.Serializable]
public class SerializableGroupMemberList
{
    public List<GroupMember> groupMembers;

    public SerializableGroupMemberList(List<GroupMember> groupMembers)
    {
        this.groupMembers = groupMembers;
    }
}
[System.Serializable]
public class GroupMember
{
    public string Name;
    public int DateOfBirth;
    public string Color;
    public GroupMember()
    {
    }
    public GroupMember(string name, int dateOfBirth, string color)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        Color = color;
    }
}