using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatAttributesPanelController : MonoBehaviour
{

    enum StatType { 
    
        Hp = 0,
        Armor,
        Attack
    }


    [SerializeField]
    TextMeshProUGUI HpLevel;
    [SerializeField]
    TextMeshProUGUI Hp;
    [SerializeField]
    ProgressBar HpProgress;
    [SerializeField]
    Button HpAdd;

    [SerializeField]
    TextMeshProUGUI ArmorLevel;
    [SerializeField]
    TextMeshProUGUI Armor;
    [SerializeField]
    TextMeshProUGUI ArmorBonus;
    [SerializeField]
    Button ArmorAdd;

    [SerializeField]
    TextMeshProUGUI AttLevel;
    [SerializeField]
    TextMeshProUGUI Att;
    [SerializeField]
    TextMeshProUGUI AttBonus;
    [SerializeField]
    Button AttAdd;


    PlayerController Player;


    private void Awake()
    {
        Player = PlayerController.Singleton;
    }

    //void Start()
    //{
        
    //}

    
    //void Update()
    //{
        
    //}


    // Refresh UI
    public void RefreshData(PawnStat.Stat hp, PawnStat.Stat armor, PawnStat.Stat att) {

        Player = PlayerController.Singleton;

        var p_av = Player.Pawn.Stats.Leveling.PointsRemaining > 0;

        // add buttons
        HpAdd.gameObject.SetActive(p_av);
        ArmorAdd.gameObject.SetActive(p_av);
        AttAdd.gameObject.SetActive(p_av);

        // hp stats
        Hp.text = string.Format("{0} / {1}", hp.Current, hp.Capacity());
        HpProgress.SetProgress(hp.Current, hp.Capacity());
        HpLevel.text = hp.Points + "";

        // armor stats
        Armor.text = armor.Current + "";
        ArmorLevel.text = armor.Points + "";
        ArmorBonus.text = armor.Bonus + "";

        // attack stats
        Att.text = att.Current + "";
        AttLevel.text = att.Points + "";
        AttBonus.text = att.Bonus + "";
    }

    // Add point to PawnStat
    public void AddPointToStat(int stattype) {

        StatType st = (StatType)stattype;
        PawnStat ps = Player.Pawn.Stats;

        switch (st) {

            case StatType.Hp:
                ps.HitPoints.AddPoint();
                break;

            case StatType.Armor:
                ps.Defence.AddPoint();
                break;

            case StatType.Attack:
                ps.Attack.AddPoint();
                break;


            default:
                break;
        }

        ps.Leveling.PointsRemaining--;
        HudPanelController.Singleton.RefreshData(ps);

    }
}
