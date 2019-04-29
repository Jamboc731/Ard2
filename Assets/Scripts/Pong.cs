using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong : MonoBehaviour {

    public int force;
    public Manager man;
    public ArduinoConnect ard;
    Rigidbody rb;
    [SerializeField] GameObject but;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        but.SetActive(false);
        Launch();
	}

    private void Launch()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 5, 0);
        rb.AddForce(new Vector3(1, Random.Range(-0.9f, 0.9f), 0) * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity *= 1.05f;
        }

        if (collision.gameObject.CompareTag("Left"))
        {
            man.scoreTwo++;
            if(man.scoreTwo == 1)
            {
                Launch();
                ard.WriteToArduino("f");
            }
            else if (man.scoreTwo == 2)
            {
                ard.WriteToArduino("d");
                Win(2);
            }
        }
        if (collision.gameObject.CompareTag("Right"))
        {
            man.scoreOne++;
            if (man.scoreOne == 1)
            {
                Launch();
                ard.WriteToArduino("a");
            }
            else if (man.scoreOne == 2)
            {
                ard.WriteToArduino("s");
                Win(1);
            }
        }
    }
    
    private void Win(int winner)
    {
        if (winner == 1)
        {
            StartCoroutine(FlashP1());
        }
        else if (winner == 2)
        {
            StartCoroutine(FlashP2());
        }
        rb.velocity = Vector3.zero;
        but.SetActive(true);
    }

    IEnumerator FlashP1()
    {
        while (true)
        {
            ard.WriteToArduino("a");
            ard.WriteToArduino("x");
            yield return new WaitForSeconds(0.25f);
            ard.WriteToArduino("z");
            ard.WriteToArduino("s");
        }
    }

    IEnumerator FlashP2()
    {
        while (true)
        {
            ard.WriteToArduino("f");
            ard.WriteToArduino("c");
            yield return new WaitForSeconds(0.25f);
            ard.WriteToArduino("v");
            ard.WriteToArduino("d");
        }
    }

}
