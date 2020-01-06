using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIYarnTextController : MonoBehaviour
{
    public static UIYarnTextController current;
    public bool StoppedWriting;
    public TMP_Text DocumentText;

    [SerializeField] private List<TMP_Text> actualChoiceText;
    [SerializeField] private List<TMP_Text> storageChoiceText;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject choicePanel;


    private void Awake()
    {
        current = this;
    }

    public void FindIntersectingWord(Vector2 mousePos)
    {
        int index = TMP_TextUtilities.FindIntersectingWord(DocumentText, DieCursor.current.mousePossy, playerCamera);
        foreach (var storageText in storageChoiceText)
        {
            if (DocumentText.textInfo.wordInfo[index].GetWord() == storageText.textInfo.wordInfo[0].GetWord())
            {
                UISpinYarn.Current.ActivateSuggestionPanel(mousePos, playerCamera);

                foreach (var choiceText in actualChoiceText)
                {
                    choiceText.transform.parent.gameObject.SetActive(true);
                    choiceText.text = storageText.text;
                }

                DieCursor.current.RightClicked = true;
            }
        }
    }

    public void ActivateAndStoreOptionText(int i, string s)
    {
        storageChoiceText[i].gameObject.SetActive(true);
        storageChoiceText[i].text = s;
    }

    public void DeselectChoiceOptions()
    {
        choicePanel.SetActive(false);
        DieCursor.current.RightClicked = false;
    }

    public void DeactivateChoiceOptions()
    {
        choicePanel.SetActive(false);
        foreach (var choiceCut in storageChoiceText)
        {
            choiceCut.gameObject.SetActive(false);
        }
    }
}