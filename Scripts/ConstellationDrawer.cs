using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ConstellationDrawer : MonoBehaviour
{
    public PlanetSelector planetSelector; // Referencia al script de selecci�n de planetas
    public Material lineMaterial; // Material para las l�neas de la constelaci�n
    public Button buttonDraw;


    void Start()
    {
        // Aseg�rate de que la referencia al bot�n est� asignada
        if (buttonDraw != null)
        {
            // A�adir la funci�n de dibujar constelaci�n al evento OnClick del bot�n
            buttonDraw.onClick.AddListener(DrawConstellation);
        }
        else
        {
            Debug.LogError("El bot�n Draw no est� asignado.");
        }
    }

    void DrawConstellation()
    {
        if (planetSelector.selectedPlanets.Count < 2) return; // No dibujar si no hay suficientes planetas seleccionados

        // Crear un GameObject para contener el LineRenderer
        GameObject constellation = new GameObject("Constellation");
        LineRenderer lineRenderer = constellation.AddComponent<LineRenderer>();

        lineRenderer.positionCount = planetSelector.selectedPlanets.Count;
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.1f; // Ancho de la l�nea
        lineRenderer.endWidth = 0.1f;

        for (int i = 0; i < planetSelector.selectedPlanets.Count; i++)
        {
            lineRenderer.SetPosition(i, planetSelector.selectedPlanets[i].transform.position);
        }
    }
}