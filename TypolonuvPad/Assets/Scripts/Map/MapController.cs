using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{

    public static MapController Singleton;

    [SerializeField]
    Tilemap GroundTm;
    [SerializeField]
    Tilemap PoiTm;
    [SerializeField]
    Tilemap FowTm;
    [SerializeField]
    Tilemap ObstTm;
    [SerializeField]
    Tilemap PlayerTm;
    [SerializeField]
    Tilemap NpcTm;

    [SerializeField]
    Tile PlayerTile;

    MapCell[,] MapCell;
    Vector2Int CoordOffset;

    PlayerController Player;
    GameControlller Game;
    CombatController Combat;


    private void Awake()
    {

        if (Singleton == null)
            Singleton = this;

        FowTm.gameObject.SetActive(true);

        var x = GroundTm.cellBounds.size.x;
        var y = GroundTm.cellBounds.size.y;
        CoordOffset = new Vector2Int(
            GroundTm.cellBounds.xMin, GroundTm.cellBounds.yMin);

        MapCell = new MapCell[x, y];
        for (int i = GroundTm.cellBounds.xMin; i < GroundTm.cellBounds.xMax; i++) {

            for (int j = GroundTm.cellBounds.yMin; j < GroundTm.cellBounds.yMax; j++) {

                //Debug.Log("ij: " + i + ", " + j);
                //Debug.Log("coords: " + (i - CoordOffset.x) + ", " + (j - CoordOffset.y));
                MapCell[i - CoordOffset.x, j - CoordOffset.y] = new MapCell(i, j);
            }
        }

        PlayerTm.cellBounds.SetMinMax(GroundTm.cellBounds.min, GroundTm.cellBounds.max);
        PoiTm.cellBounds.SetMinMax(GroundTm.cellBounds.min, GroundTm.cellBounds.max);
        NpcTm.cellBounds.SetMinMax(GroundTm.cellBounds.min, GroundTm.cellBounds.max);

        Player = PlayerController.Singleton;
        Game = GameControlller.Singleton;
        Combat = CombatController.Singleton;
        RevealSur(Player.Coords);
    }

    // Get cell from map on coordinates
    public MapCell GetCell(int x, int y) {

        try
        {

            var cell = MapCell[x - CoordOffset.x, y - CoordOffset.y];
            return cell;
        }
        catch (System.Exception) {

            
        }

        return null;
    }
    public MapCell GetCell(Vector3Int v3) {

        return GetCell(v3.x, v3.y);
    }

    // Moves player avatar
    public void MovePlayer(int dir) {

        Game = GameControlller.Singleton;

        Vector3Int v3;
        switch ((Direction)dir) {

            case Direction.Right:
                v3 = Vector3Int.right;
                //Game.Log("Pohybuji se na východ");
                break;
            case Direction.Down:
                v3 = Vector3Int.down;
                //Game.Log("Pohybuji se na jih");
                break;
            case Direction.Left:
                v3 = Vector3Int.left;
                //Game.Log("Pohybuji se na západ");
                break;
            case Direction.Up:
            default:
                v3 = Vector3Int.up;
                //Game.Log("Pohybuji se na sever");
                break;
        }

        var dest = Player.Coords + v3;

        // check obstacle in movement dest
        if (GetCell(dest).HasTile(ObstTm)) {

            Game.Warn("Nelze se pohnout, cílová destinace je nepøístupná");
            return;
        }


        // check move points
        if (Player.Pawn.Stats.ActionPoints.Current == 0) {

            Game.Warn("Nedostatek pohybových bodù");
            return;
        }
            


        // move icon
        var cell = GetCell(Player.Coords.x, Player.Coords.y);
        cell.RemoveSprite(PlayerTm);
        Player.Coords = dest;
        cell = GetCell(Player.Coords.x, Player.Coords.y);
        cell.ChangeSprite(PlayerTm, PlayerTile);

        // sub point
        Player.Pawn.Stats.ActionPoints.Add(-1);

        // Visuals
        HudPanelController.Singleton.RefreshData(Player.Pawn.Stats);
        RevealSur(Player.Coords);

        // Triggers
        CheckTileAction();
        if (Combat.Encounter()) {

            Combat.SetupCombat();
        }
    }

    // Reveal surrouning tiles (FOW)
    public void RevealSur(Vector3Int coords) {

        for (int i = -1; i < 2; i++) {

            for (int j = -1; j < 2; j++) {

                var cell = GetCell(i + coords.x, j + coords.y);
                if (cell != null) {

                    cell.RemoveSprite(FowTm);
                }
            }
        }
    }

    // Check if can camp here
    public bool IsCamp(Vector3Int coords) {

        var cell = GetCell(coords);
        return cell.HasTile(PoiTm);
    }

    // Special tiles - triggering action
    private void CheckTileAction() {

        var coords = Player.Coords;
        var cell = GetCell(coords);
        

        if (cell.HasTile(NpcTm)) {

            var n = cell.GetName(NpcTm, coords);
            Game.Log("Zaèínám rozhovor s " + n);

            switch (n) {

                case "wizard":

                    if(GameControlller.Flags.Isset("Wizard")) {

                        Game.Log("Èarodìj hledí zamyšlenì pøed sebe.");
                        break;
                    }

                    Game.Log("Èarodìj mumlal nìco o bájném meèi ukrytém v nedaleké jeskyni.");
                    Game.Log("Pokud je meè opravdu tak silný, pomùže ti pøemoci Topolona");
                    RevealSur(new Vector3Int(9, 11, 0));
                    Game.PosLog("Odhalena pozice jeskynì");
                    GameControlller.Flags.SetFlag("Wizard", true);
                    break;

                case "slave":

                    if (GameControlller.Flags.Isset("Topolon"))
                    {

                        Game.Log("Zbyla tu jen známka po lidské pøitomnosti a hromada øetìzù");
                        break;
                    }

                    Game.Log("Tábor otrokù je velmi špatnì hlídaný. Lehce pøemáháš strážce...");
                    Game.Log("Jeden z otrokù ti lámaným zpùsobem ve tvém jazyce odhaluje pozice základny Topolona!");
                    RevealSur(new Vector3Int(-15, -10, 0));
                    Game.PosLog("Odhalena pozice Topolona");
                    GameControlller.Flags.SetFlag("Topolon", true);
                    break;


                default:
                    Game.Warn("Nezpracovany trigger pro NPC");
                    break;
            }
        }

        if (cell.HasTile(PoiTm))
        {

            var n = cell.GetName(PoiTm, coords);
            Game.Log("Objev zajímavosti - " + n);

            switch (n)
            {
                case "cave":

                    if (GameControlller.Flags.Isset("Cave"))
                    {

                        Game.Log("V jeskyni není již nic zajímavého...");
                        Game.Log("V jeskyni lze táboøit");
                        break;
                    }

                    var sword = MasterController.Singleton.SpawnItem("GaliaSword", 10);
                    Player.Pawn.Inventory.AddItem(sword);
                    Game.PosLog("Získal jsi bájný meè Gálie!");
                    break;

                case "base":

                    Game.Log("Typolon: Pøiprav se na smrt!");
                    Combat.BossFight();

                    break;

                default:
                    Game.Warn("Nezpracovany trigger pro POI");
                    break;
            }
        }
    }
}
