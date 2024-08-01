using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put on MapPointLocker
public class MapPointLocker : MonoBehaviour
{
    #region Variables
    // array of MapPoint objs:
    [SerializeField] MapPoint[] mapPoints;  
    // creates visible ARRAY property. Empty by default, and we have to manually give it a size (3 for island 1). They don't even have to be level MapPoints
    #endregion

    #region Unity Base Methods
    void Awake() {
        // locks the map points
        SetMapPoints();
    }
    #endregion

    #region User Methods
    void SetMapPoints() {
        // why are we hard-coding the indices?
        if (DataManager.instance.gameData.lockedLevels[1].isLocked) {
            mapPoints[0].isLocked = true;   // this will lock the fucking corner (yes, need to beat level 1 first)
        }
        if (DataManager.instance.gameData.lockedLevels[2].isLocked) {
            mapPoints[1].isLocked = true;
        }
        if (DataManager.instance.gameData.lockedLevels[3].isLocked) {
            mapPoints[2].isLocked = true;
        }
        if (DataManager.instance.gameData.lockedLevels[4].isLocked) {
            // mapPoints[3].isLocked = true; // <--- outside boundaries
            // do nothing. 

        }
    }
    #endregion
}
