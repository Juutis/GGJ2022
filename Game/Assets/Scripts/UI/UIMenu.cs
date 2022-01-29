using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public static UIMenu main;
    private void Awake() {
        main = this;
    }

    private List<UIMenuSelection> selections = new List<UIMenuSelection>();

    [SerializeField]
    private UIMenuSelection continueSelection;
    [SerializeField]
    private UIMenuSelection mainmenuSelection;
    [SerializeField]
    private UIMenuSelection restartSelection;

    private int selectorIndex = 0;

    private UIMenuSelection CurrentSelection;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Sprite werewolfSelectorSprite;
    [SerializeField]
    private Sprite humanSelectorSprite;

    private bool IsEnabled = false;
    private bool IsShowing = false;
    [SerializeField]
    private bool IsMainMenu = false;



    private void Start() {
        /*if (selections.Count > 0) {
            CurrentSelection = selections[selectorIndex];
        }*/
    }

    public void Show(TargetEntityType faction, bool hideContinue = false) {
        Time.timeScale = 0f;
        IsShowing = true;
        selections = new List<UIMenuSelection>();
        if (!hideContinue) {
            continueSelection.gameObject.SetActive(true);
            selections.Add(continueSelection);
        }
        selections.Add(mainmenuSelection);
        selections.Add(restartSelection);
        foreach(UIMenuSelection selection in selections) {
            selection.SetSprite(faction == TargetEntityType.Werewolf ? werewolfSelectorSprite : humanSelectorSprite);
        }
        CurrentSelection = selections[selectorIndex];
        CurrentSelection.SetSelected(true);
        animator.SetTrigger("Show");
    }

    public void Hide() {
        animator.SetTrigger("Hide");
    }

    public void WasShown() {
        IsShowing = false;
        IsEnabled = true;
    }

    public void WasHidden() {
        IsEnabled = false;
        Time.timeScale = 1f;
    }

    public void MoveSelector (bool directionDown) {
        if (directionDown) {
            if (selectorIndex < selections.Count - 1) {
                selectorIndex += 1;
            } else {
                return;
            }
        } else {
            if (selectorIndex > 0) {
                selectorIndex -= 1;
            } else {
                return;
            }
        }

        CurrentSelection = selections[selectorIndex];
        foreach(UIMenuSelection selection in selections) {
            if (selection.IsSelected) {
                selection.SetSelected(false);
            }
        }
        CurrentSelection.SetSelected(true);
    }

    private void Update() {
        if (!IsEnabled) {
            if (!IsMainMenu && !IsShowing && Input.GetKeyDown(KeyCode.P)){
                Show(TargetEntityManager.main.Player.TargetType);
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveSelector(true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveSelector(false);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            Select();
        }
    }

    public void Select() {
        selectorIndex = 0;
        UISelectionType selectionType = CurrentSelection.SelectionType;
        if (selectionType == UISelectionType.Continue) {
            Hide();
        }
        if (selectionType == UISelectionType.MeinMenu) {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
        if (selectionType == UISelectionType.Restart) {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }
    }
}


public enum UISelectionType {
    Continue,
    Restart,
    MeinMenu
}