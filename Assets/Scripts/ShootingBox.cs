using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBox : MonoBehaviour, HandlePlayerCollision
{

    public string shootDirection = GameConstants.SHOOT_DIRECTION_LEFT;

    public float shootCoolDown = 1f; //minimum interval between shoots

    public float nextShoot = 1f; //in secs

    public bool isAutomatic = false;

    public Transform shootPrefab;
    // Use this for initialization
    void Start()
    {

        if(isAutomatic)
        {
            InvokeRepeating("Shoot", shootCoolDown, nextShoot + shootCoolDown);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Shoot()
    {

        if (shootDirection.Equals(GameConstants.SHOOT_DIRECTION_LEFT) && Time.time > nextShoot)
        {
            DoShoot(Vector2.left);
        }
        else if (shootDirection.Equals(GameConstants.SHOOT_DIRECTION_RIGHT) && Time.time > nextShoot)
        {
            DoShoot(Vector2.right);
        }
        else if (shootDirection.Equals(GameConstants.SHOOT_DIRECTION_UP) && Time.time > nextShoot)
        {
            DoShoot(Vector2.up);
        }
        else if (shootDirection.Equals(GameConstants.SHOOT_DIRECTION_DOWN) && Time.time > nextShoot)
        {
            DoShoot(Vector2.down);
        }
    }

    private void DoShoot(Vector2 direction)
    {
        nextShoot = Time.time + shootCoolDown;
        Transform shoot = Instantiate(shootPrefab, transform.position, transform.rotation);
        shoot.GetComponent<Mover>().StartMoving(direction);
    }

    public void HandleExitCollision(PlayerMovement player)
    {

    }

    public void HandleCollision(PlayerMovement player)
    {
        if (!isAutomatic)
        {
            Shoot();
        }
    }
}