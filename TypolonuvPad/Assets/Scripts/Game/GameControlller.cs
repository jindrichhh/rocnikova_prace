using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Direction { 

    Right,
    Down,
    Left,
    Up
}


public class GameControlller : MonoBehaviour
{
    public class GameFlag {

        Dictionary<string, bool> Flags;

        public GameFlag() {

            Flags = new Dictionary<string, bool>();
        }


        public void AddFlag(string key) {

            Flags.Add(key, false);
        }

        public void SetFlag(string key, bool bit) {

            if (!Flags.ContainsKey(key))
                return;

            Flags[key] = bit;
        }

        public bool Isset(string key) {

            if (!Flags.ContainsKey(key))
                return false;

            return Flags[key];
        }
    }


    public static GameControlller Singleton;
    public static GameFlag Flags;
    

    [SerializeField]
    LoadingScreenController Lsc;

    [Header("Logging")]
    [SerializeField]
    GameObject LogItemModel;
    [SerializeField]
    GameObject LogContainer;


    MapController Map;
    PlayerController Player;
    HudPanelController Hud;


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;

        Map = MapController.Singleton;
        Player = PlayerController.Singleton;
        Hud = HudPanelController.Singleton;

        Flags = new GameFlag();
        Flags.AddFlag("Wizard");
        Flags.AddFlag("Slaves");
        Flags.AddFlag("Sword");
        Flags.AddFlag("Topolon");

        Lsc.gameObject.SetActive(true);
    }

    void Start()
    {
        HudPanelController.Singleton.RefreshData(Player.Pawn.Stats);
        Log("Hra spuštìna");
    }


    float wait_timer = 3.0f;
    void Update()
    {

        wait_timer -= Time.deltaTime;
        if (wait_timer < 0) {

            HideLsc();
        }

    }



    // Hides loading screen
    public void HideLsc() {

        Lsc.gameObject.SetActive(false);        
    }

    public void EndTurn() {

        Hud = HudPanelController.Singleton;

        var inv = Player.Pawn.Inventory;
        var meat = inv.FindItem("Meat");
        var wood = inv.FindItem("Firewood");

        // check sleeping on camp
        if (Map.IsCamp(Player.Coords) && wood != null)
        {
            float basic = 0.4f;
            Log("Táboøení zahájeno");
            if (meat != null)
            {
                Log("Peèené maso! 200 % bonus k obnovì zdraví");
                inv.RemoveItem(meat);
                Hud.Camping.SetBack_CampFood();
                basic *= 2;
            }
            else {
                Log("Lehká veèeøe, 100 % bonus k obnovì zdraví");
                Hud.Camping.SetBack_Camp();
            }

            inv.RemoveItem(wood);
            Player.Pawn.Stats.Heal(basic);
        }
        else {

            Log("Pøenocování bez ohništì");
            Hud.Camping.SetBack_Forest();
            Player.Pawn.Stats.Heal(10);
        }

        Player.Pawn.Stats.ActionPoints.Renew();
        Hud.RefreshData(Player.Pawn.Stats);
    }


    public TextMeshProUGUI Log(string fmt, params object[] pars) {

        var item = Instantiate(LogItemModel);
        var text = item.GetComponentInChildren<TextMeshProUGUI>();
        text.text = string.Format(fmt, pars);

        item.transform.SetParent(LogContainer.transform);

        return text;
    }

    public void Warn(string fmt, params object[] pars) {

        var text = Log(fmt, pars);
        text.color = Utils.UnityHelper.FromSysColor(0xFFEE9B01);
    }

    public void PosLog(string fmt, params object[] pars) {

        var text = Log(fmt, pars);
        text.color = Utils.UnityHelper.FromSysColor(0xFF00FF00);
    }
}
