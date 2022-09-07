using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsMap : MonoBehaviour
{
    
    float initLon, initLat;
    Vector2 playerCoordinates;
    [SerializeField] RectTransform topPoint, bottomPoint, leftPoint, rightPoint, middlePoint;
    [SerializeField] float updatePosEvery = 2f;
    [SerializeField] GameObject playerIcon;

    private void Start()
    {
        //StartCoroutine(IEStart());
        Debug.Log(middlePoint.position);
    }

    private void Update()
    {
        //if(!Input.location.isEnabledByUser)
        //{
        //    return;
        //}
    }

    //makes sure location is enabled and finds initial lon and lat
    IEnumerator IEStart()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            yield break;

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            initLon = Input.location.lastData.longitude;
            initLat = Input.location.lastData.latitude;
            playerCoordinates = new Vector2(initLon, initLat);

        }
    }

    IEnumerator IEUpdate()
    {
        Vector2 oldGPS = new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude);
        yield return new WaitForSeconds(updatePosEvery);
        Vector2 newGPS = new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude);
        Vector2 movement = newGPS - oldGPS;
        playerCoordinates += movement;
    }
}
