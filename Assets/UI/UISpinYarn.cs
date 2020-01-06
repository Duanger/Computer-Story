using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;


public class UISpinYarn : DialogueUIBehaviour
{
    // Start is called before the first frame update
    public static UISpinYarn Current;
    [SerializeField] private RectTransform panelRect,panelParent;
    [SerializeField] private List<Button> optionButtons;
 
    
    public float textSpeed = 0.025f;
    public GameObject dialogueContainer;




    private Yarn.OptionChooser SetSelectedOption;

    void Awake ()
    {
        Current = this;
        // Start by hiding the container, line and option buttons
        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        UIYarnTextController.current.DocumentText.gameObject.SetActive (false);

        foreach (var button in optionButtons) {
            button.gameObject.SetActive (false);
        }
    }
   public override IEnumerator RunLine (Yarn.Line line)
        {
            // Show the text
           UIYarnTextController.current.DocumentText.gameObject.SetActive (true);

            if (textSpeed > 0.0f) {
                // Display the line one character at a time
                var stringBuilder = new StringBuilder ();

                foreach (char c in line.text) {
                    stringBuilder.Append (c);
                    UIYarnTextController.current.DocumentText.text = stringBuilder.ToString ();
                    yield return new WaitForSeconds (textSpeed);
                }
            } else {
                // Display the line immediately if textSpeed == 0
                UIYarnTextController.current.DocumentText.text = line.text;
            }
            //Here is where the werk is done
            UIYarnTextController.current.StoppedWriting = true;
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
             
                UIYarnTextController.current.ActivateAndStoreOptionText(i,optionString);
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

        public void ActivateSuggestionPanel(Vector2 m,Camera c)
        {
            panelRect.gameObject.SetActive(true);
            Vector2 worldPos = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelParent, m, c, out worldPos);
            panelRect.anchoredPosition = worldPos;
        }
}

      
        

