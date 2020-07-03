using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GoogleSheetsToUnity.ThirdPary;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataObject : ScriptableObject
{
    [HideInInspector]
    public string associatedSheet = "19xpmX-VBPAMhxbAsU_FhWAQwbS_Ra0cMqzAov1VA1OI";
    [HideInInspector]
    public string associatedWorksheet = "Form Responses 1";

    public int health;
    public int attack;
    public int defence;
    public List<string> items = new List<string>();
    //SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search(associatedSheet, associatedWorksheet), UpdateStats);

    //SpreadsheetManager.Read(new GSTU_Search(associatedSheet, associatedWorksheet), callback, mergedCells);

    public Dictionary<String, GSTU_Cell> data;

    void Start()
    {
        SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search(associatedSheet, associatedWorksheet), UpdateMethodOne);

    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        data = ss.Cells;
    }
}




