using UnityEngine;
using UnityEngine.UI;

namespace Venture.Prefabs.Documents
{
	public class SignIn : MonoBehaviour
	{
		private Button button_SignIn;

		void Awake()
		{
			button_SignIn = transform.Find("Content").Find("SignInButton").GetComponent<Button>();
			button_SignIn.onClick.AddListener(async () =>
			{
					gameObject.GetComponentInChildren<Text>().text = "Signing In...";
					gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
					Document.Instance.Submit();
					await User.Instance.SignIn();
			});
		}

		void Start()
		{
			button_SignIn.onClick.Invoke();
		}
	}
}
