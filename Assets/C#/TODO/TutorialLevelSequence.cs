using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TD
{
    public class TutorialLevelSequence : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] HighlightObjects;
        private string[] tutorialStrings;
        private int tutorialLeft = 0;
        private void Start()
        {
            tutorialStrings=new string[]{   LocalizationData.GetLocalizedString(14) ,
                                            LocalizationData.GetLocalizedString(17),
                                            LocalizationData.GetLocalizedString(18),
                                            LocalizationData.GetLocalizedString(19),
                                            LocalizationData.GetLocalizedString(20),
                                            LocalizationData.GetLocalizedString(21),
                                            LocalizationData.GetLocalizedString(22),
                                            LocalizationData.GetLocalizedString(23),
                                            LocalizationData.GetLocalizedString(24),
                                            LocalizationData.GetLocalizedString(25),
                                            LocalizationData.GetLocalizedString(26)};
            IngameUI.Instance.ShowTutorialMessage(tutorialStrings[tutorialLeft]);
            HighlightObjects[tutorialLeft].SetActive(true);
        }
        private void Update()
        {
            var ui = IngameUI.Instance;
            if (tutorialLeft == 3)
            {
                if (ui.HasBuiltTower)
                {
                    ui.ShowTutorialMessage(tutorialStrings[tutorialLeft]);
                    HighlightObjects[tutorialLeft].SetActive(true);
                }
            }else if(tutorialLeft == 5)
            {
                if (ui.IsInDefenseMode)
                {
                    ui.ShowTutorialMessage(tutorialStrings[tutorialLeft]);
                    HighlightObjects[tutorialLeft].SetActive(true);
                }
            }
        }

        public void StopTutorial()
        {
            HighlightObjects[tutorialLeft].SetActive(false);
            tutorialLeft = 11;
            IngameUI.Instance.HideTutorialMessage();
        }

        public void HideTutorialMessage()
        {
            tutorialLeft++;
            IngameUI.Instance.HideTutorialMessage();
            if(tutorialLeft<11)
                HighlightObjects[tutorialLeft - 1].SetActive(false);
            switch (tutorialLeft) {
                case 1:
                case 2:
                case 4:
                case 6:
                case 7:
                case 8:
                case 9:
                    IngameUI.Instance.ShowTutorialMessage(tutorialStrings[tutorialLeft]);
                    HighlightObjects[tutorialLeft].SetActive(true);
                    break;
                case 10:
                    IngameUI.Instance.ShowTutorialMessage(tutorialStrings[tutorialLeft]);
                    break;
            }
        }
    }
}