using UnityEngine;
using UnityEngine.UI;

namespace Venture.Documents
{
	public class SignIn : MonoBehaviour
	{
		Button signInButton;

		void Awake()
		{
			signInButton = transform.Find("Content").Find("SignInButton").GetComponent<Button>();
			signInButton.onClick.AddListener(async () =>
			{
				gameObject.GetComponentInChildren<Text>().text = "Signing In...";
				gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
				Managers.Document.Instance.Submit();
				await Managers.User.Instance.SignIn();
			});
		}

		void Start()
		{
			signInButton.onClick.Invoke();
		}
	}
}
