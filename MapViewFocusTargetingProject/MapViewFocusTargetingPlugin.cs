using BepInEx;
using HarmonyLib;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Game;
using SpaceWarp.API.Game.Extensions;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using UnityEngine;
using KSP.Game;
using KSP.Sim.impl;
using KSP.Map;
using LibNoise;
using UnityEngine.UIElements;

namespace MapViewFocusTargeting;

[BepInPlugin("com.github.scarecrow71.MapViewFocusTargeting", "MapViewFocusTargeting", "1.0.0")]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]

public class MapViewFocusTargetingPlugin : BaseSpaceWarpPlugin
{
    // These are useful in case some other mod wants to add a dependency to this one
    public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    public const string ModName = MyPluginInfo.PLUGIN_NAME;
    public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

    public static MapViewFocusTargetingPlugin Instance { get; set; }

    private GameInstance game;
    private MapItem mapBody;
    private MapItem mapVessel;
    private VesselComponent vessel;

    private const float NOTIFICATION_DURATION = 5.0f;

    private int selectedCelestialBody;
    private string[] allCelestialBodies = new string[]
    {
        "Kerbol",
        "Moho",
        "Eve",
        "Gilly",
        "Kerbin",
        "Mun",
        "Minmus",
        "Duna",
        "Ike",
        "Dres",
        "Jool",
        "Laythe",
        "Vall",
        "Tylo",
        "Bop",
        "Pol",
        "Eeloo"
    };

    /// Runs when the mod is first initialized.
    public override void OnInitialized()
    {
        base.OnInitialized();

        Instance = this;
        game = GameManager.Instance.Game;
        selectedCelestialBody = -1;

        // Attempt to get the current active vessel.  If there is an active vessel, set throttle to 0 and capture the active vessel
        try
        {
            var currentVessel = Vehicle.ActiveVesselVehicle;
            
            if (currentVessel != null)
            {
                currentVessel.SetMainThrottle(0f);
                vessel = game.ViewController.GetActiveVehicle(true)?.GetSimVessel(true);
            }
        }
        catch (Exception e) {}
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            FocusOnCurrentVessel();
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            RemoveTarget();
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            SetTarget();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            ClearFocus();
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            FocusOnCelestialBody(selectedCelestialBody - 1);
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            FocusOnCelestialBody(selectedCelestialBody + 1);
        }
    }


    void SendAlertNotification(string title, string message)
    {
        NotificationData notificationData = new NotificationData
        {
            Tier = NotificationTier.Alert,
            Importance = NotificationImportance.Medium,
            AlertTitle =
            {
                LocKey = title
            },
            FirstLine =
            {
                LocKey = message
            },
            IsTimerActive = true,
            TimerDuration = NOTIFICATION_DURATION,
            TimeStamp = DateTime.Now.ToOADate()
        };

        game.Notifications.ProcessNotification(notificationData);
    }
    void FocusOnCurrentVessel()
    {
        try
        {
            mapVessel = new MapItem(vessel.GlobalId, MapItemType.Vessel);
            Utils.MPSetFocusedMapItem(mapVessel);
            selectedCelestialBody = -1;
            SendAlertNotification("Focus Change", "The focus has been set to " + vessel.Name);
        }
        catch (Exception e) { }
    }

    void RemoveTarget()
    {
        try
        {
            if (selectedCelestialBody > 0)
            {
                vessel.ClearTarget();
                SendAlertNotification("Target Removed", vessel.Name + " is no longer targeting " + allCelestialBodies[selectedCelestialBody]);
            }
        }
        catch (Exception e) { }
    }

    void SetTarget()
    {
        if (selectedCelestialBody > 0)
        {
            try
            {
                var focusedBody = game.ViewController.GetBodyByName(allCelestialBodies[selectedCelestialBody]);
                vessel.SetTargetByID(focusedBody.GlobalId);
                SendAlertNotification("Target Change", "The Target has been set to " + focusedBody.Name);
            }
            catch (Exception e) { }
        }
    }

    void ClearFocus()
    {
        try
        {
            var currentVessel = Vehicle.ActiveVesselVehicle;

            if (currentVessel != null)
            {
                vessel = game.ViewController.GetActiveVehicle(true)?.GetSimVessel(true);
                selectedCelestialBody = -1;
            }
        }
        catch (Exception e) { }
        // return;
    }

    void FocusOnCelestialBody(int targetBody)
    {
        selectedCelestialBody = targetBody;
        if (selectedCelestialBody < 0)
        {
            selectedCelestialBody = allCelestialBodies.Length - 1;
        }
        else if (selectedCelestialBody > allCelestialBodies.Length - 1)
        {
            selectedCelestialBody = 0;
        }

        try
        {
            var focusedBody = game.ViewController.GetBodyByName(allCelestialBodies[selectedCelestialBody]);
            mapBody = new MapItem(focusedBody.GlobalId, MapItemType.CelestialBody);
            Utils.MPSetFocusedMapItem(mapBody);
            SendAlertNotification("Focus Change", "The focus has been set to " + focusedBody.Name);
        }
        catch (Exception e) { }
    }
}
