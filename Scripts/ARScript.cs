using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

public class ARScript : MonoBehaviour
{
    private string api = Utils.API_URL + "/stars/planet";

    public int starCount;
    public GameObject starPrefab;
    public Camera mainCamera;  // Referencia a la cámara principal (AR Camera)

    void Start()
    {
        string uri = api + "?planet_name=" + Utils.currentPlanet + "&n_closest=" + starCount;
        StartCoroutine(GetPlanet(uri));
    }

    IEnumerator GetPlanet(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                UnityEngine.Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                UnityEngine.Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                Process(webRequest.downloadHandler.text);
                break;
        }
    }

    void Process(string json)
    {
        StarDataResponse starData = JsonUtility.FromJson<StarDataResponse>(json);

        PlanetData planet = starData.planet[0];
        Vector3 planetPosition = ConvertTo3DPosition(planet.ra, planet.dec);

        // Crea el planeta y mueve la cámara a esa posición
        CreatePlanetObject(planetPosition, 0);

        foreach (StarData star in starData.stars)
        {
            Vector3 starPosition = ConvertTo3DPosition(star.ra, star.dec) + UnityEngine.Random.insideUnitSphere * 5;
            CreateStarObject(starPosition, star.phot_g_mean_mag);
        }
    }

    void CreatePlanetObject(Vector3 position, float magnitude)
    {
        MoveCameraToPlanet(position * 10);
    }

    Vector3 ConvertTo3DPosition(float ra, float dec)
    {
        float x = Mathf.Cos(dec) * Mathf.Cos(ra);
        float y = Mathf.Cos(dec) * Mathf.Sin(ra);
        float z = Mathf.Sin(dec);

        return new Vector3(x, y, z);
    }

    void CreateStarObject(Vector3 position, float magnitude)
    {
        GameObject star = Instantiate(starPrefab);
        star.tag = "star";
        star.transform.position = position * 10;  // Ajusta la posición de las estrellas
        star.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        float color = 1 - (magnitude / 20);
        star.GetComponent<Renderer>().material.color = new Color(color, color, color);
    }

    // Mueve la cámara a la posición del planeta y ajusta su rotación
    void MoveCameraToPlanet(Vector3 planetPosition)
    {
        // Mueve la cámara a la posición del planeta
        mainCamera.transform.position = planetPosition;

        // Opcional: rota la cámara para que mire hacia afuera desde el planeta
        mainCamera.transform.LookAt(planetPosition + new Vector3(0, 0, -1));  // Ajusta la dirección en la que la cámara debe mirar
    }

    [System.Serializable]
    public class StarDataResponse
    {
        public PlanetData[] planet;
        public StarData[] stars;
    }

    [System.Serializable]
    public class StarData
    {
        public float ra;
        public float dec;
        public float parallax;
        public float phot_g_mean_mag;
        public string source_id;
    }

    [System.Serializable]
    public class PlanetData
    {
        public string pl_name;
        public string hostname;
        public float ra;
        public float dec;
    }
}