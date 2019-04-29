using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public Text scoreP1;
    public Text scoreP2;

    public int scoreOne;
    public int scoreTwo;

    private void Update()
    {
        scoreP1.text = scoreOne.ToString();
        scoreP2.text = scoreTwo.ToString();
    }


}
