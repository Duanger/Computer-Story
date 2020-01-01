using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DieNonComputerCheck : MonoBehaviour
{
    public static DieNonComputerCheck current;
    public Vector2 mousePositiano;
    public bool isPaused;

    public bool mouseDetected;
   [SerializeField] private ComputerInputModule _compInputModule;
    void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            _compInputModule.enabled = false;
        }
        else
        {
            _compInputModule.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isPaused = !isPaused;
        }
        
    }
}
