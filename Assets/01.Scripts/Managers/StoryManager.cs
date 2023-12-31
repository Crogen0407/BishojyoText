using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoSingleton<StoryManager>
{
    //Controllers
    public SlideController SlideController { get; private set; }

    private void Init()
    {
        SlideController = FindObjectOfType<SlideController>();
    }


    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (arg0, mode) =>
        {
            Init();
        };
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SlideController.CurrentSlide++;
        }
    }
}