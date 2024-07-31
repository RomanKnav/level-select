using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// what obj is this script put on? on the individual map point objects. Each is a gateway to a LEVEL.

// note: a mapPoint is simply a gameObject with a square sprite, circle collider, and this script on it.

// SCRIPT ADDED POST-DOWNLOAD:
public class MapPoint : MonoBehaviour
{
    // way too many fucking vars

    // wtf is region and endregion? just for organizing. Collapsable!
    #region Variables           
    [Header("Waypoints")]
    public MapPoint up;
    public MapPoint right, down, left;      // different directions player can go

    [Header("Scene options")]
    [SerializeField] int levelIndex = 0;
    [HideInInspector] public string sceneToLoad;    // what is this? does not show this public var in Inspector window
    [TextArea(1, 2)]
    public string levelName;

    // for the different types of points on the map
    [Header("MapPoint Options")]
    [HideInInspector] public bool isLocked;
    public bool isLevel;
    public bool isCorner;
    public bool isWarpPoint;

    // what is a warp point? point that takes to another "level"
    [Header("Warp Options")]
    public bool autoWarped;
    [HideInInspector] public bool hasWarped;
    public MapPoint warpPoint;

    [Header("Image Options")]
    [SerializeField] Sprite unlockedSprite = null; 
    [SerializeField] Sprite lockedSprite = null;

    [Header("Level UI Objects")]
    [SerializeField] TextMeshProUGUI levelText = null; 
    [SerializeField] GameObject levelPanel = null;  
    // wtf exactly is levelPanel? it's a text box that appears when player interacts with different levels.  

    SpriteRenderer spriteRenderer;  // determines image to set on the levels
    #endregion   

    #region Unity Base Methods

    void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // this is a GameObject:
        if (levelPanel != null) {
            levelPanel.SetActive(false);    // hidden by default
        }

        // a warp point is a point on the map that takes player from 1 map to another:
        if (!isLevel && !isWarpPoint) {
            if (isLocked && lockedSprite != null) 
                spriteRenderer.sprite = lockedSprite;   // draw lockedSprite if not a level/warpedPoint
            else
                spriteRenderer.sprite = null;
        }

        // if we have a level...
        else {
            if (isLevel) {
                // DataManger.cs has no fucking "sceneToLoad" property. But does have "currentLevelName"
                sceneToLoad = DataManager.instance.gameData.lockedLevels[levelIndex].sceneToLoad;
                isLocked = DataManager.instance.gameData.lockedLevels[levelIndex].isLocked;             // this is a bool
            }

            if (isLocked) {
                // in what kind of scenario would this actually be null?
                if (spriteRenderer.sprite != null) 
                    spriteRenderer.sprite = lockedSprite;   // apply locked image
            }
            else {
                // use unlocked image:
                if (spriteRenderer.sprite != null) 
                    spriteRenderer.sprite = unlockedSprite;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {   // collision refers to other object. What else but player can collide with the levels?
        if (collision.tag == "Player") {
            if (isLocked) {
                if (levelPanel != null) 
                    levelPanel.SetActive(true); 

                if (levelText != null) 
                    levelText.text = "Level Locked";
            }
            // if level unlocked, display levelName in levelPanel:
            else {
                if (levelPanel != null) 
                    levelPanel.SetActive(true); 
                
                if (levelText != null)
                    levelText.text = levelName;
            }
        }
    }

    // shit to do when player exits collision with level:
    void OnTriggerExit2D(Collider2D collision) {   // collision refers to other object.
        if (collision.tag == "Player") {
            if (levelPanel != null) 
                levelPanel.SetActive(false);

            hasWarped = false;      // what's this?

            if (levelText != null)
                levelText.text = "";    // reset levelText
        }
    }
    #endregion
}
