using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public float showSpeed         = 450f;
    public float mainMenuScale     = 250f;
    public float levelMenuScale    = 150f;
    public float settingsMenuScale = 150f;
    public float backButtonScale   = 150f;

    public GameObject mainMenuLevels  = null;
    public GameObject mainMenuOptions = null;
    public GameObject qualityImg      = null;
    public GameObject lenguageImg     = null;
    public GameObject valumeImg       = null;
    public GameObject valumeSlider    = null;
    public GameObject qualityLow      = null;
    public GameObject qualityMid      = null;
    public GameObject qualityHig      = null;
    public GameObject rusLenguage     = null;
    public GameObject engLenguage     = null;
    public GameObject backButton      = null;
    public List<GameObject> LevelMenuButtons = new List<GameObject>();

    // Use this for initialization
    void Start () {
        showMainMenu     = true;
        showLevelMenu    = false;
        showSettingsMenu = false;
        showBackButton   = false;

        move = new moveMenu(mainMenuMove);
        move += levelMenuMove;
        move += settingsMenuMove;
        move += backMove;

        float xy = (Screen.width / Screen.height * mainMenuScale);
        SetRectSize(mainMenuLevels,  xy, xy);
        SetRectSize(mainMenuOptions, xy, xy);

        xy = (Screen.width / Screen.height * levelMenuScale);
        for (int i = 0; i < 3; i++)
            SetRectSize(LevelMenuButtons[i], xy, xy);

        xy = (Screen.width / Screen.height * settingsMenuScale);
        SetRectSize(qualityImg,  xy * 2f, xy);
        SetRectSize(lenguageImg, xy * 2f, xy);
        SetRectSize(valumeImg,   xy * 2f, xy);


        SetRectSize(valumeSlider, xy * 4f, xy / 2);
        SetRectSize(qualityLow,   xy, xy);
        SetRectSize(qualityMid,   xy, xy);
        SetRectSize(qualityHig,   xy, xy);
        SetRectSize(rusLenguage,  xy, xy);
        SetRectSize(engLenguage,  xy, xy);

        xy = (Screen.width / Screen.height * backButtonScale);
        SetRectSize(backButton, xy, xy);

        SetRectTransform(backButton,      0.05f, -0.5f);
        SetRectTransform(mainMenuLevels, -0.5f,   0.5f);
        SetRectTransform(mainMenuOptions, 1.5f,   0.5f);

        if (LevelMenuButtons.Count == 3) {
            SetRectTransform(LevelMenuButtons[0], 0.25f, -0.5f);
            SetRectTransform(LevelMenuButtons[1], 0.5f,   1.5f);
            SetRectTransform(LevelMenuButtons[2], 0.75f, -0.5f);
        }

        SetRectTransform(qualityImg,  -0.5f, 0.75f);
        SetRectTransform(lenguageImg, -0.5f, 0.75f -  0.25f);
        SetRectTransform(valumeImg,   -0.5f, 0.75f - 2 * 0.25f);
        SetRectTransform(valumeSlider, 1.5f, 0.75f);
        SetRectTransform(qualityLow,   1.5f, 0.5f);
        SetRectTransform(qualityMid,   1.5f, 0.5f);
        SetRectTransform(qualityHig,   1.5f, 0.5f);
        SetRectTransform(rusLenguage,  1.5f, 0.25f);
        SetRectTransform(engLenguage,  1.5f, 0.25f);

    }

    private void SetRectSize(GameObject go,float sizeX, float sizeY)
    {
        RectTransform rt = go.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(sizeX, sizeY);
    }
    private void SetRectTransform(GameObject go, float screenOffsetX, float screenOffsetY)
    {
        go.transform.position = new Vector2(Screen.width * screenOffsetX, Screen.height * screenOffsetY);
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !showMainMenu) mainMenuTrigger();
        move();
    }

    #region triggers
    private bool showMainMenu;
    private bool showLevelMenu;
    private bool showSettingsMenu;
    private bool showBackButton;

    public void mainMenuTrigger () {
        showMainMenu     = true;
        showLevelMenu    = false;
        showSettingsMenu = false;
        showBackButton   = false;
    }
    public void levelMenuTrigger () {
        showLevelMenu    = true;
        showBackButton   = true;
        showMainMenu     = false;
        showSettingsMenu = false;
    }
    public void optionsMenuTrigger () {
        showSettingsMenu = true;
        showBackButton   = true;
        showMainMenu     = false;
        showLevelMenu    = false;
    }
    public void backButtonTrigger () {
        mainMenuTrigger();
    }
    #endregion

    #region Move
    delegate void moveMenu();
    moveMenu move;

    private void moveRectY (GameObject rect, float speed, bool when) {
        if (when) {
            rect.transform.Translate(0, speed * Time.deltaTime, 0);
        }
    }
    private void moveRectX (GameObject rect, float speed, bool when) {
        if (when) {
            rect.transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }
    private void backMove () {
        if (showBackButton) { moveRectY(backButton,  showSpeed, backButton.transform.position.y < Screen.height /  10  ); }
        else                { moveRectY(backButton, -showSpeed, backButton.transform.position.y > Screen.height * -0.5f); }
    }
    private void mainMenuMove () {
        if (showMainMenu) {
            moveRectX(mainMenuLevels,   showSpeed, mainMenuLevels.transform.position.x  < Screen.width * 0.25f);
            moveRectX(mainMenuOptions, -showSpeed, mainMenuOptions.transform.position.x > Screen.width * 0.75f);
        }
        else {
            moveRectX(mainMenuLevels, -showSpeed, mainMenuLevels.transform.position.x  > Screen.width * -0.5f);
            moveRectX(mainMenuOptions, showSpeed, mainMenuOptions.transform.position.x < Screen.width *  1.5f);
        }
    }
    private void levelMenuMove () {
        if (showLevelMenu) {
            moveRectY(LevelMenuButtons[0],  showSpeed, LevelMenuButtons[0].transform.position.y < Screen.height / 2);
            moveRectY(LevelMenuButtons[1], -showSpeed, LevelMenuButtons[1].transform.position.y > Screen.height / 2);
            moveRectY(LevelMenuButtons[2],  showSpeed, LevelMenuButtons[2].transform.position.y < Screen.height / 2);
        }
        else {
            moveRectY(LevelMenuButtons[0], -showSpeed, LevelMenuButtons[0].transform.position.y > Screen.height * -0.5f);
            moveRectY(LevelMenuButtons[1],  showSpeed, LevelMenuButtons[1].transform.position.y < Screen.height *  1.5f);
            moveRectY(LevelMenuButtons[2], -showSpeed, LevelMenuButtons[2].transform.position.y > Screen.height * -0.5f);
        }
    }
    private void settingsMenuMove () {
        if (showSettingsMenu) {
            moveRectX(qualityImg,    showSpeed, qualityImg.transform.position.x   < Screen.width / 4);
            moveRectX(lenguageImg,   showSpeed, lenguageImg.transform.position.x  < Screen.width / 4);
            moveRectX(valumeImg,     showSpeed, valumeImg.transform.position.x    < Screen.width / 4);
            moveRectX(valumeSlider, -showSpeed, valumeSlider.transform.position.x > Screen.width * 0.70f);
            moveRectX(qualityLow,   -showSpeed, qualityLow.transform.position.x   > Screen.width * 0.9f);
            moveRectX(qualityMid,   -showSpeed, qualityMid.transform.position.x   > Screen.width * 0.70f);
            moveRectX(qualityHig,   -showSpeed, qualityHig.transform.position.x   > Screen.width * 0.5f);
            moveRectX(rusLenguage,  -showSpeed, rusLenguage.transform.position.x  > Screen.width * 0.6f);
            moveRectX(engLenguage,  -showSpeed, engLenguage.transform.position.x  > Screen.width * 0.8f);
        }
        else {
            moveRectX(qualityImg,  -showSpeed, qualityImg.transform.position.x   > Screen.width * -0.5f);
            moveRectX(lenguageImg, -showSpeed, lenguageImg.transform.position.x  > Screen.width * -0.5f);
            moveRectX(valumeImg,   -showSpeed, valumeImg.transform.position.x    > Screen.width * -0.5f);
            moveRectX(valumeSlider, showSpeed, valumeSlider.transform.position.x < Screen.width *  1.5f);
            moveRectX(qualityLow,   showSpeed, qualityLow.transform.position.x   < Screen.width *  1.5f);
            moveRectX(qualityMid,   showSpeed, qualityMid.transform.position.x   < Screen.width *  1.5f);
            moveRectX(qualityHig,   showSpeed, qualityHig.transform.position.x   < Screen.width *  1.5f);
            moveRectX(rusLenguage,  showSpeed, rusLenguage.transform.position.x  < Screen.width *  1.5f);
            moveRectX(engLenguage,  showSpeed, engLenguage.transform.position.x  < Screen.width *  1.5f);
        }
    }
    #endregion
}
