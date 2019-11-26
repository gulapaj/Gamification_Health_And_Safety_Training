using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class JokeDialogManager : Singleton<JokeDialogManager>
{
    public Flowchart flowchart;
    public List<int> jokeID;
    // Start is called before the first frame update
    void Start()
    {
        //generates all jokes ID
        jokeID = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetJokeID()
    {
        int randJoke = Random.Range(0, jokeID.Count); //gets the first joke

        //joke wont be picked again
        jokeID.Remove(jokeID.IndexOf(randJoke));

        return randJoke;
    }
}
