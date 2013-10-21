using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect.Toolkit.Interaction;

namespace ClownSchool {
    class InteractionClient : IInteractionClient {
        public InteractionInfo GetInteractionInfoAtLocation(int skeletonTrackingId, InteractionHandType handType, double x, double y) {
            var info = new InteractionInfo();
            info.IsGripTarget = true;
            info.IsPressTarget = false;
            info.PressAttractionPointX = 0f;
            info.PressAttractionPointY = 0f;
            info.PressTargetControlId = 0;

            return info;
        }
    }
}
