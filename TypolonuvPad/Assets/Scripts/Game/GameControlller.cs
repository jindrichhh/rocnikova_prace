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

        // Register new flag
        public void AddFlag(string key) {

            Flags.Add(key, false);
        }

        // Set registered flag to value
        public void SetFlag(string key, bool bit) {

            if (!Flags.ContainsKey(key))
                return;

            Flags[key] = bit;
        }

        // Check if registered flag is set
        public bool Isset(string key) {

            if (!Flags.ContainsKey(key))
                return false;

            return Flags[key];
        }
    }


    public static GameControlller Singleton;
    public static GameFlag Flags;
    public static int Kills = 0;


    [SerializeField]
    LoadingScreenController Lsc;

    [Header("Logging")]
    [SerializeField]
    GameObject LogItemModel;
    [SerializeField]
    GameObject LogContainer;

    [SerializeField]
    GameObject VictoryOverlay;
    [SerializeField]
    TextMeshProUGUI KillText;
    [SerializeField]
    TextMeshProUGUI ItemText;
    [SerializeField]
    TextMeshProUGUI LevelText;

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
        Log("Hra spu?t?na");
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

    // End of turn on map
    public void EndTurn() {

        Map = MapController.Singleton;
        Player = PlayerController.Singleton;
        Hud = HudPanelController.Singleton;

        var inv = Player.Pawn.Inventory;
        var meat = inv.FindItem("Meat");
        var wood = inv.FindItem("Firewood");

        // check sleeping on camp
        if (Map.IsCamp(Player.Coords) && wood != null)
        {
            float basic = 0.4f;
            Log("T?bo?en? zah?jeno");
            if (meat != null)
            {
                Log("Pe?en? maso! 200 % bonus k obnov? zdrav?");
                inv.RemoveItem(meat);
                Hud.Camping.SetBack_CampFood();
                basic *= 2;
            }
            else {
                Log("Lehk? ve?e?e, 100 % bonus k obnov? zdrav?");
                Hud.Camping.SetBack_Camp();
            }

            inv.RemoveItem(wood);
            Player.Pawn.Stats.Heal(basic);
        }
        else {

            Log("P?enocov?n? bez ohni?t?");
            Hud.Camping.SetBack_Forest();
            Player.Pawn.Stats.Heal(10);
        }

        Player.Pawn.Stats.ActionPoints.Renew();
        Hud.RefreshData(Player.Pawn.Stats);
    }

    // Logs message to infopanel
    public TextMeshProUGUI Log(string fmt, params object[] pars) {

        var item = Instantiate(LogItemModel);
        var text = item.GetComponentInChildren<TextMeshProUGUI>();
        text.text = string.Format(fmt, pars);

        item.transform.SetParent(LogContainer.transform);

        return text;
    }

    // Logs warning to infopanel
    public void Warn(string fmt, params object[] pars) {

        var text = Log(fmt, pars);
        text.color = Utils.UnityHelper.FromSysColor(0xFFEE9B01);
    }

    // Logs possitive message to infopanel
    public void PosLog(string fmt, params object[] pars) {

        var text = Log(fmt, pars);
        text.color = Utils.UnityHelper.FromSysColor(0xFF00FF00);
    }

    // End of game, player won
    public void EndGameVictory() {

        if (Player == null)
            Player = PlayerController.Singleton;

        KillText.text = "Zabit?ch nep??tel: " + Kills;
        ItemText.text = "Nalezen?ch p?edm?t?: " + InventoryController.Found;
        LevelText.text = "Hr??ova ?rove?: " + Player.Pawn.Stats.Leveling.CurrentLevel;
        VictoryOverlay.gameObject.SetActive(true);
    }
}
