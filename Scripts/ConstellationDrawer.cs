using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ConstellationDrawer : MonoBehaviour
{
    public PlanetSelector planetSelector; // Referencia al script de selección de planetas
    public Material lineMaterial; // Material para las líneas de la constelación
    public Button buttonDraw;


    void Start()
    {
        // Asegúrate de que la referencia al botón esté asignada
        if (buttonDraw != null)
        {
            // Añadir la función de dibujar constelación al evento OnClick del botón
            buttonDraw.onClick.AddListener(DrawConstellation);
        }
        else
        {
            Debug.LogError("El botón Draw no está asignado.");
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
        lineRenderer.startWidth = 0.1f; // Ancho de la línea
        lineRenderer.endWidth = 0.1f;

        for (int i = 0; i < planetSelector.selectedPlanets.Count; i++)
        {
            lineRenderer.SetPosition(i, planetSelector.selectedPlanets[i].transform.position);
        }
    }
}