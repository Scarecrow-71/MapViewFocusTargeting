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
            try
            {
                mapVessel = new MapItem(vessel.GlobalId, MapItemType.Vessel);
                Utils.MPSetFocusedMapItem(mapVessel);
                selectedCelestialBody = -1;

                NotificationData notificationData = new NotificationData
                {
                    Tier = NotificationTier.Alert,
                    Importance = NotificationImportance.Medium,
                    AlertTitle =
                    {
                        LocKey = "Focus Change"
                    },
                    FirstLine =
                    {
                        LocKey = "The focus has been set to " + vessel.Name
                    },
                    IsTimerActive = true,
                    TimerDuration = 5f,
                    TimeStamp = DateTime.Now.ToOADate()
                };

                game.Notifications.ProcessNotification(notificationData);
            }
            catch (Exception e) { }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (selectedCelestialBody > 0)
            {
                try
                {
                    vessel.ClearTarget();

                    NotificationData notificationData = new NotificationData
                    {
                        Tier = NotificationTier.Alert,
                        Importance = NotificationImportance.Medium,
                        AlertTitle =
                    {
                        LocKey = "Target Removed"
                    },
                        FirstLine =
                    {
                        LocKey = vessel.Name + " is no longer targeting a celestial body."
                    },
                        IsTimerActive = true,
                        TimerDuration = 5f,
                        TimeStamp = DateTime.Now.ToOADate()
                    };

                    game.Notifications.ProcessNotification(notificationData);
                }
                catch (Exception e) { }
            }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (selectedCelestialBody > 0)
            {
                try
                {
                    var focusedBody = game.ViewController.GetBodyByName(allCelestialBodies[selectedCelestialBody]);
                    vessel.SetTargetByID(focusedBody.GlobalId);

                    NotificationData notificationData = new NotificationData
                    {
                        Tier = NotificationTier.Alert,
                        Importance = NotificationImportance.Medium,
                        AlertTitle =
                    {
                        LocKey = "Target Change"
                    },
                        FirstLine =
                    {
                        LocKey = "The Target has been set to " + focusedBody.Name
                    },
                        IsTimerActive = true,
                        TimerDuration = 5f,
                        TimeStamp = DateTime.Now.ToOADate()
                    };

                    game.Notifications.ProcessNotification(notificationData);
                }
                catch (Exception e) { }
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
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
            return;
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            selectedCelestialBody = selectedCelestialBody - 1;
            if (selectedCelestialBody < 0)
            {
                selectedCelestialBody = 16;
            }

            try
            {
                var focusedBody = game.ViewController.GetBodyByName(allCelestialBodies[selectedCelestialBody]);
                mapBody = new MapItem(focusedBody.GlobalId, MapItemType.CelestialBody);
                Utils.MPSetFocusedMapItem(mapBody);

                NotificationData notificationData = new NotificationData
                {
                    Tier = NotificationTier.Alert,
                    Importance = NotificationImportance.Medium,
                    AlertTitle =
                    {
                        LocKey = "Focus Change"
                    },
                    FirstLine =
                    {
                        LocKey = "The focus has been set to " + focusedBody.Name
                    },
                    IsTimerActive = true,
                    TimerDuration = 5f,
                    TimeStamp = DateTime.Now.ToOADate()
                };

                game.Notifications.ProcessNotification(notificationData);
            }
            catch (Exception e) { }
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            selectedCelestialBody = selectedCelestialBody + 1;
            if (selectedCelestialBody > 16)
            {
                selectedCelestialBody = 0;
            }

            try
            {
                var focusedBody = game.ViewController.GetBodyByName(allCelestialBodies[selectedCelestialBody]);
                mapBody = new MapItem(focusedBody.GlobalId, MapItemType.CelestialBody);
                Utils.MPSetFocusedMapItem(mapBody);

                NotificationData notificationData = new NotificationData
                {
                    Tier = NotificationTier.Alert,
                    Importance = NotificationImportance.Medium,
                    AlertTitle =
                    {
                        LocKey = "Focus Change"
                    },
                    FirstLine =
                    {
                        LocKey = "The focus has been set to " + focusedBody.Name
                    },
                    IsTimerActive = true,
                    TimerDuration = 5f,
                    TimeStamp = DateTime.Now.ToOADate()
                };

                game.Notifications.ProcessNotification(notificationData);
            }
            catch (Exception e) { }
        }
    }
}
