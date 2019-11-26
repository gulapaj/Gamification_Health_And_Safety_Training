using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicLevelGenerator : MonoBehaviour
{
    private const string SCENE = "JordanLevelDynam";
    private const string TUTORIAL = "TutorialLevel";

    public List<GameObject> layouts;
    public List<GameObject> stages;
    public List<GameObject> spawnPoints;
    public List<GameObject> centerPoints;
    public List<GameObject> NPCs;
    public GameObject NPC;
    public GameObject player;

    //test Joke
    public List<GameObject> jokeSpawnPoints;
    int numOfNPCwithJoke;
    public GameObject JOKE;
    public List<GameObject> JOKEs;

    // Temporary values for debug purposes
    //int numOfNPCs = 5;
    int numOfNPCs;//= StorageManager.Instance.currentLevel.GetComponent<LevelMono>().NPC.Count;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.tutorial)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(TUTORIAL));
        }
        else
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SCENE));
        }

        numOfNPCs = StorageManager.Instance.currentLevel.GetComponent<LevelMono>().NPC.Count;

        int randL = Random.Range(0, layouts.Count);
        Instantiate(layouts[randL]);
        LevelManager.Instance.layoutRotation = layouts[randL].transform.rotation.eulerAngles;

        centerPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("CenterPoint"));

        for (int i = 0; i<centerPoints.Count; i++)
        {
            int randS = Random.Range(0, centerPoints.Count);
            Instantiate(stages[randS], centerPoints[i].transform);
        }

        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint"));

        int rand = Random.Range(0, spawnPoints.Count);

        GameObject p = Instantiate(player, spawnPoints[rand].transform);
        p.transform.parent = null;
        spawnPoints.Remove(spawnPoints[rand]);

        for (int i = 0; i< numOfNPCs; i++)
        {
            rand = Random.Range(0, spawnPoints.Count);
            int randNPC = Random.Range(0, numOfNPCs - i);

            NPC = NPCs[randNPC];
            GameObject npc = Instantiate(NPC, spawnPoints[rand].transform);
            npc.GetComponent<NPCMovement>().npcId = i+1;
            npc.transform.parent = null;
            spawnPoints.Remove(spawnPoints[rand]);
            NPCs.Remove(NPCs[randNPC]);
        }


        //test Jokes
        jokeSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("JokeSpawn"));

        numOfNPCwithJoke = jokeSpawnPoints.Count;

        Debug.LogWarning(numOfNPCwithJoke);
        for (int i = 0; i < numOfNPCwithJoke; i++)
        {
            //selects one from all joke spawner
            int randX = Random.Range(0, jokeSpawnPoints.Count);

            GameObject j = Instantiate(JOKE, jokeSpawnPoints[randX].transform);
         //   j.transform.parent = null;
            jokeSpawnPoints.Remove(jokeSpawnPoints[randX]); //the spawnpoint is used
        }

    }

    // Update is called once per frame
    void Update()
    {
        AudioListener[] gos = FindObjectsOfType<AudioListener>();
        foreach (AudioListener a in gos)
        {
        //Debug.LogWarning("audio listener on "+a.gameObject);
        }
    }
}
