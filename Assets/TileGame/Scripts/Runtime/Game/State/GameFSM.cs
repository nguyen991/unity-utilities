using NUtilities.FSM;
using TileGame.Game.Controller;
using TileGame.Game.Manager;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GameFSM : StateMachine<GameManager>
    {
        public GameFSM(GameManager owner) : base(owner)
        {
        }
    }
    
    public class GameFSMState : State<GameManager>
    {
        public GameFSMState(string name) : base(name)
        {
        }
    }
}