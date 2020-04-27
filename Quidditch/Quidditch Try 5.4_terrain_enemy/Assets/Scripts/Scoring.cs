using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    // DESCRIPTION: double click the button below to see description
    /* Description:
     * This script is used to account the points when a ball collides with the
     * collider inside a hoop (+10 points) and the ball reappears somewhere
     * random in the pitch.
     * When the player gets to the winCount (by default 30), the text "You Won!"
     * is displayed in the sky.
     */

    public int count;
    public Text countText1;
    public Text countText2;
    public Text winText1;
    public Text winText2;
    public int winCount = 30;

    private float xPos;
    private float yPos;
    private float zPos;

    public GameObject enemy;

    void Start()
    {
        count = 0;
        SetCountText();
        winText1.text = "";
        winText2.text = "";

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hoop"))
        {
            count = count + 10;
            SetCountText();
            SetBallBack();
            DidWin();
        }
    }
    void DidWin()
    {
        if (count >= winCount)
        {
            winText1.text = "You Won!";
            winText2.text = "You Won!";
            Destroy(enemy);
        }

    }
    void SetCountText()
    {
        countText1.text = "Score: " + count.ToString();
        countText2.text = "Score: " + count.ToString();
    }
    void SetBallBack()
    {
        xPos = Random.Range(-40, 25);
        yPos = Random.Range(1, 10);
        zPos = Random.Range(-130, -60);
        transform.position = new Vector3(xPos, yPos, zPos);
       
    }
}
