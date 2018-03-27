using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google;

public class VentureManager : MonoBehaviour
{
    public static VentureManager instance = null;
    public static GoogleSignInUser user = null;
    public Transform canvas;
    public GameObject[] forms;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = "356906761499-utkt6d8uicpjckqu2ppq980mr4kfjdv7.apps.googleusercontent.com",
            RequestIdToken = true,
            UseGameSignIn = false
        };
        if (user == null)
        {
			//instantiateForm(0);
			Instantiate(forms[0], canvas);
        }
    }

	// void instantiateForm(int index)
	// {
	// 	GameObject form = Instantiate(forms[index]);
    //     form.transform.SetParent(canvas);
	// 	form.GetComponent<RectTransform>().ForceUpdateRectTransforms();
		
	// }

    // void Start()
    // {

    // }

    // bool active = true;

    // void Open()
    // {
    // 	active = true;		
    // 	foreach (Transform child in transform)
    // 		child.gameObject.SetActive(active);
    // }

    // void Done()
    // {
    // 	active = false;
    // 	foreach (Transform child in transform)
    // 		child.gameObject.SetActive(active);
    // }
}