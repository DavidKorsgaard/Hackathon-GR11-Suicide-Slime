using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class IntroAndStartGame : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;         //Textbox for intro text
    [SerializeField] GameObject startOverlay;       //Object that says tap to start
    [SerializeField] GameObject tap_tekst;

    [TextArea]                                  //Makes it easy to edit text in Unity
    [SerializeField] string[] dialogueLines;    //Array og strings each for a line of dialogue

    private int currentline = 0;                //Keeps track of the line of dialogue we are at
    private bool dialogueFinished = false;      //Prevents skipping ahead before we have seen all of the dialogue
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f;                //Pauses entire game 
        tap_tekst.SetActive(false);      //Hides "Tap to start" until dialogue is done
        ShowNextDialogueLine();             //Starts dialogue
    }


    void Update()
    {
        if (!dialogueFinished && Input.GetMouseButtonDown(0))    //Checks if dialogue is not finished yet and if player clicks
        {
            ShowNextDialogueLine();
        }
        else if (dialogueFinished && !gameStarted && Input.GetMouseButtonDown(0) || Input.touchCount > 0)   //Checks is dialogue is finished, the game has not started yet and that the player clicks
        {
            StartGame();
        }
    }

    void ShowNextDialogueLine()
    {
        if (currentline < dialogueLines.Length)   //if there are still lines left in the dialogue
        {
            dialogueText.text = dialogueLines[currentline];   //Shows current line
            currentline++;                                    //Goes through to next line
        }
        else
        {
            dialogueFinished = true;                          
            dialogueText.text = "";                           //When dialogue is finished no text will appear
            tap_tekst.SetActive(true);                    //Overlay for "Tap to start" is set to active
        }
    }

    void StartGame()
    {
        Time.timeScale = 1f;                //Unpauses game
        gameStarted = true;
        tap_tekst.SetActive(true);
        startOverlay.SetActive(false);      //Hide UI elements
    }
}
