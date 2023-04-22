using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{

    [SerializeField] public float playerSpeed = 3f;
    [SerializeField] public TextMeshProUGUI textMesh;
    [SerializeField] private List<GameObject> discoLights;
    [SerializeField] private GameObject npcParent;

    public bool isSquidGame;
    public bool isRedPhaseOfSquidGame;
    public bool isSuperDance;
    public static Manager instance;
    List<string> strings;
    public List<GameObject> npcArr;
    string text;

    public List<Sprite> defaultNpcSprites;
    
    void Awake()
    {
        //create singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // initialize values
        instance.isSquidGame = false;
        instance.isSuperDance = false;
        instance.isRedPhaseOfSquidGame = false;
        strings = new List<string>();
        npcArr = new GameObject[npcParent.transform.childCount].ToList<GameObject>();

        int i = 0;
        foreach(Transform child in npcParent.transform)
        {
            npcArr[i] = child.gameObject;
            i++;
        }
        foreach ( GameObject npc in npcArr)
        {
            if (npc != null)
            {
                defaultNpcSprites.Add(npc.GetComponent<SpriteRenderer>().sprite);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        strings.Add("!!Sound on!!");
        strings.Add("");
        strings.Add("W, A, S, D to move");
        strings.Add("Adjust brightness with \"O\" and \"P\"");
        strings.Add("Spawn Discolight with \"F\"");
        strings.Add("Cheatcodes are:\n- \"DOGE\"\n- \"NINJA\"\n- \"SQUIDGAME\"\n- \"BOOM\"\n- \"PEW\"\n- \"EXMATRIKULUS\n");
        strings.Add("\n_______________________\n");

        foreach(string str in strings)
        {
            text += str + "\n"; 
        }
        textMesh.text = text;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void adjustPlayerSpeed(float modificator)
    {
        if(modificator == 1f)
        {
            playerSpeed = 3f;
            return;
        }
        playerSpeed *= modificator;
    }

    public void displayMessage(string message, float Seconds = 2f)
    {
        StartCoroutine(DisplayMessage(message, Seconds));
    }

    IEnumerator DisplayMessage(string message, float delaySeconds)
    {

        textMesh.text += message + "\n";
        yield return new WaitForSeconds(delaySeconds);
        int index = textMesh.text.IndexOf(message);
        if (index >= 0)
        {
            string newString = textMesh.text.Remove(index);
            textMesh.text = newString;
        }
    }
}
