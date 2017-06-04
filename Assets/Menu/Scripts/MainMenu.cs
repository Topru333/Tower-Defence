using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace TD
{
    public class MainMenu : MonoBehaviour
    {

        LevelsMenu levelsMenu;

        public float showSpeed = 450f; // Скорость анимации движений
        public float mainMenuScale = 250f; // Относительный размер главного меню
        public float levelMenuScale = 150f; // Относительный размер меню уровней
        public float settingsMenuScale = 150f; // Относительный размер меню настроек
        public float backButtonScale = 150f; // Относительный размер кнопки возврата в гл. меню

        public GameObject canvas = null; // Канвась для хранения кнопок

        public GameObject mainMenuLevels = null; // Кнопка уровней на гл. меню
        public GameObject mainMenuOptions = null; // Кнопка настроек на гл. меню
        public GameObject qualityImg = null; // Картинка обозначающая качество графики
        public GameObject lenguageImg = null; // Картинка обозначающая язык
        public GameObject valumeImg = null; // Картинка обозначающая звук
        public GameObject valumeSlider = null; // Слайдер состояния громкости
        public GameObject qualityLow = null; // Кнопка низкого качества
        public GameObject qualityMid = null; // Кнопка среднего качества
        public GameObject qualityHig = null; // Кнопка высокого качества
        public GameObject rusLenguage = null; // Кнопка русского языка
        public GameObject engLenguage = null; // Кнопка английского языка
        public GameObject prefabLevel = null; // Префаб кнопки уровня
        public GameObject prefabDescriptionLevel = null; // Префаб панельки с описанием уровня
        public GameObject backButton = null; // Кнопка возврата в гл. меню



        // Use this for initialization
        void Start()
        {
            LocalizationData.Load();
            SettingsHolder.Load();
            levelsMenu = new LevelsMenu(new List<GameObject>(), GetComponent<LevelLoader>(), prefabLevel, canvas, showSpeed, prefabDescriptionLevel);
            levelsMenu.createButtons();

            showMainMenu = true;
            showSettingsMenu = false;
            showBackButton = false;

            move = new moveMenu(mainMenuMove);
            move += levelsMenu.move;
            move += settingsMenuMove;
            move += backMove;

            float xy = (Screen.width / Screen.height * mainMenuScale);
            SetRectSize(mainMenuLevels, xy, xy);
            SetRectSize(mainMenuOptions, xy, xy);

            levelsMenu.setRectSizeLevels(Screen.width / Screen.height * levelMenuScale);

            xy = (Screen.width / Screen.height * settingsMenuScale);
            SetRectSize(qualityImg, xy * 2f, xy);
            SetRectSize(lenguageImg, xy * 2f, xy);
            SetRectSize(valumeImg, xy * 2f, xy);


            SetRectSize(valumeSlider, xy * 4f, xy / 2);
            SetRectSize(qualityLow, xy, xy);
            SetRectSize(qualityMid, xy, xy);
            SetRectSize(qualityHig, xy, xy);
            SetRectSize(rusLenguage, xy, xy);
            SetRectSize(engLenguage, xy, xy);

            xy = (Screen.width / Screen.height * backButtonScale);
            SetRectSize(backButton, xy, xy);

            SetRectTransform(backButton, 0.05f, -0.5f);
            SetRectTransform(mainMenuLevels, -0.5f, 0.5f);
            SetRectTransform(mainMenuOptions, 1.5f, 0.5f);

            levelsMenu.setRectTransLevels(0.25f, -0.5f, 0.5f, 1.5f, 0.75f, -0.5f); // Место трех уровней с которых начнем крутить

            SetRectTransform(qualityImg, -0.5f, 0.75f);
            SetRectTransform(lenguageImg, -0.5f, 0.75f - 0.25f);
            SetRectTransform(valumeImg, -0.5f, 0.75f - 2 * 0.25f);
            SetRectTransform(valumeSlider, 1.5f, 0.75f);
            SetRectTransform(qualityLow, 1.5f, 0.5f);
            SetRectTransform(qualityMid, 1.5f, 0.5f);
            SetRectTransform(qualityHig, 1.5f, 0.5f);
            SetRectTransform(rusLenguage, 1.5f, 0.25f);
            SetRectTransform(engLenguage, 1.5f, 0.25f);
            ReloadLocale();
        }

        private void SetRectSize(GameObject go, float sizeX, float sizeY)
        {
            RectTransform rt = go.GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta = new Vector2(sizeX, sizeY);
        }
        private void SetRectTransform(GameObject go, float screenOffsetX, float screenOffsetY)
        {
            go.transform.position = new Vector2(Screen.width * screenOffsetX, Screen.height * screenOffsetY);
        }

        private void SetButtonName(GameObject btn,int nameID)
        {
            btn.GetComponentInChildren<Text>().text = LocalizationData.GetLocalizedString(nameID);
        }
        private void ReloadLocale() {
            SetButtonName(mainMenuLevels, 1);
            SetButtonName(mainMenuOptions, 2);
            SetButtonName(valumeImg, 5);
            SetButtonName(qualityImg, 3);
            SetButtonName(lenguageImg, 4);
            SetButtonName(qualityHig, 6);
            SetButtonName(qualityMid, 7);
            SetButtonName(qualityLow, 8);
            SetButtonName(backButton, 9);
            SetButtonName(rusLenguage, 15);
            SetButtonName(engLenguage, 16);
        }
        public void SetLocale(int l)
        {
            if (SettingsHolder.LaunguageID != l)
            {
                SettingsHolder.SetLocale(l);
                ReloadLocale();
            }
        }
        public void SetQuality(int q) {
            SettingsHolder.GraphicsQualityLevel = q;
            QualitySettings.SetQualityLevel(q);
        }
        public void SaveSettings()
        {
            SettingsHolder.Save();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !showMainMenu) mainMenuTrigger();
            move();
        }

        #region triggers
        private bool showMainMenu;
        private bool showSettingsMenu;
        private bool showBackButton;

        public void mainMenuTrigger()
        {
            showMainMenu = true;
            levelsMenu.turnOff();
            showSettingsMenu = false;
            showBackButton = false;
        }
        public void levelMenuTrigger()
        {
            levelsMenu.turnOn();
            showBackButton = true;
            showMainMenu = false;
            showSettingsMenu = false;
        }
        public void optionsMenuTrigger()
        {
            showSettingsMenu = true;
            showBackButton = true;
            showMainMenu = false;
            levelsMenu.turnOff();
        }
        public void backButtonTrigger()
        {
            mainMenuTrigger(); SaveSettings();
        }
        #endregion

        #region Move
        delegate void moveMenu();
        moveMenu move;

        private static void moveRectY(GameObject rect, float speed, bool when)
        {
            if (when)
            {
                rect.transform.Translate(0, speed * Time.deltaTime, 0);
            }
        }
        private static void moveRectX(GameObject rect, float speed, bool when)
        {
            if (when)
            {
                rect.transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
        private void backMove()
        {
            if (showBackButton) { moveRectY(backButton, showSpeed, backButton.transform.position.y < Screen.height / 10); }
            else { moveRectY(backButton, -showSpeed, backButton.transform.position.y > Screen.height * -0.5f); }
        }
        private void mainMenuMove()
        {
            if (showMainMenu)
            {
                moveRectX(mainMenuLevels, showSpeed, mainMenuLevels.transform.position.x < Screen.width * 0.25f);
                moveRectX(mainMenuOptions, -showSpeed, mainMenuOptions.transform.position.x > Screen.width * 0.75f);
            }
            else
            {
                moveRectX(mainMenuLevels, -showSpeed, mainMenuLevels.transform.position.x > Screen.width * -0.5f);
                moveRectX(mainMenuOptions, showSpeed, mainMenuOptions.transform.position.x < Screen.width * 1.5f);
            }
        }
        private void settingsMenuMove()
        {
            if (showSettingsMenu)
            {
                moveRectX(qualityImg, showSpeed, qualityImg.transform.position.x < Screen.width / 4);
                moveRectX(lenguageImg, showSpeed, lenguageImg.transform.position.x < Screen.width / 4);
                moveRectX(valumeImg, showSpeed, valumeImg.transform.position.x < Screen.width / 4);
                moveRectX(valumeSlider, -showSpeed, valumeSlider.transform.position.x > Screen.width * 0.70f);
                moveRectX(qualityLow, -showSpeed, qualityLow.transform.position.x > Screen.width * 0.9f);
                moveRectX(qualityMid, -showSpeed, qualityMid.transform.position.x > Screen.width * 0.70f);
                moveRectX(qualityHig, -showSpeed, qualityHig.transform.position.x > Screen.width * 0.5f);
                moveRectX(rusLenguage, -showSpeed, rusLenguage.transform.position.x > Screen.width * 0.6f);
                moveRectX(engLenguage, -showSpeed, engLenguage.transform.position.x > Screen.width * 0.8f);
            }
            else
            {
                moveRectX(qualityImg, -showSpeed, qualityImg.transform.position.x > Screen.width * -0.5f);
                moveRectX(lenguageImg, -showSpeed, lenguageImg.transform.position.x > Screen.width * -0.5f);
                moveRectX(valumeImg, -showSpeed, valumeImg.transform.position.x > Screen.width * -0.5f);
                moveRectX(valumeSlider, showSpeed, valumeSlider.transform.position.x < Screen.width * 1.5f);
                moveRectX(qualityLow, showSpeed, qualityLow.transform.position.x < Screen.width * 1.5f);
                moveRectX(qualityMid, showSpeed, qualityMid.transform.position.x < Screen.width * 1.5f);
                moveRectX(qualityHig, showSpeed, qualityHig.transform.position.x < Screen.width * 1.5f);
                moveRectX(rusLenguage, showSpeed, rusLenguage.transform.position.x < Screen.width * 1.5f);
                moveRectX(engLenguage, showSpeed, engLenguage.transform.position.x < Screen.width * 1.5f);
            }
        }
        #endregion



        // Минимум 3 уровня!!!!!! 
        // Minimum 3 levels!!!!!!
        private class LevelsMenu
        {

            private int centerIndex;

            private GameObject leftObject,
                               centerObject,
                               rightObject;

            private bool rightExist
            {
                get
                {
                    if (centerIndex + 1 < levelMenuButtons.Count - 1) return true;
                    else return false;
                }
            }

            public void moveRight()
            {
                if (rightExist)
                {
                    animateRight(++centerIndex);
                }
                else
                {
                    animateEndRight();
                    centerIndex = levelMenuButtons.Count - 1;
                }
            }

            private void animateRight(int newCenter)
            {
                // Здесь должно быть движение но пока что как то так :D
                rightObject = levelMenuButtons[++newCenter];
                centerObject = levelMenuButtons[newCenter];
                leftObject = levelMenuButtons[--newCenter];
            }
            private void animateEndRight()
            {
                rightObject = null;
                centerObject = levelMenuButtons[levelMenuButtons.Count - 1];
                leftObject = levelMenuButtons[levelMenuButtons.Count - 2];
            }

            private List<GameObject> levelMenuButtons;  // Список кнопок уровней
            private List<LevelData> levelsData;        // Список информации о уровнях
            private LevelLoader levelLoader;       // Загрузчик уровней
            private GameObject buttonPrefab;      // Шаблон кнопок
            private GameObject panel;       // Панель с описанием уровня
            private GameObject canvas;            // Канвас для кнопок
            private bool active;            // Флаг активно ли меню
            private float showspeed;         // Скорость анимации 
            public void turnOn()
            {
                active = true;
            }
            public void turnOff()
            {
                active = false;
            }

            public List<GameObject> getButtons
            {
                get
                {
                    return levelMenuButtons;
                }
            }

            // Движение из за границ при открытии меню уровней
            public void move()
            {
                //updateText();
                if (active)
                {
                    if (leftObject != null) moveRectY(leftObject, showspeed, leftObject.transform.position.y < Screen.height / 2);
                    if (rightObject != null) moveRectY(rightObject, showspeed, rightObject.transform.position.y < Screen.height / 2);
                    moveRectY(centerObject, -showspeed, centerObject.transform.position.y > Screen.height / 2);
                    //moveRectY(panel,         showspeed, panel.transform.position.y        < Screen.height / 7);
                }
                else
                {
                    if (leftObject != null) moveRectY(leftObject, -showspeed, levelMenuButtons[0].transform.position.y > Screen.height * -0.5f);
                    if (rightObject != null) moveRectY(rightObject, -showspeed, levelMenuButtons[2].transform.position.y > Screen.height * -0.5f);
                    moveRectY(centerObject, showspeed, levelMenuButtons[1].transform.position.y < Screen.height * 1.5f);
                    //moveRectY(panel,        -showspeed, panel.transform.position.y                > Screen.height * -0.5f);
                }
            }
            /*
                    private void updateText () {

                        panel.transform.GetChild(0).GetComponent<Text>().text = levelsData[centerIndex].name + '\n' + levelsData[centerIndex].description;
                    }*/

            // Размер кнопок
            public void setRectSizeLevels(float xy)
            {
                for (int i = 0; i < levelMenuButtons.Count; i++)
                    SetRectSize(levelMenuButtons[i], xy, xy);
            }

            // Место кнопок в которых они появляются
            public void setRectTransLevels(float x1, float y1, float x2, float y2, float x3, float y3)
            {
                SetRectTransform(levelMenuButtons[0], 0.25f, -0.5f);
                SetRectTransform(levelMenuButtons[1], 0.5f, 1.5f);
                SetRectTransform(levelMenuButtons[2], 0.75f, -0.5f);
                for (int i = 2; i < levelMenuButtons.Count; i++) SetRectTransform(levelMenuButtons[i], 0.75f, -0.5f);

            }

            private void SetRectSize(GameObject go, float sizeX, float sizeY)
            {
                RectTransform rt = go.GetComponent(typeof(RectTransform)) as RectTransform;
                rt.sizeDelta = new Vector2(sizeX, sizeY);
            }
            private void SetRectTransform(GameObject go, float screenOffsetX, float screenOffsetY)
            {
                go.transform.position = new Vector2(Screen.width * screenOffsetX, Screen.height * screenOffsetY);
            }

            public void createButtons()
            {
                levelsData = levelLoader.getLevels;
                for (int i = 0; i < levelsData.Count; i++)
                {
                    levelMenuButtons.Add(Instantiate(buttonPrefab));
                    string _name = levelsData[i].name;
                    levelMenuButtons[i].GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(_name); });
                    levelMenuButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(levelsData[i].icon, new Rect(0, 0, levelsData[i].icon.width, levelsData[i].icon.height), new Vector2(0.2f, 0.2f));
                    levelMenuButtons[i].transform.GetChild(1).GetComponent<Text>().text = levelsData[i].description;
                    levelMenuButtons[i].transform.SetParent(canvas.transform);
                }

                leftObject = levelMenuButtons[0];
                centerObject = levelMenuButtons[1];
                rightObject = levelMenuButtons[2];
            }

            public LevelsMenu(List<GameObject> levelMenuButtons, LevelLoader levelLoader, GameObject buttonPrefab, GameObject canvas, float showspeed, GameObject panelPrefab)
            {
                this.levelMenuButtons = levelMenuButtons;
                this.levelLoader = levelLoader;
                this.buttonPrefab = buttonPrefab;
                this.canvas = canvas;
                this.showspeed = showspeed;
                //this.panel            = Instantiate(panelPrefab);
                //panel.transform.SetParent(canvas.transform);
                /* SetRectSize(panel, Screen.width/2, Screen.width/8);
                 SetRectSize(panel.transform.GetChild(0).gameObject, Screen.width / 2, Screen.width / 8);
                 SetRectTransform(panel, 0.5f, -0.5f);
                 */
            }

        }
    }
}