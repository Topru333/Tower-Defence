using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public float showSpeed = 450f;
    public float mainMenuScale = 250f;
    public float levelMenuScale = 150f;
    public float settingsMenuScale = 150f;
    public List<GameObject> mainMenuButtons = new List<GameObject>();
    public List<GameObject> LevelMenuButtons = new List<GameObject>();
    public List<GameObject> SettingImg = new List<GameObject>();
    public List<GameObject> SettingValues = new List<GameObject>();

    // Use this for initialization
    void Start () {
        showMainMenu = true;
        showLevelMenu = false;
        showSettingsMenu = false;


        float xy = (Screen.width / Screen.height * mainMenuScale);
        for(int i = 0; i < 2; i++)
            SetRectSize(mainMenuButtons[i], xy, xy);

        xy = (Screen.width / Screen.height * levelMenuScale);
        for (int i = 0; i < 3; i++)
            SetRectSize(LevelMenuButtons[i], xy, xy);

        xy = (Screen.width / Screen.height * settingsMenuScale);
        for (int i = 0; i < 3; i++)
            SetRectSize(SettingImg[i], xy*2f, xy);

        SetRectSize(SettingValues[0], xy * 4f, xy / 2);
        for (int i = 1; i < 6; i++)
            SetRectSize(SettingValues[i], xy, xy);

        if (mainMenuButtons.Count == 2) {
            SetRectTransform(mainMenuButtons[0], -0.5f, 0.5f);
            SetRectTransform(mainMenuButtons[1], 1.5f, 0.5f);
        }
        if (LevelMenuButtons.Count == 3) {
            SetRectTransform(LevelMenuButtons[0], 0.25f, -0.5f);
            SetRectTransform(LevelMenuButtons[1], 0.5f, 1.5f);
            SetRectTransform(LevelMenuButtons[2], 0.75f, -0.5f);
        }
        if (SettingImg.Count == 3 && SettingValues.Count == 6) {
            int i;
            for (i = 0; i<3; i++)
                SetRectTransform(SettingImg[i], -0.5f, 0.75f - i * 0.25f);
            SetRectTransform(SettingValues[0], 1.5f, 0.75f);
            for (i = 1; i < 4; i++)
                SetRectTransform(SettingValues[i], 1.5f, 0.5f);
            for (i = 4; i < 6; i++)
                SetRectTransform(SettingValues[i], 1.5f, 0.25f);
        }

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
        if (Input.GetKeyDown(KeyCode.Escape) && !showMainMenu) MainMenuTrigger();
        MainMenuMove();
        LevelMenuMove();
        SettingsMenuMove();
    }

    #region triggers
    private bool showMainMenu;
    private bool showLevelMenu;
    private bool showSettingsMenu;

    public void MainMenuTrigger () {
        showMainMenu = true;
        showLevelMenu = false;
        showSettingsMenu = false;
    }
    public void LevelMenuTrigger () {
        showLevelMenu = true;
        showMainMenu = false;
        showSettingsMenu = false;
    }
    public void OptionsMenuTrigger () {
        showSettingsMenu = true;
        showMainMenu = false;
        showLevelMenu = false;
    }
    #endregion

    #region Move
    private void MainMenuMove () {
        if (showMainMenu) {
            if (mainMenuButtons[0].transform.position.x < Screen.width *0.25f) {
                mainMenuButtons[0].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (mainMenuButtons[1].transform.position.x > Screen.width * 0.75f) {
                mainMenuButtons[1].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
        }
        else {
            if (mainMenuButtons[0].transform.position.x > Screen.width * -0.5f) {
                mainMenuButtons[0].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (mainMenuButtons[1].transform.position.x < Screen.width * 1.5f) {
                mainMenuButtons[1].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
    private void LevelMenuMove () {
        if (showLevelMenu) {
            if (LevelMenuButtons[0].transform.position.y < Screen.height / 2) {
                LevelMenuButtons[0].transform.Translate(0, showSpeed * Time.deltaTime, 0);
            }
            if (LevelMenuButtons[1].transform.position.y > Screen.height / 2) {
                LevelMenuButtons[1].transform.Translate(0, -showSpeed * Time.deltaTime, 0);
            }
            if (LevelMenuButtons[2].transform.position.y < Screen.height / 2) {
                LevelMenuButtons[2].transform.Translate(0, showSpeed * Time.deltaTime, 0);
            }
        }
        else {
            if (LevelMenuButtons[0].transform.position.y > Screen.height * -0.5f) {
                LevelMenuButtons[0].transform.Translate(0, -showSpeed * Time.deltaTime, 0);
            }
            if (LevelMenuButtons[1].transform.position.y < Screen.height * 1.5f) {
                LevelMenuButtons[1].transform.Translate(0, showSpeed * Time.deltaTime, 0);
            }
            if (LevelMenuButtons[2].transform.position.y > Screen.height * -0.5f) {
                LevelMenuButtons[2].transform.Translate(0, -showSpeed * Time.deltaTime, 0);
            }
        }
    }
    private void SettingsMenuMove () {
        if (showSettingsMenu) {
            if (SettingImg[0].transform.position.x < Screen.width / 4) {
                SettingImg[0].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingImg[1].transform.position.x < Screen.width / 4) {
                SettingImg[1].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingImg[2].transform.position.x < Screen.width / 4) {
                SettingImg[2].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[0].transform.position.x > Screen.width * 0.70f) {
                SettingValues[0].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[1].transform.position.x > Screen.width * 0.9f) {
                SettingValues[1].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[2].transform.position.x > Screen.width * 0.70f) {
                SettingValues[2].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[3].transform.position.x > Screen.width * 0.5f) {
                SettingValues[3].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[4].transform.position.x > Screen.width * 0.6f) {
                SettingValues[4].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[5].transform.position.x > Screen.width * 0.8f) {
                SettingValues[5].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
        }
        else {
            if (SettingImg[0].transform.position.x > Screen.width * -0.5f) {
                SettingImg[0].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingImg[1].transform.position.x > Screen.width * -0.5f) {
                SettingImg[1].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingImg[2].transform.position.x > Screen.width * -0.5f) {
                SettingImg[2].transform.Translate(-showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[0].transform.position.x < Screen.width * 1.5f) {
                SettingValues[0].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[1].transform.position.x < Screen.width * 1.5f) {
                SettingValues[1].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[2].transform.position.x < Screen.width * 1.5f) {
                SettingValues[2].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[3].transform.position.x < Screen.width * 1.5f) {
                SettingValues[3].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[4].transform.position.x < Screen.width * 1.5f) {
                SettingValues[4].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
            if (SettingValues[5].transform.position.x < Screen.width * 1.5f) {
                SettingValues[5].transform.Translate(showSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
    #endregion
}
