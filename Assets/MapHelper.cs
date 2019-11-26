using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHelper : MonoBehaviour
{
    RectTransform transform;
    Vector3 angle;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<RectTransform>();
        angle = new Vector3(90,0,0) + LevelManager.Instance.layoutRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = angle;
    }
}
