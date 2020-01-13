using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTeleport : MonoBehaviour, HandlePlayerCollision
{

    public SimpleTeleport destination;
    public float transportationDuration = 1.5f; //how much time to get to the other point

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
        if(player!=null && !player.GetIsMovingBetweenLevels())
        {
            HandleCollision(player);
        }
    }

    public void HandleExitCollision(PlayerMovement player)
    {

    }
    public void HandleCollision(PlayerMovement player)
    {

        if (destination != null)
        {

            player.SetIsMovingBetweenTeleportPoints(true);
            player.StopMovementVelocity();
            player.SetReachedTarget(true);
            player.HideSprites();
            player.transform.position = destination.transform.position;
           

            StartCoroutine(ReappearOnDestination(player));
        }
    }

    //re-appear on the other end point
    IEnumerator ReappearOnDestination(PlayerMovement player)
    {
        yield return new WaitForSeconds(transportationDuration);
        player.ShowSprites();
        SimpleMovementRestrictions checkPoint = destination.GetComponent<SimpleMovementRestrictions>();
        if(checkPoint!=null)
        {
            player.ApplyMovementRestrictions(checkPoint);
        }

        player.SetIsMovingBetweenTeleportPoints(false);

    }
}