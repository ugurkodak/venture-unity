using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;

namespace Venture
{
    // Contains references to letters
    public class ManagerLetter : MonoBehaviour
    {
        ObservableCollection<Letter> queue;

        public Canvas Canvas;
        public Letter LetterSignIn;
        public Letter LetterRegister;
        //TODO
        //public Letter Settings;
        //public Letter Character;

        void Awake()
        {
            queue = new ObservableCollection<Letter>();
        }

        public async Task Open(Letter letter)
        {
            int index = queue.IndexOf(letter);
            if (index == -1)
            {
                // New letter
                Letter ltr = Instantiate(letter, Canvas.transform);
                queue.Add(ltr);
                await ltr.Open();
            }
            else
            {
                // Open existing letter and move to last index
                await letter.Open();
                letter.gameObject.SetActive(true);
                queue.Move(index, queue.Count - 1);
            }
        }

        //TODO
        //public async Task Queue();
        //public async Task Postpone();

        public async Task Submit(Letter letter)
        {
            await letter.Submit();
            queue.Remove(letter);
            Destroy(letter.gameObject);
        }

        public async Task Discard(Letter letter)
        {
            await letter.Discard();
            queue.Remove(letter);
            Destroy(letter.gameObject);
        }
    }
}