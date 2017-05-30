using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TD
{
#region DragableItem
    public class DragNDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject movingObject = null;
        public static bool AllowDragNDrop;
        public UnityEngine.Events.UnityAction onBeginDragAction;
        public UnityEngine.Events.UnityAction<PointerEventData> onDragEndAction;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!movingObject || !AllowDragNDrop)
                return;
            onBeginDragAction();
            movingObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!movingObject || !AllowDragNDrop)
                return;
            movingObject.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!movingObject||!AllowDragNDrop)
                return;
            onDragEndAction(eventData);
            movingObject.SetActive(false);
        }
    }
#endregion

    public class IngameUI : MonoBehaviour
    {
        private static IngameUI _instance;
        public static IngameUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<IngameUI>();
                    singleton.name = typeof(IngameUI).ToString();
                }
                return _instance;
            }
        }
        private int selectedTower = 0;
        private bool inTowerBuildMenu=true;
        private bool gameIsSpeedUp = false;
        private bool canShowBuidMenu = false;
        private bool inPauseMenu = false;
        private bool levelEnded = false;
        private bool towerUpgradeSellDialogActive = false;

        public RectTransform towerPanel;
        public RectTransform towerBuildPanel;
        public RectTransform pauseMenuPanel;
        public RectTransform EndLevelDialog;
        public RectTransform ShowHideTowerBuildMenuImg;
        public GameObject towerSellUpgradeDialog;

        public Button startWaveSpeedUpButton;
        public Text ExperienceLabel;
        public Text WaveNumberLabel;
        public Text GoldLabel;
        public Text MainTowerStateLabel;
        public Text TowerBuildLabel;
        public Text PauseResumeLabel;
        public Text StartWaveSpeedUpLabel;
        public Text EndLevelDialogHeaderLabel;
        public Image StartWaveSpeedUpImage;
        public Image PauseResumeImage;

        public Sprite ResumeSprite;
        public Sprite PauseSprite;
        public Sprite SpeedUpSprite;
        public Sprite SlowDownSprite;

        private GameObject towerIcon;
        private List<GameObject> towerButtons=new List<GameObject>();

        private float towerPanelUISize;
        private int waveCount,
                    mainTowerLifeCount;
        [SerializeField]
        private int sellImageSize=50;
        private int wavePast=0;
        private Vector2 lastMousePos;
        private int currentTowerCellX, currentTowerCellY;

        private void Awake()
        {
            _instance = this;
            var currentLvl = LevelManager.Instance.CurrentLevel;
            
            waveCount               = currentLvl.GetWaveCount();
            mainTowerLifeCount      = currentLvl.GetMainTowerLifeCount();
            TowerGridSystem.Instance.ToggleGridVizualization();
            towerPanelUISize = towerPanel.rect.height;
            GameObject towerButton  = new GameObject("towerbtn");
            towerButton.AddComponent<RectTransform>().sizeDelta = new Vector2( towerPanelUISize*0.95f, towerPanelUISize * 0.95f);
            //towerButton.AddComponent<Button>();
            towerButton.AddComponent<DragNDropItem>();
            towerButton.AddComponent<Image>();
            towerButton.SetActive(false);
            towerIcon = new GameObject("towerIcon");
            towerIcon.transform.SetParent(transform);
            towerIcon.AddComponent<RectTransform>().sizeDelta = new Vector2(towerPanelUISize * 0.95f, towerPanelUISize * 0.95f);
            towerIcon.AddComponent<Image>().raycastTarget=false;
            towerIcon.SetActive(false);
            int i = 0;
            var towers = currentLvl.GetAvaliableToBuildTowers();
            foreach (GameObject tower in towers)
            {
                GameObject go = Instantiate(towerButton);
                //go.GetComponent<Button>().onClick.AddListener(() => SwitchSelectedTower(go,i-1));
                go.GetComponent<DragNDropItem>().onBeginDragAction = () => { SwitchSelectedTower(go, i - 1); };
                go.GetComponent<DragNDropItem>().onDragEndAction = OnTowerDragEnd;
                go.transform.SetParent(towerPanel.transform);
                go.GetComponent<Image>().sprite = tower.GetComponent<Tower>().icon;
                go.SetActive(true);
                towerButtons.Add(go);
                i++;
            }
            DragNDropItem.movingObject = towerIcon;
            DragNDropItem.AllowDragNDrop = true;
            WaveNumberLabel.text = string.Format("Wave:{0}/{1}", wavePast, waveCount);
        }

        private void Update()
        {
            if (inTowerBuildMenu && !inPauseMenu)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    towerSellUpgradeDialog.transform.position = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rh;
                    if (Physics.Raycast(ray, out rh))
                    {
                        if (TowerGridSystem.Instance.GetTowerCellAt(rh.point,out currentTowerCellX, out currentTowerCellY) && TowerGridSystem.Instance.GetCellInfo(rh.point)== TowerGridSystem.CellState.Builded)
                        {
                            towerSellUpgradeDialog.SetActive(true);
                            lastMousePos = Input.mousePosition;
                            towerUpgradeSellDialogActive = true;
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0)&& towerUpgradeSellDialogActive)
                {
                    towerSellUpgradeDialog.SetActive(false);
                    if (Vector2.Distance(Input.mousePosition, lastMousePos) < sellImageSize)
                    {
                        if (Input.mousePosition.y - lastMousePos.y > 0)
                            TowerGridSystem.Instance.SellTowerAt(currentTowerCellX, currentTowerCellY);
                        else
                            TowerGridSystem.Instance.UpgradeTowerAt(currentTowerCellX, currentTowerCellY);
                    }
                    
                    towerUpgradeSellDialogActive = false;
                }


#else
                if (Input.touchCount>0&& Input.GetTouch(0).phase== TouchPhase.Began)
                {
                    towerSellUpgradeDialog.transform.position = Input.GetTouch(0).position;
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit rh;
                    if (Physics.Raycast(ray, out rh))
                    {
                        if (TowerGridSystem.Instance.GetTowerCellAt(rh.point, out currentTowerCellX, out currentTowerCellY) && TowerGridSystem.Instance.GetCellInfo(rh.point) == TowerGridSystem.CellState.Builded)
                        {
                            towerSellUpgradeDialog.SetActive(true);
                            lastMousePos = Input.GetTouch(0).position;
                            towerUpgradeSellDialogActive = true;
                        }
                    }
                }
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && towerUpgradeSellDialogActive)
                {
                    towerSellUpgradeDialog.SetActive(false);
                if (Vector2.Distance(Input.mousePosition, lastMousePos) < sellImageSize)
                    {
                    if (Input.GetTouch(0).position.y - lastMousePos.y > 0)
                        TowerGridSystem.Instance.SellTowerAt(currentTowerCellX, currentTowerCellY);
                    else
                        TowerGridSystem.Instance.UpgradeTowerAt(currentTowerCellX, currentTowerCellY);
                }

                    towerUpgradeSellDialogActive = false;
                }
#endif
            }
        }

        private void OnTowerDragEnd(PointerEventData eventData) {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit rh;
            var tower = LevelManager.Instance.CurrentLevel.GetAvaliableToBuildTowers()[selectedTower];
            if (Physics.Raycast(ray, out rh))
            {
                if (TowerGridSystem.Instance.GetCellInfo(rh.point) == TowerGridSystem.CellState.CanBuild)
                {
                    if (LevelManager.Instance.CurrentLevel.SpendMoney(tower.GetComponent<Tower>().GetPrice()))
                        TowerGridSystem.Instance.BuildTowerAt(rh.point, tower);
                }
            }
        }
        
        // Обновляет строку с опытом
        public void UpdateExperince(int Exp)
        {
            ExperienceLabel.text = string.Format("Exp:{0}", Exp);
        }

        // Обновляет строку с номером волны
        public void IncreaseWaveNumber()
        {
            WaveNumberLabel.text = string.Format("Wave:{0}/{1}", ++wavePast,waveCount);
        }

        // Обновляет строку с золотом
        public void UpdateGold(int gold)
        {
            GoldLabel.text = string.Format("Gold:{0}", gold);
        }

        public void UpdateMainTowerState(int livesLeft)
        {
            MainTowerStateLabel.text= string.Format("Main Tower State:{0}/{1}", livesLeft, mainTowerLifeCount);
        }

        // Открывает меню паузы
        public void TogglePauseMenu()
        {
            if (levelEnded)
                return;
#if DEBUG
            Debug.Log("Открыто меню паузы.");
#endif
            inPauseMenu = !inPauseMenu;
            PauseResumeImage.sprite = inPauseMenu ? ResumeSprite : PauseSprite;
            DragNDropItem.AllowDragNDrop = !inPauseMenu;
            if (!inTowerBuildMenu)
                LevelManager.Instance.Pause();
            pauseMenuPanel.gameObject.SetActive(inPauseMenu);
            if(!inPauseMenu)
                pauseMenuPanel.sizeDelta = new Vector2(pauseMenuPanel.sizeDelta.x, pauseMenuPanel.sizeDelta.y);
            else
                pauseMenuPanel.sizeDelta = new Vector2(pauseMenuPanel.sizeDelta.x, pauseMenuPanel.sizeDelta.x * 4);
        }

        // Перезапускает уровень, данное действие запускается из меню паузы
        public void RestartGame() {
#if DEBUG
            Debug.Log("Рестарт игры.");
#endif
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        // Показывает меню настроек, данная кнопка запускается из меню паузы
        // TODO: Сделать меню настроек
        public void ShowSettingsMenu() {
#if DEBUG
            Debug.Log("Открыто меню настроек.");
#endif
        }

        // Возвращает в главное меню, данное действие запускается из меню паузы
        public void ToMainMenu() {
#if DEBUG
            Debug.Log("Возврат в главное меню.");
#endif
            LevelManager.Instance.Pause();
            if (gameIsSpeedUp)
                LevelManager.Instance.GameSpeed(1);
            gameIsSpeedUp = !gameIsSpeedUp;
            
            SceneManager.LoadScene("MainMenu");
        }

        // Запускает волну NPC и скрывает меню постройки башен
        public void StartWave() {
#if DEBUG
            Debug.Log("Вызвана волна NPC.");
#endif
            if (inPauseMenu)
                return;
            LevelManager.Instance.CurrentLevel.WaveStart();
            canShowBuidMenu = true;
            ToggleTowerBuildMenu();

            StartWaveSpeedUpImage.sprite = SpeedUpSprite;
            startWaveSpeedUpButton.onClick.SetPersistentListenerState(0,UnityEngine.Events.UnityEventCallState.Off);
            startWaveSpeedUpButton.onClick.AddListener(ToggleSpeedUpMode);
            //StartWaveSpeedUpLabel.text = "Speed Up";
        }

        // Ускоряет/Замедляет игру
        public void ToggleSpeedUpMode() {
#if DEBUG
            Debug.Log("Скорость игры изменена.");
#endif
            if (inTowerBuildMenu||inPauseMenu|| levelEnded)
                return;
            gameIsSpeedUp = !gameIsSpeedUp;
            StartWaveSpeedUpImage.sprite = gameIsSpeedUp ? SlowDownSprite : SpeedUpSprite;
            /*if (gameIsSpeedUp)
                StartWaveSpeedUpLabel.text = "Slow Down";
            else
                StartWaveSpeedUpLabel.text = "Speed Up";*/
            LevelManager.Instance.GameSpeed(gameIsSpeedUp?5:1);
        }

        // Скрывает/Показывает меню постройки башен
        public void ToggleTowerBuildMenu() {
            if (!canShowBuidMenu || inPauseMenu|| levelEnded)
                return;
            if(!inPauseMenu)
                LevelManager.Instance.Pause();
            TowerGridSystem.Instance.ToggleGridVizualization();
            inTowerBuildMenu = !inTowerBuildMenu;
            // TODO: Сделать плавный переход
            if (inTowerBuildMenu)
            {
                //TowerBuildLabel.text = "Close";
                ShowHideTowerBuildMenuImg.rotation = Quaternion.Euler(0, 0, -90);
                towerBuildPanel.localPosition = new Vector3(towerBuildPanel.localPosition.x, towerBuildPanel.localPosition.y + towerPanelUISize, towerBuildPanel.localPosition.z);
            }
            else
            {
                ShowHideTowerBuildMenuImg.rotation = Quaternion.Euler(0, 0, 90);
                TowerBuildLabel.text = "Open";
                towerBuildPanel.localPosition = new Vector3(towerBuildPanel.localPosition.x, towerBuildPanel.localPosition.y - towerPanelUISize, towerBuildPanel.localPosition.z);
            }
        }

        // Меняет номер выбранной башни
        public void SwitchSelectedTower(GameObject go,int newID) {
            selectedTower = newID;
            towerIcon.GetComponent<Image>().sprite = towerButtons[newID].GetComponent<Image>().sprite;
        }

        // Показывает диалог прокачки/продажи башни
        public void ShowTowerUpgradeDialog(Vector2 posOnScreen) {
            // TODO: Реализация
        }

        // Показывает диалог окончания уровня
        public void ShowEndLevelDialog(bool won) {
#if DEBUG
            Debug.Log("Вызван диалог окончания уровня.");
#endif
            if (inTowerBuildMenu)
                ToggleTowerBuildMenu();
            if (inPauseMenu)
                TogglePauseMenu();
            EndLevelDialog.gameObject.SetActive(true);
            EndLevelDialogHeaderLabel.text = won ? "Congratulations you won!" : "You ara a failure, we lost a war to these ugly spheres!";
            levelEnded = true;
            LevelManager.Instance.Pause();
        }
    }
}