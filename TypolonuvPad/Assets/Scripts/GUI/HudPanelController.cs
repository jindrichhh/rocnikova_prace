using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudPanelController : MonoBehaviour
{

    public static HudPanelController Singleton;

    public GameObject InventoryPanel;
    public GameObject AttributePanel;

    public CampingScreenController Camping;

    [SerializeField]
    TextMeshProUGUI ActionPoints;
    [SerializeField]
    ProgressBar ActionPointsBar;


    void Start()
    {
        if (Singleton == null)
            Singleton = this;
    }

    void Update()
    {
        
    }


    // Refresh UI
    public void RefreshData(PawnStat stats) {

        AttributePanel.GetComponent<AttributePanelController>().RefreshData(stats);

        var ap = stats.ActionPoints;
        ActionPoints.text = string.Format("{0} / {1}", ap.Current, ap.Capacity());
        ActionPointsBar.SetProgress(ap.Current, ap.Capacity());
    }

    // Scrolls to bottom
    public void ScrollToBottom(ScrollRect scroll) {

        scroll.normalizedPosition = new Vector2(0, 0);
    }
}
