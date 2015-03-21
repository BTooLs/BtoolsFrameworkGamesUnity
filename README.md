BtoolsFrameworkGamesUnity
=========================

A collection of utility classes and some common abstract systems used in game design, built in C# for Unity 4x/5X.

## BFGU.util ##

Simple components that I often use in my prototypes.

### Classes: ###

**Shaker**: Shake/Earthquake effect on an object using position alteration.

**VersionTime**: Save on build/display on screen the date/hour of the last time project modification.

**TweenObject**: Animates an object between two states (composed of position and rotation). Use it especially for camera cinamatics.

**TouchDragCamera**: Animates an object (or camera) at touch & drag.

**PrefabManager**: Static class to load and cache prefabs.

**PinchZoom**: Basic pinch/zoom camera from Unity tutorials.

**ClickableTrigger** and **IClickableListener**: Used to click on 3d objects from scene. Built before SystemEvent and 4.6 UI. Clicks or touches game objects with colliders and a compoent that has IClickable

## BFGU.analytics ##

I always try to decouple my projects from 3rd party services/SDK/libraries. The most used SDK's are for crash logers and analytics services. This namespace contains all you need to decouple **Google Analytics** and **Unity Analytics** from your code.

### Features ###

- track custom events
- track screen views
- set unique user identifier / user ID
- *TODO: set user parameters*
- *TODO: track IAP*
- *TODO: Make DebugClient just for console output*

### Supported libraries ###

- Google Analytics
- Unity Analytics
- *TODO: GameAnalytics.com*
- *TODO: Swrve (need access)*
- *TODO: Flurry (need access)*

### How to use it ###

1. Install Google Analytics and Unity Analytics SDK's in your project.
2. Put GAV3 and Analytics prefabs in your first scene, and fill all the parameters.
3. In your code use Analytics.Track and Analytics. Screen from BFGU.Analytics namespace.
Tested with Unity5.0, WebGL preview, GAV3 and UnityAnalytics Beta.

### Classes: ###

**Analytics**: Main class and entry point. Entire project sends events here. All AnalyticsClients attach themselvs here.

**AnalyticsSetup**: Acts like an interface/easier setup. Put it only in your first scene.

**AnalyticsUI**: Just a wrapper for Analytics.Track( eventName, "UI" ); call. Usually I put one of these in the scene, and use it with the EventSystem OnClick listeners.

**IAnalyticsClient**: All clients must implenet this interface, and attach itself to the Analytics events.