using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CombatController : MonoBehaviour
{
    const float ENC_CHANCE = 10; // encounter chance in %
    //const float ENC_STRENGHT = 1f; // > 1 more strength, 1 = same level (combined)
    //const float ENC_ELITE_CHANCE = 20f; // in %
    const int ENC_MAX_EN = 3; // maximum of enemies

    public static CombatController Singleton;

    [SerializeField]
    Sprite RomanSprite;
    [SerializeField]
    Sprite WildSprite;
    [SerializeField]
    Sprite TopolonSprite;

    [SerializeField]
    Image BackgroundImage;
    [SerializeField]
    Sprite TopolonBackSprite;

    [SerializeField]
    Image OverscreenImage;
    [SerializeField]
    TextMeshProUGUI TitleText;
    [SerializeField]
    TextMeshProUGUI VictoryText;

    [SerializeField]
    GameObject CombatUi;

    [SerializeField]
    GameObject EnemyModel;
    [SerializeField]
    GameObject EnemyContainer;

    PlayerController Player;
    MasterController Master;

    //private List<PawnController> Scenario;
    Queue<PawnController> CombatQueue;
    List<EnemyController> Enemies;

    public bool Special = false; // combat with boos


    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;

        Player = PlayerController.Singleton;
        Master = MasterController.Singleton;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }


    // Checks if player encounters enemy on map
    public bool Encounter() {

        var roll = Random.Range(0, 100);
        //Debug.Log("roll " + roll);
        if (roll > ENC_CHANCE)
            return false;

        GameControlller.Singleton.Log("Pozor nepøítel!");

        return true;
    }

    // Setup boos fight
    public void BossFight() {

        // cleanup
        foreach (Transform child in EnemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

        CombatQueue = new Queue<PawnController>();
        CombatQueue.Enqueue(Player.Pawn);
        Enemies = new List<EnemyController>();

        OverscreenImage.sprite = TopolonSprite;
        TitleText.text = "Souboj s císaøem Typolonem";
        Special = true;

        var unit = Instantiate(EnemyModel);
        unit.transform.SetParent(EnemyContainer.transform);
        var enemy = unit.GetComponent<EnemyController>();
        enemy.Init("Typolon", Random.Range(10, 15));
        var pawn = enemy.Pawn;
        enemy.SetImage(pawn.Model.Model);
        enemy.SetupInventory(pawn);

        CombatQueue.Enqueue(pawn);
        Enemies.Add(enemy);

        BackgroundImage.sprite = TopolonBackSprite;

        StartCoroutine(ShowScreen());
    }

    // Setup combat
    public void SetupCombat() {

        // cleanup
        foreach (Transform child in EnemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

        CombatQueue = new Queue<PawnController>();
        CombatQueue.Enqueue(Player.Pawn);
        Enemies = new List<EnemyController>();

        List<string> poss = new List<string>();

        var libvals = Master.Library.GetLib(AssetLibrary.LibraryType.Pawns).Objects.Values;
        var pawns = libvals.Select(x => x as PawnSetup).ToList();

        // count of enemies
        var en_cnt = 1;
        if (Player.Pawn.Stats.Leveling.CurrentLevel > ENC_MAX_EN)
        {
            en_cnt = Random.Range(1, ENC_MAX_EN+1);
        }

        // has elite unit
        var elite = false;// Random.Range(0, 100) > ENC_ELITE_CHANCE;

        // type select
        var enemytype = Random.Range(1, Utils.EnumHelper.GetValues<PawnSetup.EnemyType>().Count);
        // get names of units of that type
        poss.AddRange(pawns.Where(x => x.TypeInt == enemytype && x.Rank == PawnSetup.EnemyRank.Pawn).Select(y => y.Id));

        switch ((PawnSetup.EnemyType)enemytype) {

            case PawnSetup.EnemyType.Roman:
                OverscreenImage.sprite = RomanSprite;
                TitleText.text = "Souboj s Øímany";
                break;

            case PawnSetup.EnemyType.Wild:
                OverscreenImage.sprite = WildSprite;
                TitleText.text = "Souboj s divoèinou";
                elite = false;
                break;

            case PawnSetup.EnemyType.Player:
            default:
                break;
        }

        var elites = pawns.Where(x => x.TypeInt == enemytype && x.Rank == PawnSetup.EnemyRank.Elite).Select(y => y.Id).ToList();

        var level_pool = Player.Pawn.Stats.Leveling.CurrentLevel;
        for (int i = 0; i < en_cnt; i++) {

            var targetlvl = Random.Range(1, level_pool - en_cnt + i);
            level_pool -= targetlvl;

            var target = Utils.CollectionExt.GetRandomItem(poss);
            if(elite && i == 0)
                target = Utils.CollectionExt.GetRandomItem(elites);
            Debug.Log("Unit " + target + " with lvl: " + targetlvl);

            var unit = Instantiate(EnemyModel);
            unit.transform.SetParent(EnemyContainer.transform);
            var enemy = unit.GetComponent<EnemyController>();
            enemy.Init(target, targetlvl);
            var pawn = enemy.Pawn;
            enemy.SetImage(pawn.Model.Model);
            enemy.SetupInventory(pawn);

            CombatQueue.Enqueue(pawn);
            Enemies.Add(enemy);
        }


        StartCoroutine(ShowScreen());

    }

    // Starts combat
    public void StartCombat() {

        CombatUi.SetActive(true);

        NewTurn();
    }

    // New turn of pawn
    public void NewTurn() {

        var pawn = CombatQueue.Dequeue();
        var alive = Enemies.Where(x => !x.Dead).ToList();
        foreach (var enemy in alive) {

            enemy.ToggleAttackButton(pawn.IsPlayer);
        }

        if (!pawn.IsPlayer)
            StartCoroutine(SimulateAttack(pawn));    
    }

    // Simple AI - attacks player with simulated thinking
    private IEnumerator SimulateAttack(PawnController pawn) {

        if (!pawn.Dead) {

            var enemy = Enemies.Where(x => x.Pawn == pawn).First();
            yield return new WaitForSeconds(2);

            enemy.DoDamage();
        }
        
        EndTurn(pawn);
    }

    // Pawn ended turn
    public void EndTurn(PawnController pawn) {

        if (!pawn.Dead)
            CombatQueue.Enqueue(pawn);

        if (CombatQueue.Count <= 1)
        {

            StartCoroutine(EndCombat());
        }
        else {

            NewTurn();
        }
    }

    // Combat is over
    private IEnumerator EndCombat() {

        CombatUi.SetActive(false);
        OverscreenImage.transform.parent.gameObject.SetActive(true);
        VictoryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);

        // drops from dead
        foreach (var enemy in Enemies) {

            foreach (var item in enemy.Pawn.Model.Items) {

                if (item.DropChance > Random.Range(0.0f, 1.0f)) {

                    int lvl = enemy.Pawn.Stats.Leveling.CurrentLevel;
                    var inst = Master.SpawnItem(item.Id);
                    inst.Init();
                    if (inst.Type != InventoryItem.ItemType.Material)
                        inst.Level = lvl;

                    Player.Pawn.Inventory.AddItem(inst);
                }
            }
        }

        OverscreenImage.transform.parent.gameObject.SetActive(false);
        VictoryText.gameObject.SetActive(false);

        if (Special)
            GameControlller.Singleton.EndGameVictory();
    }

    // Show combat screen
    private IEnumerator ShowScreen() {

        //Debug.Log(gameObject.name);
        VictoryText.gameObject.SetActive(false);
        OverscreenImage.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);

        OverscreenImage.transform.parent.gameObject.SetActive(false);
        StartCombat();
        yield return null;
    }
}
