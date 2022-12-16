using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampingScreenController : MonoBehaviour
{

    [SerializeField]
    Sprite CampSprite;
    [SerializeField]
    Sprite CampFoodSprite;
    [SerializeField]
    Sprite ForestSprite;

    [SerializeField]
    Image Background;
    [SerializeField]
    TextMeshProUGUI CampText;


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SetBack_Camp() {

        StartCoroutine(SetBack(CampSprite, "T�bo�en� . . ."));
    }

    public void SetBack_CampFood()
    {
        StartCoroutine(SetBack(CampFoodSprite, "T�bo�en� s bohatou ve�e�� . . ."));
    }

    public void SetBack_Forest()
    {
        StartCoroutine(SetBack(ForestSprite, "Sp�nek bez ohn�"));
    }

    private IEnumerator SetBack(Sprite sprite, string text) {

        Background.enabled = true;
        Background.sprite = sprite;
        CampText.enabled = true;
        CampText.text = text;

        yield return new WaitForSeconds(5);

        CampText.enabled = false;
        Background.enabled = false;
        yield return null;
    }
}
