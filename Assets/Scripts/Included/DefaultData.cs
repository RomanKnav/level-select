using System.Collections.Generic;
using UnityEngine;

// this is a new kind of class. We can instantiate it as an object as with...... public DefaultData gameData = new DefaultData();
// where's this put? in DataManager

[System.Serializable]
public class DefaultData
{
    // String to store the current level to load and set as spawn point in the level select menu
    public string currentLevelName;
    // List of the game levels to set the scene to load & locked status
    // how do the locked levels end up in here?
    public List<LockedLevels> lockedLevels = new List<LockedLevels>();  
    // where are the values here assigned? Not in editor. Script not used in Editor. Only in DataManager.cs
    // NOT in MapPointLocker.cs.
}

// how does this work? LockedLevels public class.
// shit for each individual obj in LockedLevelsF
[System.Serializable]
public class LockedLevels
{
    #region Variables
    // Scene Name
    public string sceneToLoad;      // where the fuck is this assigned?
    // Locked status
    public bool isLocked;
    public bool isBeaten;           // why do these vars have to be initiated twice?
    public bool beenPlayed;
    #endregion
}