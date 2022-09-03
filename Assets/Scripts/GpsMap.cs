using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsMap : MonoBehaviour
{
    
    float initLon, initLat;
    Vector3 playerCoordinates;
    [SerializeField] float updatePosEvery = 2f;

    private void Start()
    {
        StartCoroutine(IEStart());
    }

    private void Update()
    {
        if(!Input.location.isEnabledByUser)
        {
            return;
        }
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
            playerCoordinates = new Vector3(initLon, 0f, initLat);
        }
    }

    IEnumerator IEUpdate()
    {
        Vector3 oldGPS = new Vector3(Input.location.lastData.longitude, 0f, Input.location.lastData.latitude);
        yield return new WaitForSeconds(updatePosEvery);
        Vector3 newGPS = new Vector3(Input.location.lastData.longitude, 0f, Input.location.lastData.latitude);
        Vector3 movement = newGPS - oldGPS;
        playerCoordinates += movement;
    }

}
