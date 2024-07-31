using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// what object is this put on?
public class LSPlayerControls : MonoBehaviour
{
    #region Variables
    // starting position:
    [SerializedField] MapPoint startPoint = null;
    [SerializedField] float moveSpeed = 3f;
    [SerializedField] float teleportTime = 1f;
    [SerializedField] Transform playerSprite = null;

    MapPoint[] allPoints;               // array of MapPoint objs
    MapPoint prevPoint, currentPoint;   // 2 vars declared on same line
    // wtf is currentPoint? our actual currentPoint or the next one?
    // WHERE IS THIS ASSIGNED? we haven't assigned this script yet.

    Animator animator;                  // ANIMATING CRAP
    SpriteRenderer spriteRenderer;
    float x, y;
    bool canMove = true;
    int direction; 
    bool animationSet = false;
    Vector2 movement;                   // vector to store input in
    #endregion

    #region Unity built-in Methods
    void Awake() {
        allPoints = FindObjectsOfType<MapPoint>();   // notice that it's fucking PLURAL. Why done in awake()?
    }

    void Start() {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<spriteRenderer>();
        spriteRenderer.enabled = false;
        canMove = false;                            // initially true so why changed here?
        SetPlayerPos();
    }

    void Update() {
        if (canMove) {
            // will this automatically fucking move character from 1 point to another??? yes. currentPoint.transform.position is the point to move to
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime());

            // wtf is going on here? if the distance between these 2 points (curr pos and next pos) is less than 0.1 unity units...
            if (Vector3.Distance(transform.position, currentPoint.transform.position) < 0.1f) {
                
                CheckMapPoint();
            }
            else {
                // play correct animation:
                if (!animationSet) {
                    SetAnimation();
                }
            }
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// unlike Update() (runs every frame, this runs every X frames):
    void FixedUpdate()
    {
        GetMovement();    
    }
    #endregion

    #region Custom Methods (A Shit Ton)
    // what this do?
    void AutoMove() {
        if (currentPoint.up != null && currentPoint.up != prevPoint) {
            SetNextPoint(currentPoint.up);
            direction = 1;                      // what does this mean? "animation direction"
            animationSet = false;
        }
        else if (currentPoint.right != null && currentPoint.right != prevPoint) {
            SetNextPoint(currentPoint.right);
            direction = 2;
            animationSet = false;
        }
        else if (currentPoint.down != null && currentPoint.down != prevPoint) {
            SetNextPoint(currentPoint.down);
            direction = 3;
            animationSet = false;
        }
        else if (currentPoint.left != null && currentPoint.left != prevPoint) {
            SetNextPoint(currentPoint.left);
            direction = 4;
            animationSet = false;            
        }
    }

    // very similar to prev func. Why 2?
    void CheckInput() {
        if (y > 0.5f) {
            if (currentPoint.up != null) {
                SetNextPoint(currentPoint.up);
                direction = 1;
                animationSet = false;
            }
        }
        if (x > 0.5f) {
            if (currentPoint.right != null) {
                SetNextPoint(currentPoint.right);
                direction = 2;
                animationSet = false;
            }            
        }
        if (y < -0.5f) {
            if (currentPoint.down != null) {
                SetNextPoint(currentPoint.down);
                direction = 3;
                animationSet = false;
            }            
        }
        if (x < -0.5f) {
            if (currentPoint.left != null) {
                SetNextPoint(currentPoint.left);
                direction = 4;
                animationSet = false;
            }               
        }
    }

    void CheckMapPoint() {
        // MapPoint properties:
        if (currentPoint.isWarpPoint && !currentPoint.hasWarped) {
            if (direction != 0) {
                direction = 0;          // none of the 4, so no movement
                SetAnimation();
            }
            if (currentPoint.autoWarp) {
                StartCoroutine(TeleportPlayer(teleportTime));   
                // what's coroutine again? NOT asynchronous.
            }
        }    

        if (currentPoint.isCorner && currentPoint.isWarpPoint) {
            if (direction != 0) {
                direction = 0;
                SetAnimation();
            }

            CheckInput();
            SelectLevel();
        }

        if (currentPoint.isCorner) {
            AutoMove();
        }

        else {
            // MAKE THIS A FUCKING FUNCTION:
            if (direction != 0) {
                direction = 0;
                SetAnimation();
            }

            CheckInput();
            SelectLevel();
        }
    }

    // animation to play depending on direction player is moving in:
    void SetAnimation() {
        animationSet = true;

        switch(direction) {
            case 0:
                animator.Play("Idle");
                break;
            case 1:
                animator.Play("Up");
                break;
            case 2:
                animator.Play("Right");
                break;
            case 3:
                animator.Play("Down");
                break;    
            case 4:
                animator.Play("Left");
                break;                                                                
        }
    }

    void SetNextPoint(MapPoint nextPoint) {
        playerSprite.localPosition = Vector2.zero;      // why (0, 0)?
        prevPoint = currentPoint;
        currentPoint = nextPoint;                       // simply variables to change when player moves (smart)
    }

    // when do we use this?
    void SetPlayerPos() {
        // what's happening here? it's a STRING that we manually insert in the editor if the mapPoint is a level!
        // if we have a non-level point, set our point to the start point?
        if (DataManager.instance.gameData.currentLevelName == "") {
            transform.position = startPoint.transform.position;
            spriteRenderer.enabled = true;
            currentPoint = startPoint;
            prevPoint = currentPoint;
            canMove = true;
        }
        else {
            foreach(MapPoint point in allPoints) {
                if (point.isLevel) {
                    if (point.sceneToLoad == DataManager.instance.gameData.currentLevelName) {
                        transform.position = point.transform.position;
                        spriteRenderer.enabled = true;
                        currentPoint = point;
                        prevPoint = currentPoint;
                        canMove = true;
                    }
                }
            }
        }
    }

    public void GetMovement() {
        // input vector
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        x = movement.x;
        y = movement.y;
    }

    public void SelectLevel() {
        if (Input.GetButtonDown("Fire1")) {
            if (currentPoint != null) {

            }
        }
    }

    IEnumerator TeleportPlayer(float time) {
        yield return WaitForSeconds(time);
    }
    #endregion
}
