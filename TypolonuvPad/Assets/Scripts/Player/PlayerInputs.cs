using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    [SerializeField]
    Camera MainCamera;

    PlayerController Player;


    private void Awake()
    {
        Player = gameObject.GetComponent<PlayerController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();

        CheckKeyboard();
    }


    // Change position of camera
    private void MoveCamera()
    {

        const float speed = 3f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            MainCamera.gameObject.transform.position += Vector3.up * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {

            MainCamera.gameObject.transform.position += Vector3.down * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {

            MainCamera.gameObject.transform.position += Vector3.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            MainCamera.gameObject.transform.position += Vector3.left * Time.deltaTime * speed;
        }

    }

    // Change Z of camera
    private void ZoomCamera()
    {

        const float speed = 0.7f;
        const float MIN = 3;
        const float MAX = 10;

        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {

            MainCamera.orthographicSize -= speed;
            if (MainCamera.orthographicSize <= MIN)
            {

                MainCamera.orthographicSize = MIN;
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {

            MainCamera.orthographicSize += speed;
            if (MainCamera.orthographicSize >= MAX)
            {

                MainCamera.orthographicSize = MAX;
            }
        }
    }

    // Keyboard inputs
    private void CheckKeyboard() {

        // test key
        if (Input.GetKeyDown(KeyCode.T)) {

            Player.Pawn.Stats.Leveling.AddExp(Random.Range(10, 50));
            HudPanelController.Singleton.RefreshData(Player.Pawn.Stats);
        }


        // inventory
        if (Input.GetKeyDown(KeyCode.I)) {

            Player.Hud.InventoryPanel.GetComponent<InventoryPanelController>().TogglePanel();
        }

        // stats
        if (Input.GetKeyDown(KeyCode.H))
        {
            var ap = Player.Hud.AttributePanel.GetComponent<AttributePanelController>();
            ap.TogglePanel();
        }
    }
}
