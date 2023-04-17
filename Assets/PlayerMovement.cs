using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class DanceMove
{

    public string[] moves;
    public float[] durations;
    public float[] speeds;
    public int size;

    public DanceMove(string[] moves, float[] durations)
    {
        if (moves.Length == durations.Length)
        {
            this.size = moves.Length;
            this.moves = moves;
            this.durations = durations;
            this.speeds = Enumerable.Repeat(1f, this.size).ToArray();
        }
        else
        {
            this.size = -1;
            this.moves = null;
            this.durations = null;
            this.speeds = null;
        }
    }

    public DanceMove(string[] moves, float[] durations, float[] speeds)
    {
        if (moves.Length == durations.Length && moves.Length == speeds.Length)
        {
            this.size = moves.Length;
            this.moves = moves;
            this.durations = durations;
            this.speeds = speeds;
        }
        else
        {
            this.size = -1;
            this.moves = null;
            this.durations = null;
            this.speeds = null;
        }
    }

    public DanceMove(string[] moves)
    {

            this.size = moves.Length;
            this.moves = moves;
            this.durations = Enumerable.Repeat(1f, this.size).ToArray();
            this.speeds = Enumerable.Repeat(1f, this.size).ToArray();
    }

    public DanceMove(string[] moves, float durations, float speeds)
    {

        this.size = moves.Length;
        this.moves = moves;
        this.durations = Enumerable.Repeat(durations, this.size).ToArray();
        this.speeds = Enumerable.Repeat(speeds, this.size).ToArray();
    }


}
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer backgroundSr;

    SpriteRenderer spriteRenderer;
    bool isDancing;
    bool isSuperDance;
    DanceMove dance0;
    DanceMove dance1;
    DanceMove superDance0;
    DanceMove superDance1;



    // Start is called before the first frame update
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        isDancing = false;
        isSuperDance = false;

        dance0 = new DanceMove(
            new string[] { "up", "left", "right", "down" },
            new float[] { 1f, 1f, 1f, 1f },
            new float[] {1f,4f,4f,1f}
            );

        dance1 = new DanceMove(
            new string[] {"left", "right", "left", "right", "left", "right", "left", "right"},
            0.2f,
            5f
            );

        superDance0 = new DanceMove(
            new string[] { "up", "down", "up", "down" },
            new float[] { 1f, 1f, 1f, 1f },
            new float[] { 1f, 4f, 4f, 1f }
            );

        superDance1 = new DanceMove(
            new string[] { "left", "right", "left", "right", "left", "right", "left", "right" },
            0.375f,
            5f
            );

    }

    // Update is called once per frame
    void Update()
    {
        //Move
        if (!isDancing)
        {
            Move("auto");
        }

        if (Input.GetKeyDown(KeyCode.Keypad0) && !isDancing)
        {
            //Throw some sick moves
            StartCoroutine(dance(dance1));
            StartCoroutine(dance(dance0));

        }
        if(Input.GetKeyDown(KeyCode.Keypad8) && !isDancing)
        {
            //Do the super dance
            isSuperDance = true;
            StartCoroutine(dance(superDance0));
            StartCoroutine(dance(superDance1));
        }

        
    }
    IEnumerator dance(DanceMove danceMove)
    {
        //keep track of nr of dancemoves and the duration per move
        float elapsedTime = 0f;
        int currentIndex = 0;
        isDancing = true;

        //dimm lights if superdancing
        if (isSuperDance)
        {
            backgroundSr.enabled = false;
           
        }
        //iterate over all moves and call move function with corresponding instruction
        if(danceMove.size >= 0)
        {
            while(currentIndex < danceMove.size)
            {
                //make move at current index
                while(elapsedTime < danceMove.durations[currentIndex])
                {
                    Move(danceMove.moves[currentIndex], danceMove.speeds[currentIndex]);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                elapsedTime = 0f;
                currentIndex++;
                yield return null;
            }
        }
        isSuperDance = false;
        isDancing = false;       
    }
 



    private void Move(string direction, float Speed = 1f)
    {
        Vector3 moveVector = Vector3.zero;

        if (direction == "auto")
        {
            // Get input and save state in moveVector
            if (Input.GetKey(KeyCode.W)) moveVector.y = 1;
            if (Input.GetKey(KeyCode.A)) moveVector.x = -1;
            if (Input.GetKey(KeyCode.S)) moveVector.y = -1;
            if (Input.GetKey(KeyCode.D)) moveVector.x = 1; 
        }
        else if(direction == "left")
        {
            moveVector.x = -1;
        }
        else if(direction == "right")
        {
            moveVector.x = 1;
        } 
        else if(direction == "up")
        {
            moveVector.y = 1;
        } 
        else if(direction == "down")
        {
            moveVector.y = -1;
        }

        // Normalize vector, so that magnitude for diagonal movement is also 1
        moveVector.Normalize();

        // Frame rate independent movement
        transform.position += Time.deltaTime * moveVector * Speed;
        // Flip the sprite if facing to the left
        if (moveVector.x > 0)
            spriteRenderer.flipX = true;
        else if (moveVector.x < 0)
            spriteRenderer.flipX = false;
    }


    
}


