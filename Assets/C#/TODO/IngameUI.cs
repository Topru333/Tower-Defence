using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public class IngameUI : Singleton<IngameUI>
    {

        private bool inTowerBuildMenu = true,  // Находится ли пользователь в меню постройки
                        gameIsSpeedUp = false,  // Включено ли ускорение игры
                        canShowBuidMenu = false,  // Можно ли скрыть меню постройки башен
                        inPauseMenu = false,      // Внутри меню паузы
                        levelEnded = false,       // Внутри диалога окончания уровня
                        towerUpgradeSellDialogActive = false,   // Активен ли диалог продажи/прокачки башни
                        inTutorialMessage=false;
        [SerializeField]
        private RectTransform TowerPanel;        // Панель постойки башен
        [SerializeField]
        private RectTransform TowerBuildPanel;   // Панель постойки башен вместе с кнопкой скрытия/показа этой панели
        [SerializeField]
        private RectTransform PauseMenuPanel;    // Панель меню паузы
        [SerializeField]
        private RectTransform EndLevelDialog;    // Диалог окончания уровня
        [SerializeField]
        private RectTransform ShowHideTowerBuildMenuImg; // Изображение скрытия/показа панели постойки башен
        [SerializeField]
        private GameObject TowerSellUpgradeDialog;   // Кнопка продажи/прокачки башни
        [SerializeField]
        private GameObject TutorialDialog;  // Диалоговое окно обучающего уровня

        [SerializeField]
        private Button StartWaveSpeedUpButton; // Кнопка начала волны/ускорения игры
        [SerializeField]
        private Text ExperienceLabel;          // Строка с опытом
        [SerializeField]
        private Text WaveNumberLabel;          // Строка с номером волны
        [SerializeField]
        private Text GoldLabel;                // Строка с деньгами
        [SerializeField]
        private Text MainTowerStateLabel;      // Строка с состоянием уровня
        [SerializeField]
        private Text TutorialDialogLabel;      // Строка с состоянием уровня

        [SerializeField]
        private Text EndLevelDialogHeaderLabel;  // Заголовок диалога окончания уровня
        [SerializeField]
        private Image StartWaveSpeedUpImage;     // Изображение кнопки начала волны/ускорения
        [SerializeField]
        private Image PauseResumeImage;          // Изображение кнопки паузы
        [SerializeField]
        private Image SellUpgradeDialogImage;
        [SerializeField]
        private RawImage Star1;
        [SerializeField]
        private RawImage Star2;
        [SerializeField]
        private RawImage Star3;

        [SerializeField]
        private Sprite ResumeSprite;     // Спрайт возобновления из паузы
        [SerializeField]
        private Sprite PauseSprite;      // Спрайт паузы
        [SerializeField]
        private Sprite SpeedUpSprite;    // Спрайт ускорения игры
        [SerializeField]
        private Sprite SlowDownSprite;   // Спрайт замедления игры

        [SerializeField]
        private Sprite UpgradeSellMenu_NoneSelected;
        [SerializeField]
        private Sprite UpgradeSellMenu_SellSelected;
        [SerializeField]
        private Sprite UpgradeSellMenu_UpgradeSelected;
        private StringBuilder stringBuilderForConcat;

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
        public bool HasBuiltTower
        {
            get;
            private set;
        }
        public bool IsInDefenseMode
        {
            get;
            private set;
        }


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
            HasBuiltTower = IsInDefenseMode = false;
            stringBuilderForConcat = new StringBuilder(32); 
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
                    {
                        towerSystem.BuildTowerAt(rh.point, tower);
                        HasBuiltTower = true;
                    }
                }
            }
        }
        
        // Обновляет строку с опытом
        public void UpdateExperince(int Exp)
        {
            stringBuilderForConcat.Remove(0, stringBuilderForConcat.Length);// Используем stringbuilder для сокращения вызовов сборщика мусора
            stringBuilderForConcat.Append("Exp:").Append(Exp);
            ExperienceLabel.text = stringBuilderForConcat.ToString();//string.Format("Exp:{0}", Exp);
        }

        // Обновляет строку с номером волны
        public void IncreaseWaveNumber()
        {
            stringBuilderForConcat.Remove(0, stringBuilderForConcat.Length);
            stringBuilderForConcat.Append("Wave:").Append(++wavePast).Append("/").Append(waveCount);
            WaveNumberLabel.text = stringBuilderForConcat.ToString();//"Wave:"+ (++wavePast)+"/" + waveCount;
        }

        // Обновляет строку с золотом
        public void UpdateGold(int gold)
        {
            GoldLabel.text = gold.ToString();
        }

        public void UpdateMainTowerState(int livesLeft)
        {
            stringBuilderForConcat.Remove(0, stringBuilderForConcat.Length);
            stringBuilderForConcat.Append(livesLeft).Append("/").Append(mainTowerLifeCount);
            MainTowerStateLabel.text = stringBuilderForConcat.ToString();//livesLeft+"/"+ mainTowerLifeCount;
        }

        // Открывает меню паузы
        public void TogglePauseMenu()
        {
            if (levelEnded|| inTutorialMessage)
                return;
            Logger.Log("Открыто меню паузы.");

            inPauseMenu = !inPauseMenu;
            PauseResumeImage.sprite = inPauseMenu ? ResumeSprite : PauseSprite;
            DragNDropItem.AllowDragNDrop = !inPauseMenu;
            if (!inTowerBuildMenu)
            {
                if(inPauseMenu)
                    LevelManager.Instance.Pause();
                else
                    LevelManager.Instance.Resume();
            }
            PauseMenuPanel.gameObject.SetActive(inPauseMenu);
            if(!inPauseMenu)
                PauseMenuPanel.sizeDelta = new Vector2(PauseMenuPanel.sizeDelta.x, PauseMenuPanel.sizeDelta.y);
            else
                PauseMenuPanel.sizeDelta = new Vector2(PauseMenuPanel.sizeDelta.x, PauseMenuPanel.sizeDelta.x * 4);
        }

        // Перезапускает уровень, данное действие запускается из меню паузы
        public void RestartGame() {
            Logger.Log("Рестарт игры.");
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        // Показывает меню настроек, данная кнопка запускается из меню паузы
        public void ShowSettingsMenu() {
            Logger.Log("Открыто меню настроек.");
        }

        // Возвращает в главное меню, данное действие запускается из меню паузы
        public void ToMainMenu() {
            Logger.Log("Возврат в главное меню.");

            if(gameIsSpeedUp)
                ToggleSpeedUpMode();
            LevelManager.Instance.Resume();
            SceneManager.LoadScene("MainMenu");
        }

        // Запускает волну NPC и скрывает меню постройки башен
        public void StartWave() {
            Logger.Log("Вызвана волна NPC.");

            if (inPauseMenu|| inTutorialMessage)
                return;
            LevelManager.Instance.CurrentLevel.WaveStart();
            canShowBuidMenu = true;
            ToggleTowerBuildMenu();

            StartWaveSpeedUpImage.sprite = SpeedUpSprite;
            StartWaveSpeedUpButton.onClick.SetPersistentListenerState(0,UnityEngine.Events.UnityEventCallState.Off);
            StartWaveSpeedUpButton.onClick.AddListener(ToggleSpeedUpMode);
            StartCoroutine("SetDefenseModeOn"); 
        }
        IEnumerator SetDefenseModeOn() {
            yield return new WaitForSeconds(3); IsInDefenseMode = true;
        }
        // Ускоряет/Замедляет игру
        public void ToggleSpeedUpMode() {
            Logger.Log("Скорость игры изменена.");

            if (inTowerBuildMenu||inPauseMenu|| levelEnded|| inTutorialMessage)
                return;
            gameIsSpeedUp = !gameIsSpeedUp;
            StartWaveSpeedUpImage.sprite = gameIsSpeedUp ? SlowDownSprite : SpeedUpSprite;
            LevelManager.Instance.GameSpeed(gameIsSpeedUp?5:1);
        }

        // Скрывает/Показывает меню постройки башен
        public void ToggleTowerBuildMenu() {
            if (!canShowBuidMenu || inPauseMenu|| levelEnded|| inTutorialMessage)
                return;
            
            TowerGridSystem.Instance.ToggleGridVizualization();
            inTowerBuildMenu = !inTowerBuildMenu;
            if (!inPauseMenu)
            {
                if (inTowerBuildMenu)
                    LevelManager.Instance.Pause();
                else
                    LevelManager.Instance.Resume();
            }
            // TODO: Сделать плавный переход
            if (inTowerBuildMenu)
            {
                ShowHideTowerBuildMenuImg.rotation = Quaternion.Euler(0, 0, -90);
                TowerBuildPanel.localPosition = new Vector3(TowerBuildPanel.localPosition.x, TowerBuildPanel.localPosition.y + towerPanelUISize, TowerBuildPanel.localPosition.z);
            }
            else
            {
                ShowHideTowerBuildMenuImg.rotation = Quaternion.Euler(0, 0, 90);
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
            Logger.Log("Вызван диалог окончания уровня.");

            if (inTowerBuildMenu)
                ToggleTowerBuildMenu();
            if (inPauseMenu)
                TogglePauseMenu();
            EndLevelDialog.gameObject.SetActive(true);
            EndLevelDialogHeaderLabel.text = won ? "Congratulations you won!" : "You lost! Want to try again?";
            levelEnded = true;
            Star1.color = won ? Color.white : Color.black;
            int lc = LevelManager.Instance.CurrentLevel.GetMainTowerLifeCount();
            Star2.color = (((float)lc / mainTowerLifeCount) > 0.4f) ? Color.white : Color.black;
            Star3.color = (((float)lc / mainTowerLifeCount) > 0.6f) ? Color.white : Color.black;
            LevelManager.Instance.Pause();
        }

        public void ShowTutorialMessage(string message)
        {
            inTutorialMessage = true;
            LevelManager.Instance.Pause();
            TutorialDialog.SetActive(true);
            TutorialDialogLabel.text = message;
        }
        public void HideTutorialMessage()
        {
            inTutorialMessage = false;
            LevelManager.Instance.Resume();
            TutorialDialog.SetActive(false);
        }
    }
}