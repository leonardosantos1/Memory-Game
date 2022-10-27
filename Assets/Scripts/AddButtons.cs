using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [SerializeField] private Transform puzzlePanel;
    [SerializeField] private GameObject puzzleButton;
    // Start is called before the first frame update

    private void Awake()
    {
        for(int i = 0; i< 12; i++)
        {
            GameObject button = Instantiate(puzzleButton);
            button.name = "" + i;
            button.transform.SetParent(puzzlePanel, false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
