using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform selector;
    [SerializeField] private RectTransform[] options;
    private int currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        MovePosition(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MovePosition(-1);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MovePosition(1);
        }
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Interact();
        }
    }

    public void MovePosition(int change)
    {
        currentPosition += change;

        if(currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if(currentPosition > options.Length -1)
        {
            currentPosition = 0;
        }

        selector.position = new Vector3(selector.position.x, options[currentPosition].position.y);
    }

    private void Interact()
    {
        if(currentPosition == 0)
        {
            SceneManager.LoadScene("GamePlay");
        }
        else if(currentPosition == 1)
        {
            Application.Quit(); //Exits build application
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; //exits unity editor version of game
            #endif
        }
    }
}
