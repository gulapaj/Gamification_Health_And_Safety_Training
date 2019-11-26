using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : Singleton<Map>
{
    public float insideMapDistance = 2000;
    public float markerSizePercent = 5;

    public GameObject playerMarker;
    public GameObject npcMarker;

    private RawImage mapBackground;

    private Transform playerTransform;
    //public GameObject[] npcs;

    private float mapWidth;
    private float mapHeight;
    private float markerWidth;
    private float markerHeight;

    List<GameObject> markersOnMap;

    // Start is called before the first frame update
    void Start()
    {
        mapBackground = GetComponentInChildren<RawImage>();
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //playerTransform = LevelManager.Instance.playerReference.transform;

        mapWidth = mapBackground.rectTransform.rect.width;
        mapHeight = mapBackground.rectTransform.rect.height;

        markerWidth = mapWidth * markerSizePercent / 100;
        markerHeight = mapHeight * markerSizePercent / 100;

        markersOnMap = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMapMarkers()
    {
        while(playerTransform == null)
        {
                if (!playerTransform)
                {
                    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                }
        }

            RemoveAllMarkers();
            FindAndDisplayMarkersForTag(playerTransform.tag, playerMarker);
            FindAndDisplayMarkersForTag("EncounterNPC", npcMarker);
    }

    private void RemoveAllMarkers()
    {
        //GameObject[] markersOnMap = GameObject.FindGameObjectsWithTag("MapMarker");

        foreach(GameObject markerOnMap in markersOnMap)
        {
            Destroy(markerOnMap);
        }
    }

    private void FindAndDisplayMarkersForTag(string tag, GameObject markerPrefab)
    {
        Vector3 playerPosition = playerTransform.position;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject target in targets)
        {
            Vector3 targetPosition = target.transform.position;
            float distanceToTarget = Vector3.Distance(targetPosition, playerPosition);

            if (distanceToTarget <= insideMapDistance)
            {
                CalculateMarkerPositionAndDrawMarker(playerPosition, targetPosition, markerPrefab);
            }
        }
    }

    private void CalculateMarkerPositionAndDrawMarker(Vector3 playerPosition, Vector3 targetPosition, GameObject markerPrefab)
    {
        Vector3 normalizedTargetPosition = NormalizedPosition(playerPosition, targetPosition);
        Vector2 markerPosition = CalculateMarkerPositionAndDrawMarker(normalizedTargetPosition);
        DrawMarker(markerPosition, markerPrefab);
    }

    private Vector3 NormalizedPosition(Vector3 playerPosition, Vector3 targetPosition)
    {
        float normalizedYTargetX = (targetPosition.x - playerPosition.x) /mapWidth; 
        float normalizedYTargetZ = (targetPosition.z - playerPosition.z) /mapHeight;

        return new Vector3(normalizedYTargetX, 0, normalizedYTargetZ);
    }

    private Vector2 CalculateMarkerPositionAndDrawMarker(Vector3 targetPosition)
    {
        float angleToTarget = Mathf.Atan2(targetPosition.x,targetPosition.z) * Mathf.Rad2Deg;
        float playerAngle = playerTransform.eulerAngles.y;
        float angleMapDegrees = angleToTarget - playerAngle - 90;

        float normalizedDistanceToTarget = targetPosition.magnitude;

        float angleRadians = angleMapDegrees * Mathf.Deg2Rad;
        float markerX = normalizedDistanceToTarget * Mathf.Cos(angleRadians);
        float markerY = normalizedDistanceToTarget * Mathf.Sin(angleRadians);

        markerX *= mapWidth / 2;
        markerY *= mapHeight / 2;
        markerX += mapWidth / 2;
        markerHeight += markerHeight / 2;

        return new Vector2(markerX, markerY);
    }

    private void DrawMarker(Vector2 position, GameObject markerPrefab)
    {
        GameObject markerGameObject = (GameObject)Instantiate(markerPrefab);
        markerGameObject.transform.SetParent(mapBackground.gameObject.transform);
        markersOnMap.Add(markerGameObject);

        RectTransform rectTransform = markerGameObject.GetComponent<RectTransform>();
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, position.x, markerWidth);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, position.y, markerHeight);
    }
}
