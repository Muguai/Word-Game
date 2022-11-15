using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LetterOnSpawn : MonoBehaviour, IPooledObject
{

    public TextMeshPro textMesh;
    public void OnObjectSpawned()
    {

        System.Random rnd = new System.Random();
        int ascii_index = rnd.Next(65, 91); //ASCII character codes 65-90
        char myRandomUpperCase = Convert.ToChar(ascii_index); //produces any char A-Z

        textMesh.text = myRandomUpperCase.ToString();
    }
}
