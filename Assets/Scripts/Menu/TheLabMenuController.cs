using Michsky.UI.Reach;
using UnityEngine;

public class TheLabMenuController : MonoBehaviour
{
    public ButtonManager simulationsButton;
    public ButtonManager periodicTableButton;
    public ButtonManager constellationsButton;
    public ButtonManager glossaryButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // EnterTheLabMenu();
    }

    // Update is called once per frame
    void Update() { }

    /// <summary>
    ///
    /// </summary>
    public void EnterTheLabMenu()
    {
        ResetButtons();
        simulationsButton.HighlightButton();
        simulationsButton.Interactable(false);
        simulationsButton.UpdateUI();
    }

    /// <summary>
    ///
    /// </summary>
    void ResetButtons()
    {
        periodicTableButton.UnhihglightButton();
        constellationsButton.UnhihglightButton();
        glossaryButton.UnhihglightButton();
        periodicTableButton.UpdateUI();
        constellationsButton.UpdateUI();
        glossaryButton.UpdateUI();
    }
}
