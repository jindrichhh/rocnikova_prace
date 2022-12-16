using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{

    public static MasterController Singleton;

    public AssetLibrary Library;

    private void Awake()
    {
        if (Singleton == null) {

            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
            
        else
            return;

        //Library.LoadAsync();

    }


    // Spawns item from library
    public InventoryItem SpawnItem(string key, int level = 0) {

        var item = Library.Get<InventoryItem>(AssetLibrary.LibraryType.Items, key);
        item.Init(level);

        return item;
    }

    // Gets graphics from library
    public Sprite GetSprite(string key) {

        return Library.Get<Sprite>(AssetLibrary.LibraryType.Graphics, key);
    }
}
