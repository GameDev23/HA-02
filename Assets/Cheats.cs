using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cheat
{
    public string sequence;
    public int index;
    public bool isActive;
    System.Func<bool, int> method;
    public Cheat(string sequence)
    {
        this.sequence = sequence;
        this.index = 0;
        this.method = null;
        this.isActive = false;
    }
    public Cheat(string sequence, System.Func<bool, int> method)
    {
        this.sequence = sequence;
        this.method= method;
    }
    public void setMethod(System.Func<bool, int> method)
    {
        this.method = method;
    }

    public bool RunMethod()
    {
        // flip is active status and run cheat
        this.isActive = !this.isActive;
        if (this.isActive)
        {
            Debug.Log("Cheat " + this.sequence + " activated");
        }
        else
        {
            Debug.Log("Cheat " + this.sequence + " deactivated");
        }
        int i = method(this.isActive);
        return true;
    }

    public bool checkInputKey(char inputKey)
    {
        // check if index is valid and not out of bounds
        if(sequence.Length > index)
        {
            Debug.Log("pressed " + inputKey);
            // check if the pressed key is the next one from the cheatcode
            if (inputKey == sequence[index])
            {
                // if key is valid  then increase index to next key
                index++;
                Debug.Log("Key was valid");
                // check if cheatcode is complete
                if (index == sequence.Length)
                {
                       // reset index and run cheat code
                        index = 0;
                        this.RunMethod();
                        return true;
                }
            }

            // check if a bad key is pressed and reset index
            else
            {
                index = 0;
                if (inputKey == sequence[index])
                    index++;
                return false;
            }
        }
        return false;
    }
}
public class Cheats : MonoBehaviour
{
    [SerializeField] Sprite dogeSprite;
    [SerializeField] SpriteRenderer player;
    [SerializeField] Sprite[] explosionsArr;
    [SerializeField] Camera cam;
    [SerializeField] Sprite projectileSprite;
    [SerializeField] AudioClip laserSound;

    List<Cheat> cheats;
    Cheat cheat0;
    Cheat cheat1;
    Cheat cheat2;
    Cheat cheat3;
    Cheat cheat4;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //initialize cheats here and put them into cheats list
        cheats = new List<Cheat>();

        cheat0 = new Cheat("ninja", ninjaCheat);
        cheat1 = new Cheat("doge", dogeCheat);
        cheat2 = new Cheat("squidgame", squidGameCheat);
        cheat3 = new Cheat("boom", explosionCheat);
        cheat4 = new Cheat("pew", laserCheat);
     


        cheats.Add(cheat0);
        cheats.Add(cheat1);
        cheats.Add(cheat2);
        cheats.Add(cheat3);
        cheats.Add(cheat4);
        //done with cheats initialization
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            char currentKey = Input.inputString.ToLower().ToCharArray()[0];
            foreach (Cheat cheat in cheats)
            {
                cheat.checkInputKey(currentKey);
            }
        }

        
        
    }

    public int ninjaCheat(bool isActive)
    {
        // enable or disable ninja abilities 
        if(isActive)
        {
            Manager.instance.displayMessage("You are a Ninja now");
            Manager.instance.adjustPlayerSpeed(0.5f);
            player.color = new Color(1f, 1f, 1f, .5f);
            return 1; 
        }
        else
        {
            Manager.instance.displayMessage("You are no longer a Ninja");
            Manager.instance.adjustPlayerSpeed(1f);
            player.color = new Color(1f, 1f, 1f, 1f);
            return 1;
        }
    }

    public int dogeCheat(bool isActive)
    {
        if(isActive)
        {
            Manager.instance.displayMessage("What the dogs doin?");
            for(int i = 0; i < Manager.instance.npcArr.Count; i++)
            {
                SpriteRenderer currentSr = Manager.instance.npcArr[i].GetComponent<SpriteRenderer>();
                currentSr.sprite = dogeSprite;
            }
            return 1;
        }
        else
        {
            Manager.instance.displayMessage("No one will ever know what the dogs where doin");
            for (int i = 0; i < Manager.instance.npcArr.Count; i++)
            {
                SpriteRenderer currentSr = Manager.instance.npcArr[i].GetComponent<SpriteRenderer>();
                Sprite defaultSprite = Manager.instance.defaultNpcSprites[i];

                currentSr.sprite = defaultSprite;
            }
            return 1;
        }
    }

    public int squidGameCheat(bool isActive)
    {
        if (isActive)
        {
            Manager.instance.displayMessage("SquidGame activated");
            Manager.instance.isSquidGame = true;
            return 1;
        }
        else
        {
            Manager.instance.displayMessage("The Squidgames are over");
            Manager.instance.isSquidGame = false;
            return 1;
        }
    }

    public int explosionCheat(bool isActive)
    {
        Manager.instance.displayMessage("BOOOM");
        StartCoroutine(explosions());
        return 1;
    }

    public int laserCheat(bool isActive)
    {
        StartCoroutine(laser());
        return 1;
    }
    IEnumerator explosions()
    {
        int n = 10;
        while(n > 0)
        {
            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);

            Vector2 pos = cam.ViewportToWorldPoint(new Vector2(x, y));
            int rand = Random.Range(0, explosionsArr.Length);
            AudioManager.instance.playExplosion(pos, explosionsArr[rand]);
            n--;
            yield return new WaitForSeconds(0.45f);
        }
    }

    IEnumerator laser()
    {

        if (Manager.instance.npcArr.Count > 0)
        {
            int rand = Random.Range(0, Manager.instance.npcArr.Count);
            GameObject target = Manager.instance.npcArr[rand];

            // get start position
            Vector2 currentPos = player.transform.position;

            // create projectile

            GameObject projectile = new GameObject("Laser");
            SpriteRenderer projectileSr = projectile.AddComponent<SpriteRenderer>();
            projectileSr.sortingOrder = 100;
            AudioSource projectileAudio = projectile.AddComponent<AudioSource>();
            projectileAudio.clip = laserSound;
            projectileAudio.Play();


            // set sprite
            projectileSr.sprite = projectileSprite;

            // set position
            projectile.transform.position = currentPos;


            // rotate projectile to face target
            Vector2 worldPosition = target.transform.position;
            Vector2 direction = (worldPosition - (Vector2)projectile.transform.position).normalized;
            projectile.transform.right = direction;
            projectile.transform.Rotate(0, 0, 90);


            // let the sprite move to target
            while (true)
            {
                Vector2 flyingDirection = Vector2.MoveTowards(projectile.transform.position, target.transform.position, 10 * Time.deltaTime);
                projectile.transform.position = flyingDirection;

                if (Vector3.Distance(projectile.transform.position, target.transform.position) < 0.1f)
                {
                    // if hit then let target explode
                    Manager.instance.displayMessage(target.name + " got pew pew'd");
                    Manager.instance.npcArr.Remove(target);
                    AudioManager.instance.playExplosion(target.transform.position);
                    Destroy(target);
                    Destroy(projectile);
                    break;
                }
                yield return null;

            }

            Debug.Log("HIT" + target.name); 
        }



    }

}
