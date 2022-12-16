using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrecacheController : MonoBehaviour
{

    MasterController Master;

    private void Awake()
    {
        Master = MasterController.Singleton;
    }

    void Start()
    {
        Master.Library.LoadAsync();
    }

    void Update()
    {
        if (Master.Library.Ready)
            SceneManager.LoadScene("Game");
    }
}
