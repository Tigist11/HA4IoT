﻿using System;
using System.Linq;
using HA4IoT.Contracts.Actuators;
using HA4IoT.Contracts.Hardware;
using HA4IoT.Contracts.Notifications;
using HA4IoT.Networking;

namespace HA4IoT.Actuators
{
    public class BinaryStateOutputActuator : BinaryStateOutputActuatorBase
    {
        private readonly IBinaryOutput _output;

        public BinaryStateOutputActuator(string id, IBinaryOutput output, IHttpRequestController httpRequestController,
            INotificationHandler notificationHandler) : base(id, httpRequestController, notificationHandler)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            _output = output;
            SetStateInternal(BinaryActuatorState.Off, new ForceUpdateStateParameter());
        }
    
        protected override void SetStateInternal(BinaryActuatorState newState, params IParameter[] parameters)
        {
            BinaryActuatorState oldState = GetState();
            bool stateHasChanged = newState != oldState;

            bool commit = !parameters.Any(p => p is DoNotCommitStateParameter);
            _output.Write(newState == BinaryActuatorState.On ? BinaryState.High : BinaryState.Low, commit);

            bool forceUpdate = parameters.Any(p => p is ForceUpdateStateParameter);
            if (forceUpdate || stateHasChanged)
            {
                NotificationHandler.Info(Id + ": " + oldState + "->" + newState);
                OnStateChanged(oldState, newState);
            }
        }

        protected override BinaryActuatorState GetStateInternal()
        {
            return _output.Read() == BinaryState.High ? BinaryActuatorState.On : BinaryActuatorState.Off;
        }
    }
}