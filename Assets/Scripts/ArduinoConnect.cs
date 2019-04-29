using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoConnect : MonoBehaviour {

    GameObject upper;
    GameObject lower;
    Vector3 upRot;
    Vector3 lowRot;
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

        string[] temp = readVal.Split('-');

        Debug.Log(readVal);

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

}
