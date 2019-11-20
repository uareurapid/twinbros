using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shootEffect;
    public Transform hitEffect;

    public bool isElectric = false;
    public bool isExplosive = false;
    public bool isBloody = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if is normal enemy destroy it?
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            if (isElectric)
            {
                player.ShowElectrocutedSpriteAnimation();
                SpecialEffectsHelper.Instance.PlayElectricityEffect(player.transform.position);
            }
        }
        
        Destroy(gameObject); //destroy the shot //avoid collid with the gun itself

    }
}
