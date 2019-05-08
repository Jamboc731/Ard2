using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoConnect : MonoBehaviour {

    [Range(0, 1)]
    [SerializeField] float smooth;
    [SerializeField] GameObject upper;
    [SerializeField] GameObject lower;
    Vector3 upRot;
    Vector3 lowRot;
    Vector3 upLast;
    Vector3 lowLast;
    public bool playing = true;
    public int commPort = 0;

    string readVal;

    private SerialPort serial;
    private Quaternion qupLast;

    private void Start()
    {
        //start the serial connection with the arduino 
        serial = new SerialPort("\\\\.\\COM" + commPort, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
        //write to the arduino to start it
        WriteToArduino("s");
    }

    private void Update()
    {
        //if the player presses r then reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetButton();
        }

        //get the positions from the arduino
        WriteToArduino("p");

        //change the positions read into to quaternions and apply them to the cubes
        readVal = ReadFromArduino();
        string[] tempa = readVal.Split('a');
        Quaternion tempb = qupLast;
        tempb = new Quaternion(float.Parse(tempa[0].Split(',')[0]), float.Parse(tempa[0].Split(',')[1]), float.Parse(tempa[0].Split(',')[2]), float.Parse(tempa[0].Split(',')[3]));
        upper.transform.rotation = Quaternion.Slerp(upper.transform.rotation, tempb, smooth);
        tempb = new Quaternion(float.Parse(tempa[1].Split(',')[0]), float.Parse(tempa[1].Split(',')[1]), float.Parse(tempa[1].Split(',')[2]), float.Parse(tempa[1].Split(',')[3]));
        lower.transform.rotation = Quaternion.Slerp(lower.transform.rotation, tempb, smooth);
        qupLast = tempb;
        

    }

    public void WriteToArduino(string msg)
    {
        //write a message to the arduino
        serial.WriteLine(msg);
        serial.BaseStream.Flush();
    }

    string ReadFromArduino()
    {
        //read from the arduino
        serial.ReadTimeout = 50;
        try
        {
            return serial.ReadLine();
        }
        catch(TimeoutException e)
        {
            return null;
        }
    }

    private void OnDestroy()
    {
        //close the serial when the game ends
        WriteToArduino("r");
        StopAllCoroutines();
        serial.Close();
    }

    public void ResetButton()
    {
        //start the reset coroutine
        StartCoroutine(Reset()); 
    }

    IEnumerator Reset()
    {
        //reset the arduino by closing and opening the serial port. this forces the sketch to restart
        Debug.Log("reset");
        serial.Close();
        Debug.Log("closed");
        yield return new WaitForSeconds(0.25f);
        serial.Open();
        Debug.Log("open");

    }

}
