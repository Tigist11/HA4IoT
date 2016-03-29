﻿using Windows.Data.Json;
using HA4IoT.Contracts.Api;
using HA4IoT.Contracts.Automations;
using HA4IoT.Contracts.Core;
using HA4IoT.Contracts.Core.Settings;

namespace HA4IoT.Contracts.Areas
{
    public interface IArea : IAutomationController, IActuatorController
    {
        AreaId Id { get; }

        ISettingsContainer Settings { get; }

        IController Controller { get; }

        JsonObject ExportConfigurationToJsonObject();

        void ExposeToApi(IApiController apiController);
    }
}
