using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoConnect : MonoBehaviour {

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

    private void Start()
    {
        serial = new SerialPort("\\\\.\\COM" + commPort, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
        WriteToArduino("s");
    }

    private void Update()
    {
        WriteToArduino("p");

        readVal = ReadFromArduino();
        //Debug.Log(readVal);
        string[] tempa = readVal.Split('a');
        Vector3 tempb = upLast;
        if (!tempa[0].Contains("n"))
        {
            tempb = new Vector3(float.Parse(tempa[0].Split(',')[0]), float.Parse(tempa[0].Split(',')[1]), float.Parse(tempa[0].Split(',')[2]));
            upLast = tempb;
        }
        //else Debug.Log(tempa[0]);
        Debug.Log(tempb);
        upper.transform.localEulerAngles = tempb;

    }

    public void WriteToArduino(string msg)
    {
        serial.WriteLine(msg);
        serial.BaseStream.Flush();
    }

    string ReadFromArduino()
    {
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
        WriteToArduino("r");
        StopAllCoroutines();
        serial.Close();
    }

    public void ResetButton()
    {
        Debug.Log("reset");
        serial.Close();
        Debug.Log("closed");
        serial.Open();
        Debug.Log("open");
    }

    void PositionObjects()
    {

    }

    void PositionObject(Vector3 rot)
    {

    }

}
