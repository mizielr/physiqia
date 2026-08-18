using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Michsky.UI.Reach;
using Physiqia.TheLab.Constellation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationManager : MonoBehaviour
{
    [Header("Containers & Prefabs")]
    [SerializeField]
    private GameObject constellationsContainer;

    [SerializeField]
    private GameObject starPrefab;

    [SerializeField]
    private GameObject filtersPanel;

    [SerializeField]
    private ButtonManager buttonPrefab;

    [SerializeField]
    private Transform gridParent;

    [Header("Pagination")]
    [SerializeField]
    private int constellationsPerPage = 10;

    [SerializeField]
    private ButtonManager previousPageButton;

    [SerializeField]
    private ButtonManager nextPageButton;

    [Header("Filters")]
    [SerializeField]
    private TMP_Dropdown hemisphereDropdown;

    [SerializeField]
    private TMP_Dropdown seasonDropdown;

    [SerializeField]
    private TMP_Dropdown groupDropdown;

    [SerializeField]
    private TMP_Dropdown typeDropdown;

    [SerializeField]
    private TMP_InputField searchField;

    [Header("Visual Settings")]
    [SerializeField]
    private float scaleFactor = 10f;

    [SerializeField]
    private float lineWidth = 0.1f;

    [SerializeField]
    private Material lineMaterial;

    [Header("Info Panels")]
    [SerializeField]
    private TMP_Text constellationName;

    [SerializeField]
    private GameObject constellationInfosPanel;

    [SerializeField]
    private GameObject starInfosPanel;

    [SerializeField]
    private GameObject canvasUWI;

    [Header("Internal")]
    private GameObject currentConstellationObj;
    private List<(string name, IConstellation instance)> constellations;
    private List<(string name, IConstellation instance)> filteredConstellations;
    private int currentPage = 0;

    /// <summary>
    ///
    /// </summary>
    void Start()
    {
        if (starPrefab == null)
            return;

        constellations = new List<(string, IConstellation)>();
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (typeof(IConstellation).IsAssignableFrom(type) && !type.IsInterface)
            {
                IConstellation instance = Activator.CreateInstance(type) as IConstellation;
                constellations.Add((type.Name, instance));
            }
        }

        if (constellations.Count == 0)
            return;

        constellations = constellations.OrderBy(c => c.instance.Metadata.Name ?? c.name).ToList();

        InitializeDropdown(
            hemisphereDropdown,
            new[] { "All", "Northern", "Southern", "Equatorial" }
        );
        InitializeDropdown(
            seasonDropdown,
            new[] { "All", "Winter", "Spring", "Summer", "Autumn", "Year-round" }
        );
        InitializeDropdown(
            groupDropdown,
            new[]
            {
                "All",
                "Zodiac",
                "Ursa Major Family",
                "Orion Family",
                "Hercules Family",
                "Perseus Family",
                "Bayer Family",
                "La Caille Family",
                "Heavenly Waters",
                "Other",
            }
        );
        InitializeDropdown(typeDropdown, new[] { "All", "Ancient", "Modern" });

        hemisphereDropdown.onValueChanged.AddListener(_ => UpdateFilters());
        seasonDropdown.onValueChanged.AddListener(_ => UpdateFilters());
        groupDropdown.onValueChanged.AddListener(_ => UpdateFilters());
        typeDropdown.onValueChanged.AddListener(_ => UpdateFilters());
        searchField.onValueChanged.AddListener(_ => UpdateFilters());

        UpdateFilters();
    }

    /// <summary>
    ///
    /// </summary>
    void Update() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dropdown"></param>
    /// <param name="options"></param>
    private void InitializeDropdown(TMP_Dropdown dropdown, string[] options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options.ToList());
    }

    /// <summary>
    ///
    /// </summary>
    private void UpdateFilters()
    {
        string hemisphere = hemisphereDropdown.options[hemisphereDropdown.value].text;
        string season = seasonDropdown.options[seasonDropdown.value].text;
        string group = groupDropdown.options[groupDropdown.value].text;
        string type = typeDropdown.options[typeDropdown.value].text;
        string search = searchField.text.ToLower();

        filteredConstellations = constellations
            .Where(c =>
            {
                var meta = c.instance.Metadata;
                return (hemisphere == "All" || meta.Hemisphere == hemisphere)
                    && (season == "All" || meta.Season == season)
                    && (group == "All" || meta.Group == group)
                    && (type == "All" || meta.Type == type)
                    && (
                        string.IsNullOrEmpty(search)
                        || (meta.CommonName?.ToLower().Contains(search) ?? false)
                        || c.name.ToLower().Contains(search)
                    );
            })
            .ToList();

        currentPage = 0;
        UpdatePage();
    }

    /// <summary>
    ///
    /// </summary>
    private void UpdatePage()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        int start = currentPage * constellationsPerPage;
        int end = Mathf.Min(start + constellationsPerPage, filteredConstellations.Count);
        for (int i = start; i < end; i++)
        {
            var (name, instance) = filteredConstellations[i];
            ButtonManager btn = Instantiate(buttonPrefab, gridParent);
            btn.SetText(instance.Metadata.Name ?? name);
            btn.onClick.AddListener(() => OnConstellationSelected(instance));
        }

        previousPageButton.Interactable(currentPage > 0);
        nextPageButton.Interactable(end < filteredConstellations.Count);
    }

    /// <summary>
    ///
    /// </summary>
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void NextPage()
    {
        if ((currentPage + 1) * constellationsPerPage < filteredConstellations.Count)
        {
            currentPage++;
            UpdatePage();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="constellation"></param>
    private void OnConstellationSelected(IConstellation constellation)
    {
        foreach (Transform child in constellationsContainer.transform)
        {
            child.gameObject.SetActive(false);
        }

        GameObject _constellation = constellationsContainer
            .transform.Find(constellation.Metadata.Name)
            ?.gameObject;
        _constellation.SetActive(true);
        constellationName.text = constellation.Metadata.Name;

        RenderLines(_constellation, constellation.Connections);
        // TODO: LOAD CONSTELLATION DATA
        constellationInfosPanel.SetActive(true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="constellation"></param>
    /// <param name="connections"></param>
    private void RenderLines(GameObject constellation, int[][] connections)
    {
        DestroyAllLineRenderers(constellation);
        foreach (int[] connection in connections)
        {
            GameObject objA = constellation.transform.Find(connection[0].ToString())?.gameObject;
            GameObject objB = constellation.transform.Find(connection[1].ToString())?.gameObject;

            if (objA != null && objB != null)
            {
                CreateLineBetween(objA, objB, 0.5f, lineMaterial, constellation.transform);
            }
            else
            {
                if (objA == null)
                {
                    // Debug.Log("OBJA: KO");
                }

                if (objB == null)
                {
                    // Debug.Log("OBJB: KO");
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parent"></param>
    private void DestroyAllLineRenderers(GameObject parent)
    {
        LineRenderer[] lineRenderers = parent.GetComponentsInChildren<LineRenderer>(true);

        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            Destroy(lineRenderer.gameObject);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="startObject"></param>
    /// <param name="endObject"></param>
    /// <param name="lineWidth"></param>
    /// <param name="lineMaterial"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private LineRenderer CreateLineBetween(
        GameObject startObject,
        GameObject endObject,
        float lineWidth,
        Material lineMaterial,
        Transform parent = null
    )
    {
        GameObject lineObj = new GameObject($"Line_{startObject.name}_to_{endObject.name}");
        if (parent != null)
        {
            lineObj.transform.SetParent(parent);
        }

        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        line.positionCount = 2;
        Vector3 startPos = startObject.transform.position;
        Vector3 endPos = endObject.transform.position;
        Debug.Log($"Ligne de {startObject.name} ({startPos}) à {endObject.name} ({endPos})");
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.useWorldSpace = true;

        if (lineMaterial != null)
        {
            line.material = lineMaterial;
        }

        return line;
    }

    /// <summary>
    ///
    /// </summary>
    public void OpenFiltersPanel()
    {
        canvasUWI.SetActive(false);
        filtersPanel.SetActive(true);
    }

    /// <summary>
    ///
    /// </summary>
    public void CloseFiltersPanel()
    {
        filtersPanel.SetActive(false);
        canvasUWI.SetActive(true);
    }

    /// <summary>
    ///
    /// /// </summary>
    public void ResetFilters()
    {
        hemisphereDropdown.value = 0;
        seasonDropdown.value = 0;
        groupDropdown.value = 0;
        typeDropdown.value = 0;
        searchField.text = string.Empty;
        UpdateFilters();
    }

    /// <summary>
    ///
    /// </summary>
    public void CloseStarInfosPanel()
    {
        starInfosPanel.SetActive(false);
    }
}
