using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] public AudioSource audioSourceBackground;
    [SerializeField] public AudioSource audioSourceExplosion;
    [SerializeField] public SpriteRenderer explosionSr;
    [SerializeField] public Sprite defaultExplosionSprite;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        explosionSr.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.instance.isRedPhaseOfSquidGame && !audioSourceBackground.mute)
        {
            audioSourceBackground.mute = true;
        }
        else if(!Manager.instance.isRedPhaseOfSquidGame && audioSourceBackground.mute)
        {
            audioSourceBackground.mute = false;
        }
    }

    public void playExplosion(Vector3 pos, Sprite ExplosionSprite = null)
    {    
        audioSourceExplosion.Play();
        StartCoroutine(explosion(pos, ExplosionSprite));
    }

    IEnumerator explosion(Vector3 pos, Sprite ExplosionSprite)
    {
        transform.position = pos;
        if (ExplosionSprite != null)
            explosionSr.sprite = ExplosionSprite;
        else
            explosionSr.sprite = defaultExplosionSprite;
        explosionSr.enabled = true;
        yield return new WaitForSeconds(1f);
        explosionSr.enabled = false;
    }
}
