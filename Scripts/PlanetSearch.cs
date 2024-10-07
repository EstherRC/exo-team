using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;


public class PlanetSearch : MonoBehaviour
{
    public TMP_InputField searchInput;
    public TMP_Dropdown searchDropdown;
    public Button goToPlanet;

    private List<string> allPlanets;
    private List<string> filteredPlanets;

    IEnumerator GetPlanets()
    {
        string uri = Utils.API_URL + "/planets";

        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                Debug.Log(JsonUtility.FromJson<Planets>(webRequest.downloadHandler.text).planets);
                allPlanets = JsonUtility.FromJson<Planets>(webRequest.downloadHandler.text).planets.ToList();
                filteredPlanets = allPlanets.GetRange(0, 10);
                searchDropdown.AddOptions(filteredPlanets);
                break;
            default:
                Debug.LogError("Error: " + webRequest.error);
                break;
        }
    }

    void Start()
    {
        searchDropdown.ClearOptions();
        allPlanets = filteredPlanets = new List<string> { "-" };
        StartCoroutine(GetPlanets());
        searchDropdown.onValueChanged.AddListener(delegate { OnSelectedOption(searchDropdown.options[searchDropdown.value].text); });
        searchInput.onValueChanged.AddListener(OnChangedSearch);
        goToPlanet.onClick.AddListener(OnPlanetSelected);
    }

    private void OnSelectedOption(string planet)
    {
        searchInput.text = planet;
    }

    private void OnChangedSearch(string planet)
    {
        // If the text is empty, display the first 10 planets
        if (planet == "")
        {
            filteredPlanets = allPlanets.GetRange(0, 10);
        }
        else
        {
            Debug.Log("Searching for: " + planet);
            filteredPlanets = allPlanets.Where(p => p.ToLower().Contains(planet.ToLower())).ToList();
            searchDropdown.ClearOptions();
            int maxResults = filteredPlanets.Count > 10 ? 10 : filteredPlanets.Count;
            searchDropdown.AddOptions(filteredPlanets.GetRange(0, maxResults));
        }
    }

    void OnPlanetSelected()
    {
        if(filteredPlanets.Count == 0)
        {
            Debug.Log("No planet selected");
            return;
        }

        Utils.currentPlanet = searchInput.text;

        Debug.Log("Going to planet: " + Utils.currentPlanet);

        SceneManager.LoadScene("PlanetAR");
    }

    [Serializable]
    public class Planets
    {
        public string[] planets;
    }
}
