using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TD
{
    public class TutorialLevelSequence : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] HighlightObjects;
        private string[] tutorialStrings= { "This is pause menu. Here you can reboot current level or quit to main menu." ,
                                            "This is tower building menu.",
                                            "To build a tower drag it on the battlefield.",
                                            "Touch tower and drag up or down to upgrade it or sell it.",
                                            "Press this button and you'll go into defense mode.",
                                            "Tired of waiting? Press this button to speed up game!",
                                            "Too fast? Press this button again to slow it down!",
                                            "Here you can see the amount of experience, you'll get more for each enemy you destroy. Try to get more experience, to get better results!",
                                            "But that's not all, for each destroyed enemy you'll get gold. You can spend it to upgrade your towers or to buy new ones.",
                                            "This is your lifecount. If it reaches zero - you'll lose.",
                                            "Are you ready? From now on you're on your own! Don't let them get to the end of the level!"};
        private int tutorialLeft = 0;
        private void Start()
        {
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