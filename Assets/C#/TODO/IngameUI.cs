using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TD
{
#region DragableItem
    public class DragNDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject MovingObject = null;   // Объект в котором будет находиться копия элемента интерфейса во время перетаскивания
        public static bool AllowDragNDrop;              // Разрешать перетаскивать элементы интерфейса
        public UnityAction OnBeginDragAction;                   // Действие вызываемое во время начала перетаскивания
        public UnityAction<PointerEventData> OnDragProceedAction;   // Действие вызываемое во время перетаскивания
        public UnityAction<PointerEventData> OnDragEndAction;   // Действие вызываемое после оканчания перетаскивания

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!MovingObject || !AllowDragNDrop)
                return;
            OnBeginDragAction();
            MovingObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!MovingObject || !AllowDragNDrop)
                return;
            OnDragProceedAction(eventData);
            MovingObject.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!MovingObject || !AllowDragNDrop)
                return;
            OnDragEndAction(eventData);
            MovingObject.SetActive(false);
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

        private bool    inTowerBuildMenu = true,  // Находится ли пользователь в меню постройки
                        gameIsSpeedUp   = false,  // Включено ли ускорение игры
                        canShowBuidMenu = false,  // Можно ли скрыть меню постройки башен
                        inPauseMenu = false,      // Внутри меню паузы
                        levelEnded = false,       // Внутри диалога окончания уровня
                        towerUpgradeSellDialogActive = false;   // Активен ли диалог продажи/прокачки башни

        public RectTransform TowerPanel;        // Панель постойки башен
        public RectTransform TowerBuildPanel;   // Панель постойки башен вместе с кнопкой скрытия/показа этой панели
        public RectTransform PauseMenuPanel;    // Панель меню паузы
        public RectTransform EndLevelDialog;    // Диалог окончания уровня
        public RectTransform ShowHideTowerBuildMenuImg; // Изображение скрытия/показа панели постойки башен
        public GameObject TowerSellUpgradeDialog;   // Кнопка продажи/прокачки башни

        public Button StartWaveSpeedUpButton; // Кнопка начала волны/ускорения игры
        public Text ExperienceLabel;          // Строка с опытом
        public Text WaveNumberLabel;          // Строка с номером волны
        public Text GoldLabel;                // Строка с деньгами
        public Text MainTowerStateLabel;      // Строка с состоянием уровня
        //public Text TowerBuildLabel;          
        //public Text PauseResumeLabel;
        //public Text StartWaveSpeedUpLabel;
        public Text EndLevelDialogHeaderLabel;  // Заголовок диалога окончания уровня
        public Image StartWaveSpeedUpImage;     // Изображение кнопки начала волны/ускорения
        public Image PauseResumeImage;          // Изображение кнопки паузы
        public Image SellUpgradeDialogImage;

        public Sprite ResumeSprite;     // Спрайт возобновления из паузы
        public Sprite PauseSprite;      // Спрайт паузы
        public Sprite SpeedUpSprite;    // Спрайт ускорения игры
        public Sprite SlowDownSprite;   // Спрайт замедления игры

        public Sprite UpgradeSellMenu_NoneSelected;
        public Sprite UpgradeSellMenu_SellSelected;
        public Sprite UpgradeSellMenu_UpgradeSelected;

        private GameObject towerIcon;    
        private List<GameObject> towerButtons=new List<GameObject>();

        private float towerPanelUISize;
        private int waveCount,          // Общее количество волн на уровне
                    mainTowerLifeCount, // Общее количество жизней главной башни
                    selectedTower = 0,  
                    wavePast = 0;
        [SerializeField]
        private int sellImageSize=50;
        private Vector2 lastMousePos;
        private int currentTowerCellX, currentTowerCellY;
        private int lastHighlightedCellX=0, lastHighlightedCellY=0;

        private void Awake()
        {
            _instance = this;
            var currentLvl = LevelManager.Instance.CurrentLevel;
            // Заполняем начальные значения
            waveCount               = currentLvl.GetWaveCount();
            mainTowerLifeCount      = currentLvl.GetMainTowerLifeCount();
            towerPanelUISize        = TowerPanel.rect.height;

            // Создаем пустой перетаскиваемый объект для панели постройки башен
            GameObject towerButton  = new GameObject("tower_button");
            towerButton.AddComponent<RectTransform>().sizeDelta = new Vector2( towerPanelUISize*0.95f, towerPanelUISize * 0.95f);
            towerButton.AddComponent<DragNDropItem>();
            towerButton.AddComponent<Image>();
            towerButton.SetActive(false);
            
            // Создаем объект-изображение, используемый для перетаскивания башни на уровень
            towerIcon = new GameObject("tower_icon");
            towerIcon.transform.SetParent(transform);
            towerIcon.AddComponent<RectTransform>().sizeDelta = new Vector2(towerPanelUISize * 0.95f, towerPanelUISize * 0.95f);
            towerIcon.AddComponent<Image>().raycastTarget=false;
            towerIcon.SetActive(false);

            int i = 0;
            var towers = currentLvl.GetAvaliableToBuildTowers();

            // Заполняем список доступных для постройки на уровне башен
            foreach (GameObject tower in towers)
            {
                GameObject go = Instantiate(towerButton, TowerPanel.transform);

                var dndItem = go.GetComponent<DragNDropItem>();
                dndItem.OnBeginDragAction = () => { SwitchSelectedTower(go, i - 1); };
                dndItem.OnDragEndAction = OnTowerDragEnd;
                dndItem.OnDragProceedAction = OnTowerDragProceed;

                go.GetComponent<Image>().sprite = tower.GetComponent<Tower>().icon;
                go.SetActive(true);

                towerButtons.Add(go);
                i++;
            }

            DragNDropItem.MovingObject   = towerIcon;
            DragNDropItem.AllowDragNDrop = true;
            
            TowerGridSystem.Instance.ToggleGridVizualization();
            sellImageSize = (int)(Mathf.Min(Screen.width, Screen.height) * 0.15f);
            TowerSellUpgradeDialog.GetComponent<RectTransform>().sizeDelta = new Vector2(sellImageSize, sellImageSize);
            WaveNumberLabel.text = string.Format("Wave:{0}/{1}", wavePast, waveCount);
        }

        private void Update()
        {
            if (inTowerBuildMenu && !inPauseMenu)
            {
                var pos = GetTouchPos();
                if (HasTouchBegan())
                    UpdateSellUpgradeDialogOnBeginTouch(pos);
                if (HasTouchEnded() && towerUpgradeSellDialogActive)
                    UpdateSellUpgradeDialogOnEndTouch(pos);
                if (HasTouchProceed())
                    UpdateSellUpgradeDialogOnTouchProceed(pos);
            }
        }

        // Проверка начато ли нажатие на кнопку мыши/экран
        private bool HasTouchBegan()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            return Input.touchCount>0 && Input.GetTouch(0).phase== TouchPhase.Began;
#endif
        }

        private bool HasTouchProceed()
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
            return Input.touchCount>0 && Input.GetTouch(0).phase== TouchPhase.Moved;
#endif
        }
        // Проверка закончено ли нажатие на кнопку мыши/экран
        private bool HasTouchEnded()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#else
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#endif
        }
        // Возвращает позицию мыши/нажатия
        private Vector2 GetTouchPos()
        {
#if UNITY_EDITOR
            return Input.mousePosition;
#else
            return Input.GetTouch(0).position;
#endif
        }

        private void UpdateSellUpgradeDialogOnBeginTouch(Vector2 pos)
        {
            TowerSellUpgradeDialog.transform.position = pos;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit rh;
            if (Physics.Raycast(ray, out rh))
            {
                if (TowerGridSystem.Instance.GetTowerCellAt(rh.point, out currentTowerCellX, out currentTowerCellY) && TowerGridSystem.Instance.GetCellInfo(rh.point) == TowerGridSystem.CellState.Builded)
                {
                    TowerSellUpgradeDialog.SetActive(true);
                    lastMousePos = pos;
                    towerUpgradeSellDialogActive = true;
                }
            }
        }
        
        private void UpdateSellUpgradeDialogOnEndTouch(Vector2 pos)
        {
            TowerSellUpgradeDialog.SetActive(false);
            float dist = Vector2.Distance(pos, lastMousePos);
            if (dist < sellImageSize&& dist> sellImageSize * 0.1f)
            {
                if (pos.y - lastMousePos.y < 0)
                    TowerGridSystem.Instance.SellTowerAt(currentTowerCellX, currentTowerCellY);
                else
                    TowerGridSystem.Instance.UpgradeTowerAt(currentTowerCellX, currentTowerCellY);
            }

            towerUpgradeSellDialogActive = false;
        }

        private void UpdateSellUpgradeDialogOnTouchProceed(Vector2 pos)
        {
            float dist = Vector2.Distance(pos, lastMousePos);
            if (dist < sellImageSize && dist > sellImageSize *0.1f)
            {
                if (pos.y - lastMousePos.y < 0)
                    SellUpgradeDialogImage.sprite = UpgradeSellMenu_SellSelected;
                else
                    SellUpgradeDialogImage.sprite = UpgradeSellMenu_UpgradeSelected;
            }
            else
            {
                SellUpgradeDialogImage.sprite = UpgradeSellMenu_NoneSelected;
            }
        }

        private void OnTowerDragProceed(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit rh;
            if (Physics.Raycast(ray, out rh))
            {
                var towerSystem = TowerGridSystem.Instance;
                if (towerSystem.GetCellInfo(rh.point)!=TowerGridSystem.CellState.Unknown)           // Если клетка существует то подсвечиваем ее и убираем подсветку с предыдущей
                {
                    towerSystem.ResetHighlightOfCell(lastHighlightedCellX, lastHighlightedCellY);
                    towerSystem.HighlightCell(rh.point, out lastHighlightedCellX, out lastHighlightedCellY);
                }else towerSystem.ResetHighlightOfCell(lastHighlightedCellX, lastHighlightedCellY);
            }
        }

        private void OnTowerDragEnd(PointerEventData eventData) {
            var towerSystem = TowerGridSystem.Instance;
            towerSystem.ResetHighlightOfCell(lastHighlightedCellX, lastHighlightedCellY);
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit rh;
            var tower = LevelManager.Instance.CurrentLevel.GetAvaliableToBuildTowers()[selectedTower];
            if (Physics.Raycast(ray, out rh))
            {
                if (towerSystem.GetCellInfo(rh.point) == TowerGridSystem.CellState.CanBuild)
                {
                    if (LevelManager.Instance.CurrentLevel.SpendMoney(tower.GetComponent<Tower>().GetPrice()))
                        towerSystem.BuildTowerAt(rh.point, tower);
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
            GoldLabel.text = string.Format("{0}", gold);
        }

        public void UpdateMainTowerState(int livesLeft)
        {
            MainTowerStateLabel.text= string.Format("{0}/{1}", livesLeft, mainTowerLifeCount);
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
            PauseMenuPanel.gameObject.SetActive(inPauseMenu);
            if(!inPauseMenu)
                PauseMenuPanel.sizeDelta = new Vector2(PauseMenuPanel.sizeDelta.x, PauseMenuPanel.sizeDelta.y);
            else
                PauseMenuPanel.sizeDelta = new Vector2(PauseMenuPanel.sizeDelta.x, PauseMenuPanel.sizeDelta.x * 4);
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
            StartWaveSpeedUpButton.onClick.SetPersistentListenerState(0,UnityEngine.Events.UnityEventCallState.Off);
            StartWaveSpeedUpButton.onClick.AddListener(ToggleSpeedUpMode);
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
                TowerBuildPanel.localPosition = new Vector3(TowerBuildPanel.localPosition.x, TowerBuildPanel.localPosition.y + towerPanelUISize, TowerBuildPanel.localPosition.z);
            }
            else
            {
                ShowHideTowerBuildMenuImg.rotation = Quaternion.Euler(0, 0, 90);
               // TowerBuildLabel.text = "Open";
                TowerBuildPanel.localPosition = new Vector3(TowerBuildPanel.localPosition.x, TowerBuildPanel.localPosition.y - towerPanelUISize, TowerBuildPanel.localPosition.z);
            }
        }

        // Меняет номер выбранной башни
        public void SwitchSelectedTower(GameObject go,int newID) {
            selectedTower = newID;
            towerIcon.GetComponent<Image>().sprite = towerButtons[newID].GetComponent<Image>().sprite;
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