using UnityEngine;
using UnityEngine.SceneManagement;

// where's this used? FOUND IT: put in the Level SCENES, on the GameObject
// only runs on the levels (changes to the level scene, which is just empty with some text)
public class SceneLoader : MonoBehaviour
{
    #region Variables
    [SerializeField] int currentLevelIndex = 0; 
    #endregion

    #region Unity Base Methods
    void Start()
    {
        // Start the load level method after 3 seconds
        Invoke("LoadLevel", 3f);
    }
    #endregion 

    #region User Methods
    // loads up the scene for the respective level
    void LoadLevel()
    {
        // Check if the level we want to unlock is NOT out of bounds of our list count.
        if (currentLevelIndex + 1 < DataManager.instance.gameData.lockedLevels.Count)
        {
            // Unlock the level
            DataManager.instance.gameData.lockedLevels[currentLevelIndex + 1].isLocked = false;
            // Save the data

            // DataManager.instance.SaveGameData(); 
            DataManager.instance.TriggerSaveData();  

            // what exactly does this do?   
            /* UNDERSTOOD: since we switch back to the levelSelect scene after every level, we need to ensure it was found in it's previous state 
            (player position, locked/unlocked levels) */
        }
        // Load back to the level select scene
        SceneManager.LoadScene("LevelSelect");
    }
    #endregion
}