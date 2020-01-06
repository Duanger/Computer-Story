using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(RectTransform))]
public class DieCursor : MonoBehaviour
{
    public static DieCursor current;
    public bool IsPointer;
    public bool RightClicked;
    public Vector2 mousePossy;

    [SerializeField] private AudioSource clickSound;
    [SerializeField] private GameObject dieAnschluss;
    [SerializeField] private GameObject rightClickBox;
    [SerializeField] private Camera cammy;
    [SerializeField] private float cursorSensitivity,lerpAlpha;
    [SerializeField] private RectTransform parentScreen;
    private float xSize, ySize;
    private RectTransform rect;

    void Awake()
    {
        current = this;
    }
    void Start()
    {
        rect = GetComponent<RectTransform>();
        xSize = rect.sizeDelta.x * rect.localScale.x;
        ySize = rect.sizeDelta.y * rect.localScale.y;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        if (!DieNonComputerCheck.current.isPaused)
        {
            MoveCursor(); 
            ConvertToScreen();
            GetMouseClick();
        }

        
    }

    void MoveCursor()
    {
        var vincusX = Input.GetAxis("Mouse X");
        var vincusY = Input.GetAxis("Mouse Y");
        Vector2 gar = rect.anchoredPosition;
        gar += new Vector2(vincusX * cursorSensitivity, vincusY * cursorSensitivity);
        //rect.anchoredPosition = gar;
        Vector2 vinger = Vector2.Lerp(rect.anchoredPosition, gar, lerpAlpha);
        //Debug.Log(rect.sizeDelta.x * rect.localScale);
        RestrictCursor(vinger);
    }

    void RestrictCursor(Vector2 v)
    {
        float xmin = xSize / 2f;
        float xmax = parentScreen.sizeDelta.x - (xSize / 2f);
        float ymax = -ySize / 2f;
        float ymin = -parentScreen.sizeDelta.y + (ySize/2f);
        var ving = (xmin, xmax, ymin, ymax);
        Debug.Log(ving);
        v = new Vector2(Mathf.Clamp(v.x, xmin, xmax),Mathf.Clamp(v.y,ymin,ymax));
        rect.anchoredPosition = v;
    }

    void ConvertToScreen()
    {
        mousePossy= cammy.WorldToScreenPoint(rect.position);
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RightClicked)
            {
                UIYarnTextController.current.DeselectChoiceOptions();
            }
            clickSound.PlayOneShot(clickSound.clip);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!RightClicked)
            {
                UIYarnTextController.current.FindIntersectingWord(mousePossy);
            }
            else
            {
                UIYarnTextController.current.DeselectChoiceOptions();
            }
        }
    }
}
