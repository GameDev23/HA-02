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
    bool decidedAgainstDancingDuringRedPhase;
    DanceMove dance0;
    DanceMove dance1;
    DanceMove dance2;
    DanceMove testDance;
    DanceMove testDance2;
    DanceMove[] danceMoves;


    // Start is called before the first frame update
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        isDancing = false;
        decidedAgainstDancingDuringRedPhase = false;

        dance0 = new DanceMove(
            new string[] { "up", "left", "right", "down" },
            new float[] { 1f, 1f, 1f, 1f },
            new float[] { 2f, 2f, 2f, 2f }
            );

        dance1 = new DanceMove(
            new string[] { "left", "right", "left", "right", "left", "right", "left", "right" },
            0.2f,
            3f
            );

        dance2 = new DanceMove(
            new string[] { "uprotl", "downrotr", "uprotl", "downrotr", "uprotl", "downrotr", "uprotl", "downrotr" },
            0.2f,
            3f
            );

        testDance = new DanceMove(
            new string[] { "rotr", "rotr", "rotr", "rotr" });
        testDance2 = new DanceMove(
            new string[] { "rotr", "rotr", "rotr", "rotr" });

        danceMoves = new DanceMove[] { dance0, dance1, dance2, testDance, testDance2 };
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDancing && !decidedAgainstDancingDuringRedPhase)
        {
            int chanceToDance = 100;
            if (Manager.instance.isRedPhaseOfSquidGame && Manager.instance.isSquidGame)
                chanceToDance = 10;
            int rand = Random.Range(1, 101);

            // only dance if rand is smaller than the chance to dance   which is 100% is there is no red light
            if (rand < chanceToDance)
            {

                isDancing = true;
                float delay = Random.Range(0f, 3f);
                Invoke(nameof(AiDance), delay);
            }
            else
            {
                decidedAgainstDancingDuringRedPhase = true;
            }

        }
        else if (!Manager.instance.isRedPhaseOfSquidGame)
            decidedAgainstDancingDuringRedPhase = false;
    }


    IEnumerator dance(DanceMove danceMove)
    {
        //keep track of nr of dancemoves and the duration per move
        float elapsedTime = 0f;
        int currentIndex = 0;

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
        isDancing = false;
        yield return null;
        
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
        // if it is squidgame and the npc starts to dance   destroy it
        if (Manager.instance.isSquidGame && Manager.instance.isRedPhaseOfSquidGame)
            Invoke(nameof(destroyGameObject), 1f);
        int rand = Random.Range(0, danceMoves.Length);
        StartCoroutine(dance(danceMoves[rand]));   
    }

    void destroyGameObject()
    {
        string name = this.name;
        AudioManager.instance.playExplosion(gameObject.transform.position);
        Manager.instance.displayMessage(name + " vanished");
        Destroy(gameObject);
    }

}


