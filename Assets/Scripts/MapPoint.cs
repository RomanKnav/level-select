using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// note: a mapPoint is simply a gameObject with a square sprite, circle collider, and this script on it.

// SCRIPT ADDED POST-DOWNLOAD:
public class MapPoint : MonoBehaviour
{
    #region Variables           
    [Header("Waypoints")]
    public MapPoint up;
    public MapPoint right, down, left;      // different directions player can go

    [Header("Scene options")]
    [SerializeField] int levelIndex = 0;            // difference between currentLevelIndex and this?
    [HideInInspector] public string sceneToLoad;    
    // what is this? does not show this public var in Inspector window. Where's it assigned? line 74

    [TextArea(1, 2)]
    public string levelName;

    // for the different types of points on the map
    [Header("MapPoint Options")]
    [HideInInspector] public bool isLocked;         // also initiated in DefaultData
    public bool isLevel;
    public bool isCorner;
    public bool isWarpPoint;

    [Header("Level Crap")]
    [HideInInspector] public bool isBeaten;
    [HideInInspector] public bool beenPlayed;


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

    // RUNS WHEN AT A MAPPOINT
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

        // if we have selected a level...
        else {
            if (isLevel) {
                // Debug.Log(beenPlayed);
                sceneToLoad = DataManager.instance.gameData.lockedLevels[levelIndex].sceneToLoad;
                isLocked = DataManager.instance.gameData.lockedLevels[levelIndex].isLocked;             // this is a bool
                isBeaten = DataManager.instance.gameData.lockedLevels[levelIndex].isBeaten;
                // beenPlayed = DataManager.instance.gameData.lockedLevels[levelIndex].beenPlayed;
            }

            if (isLocked) {
                // in what kind of scenario would this actually be null?
                if (spriteRenderer.sprite != null) 
                    spriteRenderer.sprite = lockedSprite;   // apply locked image
            }
            // if level is unlocked...
            else {
                // use unlocked image:
                if (spriteRenderer.sprite != null) 
                    spriteRenderer.sprite = unlockedSprite;
            }
        }
        Debug.Log("Number of locked levels: " + DataManager.instance.gameData.lockedLevels.Count);      // THIS HAS 5
    }

    void OnTriggerEnter2D(Collider2D collision) {   // collision refers to other object. What else but player can collide with the levels?
        // Debug.Log(DataManager.instance.gameData.currentLevelName);  // why is this initially level2?

        if (collision.tag == "Player") {
            if (isLocked) {
                if (levelPanel != null) 
                    levelPanel.SetActive(true); 

                if (levelText != null) 
                    levelText.text = "Level Locked";
            }
            // if level unlocked, display levelName in levelPanel:
            else {
                // Debug.Log("Scene loaded at this level: " + DataManager.instance.gameData.lockedLevels[levelIndex].sceneToLoad);
                if (levelPanel != null) 
                    levelPanel.SetActive(true); 
                
                if (levelText != null)
                    levelText.text = levelName;
            }
        }
    }

    // shit to do when player exits collision with level. Only possible if func above ^^^ executes
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
