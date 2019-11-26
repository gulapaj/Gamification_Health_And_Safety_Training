using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : Singleton<StorageManager>
{
    public string path = "Assets/JSON/jsontemplate.json";

    //public string question = "What is the right way to do this?";

    public GameObject module;
    public GameObject level;
    public GameObject npc;
    public GameObject encounter;
    public GameObject question;
    public GameObject choice;

    public GameObject topLevelModule;
    GameObject lev;
    GameObject np;
    GameObject enc;
    GameObject que;
    GameObject cho;

    ModuleMono modMono;
    LevelMono levMono;
    NPCMono npcMono;
    EncounterMono encMono;
    public QuestionMono queMono;
    ChoiceMono choMono;

    public GameObject currentModule;
    public GameObject currentLevel;
    public GameObject currentNPC;
    public GameObject currentEncounter;
    public GameObject currentQuestion;
    public List<GameObject> currentChoices;

    public GameObject[] loadedLevels;

    public void CreateModule(string moduleTitle, int moduleId)
    {
        Debug.Log("Creating Module");

        topLevelModule = Instantiate(module);
        modMono = topLevelModule.GetComponent<ModuleMono>();
        modMono.ModuleTitle = moduleTitle;
        modMono.ModuleId = moduleId;

        currentModule = topLevelModule;
    }

    public void CreateLevel(int levelID, string title)
    {
        Debug.Log("Creating Level");

        lev = Instantiate(level);
        lev.transform.parent = topLevelModule.transform;
        levMono = lev.GetComponent<LevelMono>();
        levMono.LevelId = levelID;
        levMono.Title = title;

        modMono.Level.Add(lev);
    }

    public void CreateNPC(int npcID, string training)
    {
        Debug.Log("Creating NPC");

        np = Instantiate(npc);
        np.transform.parent = lev.transform;
        npcMono = np.GetComponent<NPCMono>();
        npcMono.NPCId = npcID;
        npcMono.Training = training;

        levMono.NPC.Add(np);
    }

    public void CreateEncounter(int encID)
    {
        Debug.Log("Creating Encounter");

        enc = Instantiate(encounter);
        enc.transform.parent = np.transform;
        encMono = enc.GetComponent<EncounterMono>();
        encMono.EncounterId = encID;

        npcMono.Encounter.Add(enc);
    }

    public void CreateQuestion(int queID, string statement)
    {
        Debug.Log("Creating Question");

        que = Instantiate(question);
        que.transform.parent = enc.transform;
        queMono = que.GetComponent<QuestionMono>();
        queMono.QuestionId = queID;
        queMono.QuestionString = statement;

        encMono.Question.Add(que);
    }

    public void CreateChoice(int choID, string statement, bool correct)
    {
        Debug.Log("Creating Choice");

        cho = Instantiate(choice);
        cho.transform.parent = que.transform;
        choMono = cho.GetComponent<ChoiceMono>();
        choMono.ChoiceId = choID;
        choMono.ChoiceString = statement;
        choMono.Correct = correct;

        queMono.Choices.Add(cho);
    }

    public ModuleMono GetModule(GameObject module)
    {
        return module.GetComponent<ModuleMono>();
    }

    public LevelMono GetLevel(GameObject level)
    {
        return level.GetComponent<LevelMono>();
    }

    public NPCMono GetNPC(GameObject npc)
    {
        return npc.GetComponent<NPCMono>();
    }

    public EncounterMono GetEncounter(GameObject encounter)
    {
        return encounter.GetComponent<EncounterMono>();
    }

    public QuestionMono GetQuestion(GameObject question)
    {
        return question.GetComponent<QuestionMono>();
    }

    public ChoiceMono GetChoice(GameObject choice)
    {
        return choice.GetComponent<ChoiceMono>();
    }

    public GameObject[] GetLevels()
    {
        loadedLevels = GameObject.FindGameObjectsWithTag("Level");
        return loadedLevels;
    }

    public void SetCurrentLevel(int id)
    {
        currentLevel = loadedLevels[id];
    }

    public void SetCurrentNPC(int id)
    {
        currentNPC = currentLevel.GetComponent<LevelMono>().NPC[id-1];
    }

    public void SetCurrentEncounter(int id)
    {
        currentEncounter = currentNPC.GetComponent<NPCMono>().Encounter[id];
    }

    public void SetCurrentQuestion(int id)
    {
        currentQuestion = currentEncounter.GetComponent<EncounterMono>().Question[id];
        SetCurrentChoices();
    }

    public void SetCurrentChoices()
    {
        currentChoices = currentQuestion.GetComponent<QuestionMono>().Choices;
    }

    public void ClearStorage()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Module"))
        {
            Destroy(go);
        }
    }
}
