using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameController : MonoBehaviour
{

    [SerializeField] private Sprite background;

    [SerializeField] private Sprite[] puzzle;
    [SerializeField] private List<Sprite> listSpritePuzzle = new List<Sprite>();

    [SerializeField] List<Button> listButton = new List<Button>();

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flipCardAudioClip;
    [SerializeField] private AudioClip correctCardAudioClip;


    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject spriteLogo;
    [SerializeField] private GameObject textMadeBy;

    bool firstGuess, secondGuess;
    bool gameIsFinish = false;
    int countGuesses;
    int countCorrectGuesses;
    int gameGuesses;

    int firstGuessIndex, secondGuessIndex;


    string firstGuessName, secondGuessName;

    private void Awake()
    {
        puzzle = Resources.LoadAll<Sprite>("Sprites/Images Puzzle");
    }

    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzle();
        Shuffle(listSpritePuzzle);
        gameGuesses = listSpritePuzzle.Count / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddGamePuzzle()
    {
        int looper = listButton.Count;
        int index = 0;

        for(int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }

            listSpritePuzzle.Add(puzzle[index]);

            index++;

        }
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Puzzle Button");

        for(int i = 0;i < objects.Length; i++)
        {
            listButton.Add(objects[i].GetComponent<Button>());
            listButton[i].image.sprite = background;
        }
    }

    public void AddListeners()
    {
        foreach(Button button in listButton)
        {
            button.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (!firstGuess)
        {
            audioSource.PlayOneShot(flipCardAudioClip);

            firstGuess = true;
            firstGuessIndex = int.Parse(name);
            listButton[firstGuessIndex].image.sprite = listSpritePuzzle[firstGuessIndex];
            firstGuessName = listSpritePuzzle[firstGuessIndex].name;

        }
        else if (!secondGuess && int.Parse(name)!= firstGuessIndex)
        {
            audioSource.PlayOneShot(flipCardAudioClip);

            secondGuess = true;
            secondGuessIndex = int.Parse(name);
            listButton[secondGuessIndex].image.sprite = listSpritePuzzle[secondGuessIndex];
            secondGuessName = listSpritePuzzle[secondGuessIndex].name;


            countGuesses++;

            StartCoroutine("CheckIfPuzzlesMatch");

            if (firstGuessName.Equals(secondGuessName))
            {
                Debug.Log("É IGUAL");
            }
            else{
                Debug.Log("NÃO É IGUAL");
            }

        }
    }

    IEnumerator CheckIfPuzzlesMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstGuessName.Equals(secondGuessName))
        {
            yield return new WaitForSeconds(0.5f);
           
            listButton[firstGuessIndex].interactable = false;
            listButton[secondGuessIndex].interactable = false;

            CheckIfGamePuzzleIsFinish();
            if (gameIsFinish)
            {
                yield return new WaitForSeconds(1f);
                logo.SetActive(true);
                spriteLogo.SetActive(true);
                textMadeBy.SetActive(true);
                yield return new WaitForSeconds(5f);
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            listButton[firstGuessIndex].image.sprite = background;
            listButton[secondGuessIndex].image.sprite = background;
        }

        yield return new WaitForSeconds(0.25f);

        firstGuess = secondGuess = false;
    }

    void CheckIfGamePuzzleIsFinish()
    {
        countCorrectGuesses++;
        audioSource.PlayOneShot(correctCardAudioClip);

        if (countCorrectGuesses == gameGuesses)
        {
            gameIsFinish = true;
        }

    }

    void Shuffle(List<Sprite> list)
    {
        for(int i = 0;i< list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
