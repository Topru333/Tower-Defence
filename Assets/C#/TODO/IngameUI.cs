using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TD
{
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

        public GameObject towerPanel;
        public RectTransform towerBuildPanel;
        public Text ExperienceLabel;
        public Text WaveNumberLabel;
        public Text GoldLabel;
        private float towerPanelUISize;
        private int waveCount;
        private int wavePast=0;

        private void Awake()
        {
            _instance = this;
            waveCount = LevelManager.Instance.CurrentLevel.GetWaveCount();
            GameObject towerButton = new GameObject();
            towerButton.AddComponent<Button>();
            towerButton.AddComponent<Image>();
            towerButton.SetActive(false);
            int i = 0;
            var towers = LevelManager.Instance.CurrentLevel.GetAvaliableToBuildTowers();
            foreach (GameObject tower in towers)
            {
                GameObject go = Instantiate(towerButton);
                go.GetComponent<Button>().onClick.AddListener(() => SwitchSelectedTower(i-1));
                go.transform.SetParent(towerPanel.transform);
                go.GetComponent<Image>().sprite = tower.GetComponent<Tower>().icon;
                go.SetActive(true);
                i++;
            }
            towerPanelUISize = towerPanel.GetComponent<RectTransform>().rect.height;
        }

        private void Update()
        {
            if (inTowerBuildMenu)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 mposition = Input.mousePosition;
#else
                if(Input.touchCount>0&&!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 mposition = Input.GetTouch(0).position;
#endif

                    Ray ray = Camera.main.ScreenPointToRay(mposition);
                    RaycastHit rh;
                    if (Physics.Raycast(ray, out rh))
                    {
                        TowerGridSystem.Instance.BuildTowerAt(rh.point, LevelManager.Instance.CurrentLevel.GetAvaliableToBuildTowers()[selectedTower]);
                    }
                }
            }
        }
        
        // Обновляет строку с опытом
        public void UpdateExperince(int Exp)
        {
            ExperienceLabel.text = string.Format("Exp:{0}", Exp);
        }

        // Обновляет строку с опытом
        public void IncreaseWaveNumber()
        {
            WaveNumberLabel.text = string.Format("Wave:{0}/{1}", ++wavePast,waveCount);
        }

        // Обновляет строку с опытом
        public void UpdateGold(int gold)
        {
            GoldLabel.text = string.Format("Gold:{0}", gold);
        }

        // Открывает меню паузы
        // TODO: Сделать меню паузы
        public void ShowPauseMenu()
        {
#if DEBUG
            Debug.Log("Открыто меню паузы.");
#endif
            LevelManager.Instance.Pause();
        }

        // Перезапускает уровень, данное действие запускается из меню паузы
        // TODO: Реализовать перезапуск уровня
        public void RestartGame() {
#if DEBUG
            Debug.Log("Рестарт игры.");
#endif
        }

        // Показывает меню настроек, данная кнопка запускается из меню паузы
        // TODO: Сделать меню настроек
        public void ShowSettingsMenu() {
#if DEBUG
            Debug.Log("Открыто меню настроек.");
#endif
        }

        // Возвращает в главное меню, данное действие запускается из меню паузы
        // TODO: Проверить работоспособность
        public void ToMainMenu() {
#if DEBUG
            Debug.Log("Возврат в главное меню.");
#endif
            SceneManager.LoadScene("MainMenu");
        }

        // Запускает волну мобов
        // TODO: Менять действие кнопки на ускорение/замедление
        public void StartWave() {
#if DEBUG
            Debug.Log("Вызвана волна NPC.");
#endif
            LevelManager.Instance.CurrentLevel.WaveStart();
            canShowBuidMenu = true;
            ToggleTowerBuildMenu();
        }

        // Ускоряет/Замедляет игру
        // TODO: Проверить работоспособность
        public void ToggleSpeedUpMode() {
#if DEBUG
            Debug.Log("Скорость игры изменена.");
#endif
            LevelManager.Instance.GameSpeed(gameIsSpeedUp?1:5);
        }

        // Скрывает/Показывает меню постройки башен
        public void ToggleTowerBuildMenu() {
            if (!canShowBuidMenu)
                return;
            LevelManager.Instance.Pause();
            inTowerBuildMenu = !inTowerBuildMenu;
            // TODO: Сделать плавный переход
            if(inTowerBuildMenu)
                towerBuildPanel.localPosition = new Vector3(towerBuildPanel.localPosition.x, towerBuildPanel.localPosition.y + towerPanelUISize, towerBuildPanel.localPosition.z);
            else
                towerBuildPanel.localPosition = new Vector3(towerBuildPanel.localPosition.x, towerBuildPanel.localPosition.y - towerPanelUISize, towerBuildPanel.localPosition.z);
        }

        // Меняет номер выбранной башни
        public void SwitchSelectedTower(int newID) {
            selectedTower = newID;
        }

        // Показывает диалог прокачки/продажи башни
        public void ShowTowerUpgradeDialog(Vector2 posOnScreen) {
            // TODO: Реализация
        }

        // Показывает диалог окончания уровня
        // TODO: Реализация
        public void ShowEndLevelDialog() {
            LevelManager.Instance.Pause();
        }
    }
}