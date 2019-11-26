using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class API : MonoBehaviour
{
    private const string URL = "http://142.55.32.86:50171/CapstoneWeb/api/users/1";
    public Text rtext;

    [System.Obsolete]
    public void Start()
    {
        WWW request = new WWW(URL);
        StartCoroutine(OnResponse(request));
    }

    [System.Obsolete]
    private IEnumerator OnResponse(WWW req)
    {
        yield return req;

        rtext.text = req.text.Replace("\"","");
    }

}
