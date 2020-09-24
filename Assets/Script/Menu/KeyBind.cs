using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBind : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text up, down, left, right, jump;

    private GameObject currentKey;
    public Color32 changedKey = new Color32(39, 171, 249, 255);//blue
    public Color32 selectedKey = new Color32(239, 116, 36, 255);//orange

    private void Start()
    {
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        jump.text = keys["Jump"].ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyBind.keys["Up"]))
        {
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(KeyBind.keys["Down"]))
        {
            Debug.Log("Down");
        }
        if (Input.GetKeyDown(KeyBind.keys["Left"]))
        {
            Debug.Log("Left");
        }
        if (Input.GetKeyDown(KeyBind.keys["Right"]))
        {
            Debug.Log("Right");
        }
        if (Input.GetKeyDown(KeyBind.keys["Jump"]))
        {
            Debug.Log("Jump");
        }
    }

    private void OnGUI()
    {
        string newKey = "";
        Event e = Event.current;
        if (currentKey != null)
        {
            if (e.isKey)
            {
                newKey = e.keyCode.ToString();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }

            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }

            //if we have set a key
            if (newKey != "")
            {
                //we change our dictionary (that means our keybind changes too)
                keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                //change the text of our button
                currentKey.GetComponentInChildren<Text>().text = newKey;
                currentKey.GetComponent<Image>().color = changedKey;
                currentKey = null;
                SaveKeys();
            }

        }
    }

    public void ChangeKey(GameObject clickKey)
    {
        currentKey = clickKey;
        if (clickKey != null)
        {
            currentKey.GetComponent<Image>().color = selectedKey;
        }
    }

    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();

    }

}
