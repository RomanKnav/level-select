using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// with this implemented, we make changes in 

// Put on MapPointLocker
public class MapPointLocker : MonoBehaviour
{
    #region Variables
    // array of MapPoint objs:
    [SerializeField] MapPoint[] mapPointsToLock;    // POINTS WE WANT LOCKED GO IN HERE (for island 1, we want 3 points locked)
    // locking itself done by LockMapPoints()
    // creates visible ARRAY property. Empty by default, and we have to manually give it a size.
    // 0: corner 1, 1: level2, 2: warp1
    #endregion

    #region Unity Base Methods
    void Awake() {
        // locks the map points
        LockMapPoints();
    }
    #endregion

    #region User Methods
    // IN TOTAL, There's 5 levels. 3 locked points on island 1.
    // I understand this now. each if statements will lock points if the one in question is locked too.
    void LockMapPoints() {
        // index 0 not included because that's level 1!

        if (DataManager.instance.gameData.lockedLevels[1].isLocked) {
            // if level 2 is locked, lock corner 1. We need to also lock warp1
            mapPointsToLock[0].isLocked = true;   // this will lock the fucking corner (yes, need to beat level 1 first)
            mapPointsToLock[1].isLocked = true;   // this will lock the fucking corner (yes, need to beat level 1 first)
            mapPointsToLock[2].isLocked = true;   // this will lock the fucking corner (yes, need to beat level 1 first)
            // how is this corner 1? isn't mapPointsToLock[0] level 1?
        }
        // if (DataManager.instance.gameData.lockedLevels[2].isLocked) {
        //     mapPointsToLock[1].isLocked = true;
        // }
    }
    #endregion
}
