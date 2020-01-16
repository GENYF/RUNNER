using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.Collections.Generic;
using System;

public class SerialManager : MonoBehaviour
{
    [Header("[Serial]")]
    public SerialPort nowSerial;
    public char nowChar = ' ';
    public int readTimeOut = 100;

    public int nowPort = 1;
    public bool connected = false;
    public bool isStop = true;

    public int connectedPort = -1;

    public Dropdown portDrop;
    public Toggle connectToggle;
    public Text runTimeText;

    private void Awake()
    {
        portDrop.ClearOptions();
        AddDropDown();
    }

    private void AddDropDown()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        for (int i = 1; i < 20; ++i)
            options.Add(new Dropdown.OptionData("Com " + i));

        portDrop.options = options;
    }

    private void Start()
    {
        nowPort = PlayerPrefs.GetInt("Port", 1);
        portDrop.value = nowPort - 1;

        runTimeText.text = readTimeOut.ToString();

        if (!connected || nowPort != connectedPort)
        {
            try
            {
                nowSerial = new SerialPort("\\\\.\\COM" + nowPort, 115200);
                nowSerial.ReadTimeout = readTimeOut;
                nowSerial.Open();

                connectedPort = nowPort;
            }
            catch { }
        }
        if (nowSerial.IsOpen)
            connected = true;
        else
        {
            connected = false;
            connectedPort = -1;
        }
        connectToggle.isOn = connected;

        StartCoroutine("SerialUpdate");
    }

    public void TryConnect(int port)
    {
        nowPort = port + 1;
        connectedPort = -1;
        PlayerPrefs.SetInt("Port", nowPort);

        if (!connected || nowPort != connectedPort)
        {
            try
            {
                nowSerial = new SerialPort("\\\\.\\COM" + nowPort, 115200);
                nowSerial.ReadTimeout = readTimeOut;
                nowSerial.Open();

                connectedPort = nowPort;
            }
            catch { }
        }
        if (nowSerial.IsOpen)
            connected = true;
        else
        {
            connected = false;
            connectedPort = -1;
        }
        connectToggle.isOn = connected;
    }

    IEnumerator SerialUpdate()
    {
        if (connected && !isStop)
        {
            yield return null;
            try
            {
                char temp = (char)nowSerial.ReadChar();
                nowChar = temp;
            }
            catch { }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("SerialUpdate");
    }

    private void OnApplicationQuit()
    {
        if (nowSerial != null)
            nowSerial.Close();
    }
}
