using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameStartedEvent : IGameEvent { }

    public class GameEndedEvent : IGameEvent { }

    public class ScoreTimerChangedEvent : IGameEvent
    {
        public float ScoreTimer { get; set; }

        public ScoreTimerChangedEvent(int time)
        {
            ScoreTimer = time;
        }
    }
}
