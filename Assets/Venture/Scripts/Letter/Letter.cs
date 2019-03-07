using System.Threading.Tasks;
using UnityEngine;
using Venture.Data;

namespace Venture
{
    public abstract class Letter : MonoBehaviour
    {
        protected GameData data;
        protected ManagerLetter manager;

        void Start()
        {
            data = Game.Instance.Data;
            manager = Game.Instance.ManagerLetter;
        }

        // Functions to define additional behavior(mainly animation) to letters.
        public virtual Task Open() { return Task.FromResult(default(object)); }
        public virtual Task Queue() { return Task.FromResult(default(object)); }
        public virtual Task Postpone() { return Task.FromResult(default(object)); }
        public virtual Task Submit() { return Task.FromResult(default(object)); }
        public virtual Task Discard() { return Task.FromResult(default(object)); }
    }
}