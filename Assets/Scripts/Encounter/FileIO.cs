using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class FileIO : Singleton<FileIO>
{
  public void ReadFile(string path)
    {
        //StreamReader reader = new StreamReader(path);

        //Debug.Log(reader.ReadToEnd());
        //string json = reader.ReadToEnd();
        //reader.Close();

        ParseJSON(path);//json);

    }

    public void ParseJSON(string json)
    {
        JSONData testMod = new JSONData();

        testMod = JsonUtility.FromJson<JSONData>(json);
        
    

        Store(testMod);

    }

    public void Store(JSONData data)
    {
        Debug.Log("Storing");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Storage"));

        StorageManager.Instance.CreateModule(data.Modules[0].ModuleTitle, data.Modules[0].ModuleId);

        foreach (Level level in data.Modules[0].Level)
        {
            StorageManager.Instance.CreateLevel(level.LevelId, level.Title);

            foreach (NPC npc in level.NPC)
            {
                StorageManager.Instance.CreateNPC(npc.NPCId, npc.Training);

                foreach (Encounter enc in npc.Encounter)
                {
                    StorageManager.Instance.CreateEncounter(enc.EncounterId);

                    foreach (Question que in enc.Question)
                    {
                        StorageManager.Instance.CreateQuestion(que.QuestionId, que.QuestionString);

                        foreach (Choice cho in que.Choices)
                        {
                            StorageManager.Instance.CreateChoice(cho.ChoiceID, cho.ChoiceString, cho.Correct);
                        }
                    }
                }
            }
        }
    }
}
