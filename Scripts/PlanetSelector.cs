using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlanetSelector : MonoBehaviour
{
    public List<GameObject> selectedPlanets = new List<GameObject>(); // Lista para almacenar planetas seleccionados
    public Material selectedMaterial; // Material para indicar planetas seleccionados
    public Camera camera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta el clic izquierdo
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject planet = hit.collider.gameObject;

                if (planet.CompareTag("star")) // Asegúrate de que los planetas tengan el tag "Planet"
                {
                    if (!selectedPlanets.Contains(planet))
                    {
                        selectedPlanets.Add(planet); // Agregar a la lista de planetas seleccionados
                        planet.GetComponent<Renderer>().material = selectedMaterial; // Cambia el material para marcar como seleccionado
                    }
                }
            }
        }
    }
}