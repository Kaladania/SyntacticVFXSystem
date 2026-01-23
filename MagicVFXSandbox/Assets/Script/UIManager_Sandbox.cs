using QuizManager;
using SnSECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIManager_Sandbox : MonoBehaviour
{
    [SerializeField]
    private ElementIconMapTemplate _iconData = null; //holds the scriptable object data container for ui icon maps.

    [SerializeField]
    private GameObject _instructionPanel = null;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void ToggleInstructions(bool toggle)
    {
        _instructionPanel.SetActive(toggle);
    }
}
