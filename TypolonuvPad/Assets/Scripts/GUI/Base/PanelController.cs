using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{

    public PlayerController Player;


    private void Awake()
    {
        Player = PlayerController.Singleton;
        //gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    // Toggle panel
    public virtual void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }


    //public virtual void RefreshData() { 
    

    //}
}
