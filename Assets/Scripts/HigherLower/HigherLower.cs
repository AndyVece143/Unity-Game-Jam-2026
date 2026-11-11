using System.Collections;
using UnityEngine;

public class HigherLower : MonoBehaviour
{
    public int targetNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetNumber = Random.Range(10, 99);
        Debug.Log(targetNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompareNumber()
    {

    }
}
