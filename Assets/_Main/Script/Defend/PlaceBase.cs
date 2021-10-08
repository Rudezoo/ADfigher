using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBase : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Base;

    private PlacementIndicator placementIndicator;


    void Start()
    {

        Base.SetActive(false);
        placementIndicator = FindObjectOfType<PlacementIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacingBase() 
    {
        SoundManager.instance.playmenuSound();
        Base.SetActive(true);
        placementIndicator.gameObject.SetActive(false);
        InGameManager.instance.placeBtn.SetActive(false);
        Base.transform.position = placementIndicator.transform.position;
        InGameManager.instance.Ready();
    
    }
}
