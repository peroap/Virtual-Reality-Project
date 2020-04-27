using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ThrowBall : MonoBehaviour
{
    /* Description:
     *   
     * Explanation of the name of the script: This script "ThrowBall" was supposed
     * to be attached to the quaffles, so that quaffles where flying towards the
     * player (in the end we decided that the balls could reappear when scored instead
     * of flying towards the player), but used this script so that the Enemies
     * (fire balls) followed the player and kept the original name as ThrowBall.
     * 
     * What it does:
     * 1.) The enemy has a lifetime of 10 seconds. During its lifetime it will
     * always move towards the player's position with "dir". When its lifetime
     * equals zero it will not look for the player anymore and will keep going
     * with the last saved direction towards the player "lastDir".   
     * 2.) If the enemy collides with the player, a haptic feedback in both
     * controllers will be displayed and the players will lose a life (the number
     * of lifes the player has are displayed in the sky over the hoops). If the
     * player looses all lifes, the text "Game Over" will be displayed. And the
     * enemy will reappear in a random position (see below "SetEnemyBack()")   
     *    
     */

    public Transform head; 
    public GameObject enemy;
    public float lifeTime = 10f;
    private Vector3 lastDir;
    public float speed = 0.5f;
    //private float myTime = 0f; finally did not implement this

    private float xPos;
    private float yPos;
    private float zPos;

    public SteamVR_Action_Vibration hapticAction;
    // lives
    public int lives;
    public Text livesText1;
    public Text livesText2;
    public Text winText1;
    public Text winText2;

    void Start()
    {
        xPos = Random.Range(-40, 25);
        yPos = Random.Range(60, 70);
        zPos = Random.Range(-130, -60);
        Vector3 position = new Vector3(xPos, yPos, zPos);

        lives = 3;
        winText1.text = "";
        winText2.text = "";
        InvokeRepeating("Spawn", 5, 5);

    }

    // We wanted to also implement that the Enemy reappeared in several spawns,
    // Button finally chose the fire ball to reappear after 15 seconds.
    /*
    void Spawn()
    {
        // can add Time.deltaTime to instantiate objects with time 
        myTime += Time.deltaTime;
        if (myTime == 5)
        {
            Vector3 position = new Vector3(5, 10, -90);
            Instantiate(enemy, position, Quaternion.identity);
            myTime = 0;
        }
        
        xPos = Random.Range(-40, 25);
        yPos = Random.Range(30, 40);
        zPos = Random.Range(-130, -60);
        Vector3 position = new Vector3(xPos, yPos, zPos);
        Instantiate(enemy, position, Quaternion.identity);
        
    }*/

void Update()
    {
        livesText1.text = "Lives: " + lives.ToString();
        livesText2.text = "Lives: " + lives.ToString();

        if (lives <= 0 & enemy != null)
        {
            winText1.text = "Game Over";
            winText2.text = "Game Over";
            Destroy(enemy);
        }

        if (enemy != null)
        {
            if (lifeTime > 0)
            {
                Vector3 dir = head.transform.position - enemy.transform.position;
                dir.Normalize();
                enemy.transform.position += speed * dir;
                lastDir = dir;

                lifeTime -= Time.deltaTime;
            }
            if (lifeTime <= 0)
            {
                enemy.transform.position += speed * lastDir;
            }
            // If enemy goes out of boundaries, reset its position
            if (enemy.transform.position.y < 0 | enemy.transform.position.y > 100)
            {
                SetEnemyBack();
            }
            if (enemy.transform.position.z > 0 | enemy.transform.position.z < -200)
            {
                SetEnemyBack();
            }
            if (enemy.transform.position.x > 100 | enemy.transform.position.x < -100)
            {
                SetEnemyBack();
            }
            
            float distance = (enemy.transform.position - head.transform.position).sqrMagnitude;
            if (distance < 1)
            {
                if (lives > 0)
                {
                    lives = lives - 1;
                }
                Pulse(2, 75, 250, SteamVR_Input_Sources.RightHand);
                Pulse(2, 75, 250, SteamVR_Input_Sources.LeftHand);
                SetEnemyBack();
            }
        }

        // can add y component to overcome gravity 
        /*if (lives < 0)
         * {
         * winText1.text = "Game Over";
         * winText2.text = "Game Over";
         * Destroy(enemy);
         * }
        */

    }
    void SetEnemyBack()
    {
        // Enemy reappears flying in a random position over the pitch (y-coordinate
        // between 30 and 40)

        xPos = Random.Range(-40, 25);
        yPos = Random.Range(30, 40);
        zPos = Random.Range(-130, -60);
        enemy.transform.position = new Vector3(xPos, yPos, zPos);
        lifeTime = 10f;
    }
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
