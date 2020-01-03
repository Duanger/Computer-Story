using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;


public class UISpinYarn : DialogueUIBehaviour
{
    // Start is called before the first frame update
    public static UISpinYarn Current;
    [SerializeField] private Camera _gamm;
    [SerializeField] private TextMeshProUGUI computerText;
    [SerializeField] private List<Button> optionButtons;
    [SerializeField] private List<TMP_Text> optionButtonText;
    [SerializeField] private List<TMP_Text> hiddenButtonText;
    [SerializeField] private GameObject autoCorrector;
    [SerializeField] private string[] stringy;
    
    public float textSpeed = 0.025f;
    public bool FinishedTyping;
    public bool RightClicked;
    public GameObject dialogueContainer;




    private Yarn.OptionChooser SetSelectedOption;

    void Awake ()
    {
        Current = this;
        // Start by hiding the container, line and option buttons
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        computerText.gameObject.SetActive (false);

        foreach (var button in optionButtons) {
            button.gameObject.SetActive (false);
        }
    }
   public override IEnumerator RunLine (Yarn.Line line)
        {
            // Show the text
            computerText.gameObject.SetActive (true);

            if (textSpeed > 0.0f) {
                // Display the line one character at a time
                var stringBuilder = new StringBuilder ();

                foreach (char c in line.text) {
                    stringBuilder.Append (c);
                    computerText.text = stringBuilder.ToString ();
                    yield return new WaitForSeconds (textSpeed);
                }
            } else {
                // Display the line immediately if textSpeed == 0
                computerText.text = line.text;
            }
            //Here is where the werk is done
            FinishedTyping = true;
            // Hide the text and prompt
            //lineText.gameObject.SetActive (false);
        }

        /// Show a list of options, and wait for the player to make a selection.
        public override IEnumerator RunOptions (Yarn.Options optionsCollection, 
                                                Yarn.OptionChooser optionChooser)
        {
            // Do a little bit of safety checking
            if (optionsCollection.options.Count > optionButtons.Count) {
                Debug.LogWarning("There are more options to present than there are" +
                                 "buttons to present them in. This will cause problems.");
            }

            // Display each option in a button, and make it visible
            int i = 0;
            foreach (var optionString in optionsCollection.options)
            {
             
                hiddenButtonText[i].gameObject.SetActive(true);
                hiddenButtonText[i].GetComponentInChildren<TextMeshProUGUI>().text = optionString;
                /*autoCorrector.GetComponent<Image>().enabled = false;
                optionButtons[i].GetComponent<Image>().enabled = false;
                
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().enabled = false;*/
                i++;
            } 
            

            // Record that we're using it
            SetSelectedOption = optionChooser;

            // Wait until the chooser has been used and then removed (see SetOption below)
            while (SetSelectedOption != null) {
                yield return null;
            }

            // Hide all the buttons
            foreach (var button in optionButtons) {
                button.gameObject.SetActive (false);
            }
        }

        /// Called by buttons to make a selection.
        public void SetOption (int selectedOption)
        {

            // Call the delegate to tell the dialogue system that we've
            // selected an option.
            SetSelectedOption (selectedOption);

            // Now remove the delegate so that the loop in RunOptions will exit
            SetSelectedOption = null; 
        }

        /// Run an internal command.
        public override IEnumerator RunCommand (Yarn.Command command)
        {
            // "Perform" the command
            Debug.Log ("Command: " + command.text);

            yield break;
        }

        /// Called when the dialogue system has started running.
        public override IEnumerator DialogueStarted ()
        {
            Debug.Log ("Dialogue starting!");

            // Enable the dialogue controls.
            if (dialogueContainer != null)
                dialogueContainer.SetActive(true);
            

            yield break;
        }

        /// Called when the dialogue system has finished running.
        public override IEnumerator DialogueComplete ()
        {
            Debug.Log ("Complete!");

            // Hide the dialogue interface.
            /*if (dialogueContainer != null)
                dialogueContainer.SetActive(false);*/
            
            yield break;
        }
        
        public void LocateTheWord()
        {
            foreach (var buttText in hiddenButtonText)
            {
                int index = TMP_TextUtilities.FindIntersectingWord(computerText,
                    DieCursor.current.mousePossy, _gamm);
               
                
                if (computerText.textInfo.wordInfo[index].GetWord() == buttText.textInfo.wordInfo[0].GetWord())
                {
                    Debug.Log("fog");
                    if (!RightClicked)
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            autoCorrector.SetActive(true);
                            
                            for (int i = 0; i < optionButtonText.Count; i++)
                            {
                                optionButtons[i].gameObject.SetActive(true);
                                optionButtonText[i].text = buttText.text;
                            }
                            RightClicked = true;
                        }
                    }
                }
                else
                {
                    if (RightClicked)
                    {
                        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                        {
                            autoCorrector.SetActive(false);
                            RightClicked = false;
                        }
                    }
                }
            }
    
        }

      
        
}
