using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidWrapper : MonoBehaviour {

    // android object
    private AndroidJavaObject AndroidObject = null;
    // text 
    public Text Message;

    private AndroidJavaObject GetJavaObject()
    {
        if (AndroidObject == null)
        {
            AndroidObject = new AndroidJavaObject("club.etain.blelibrary.BLEControl");
        }
        return AndroidObject;
    }

    // Use this for initialization
    void Start()
    {
        // Retrieve current Android Activity from the Unity Player
        AndroidJavaClass jclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jclass.GetStatic<AndroidJavaObject>("currentActivity");

        // Pass reference to the current Activity into the native plugin,
        // using the 'setActivity' method that we defined in the ImageTargetLogger Java class
        GetJavaObject().Call("setActivity", activity);

        Message.text = GetJavaObject().Call<string>("init");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
