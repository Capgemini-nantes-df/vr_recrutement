using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PandoraBotUnity : MonoBehaviour {

    public GameObject loadLogo;
    public Text textBotResponse;
    public InputField inputAskQuestion;
    public string botid = "b0dafd24ee35a477";

    protected string text;
    protected bool waiting;

    protected string custid;

    // Use this for initialization
    void Start()
    {
        text = "";
        waiting = false;

        custid = System.Convert.ToString(Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
        {
            SendText();
        }

        if (loadLogo)
        {
            if (waiting)
            {
                loadLogo.GetComponent<SpriteRenderer>().enabled = true;

                loadLogo.transform.Rotate(new Vector3(0, 0, -2) * Time.deltaTime * 100);
            }
            else
            {
                loadLogo.GetComponent<Renderer>().enabled = false;
            }
        }

    }

    public void SendText()
    {
        inputAskQuestion.Select();
        text = inputAskQuestion.text;
        Debug.Log("Send text : " + text);
        inputAskQuestion.Clear();
        StartCoroutine(PandoraBotRequestCoRoutine(text));
        text = "";
        
    }

   

    string sanitizePandoraResponse(string wwwText)
    {
        string responseString = "";
        while (wwwText.IndexOf("<that>") >= 0)
        {
            int startIndex = wwwText.IndexOf("<that>") + 6;
            int endIndex = wwwText.IndexOf("</that>");
            responseString = responseString + wwwText.Substring(startIndex, endIndex - startIndex);

            wwwText = wwwText.Substring(endIndex);
        }
        while (responseString.IndexOf("&lt;") >= 0)
        {
            int startIndex = responseString.IndexOf("&lt;");
            int endIndex = responseString.IndexOf("&gt;") + 4;
            responseString = responseString.Remove(startIndex, endIndex - startIndex);
        }

        Debug.Log("Sanitized response: " + responseString);
        textBotResponse.text = responseString;
        return responseString;
    }


    private IEnumerator PandoraBotRequestCoRoutine(string text)
    {
        waiting = true;
        textBotResponse.text = "... Wait for response ...";
        string url = "http://www.pandorabots.com/pandora/talk-xml?botid=" + botid;
        url = url + "&input=" + WWW.EscapeURL(text);
        url = url + "&custid=" + custid;
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;

        if (www.error == null)
        {
            Debug.Log(www.text);
            PandoraBotResponseHandler(www.text);
        }
        else
        {
            Debug.LogWarning(www.error);
            //PandoraBotResponseHandler("<result><input>Hi</input><that>Bonjour !</that></result>");
        }
    }
    void PandoraBotResponseHandler(string wwwText)
    {

        string responseString = sanitizePandoraResponse(wwwText);
        waiting = false;
    }

}

public static class Extension
{
    public static void Clear(this InputField inputfield)
    {
        inputfield.Select();
        inputfield.text = "";
    }
}
