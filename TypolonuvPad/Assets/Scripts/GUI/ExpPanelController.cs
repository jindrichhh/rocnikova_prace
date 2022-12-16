using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpPanelController : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI CurrentLevel;
    [SerializeField]
    TextMeshProUGUI CurrentExp;
    [SerializeField]
    TextMeshProUGUI NextLevel;
    [SerializeField]
    TextMeshProUGUI PointsLeft;

    [SerializeField]
    ProgressBar LevelProgress;


    //void Start()
    //{
        
    //}

    //void Update()
    //{
        
    //}


    // Refresh UI
    public void RefreshData(PawnStat.Stat.Level leveling)
    {
        // exp
        CurrentLevel.text = leveling.CurrentLevel + "";
        CurrentExp.text = leveling.TotalExpPoints + "";
        NextLevel.text = leveling.NextLevel + "";
        PointsLeft.text = leveling.PointsRemaining + "";

        LevelProgress.SetProgress(leveling.ExpPoints, leveling.ExpNeeded);
    }
}
