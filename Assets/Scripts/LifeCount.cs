using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour {

    int lives = 10;
    [SerializeField] Text lifeCount;
    
    //manages the on screen text for the life counter. if the player dies then reset the scene
    public void RemoveLife()
    {
        lives--;
        lifeCount.text = lives.ToString();
        if(lives == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
