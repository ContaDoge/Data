using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GroupMember
{
    public string Name;
    public int DateOfBirth;
    public string Color;

    public GroupMember(string Name, int DateOfBirth, string Color)
    {
        this.Name = Name;
        this.DateOfBirth = DateOfBirth;
        this.Color = Color;
    }
}