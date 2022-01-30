using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public static UIMenu main;
    private void Awake()
    {
        main = this;
    }

    private List<UIMenuSelection> selections = new List<UIMenuSelection>();

    [SerializeField]
    private UIMenuSelection continueSelection;
    [SerializeField]
    private UIMenuSelection mainmenuSelection;
    [SerializeField]
    private UIMenuSelection restartSelection;
    [SerializeField]
    private UIMenuSelection startSelection;
    [SerializeField]
    private UIMenuSelection sensitivitySelection;

    private int selectorIndex = 0;

    private UIMenuSelection CurrentSelection;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Sprite werewolfSelectorSprite;
    [SerializeField]
    private Sprite humanSelectorSprite;
    [SerializeField]
    private Sprite wereWolfConceptArt;
    [SerializeField]
    private Sprite humanConceptArt;

    private bool IsEnabled = false;
    private bool IsShowing = false;
    private bool IsHiding = false;
    [SerializeField]
    private bool IsMainMenu = false;

    [SerializeField]
    private Text txtMessage;
    [SerializeField]
    private Image imgConcept;
    [SerializeField]
    private GameObject mainMenuConcept;

    [SerializeField]
    private MouseConfig mouseConfig;

    private TargetEntityType currentFaction;

    private void Start()
    {
        /*if (selections.Count > 0) {
            CurrentSelection = selections[selectorIndex];
        }*/
        if (IsMainMenu)
        {
            Show(TargetEntityType.Human, "- The night beckons -", true);
        }
    }

    public void Show(TargetEntityType faction, string message = "- Game paused -\n\n <i>Press P to continue</i> ", bool hideContinue = false)
    {
        currentFaction = faction;
        Time.timeScale = 0f;
        IsShowing = true;
        selections = new List<UIMenuSelection>();
        txtMessage.text = message;
        if (!hideContinue)
        {
            continueSelection.gameObject.SetActive(true);
            selections.Add(continueSelection);
        }
        else
        {
            continueSelection.gameObject.SetActive(false);
        }
        if (IsMainMenu)
        {
            startSelection.gameObject.SetActive(true);
            selections.Add(startSelection);
            mainmenuSelection.gameObject.SetActive(false);
            restartSelection.gameObject.SetActive(false);
        }
        if (!IsMainMenu && hideContinue) {
            sensitivitySelection.gameObject.SetActive(false);
        } else {
            sensitivitySelection.gameObject.SetActive(true);
            sensitivitySelection.SetValue(mouseConfig.Sensitivity);
           selections.Add(sensitivitySelection);
        }
        if (!IsMainMenu)
        {
            selections.Add(mainmenuSelection);
            mainmenuSelection.gameObject.SetActive(true);
            selections.Add(restartSelection);
            restartSelection.gameObject.SetActive(true);
        }
        foreach (UIMenuSelection selection in selections)
        {
            selection.SetSprite(faction == TargetEntityType.Werewolf ? werewolfSelectorSprite : humanSelectorSprite);
            selection.SetSelected(false);
        }
        if (IsMainMenu)
        {
            mainMenuConcept.SetActive(true);
            imgConcept.enabled = false;
        }
        else
        {
            mainMenuConcept.SetActive(false);
            imgConcept.enabled = true;
            imgConcept.sprite = faction == TargetEntityType.Werewolf ? wereWolfConceptArt : humanConceptArt;
        }
        CurrentSelection = selections[selectorIndex];
        CurrentSelection.SetSelected(true);
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        IsHiding = true;
        animator.SetTrigger("Hide");
    }

    public void WasShown()
    {
        IsShowing = false;
        IsEnabled = true;
    }

    public void WasHidden()
    {
        IsHiding = false;
        IsEnabled = false;
        Time.timeScale = 1f;
    }

    public void MoveSelector(bool directionDown)
    {
        if (directionDown)
        {
            if (selectorIndex < selections.Count - 1)
            {
                selectorIndex += 1;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (selectorIndex > 0)
            {
                selectorIndex -= 1;
            }
            else
            {
                return;
            }
        }
        SoundManager.main.PlaySound(GameSoundType.Jump, transform.position, false);

        CurrentSelection = selections[selectorIndex];
        foreach (UIMenuSelection selection in selections)
        {
            if (selection.IsSelected)
            {
                selection.SetSelected(false);
            }
        }
        CurrentSelection.SetSelected(true);
    }

    private void Update()
    {
        if (!IsEnabled)
        {
            if (!IsMainMenu && !IsShowing && Input.GetKeyDown(KeyCode.P))
            {
                Show(TargetEntityManager.main.Player.TargetType);
            }
            return;
        }
        if (!IsMainMenu && !IsShowing && !IsHiding && Input.GetKeyDown(KeyCode.P))
        {
            Hide();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelector(true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelector(false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Select();
        }
    }



    public void Left()
    {
        UISelectionType selectionType = CurrentSelection.SelectionType;
        if (selectionType == UISelectionType.MouseSensitivity)
        {
            if (mouseConfig.DecreaseSensitivity()) {
                sensitivitySelection.Left();
            }
            sensitivitySelection.SetValue(mouseConfig.Sensitivity);
        }
    }
    public void Right()
    {
        UISelectionType selectionType = CurrentSelection.SelectionType;
        if (selectionType == UISelectionType.MouseSensitivity)
        {
            if (mouseConfig.IncreaseSensitivity()) {
                sensitivitySelection.Right();
            }
            sensitivitySelection.SetValue(mouseConfig.Sensitivity);
        }
    }

    public void Select()
    {
        selectorIndex = 0;
        UISelectionType selectionType = CurrentSelection.SelectionType;
        if (currentFaction == TargetEntityType.Human)
        {
            SoundManager.main.PlaySound(GameSoundType.Gunshot, Vector3.zero, false);
        }
        else
        {
            SoundManager.main.PlaySound(GameSoundType.Growl, Vector3.zero, false);
        }
        if (selectionType == UISelectionType.Continue)
        {
            Hide();
        }
        if (selectionType == UISelectionType.MeinMenu)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
        if (selectionType == UISelectionType.Restart)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }
        if (selectionType == UISelectionType.Start)
        {
            IsEnabled = false;
            Time.timeScale = 1f;
            MusicPlayer.main.FadeOutMenuMusic(1f);
            Invoke("StartAfterFade", 1.2f);
        }
    }

    public void StartAfterFade()
    {
        SceneManager.LoadScene(1);
    }

}


public enum UISelectionType
{
    Continue,
    Restart,
    MeinMenu,
    Start,
    MouseSensitivity
}