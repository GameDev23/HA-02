using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;


public class NPCMovement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool isDancing;
    DanceMove dance0;
    DanceMove dance1;
    DanceMove testDance;
    DanceMove[] danceMoves;


    // Start is called before the first frame update
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        isDancing = false;

        dance0 = new DanceMove(
            new string[] { "up", "left", "right", "down" },
            new float[] { 1f, 1f, 1f, 1f },
            new float[] { 1f, 4f, 4f, 1f }
            );

        dance1 = new DanceMove(
            new string[] { "left", "right", "left", "right", "left", "right", "left", "right" },
            0.2f,
            5f
            );

        testDance = new DanceMove(
            new string[] { "leftleftuprotrrotr", "rightrightrightuprotl", "leftdown", "uprotl", "downdown", "rotr", "rotl" });

        danceMoves = new DanceMove[] { dance0, dance1, testDance };
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDancing)
        {
            float delay = Random.Range(1f, 6f);
            isDancing = true;
            Invoke(nameof(AiDance),delay);
            //Debug.Log("Invoked dance");

        }
    }


    IEnumerator dance(DanceMove danceMove)
    {
        //keep track of nr of dancemoves and the duration per move
        float elapsedTime = 0f;
        int currentIndex = 0;
        isDancing = true;

        //iterate over all moves and call move function with corresponding instruction
        if (danceMove.size >= 0)
        {
            while (currentIndex < danceMove.size)
            {
                //make move at current index
                while (elapsedTime < danceMove.durations[currentIndex])
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
        //Debug.Log("finished dancing");
        isDancing = false;
        
    }

    private void Move(string direction, float Speed = 1f)
    {
        Vector2 rotation = Vector3.zero;
        Vector3 moveVector = Vector3.zero;
        int rotations = 0;

        if (direction == "auto")
        {
            // Get input and save state in moveVector
            if (Input.GetKey(KeyCode.W)) moveVector.y = 1;
            if (Input.GetKey(KeyCode.A)) moveVector.x = -1;
            if (Input.GetKey(KeyCode.S)) moveVector.y = -1;
            if (Input.GetKey(KeyCode.D)) moveVector.x = 1;

            // Normalize vector, so that magnitude for diagonal movement is also 1
            moveVector.Normalize();
        }
        else
        {


            if (direction.Contains("left"))
            {

                moveVector.x -= Regex.Matches(direction, "left").Count;
            }
            if (direction.Contains("right"))
            {

                moveVector.x += Regex.Matches(direction, "right").Count;
            }
            if (direction.Contains("up"))
            {
                moveVector.y += Regex.Matches(direction, "up").Count;
            }
            if (direction.Contains("down"))
            {
                moveVector.y -= Regex.Matches(direction, "down").Count;
            }
            if (direction.Contains("rotl"))
            {
                rotations += Regex.Matches(direction, "rotl").Count;
            }
            if (direction.Contains("rotr"))
            {
                rotations -= Regex.Matches(direction, "rotr").Count;
            }
            if (rotations != 0)
            {
                transform.Rotate(Vector3.forward, rotations * 90 * Time.deltaTime);
            }

        }

        // Frame rate independent movement
        transform.position += Time.deltaTime * moveVector * Speed;

        // Flip the sprite if facing to the left
        if (moveVector.x > 0)
            spriteRenderer.flipX = true;
        else if (moveVector.x < 0)
            spriteRenderer.flipX = false;
    }


    public void AiDance()
    {
        int rand = Random.Range(0, danceMoves.Length);
        StartCoroutine(dance(danceMoves[rand]));     


    }

}


