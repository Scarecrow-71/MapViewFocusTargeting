# MapViewFocusTargeting 1.0.0
A mod to allow using keyboard inputs to cycle through focus of celestial bodies, as well as set/unset them as targets.


# Instructions for Usage
1.  Active Vehicle Focus.
    a.  Pressing the [HOME] key on your keyboard while in Map View will set the focus to whichever vehicle is defined as being the Active Vessel.
2.  Celestial Body Focus.
    a.  Pressing the [PG UP] key on your keyboard will cycle through and set the focus on the next Celestial Body in the Kerbol system, starting with Kerbol and going outward towards Eeloo.
    b.  Pressing the [PG DN] key on your keyboard will cycle through and set the focus on the next Celestial Body in the Kerbol system, starting with Eeloo and going inwards towards Kerbol.
    c.  The use of [PG UP] and [PG DN] allows you to cycle in either direction, at will.  For example, if you wish to set the focus to Duna, you can press [PG UP] until you cycle from Kerbol to Duna, or you can press [PG DN] until
        you cycle from Eeloo to Duna.  Should you go to far in either direction - pressing [PG UP] until you get to Ike, or pressing [PG DN] until you get to Minmus - you can press the opposite button to go the other way.
3.  Celestial Body Target.
    a.  Pressing the [NUMPAD +] key on your keyboard will set the currently focused Celestial Body as the target for the currently active vessel.
    b.  Pressing the [NUMPAD -] key on your keyboard will unset the currently focused Celestial Body as the target for the currently active vessel.


# Installation Instructions
This mod assumes you have and have already installed the following:

1.  BepInEx
2.  SpaceWarp

If you do not have either of these currently installed, please navigate to https://spacedock.info/mod/3277/Space%20Warp%20+%20BepInEx, download the zip file, and follow the instructions for installing these packages.  In lieu of that,
you can navigate to https://forum.kerbalspaceprogram.com/index.php?/topic/213036-space-warp/page/2/#comment-4253284 and read the instructions munix has in the spoiler.

Once you have BepInEx and SpaceWarp installed, simply drag the mapviewfocustargeting folder and its contents into <KSP2_Root>\BepInEx\plugins, where <KSP2_Root> is the location of your Kerbal Space Program 2 install.


# Thanks, Kudos, Props
I need to give thanks to several people for the help they provided in getting me up and running with mod development.

1.  munix, for answering multiple questions on both Discord and the KSP forums as it relates to installation of BepInEx, SpaceWarp, decompiling the KSP2 code, and every other nagging issue I had
2.  cheese3660, for helping point me in the right direction with functions, overloads, methods, and other sticky pieces of code
3.  Halbann, without whose source code for Lazy Orbit I may not have been able to finish focusing on Celestial Bodies
4.  All of the mods, admins, and forum users at the KSP forums who have put up with me these last several years as I came to learn about KSP1, and then KSP2, and then tackling mod development
5.  My wife, Lori, for always reminding me that it's just a game, and who really cares if Jebediah can get to the surface of Eve and back in one go without using docking ports (like honestly, dude, it's just a game, put it down,
    come eat dinner, play with the cats, why are you so worried about what happens to them when you aren't paying attention if you can just revert to launch...)
6.  Abe9192 for the refactored code in 1.0.1.

If you have helped me in any way, shape, or form, whether it's with playing KSP1 or KSP2, or helping with mod development, I owe you my gratitude and a debt of thanks.


# Licensing
MapViewFocusTargeting 1.0.1 is distributed under the CC BY-SA 4.0 license. Read about the license here before redistributing:  https://creativecommons.org/licenses/by-sa/4.0/


# Changelog/Numbering
MapViewFocusTargeting uses a standard numbering for its releases, in the form of x.y.z, where:

1.  x is the major version release number
2.  y is the bug fix version release number
3.  z is the minor update version release number

The current version for MapViewFocusTargeting is 1.0.0.  The current list of changes by release:

1.0.1     Code cleanup for maintenance
1.0.0     Initial Release