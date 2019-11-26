using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npc;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TestFungus"));
        npcSpawned();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void npcSpawned()
    {
        npc.gameObject.GetComponent<Rigidbody>().useGravity = true;
        for (int x = 0; x < 1; x++)
        {
            GameObject npcSpawn = Instantiate(npc) as GameObject;
            npcSpawn.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
    }
}
