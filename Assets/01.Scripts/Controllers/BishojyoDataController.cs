using System;
using System.Collections;
using System.Collections.Generic;
using Crogen.BishojyoGraph.RunTime;
using UnityEngine;

public class BishojyoDataController : MonoBehaviour
{
    //Data
    [SerializeField] private List<BishojyoContainer> bishojyoContainers;
    public BishojyoContainer currentBishojyoContainer;
    
    public int currentStoryIndex;
    
    //Controllers
    private DataController _dataController;
    
    public void Awake()
    {
        _dataController = FindObjectOfType<DataController>();

        _dataController.Save(new SaveData()
        {
            userName = "Crogen",
            currentStoryIndex = 0
        });
        currentBishojyoContainer = bishojyoContainers[_dataController.Load().currentStoryIndex];
    }
    
    
}
