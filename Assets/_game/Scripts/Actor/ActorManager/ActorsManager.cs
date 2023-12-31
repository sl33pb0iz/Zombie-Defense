using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ActorsManager : MonoBehaviour
    {
        public List<Actor> Actors = new List<Actor>();
        public GameObject Player { get; private set; }
        public void SetPlayer(GameObject player) => Player = player;
        public GameObject GetPlayer() { return Player; }
    }
}
