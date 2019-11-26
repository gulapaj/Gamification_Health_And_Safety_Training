using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Quaternion initialRotation;
    private Vector3 initialPositionOffset;
    private Transform player;
    private float speed;
    public bool cameraLocked;

    public Transform standardTransform;
    public Transform encounterTransform;
    public bool inPosition = false;

    public RaycastHit[] hits;
    public Quaternion cameraDirection;
    public Vector3 rayDirection;
    public float cameraDistance;

    public Vector3 midPoint;
    public Vector3 cameraPosition;
    public Vector3 vectorBetweenCharacters;
    public Vector3 perpendicularVector;

    public float distanceBetweenCharacters;

    Scene currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        player = transform.parent;
        speed = player.gameObject.GetComponent<PlayerMovement>().speed;
        initialRotation = transform.rotation;   
        initialPositionOffset = transform.localPosition;
        transform.parent = null;
        cameraLocked = true;

        standardTransform = transform;
        encounterTransform = transform;
    }

    void LateUpdate()
    {
        if (cameraLocked)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position + initialPositionOffset, speed);
        }
    }

    public void ToEncounterPosition()
    {
        cameraLocked = false;
        transform.SetPositionAndRotation(encounterTransform.position, encounterTransform.rotation);

        Ray ray = new Ray(encounterTransform.position, rayDirection);
        hits = Physics.RaycastAll(ray, cameraDistance);

        inPosition = true;

        foreach (RaycastHit hit in hits)
        {
            MeshRenderer meshRenderer = hit.collider.gameObject.GetComponentInChildren<MeshRenderer>();
            meshRenderer.enabled = false;
        }

        transform.LookAt(midPoint + Vector3.up * 1.7f);
    }

    public void ToStandardPosition()
    {
       // currentScene = SceneManager.GetSceneByName("JordanLevelDynam");

        if (currentScene.isLoaded) {
            transform.SetPositionAndRotation(standardTransform.position, Quaternion.Euler(new Vector3(64.28f, 0, 0)));
            cameraLocked = true;
            inPosition = false;
            foreach (RaycastHit hit in hits)
            {
                MeshRenderer meshRenderer = hit.collider.gameObject.GetComponentInChildren<MeshRenderer>();
                meshRenderer.enabled = true;
            }
       }
    }

    public void SetCameraPositions(Transform playerTransform, Transform npcTransform)
    {
        standardTransform = transform;

        vectorBetweenCharacters = npcTransform.position - playerTransform.position;
        distanceBetweenCharacters = vectorBetweenCharacters.magnitude;
        cameraDistance = distanceBetweenCharacters;
        if (cameraDistance < (transform.position - transform.GetChild(0).position).magnitude)
            cameraDistance = (transform.position - transform.GetChild(0).position).magnitude;

        midPoint = playerTransform.position + vectorBetweenCharacters.normalized * (distanceBetweenCharacters / 2);
        
        perpendicularVector = Vector3.Cross(vectorBetweenCharacters, Vector3.up).normalized * (-cameraDistance);

        cameraPosition = midPoint + perpendicularVector + Vector3.up * 1.7f;
        cameraDirection = Quaternion.FromToRotation(cameraPosition, midPoint);

        rayDirection = cameraDirection.eulerAngles;

        //cameraDirection = Quaternion.Euler(cameraDirection.eulerAngles.x, cameraDirection.eulerAngles.y, 0);

        encounterTransform.SetPositionAndRotation(cameraPosition, cameraDirection);

        Debug.LogWarning("Enc pos: "+ encounterTransform.position + " enc rot: "+ encounterTransform.rotation);
    }
}
