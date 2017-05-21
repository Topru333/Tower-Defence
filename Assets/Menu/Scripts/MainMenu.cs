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
        RectTransform rt = mainMenuButtons[0].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy, xy);
        rt = mainMenuButtons[1].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy, xy);

        xy = (Screen.width / Screen.height * levelMenuScale);
        rt = LevelMenuButtons[0].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy, xy);
        rt = LevelMenuButtons[1].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy, xy);
        rt = LevelMenuButtons[2].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy, xy);

        xy = (Screen.width / Screen.height * settingsMenuScale);
        rt = SettingImg[0].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2( xy * 2f, xy );
        rt = SettingImg[1].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2( xy * 2f, xy );
        rt = SettingImg[2].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2( xy * 2f, xy );
        rt = SettingValues[0].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy * 4f, xy/2);
        rt = SettingValues[1].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy * 1f, xy * 1f);
        rt = SettingValues[2].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy * 1f, xy * 1f);
        rt = SettingValues[3].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy * 1f, xy * 1f);
        rt = SettingValues[4].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy * 1f, xy * 1f);
        rt = SettingValues[5].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(xy * 1f, xy * 1f);

        if (mainMenuButtons.Count == 2) {
            mainMenuButtons[0].transform.position = new Vector3(Screen.width * -0.5f, Screen.height / 2);
            mainMenuButtons[1].transform.position = new Vector3(Screen.width * 1.5f, Screen.height / 2);
        }
        if (LevelMenuButtons.Count == 3) {
            LevelMenuButtons[0].transform.position = new Vector3(Screen.width * 0.25f, Screen.height * -0.5f);
            LevelMenuButtons[1].transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 1.5f);
            LevelMenuButtons[2].transform.position = new Vector3(Screen.width * 0.75f, Screen.height * -0.5f);
        }
        if (SettingImg.Count == 3 && SettingValues.Count == 6) {
            SettingImg[0].transform.position = new Vector3(Screen.width * -0.5f, Screen.height * 0.75f);
            SettingImg[1].transform.position = new Vector3(Screen.width * -0.5f, Screen.height * 0.5f);
            SettingImg[2].transform.position = new Vector3(Screen.width * -0.5f, Screen.height * 0.25f);
            SettingValues[0].transform.position = new Vector3(Screen.width * 1.5f, Screen.height * 0.75f);
            SettingValues[1].transform.position = new Vector3(Screen.width * 1.5f, Screen.height * 0.5f);
            SettingValues[2].transform.position = new Vector3(Screen.width * 1.5f, Screen.height * 0.5f);
            SettingValues[3].transform.position = new Vector3(Screen.width * 1.5f, Screen.height * 0.5f);
            SettingValues[4].transform.position = new Vector3(Screen.width * 1.5f, Screen.height * 0.25f);
            SettingValues[5].transform.position = new Vector3(Screen.width * 1.5f, Screen.height * 0.25f);
        }

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
            if (mainMenuButtons[0].transform.position.x < Screen.width / 4) {
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
