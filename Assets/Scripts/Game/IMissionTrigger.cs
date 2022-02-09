using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Game {
    public interface IMissionTrigger {
        event Action onMissionStarted;
    }
}


