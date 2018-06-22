using UnityEngine;

namespace Venture
{
	public class Document : MonoBehaviour
	{
		public static Document Instance;
		public GameObject Canvas;
		public GameObject SignIn;
		public GameObject CharacterCreation;
		public GameObject Settings;
		public GameObject Character;

		GameObject current;

		void Awake()
		{
			//Singleton
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}

		public void Open(GameObject document)
		{
			current = Instantiate(document, Canvas.transform);
			IO.Instance.Console = current.GetComponentInChildren<Console>();
			current.GetComponent<Animator>().SetTrigger("open");
		}

		public void Submit()
		{
			current.GetComponent<Animator>().SetTrigger("submit");
		}

		public void Discard()
		{
			current.GetComponent<Animator>().SetTrigger("discard");
		}
	}
}