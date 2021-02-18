using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Quest",menuName ="New Quest")]
public class Quest:ScriptableObject
{
    public string id;
    public int[] prerequisite_quest;
    public Status status;
    public enum Status {Unknown, Ongoing, Completed};
    public string title;
    public string description;
    public int reward;
    public QuestGoal goal;
    public void Complete()
    {
        status = Status.Completed;
    }
    public Quest()
    {
        status = Status.Unknown;
    }
}
