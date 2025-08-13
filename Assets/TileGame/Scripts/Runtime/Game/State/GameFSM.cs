using NUtilities.FSM;
using TileGame.Game.Controller;
using UnityEngine;

namespace TileGame.Game.State
{
    public class GameFSM : StateMachine<GameController>
    {
        public GameFSM(GameController owner) : base(owner)
        {
        }
    }
    
    public class GameFSMState : State<GameController>
    {
        public GameFSMState(string name) : base(name)
        {
        }
    }
}