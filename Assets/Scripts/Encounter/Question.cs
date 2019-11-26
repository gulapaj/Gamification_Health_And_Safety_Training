using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public int QuestionId;
    public string QuestionString;
    public List<Choice> Choices;
}
