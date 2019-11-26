using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TestFungus"));
        playerSpawned();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerSpawned()
    {
        for (int x = 0; x < 1; x++)
        {
            GameObject playerSpawn = Instantiate(player) as GameObject;
            playerSpawn.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            
        }
        
    }
}
