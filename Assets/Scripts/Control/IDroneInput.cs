using UnityEngine;

namespace Drone.Control {
    public interface IDroneInput {
        float Pitch { get; }
        float Roll { get; }
        float Yaw { get; }
        float Throttle { get; }
        bool AutoHover { get; }
        bool ControlDisabled { get; set; }
        void StopAndDisable();
    }
}


