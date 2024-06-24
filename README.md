# KSP2 WolrdVis

Visual asset and mod development tool for KSP2 

## Features:
* Run PQS planets in the editor.  Get instant feedback when developing planet assets.
* LookDev assets against actual KSP2 shaders, etc.
* ThunderKit Addressable browser provides easy viewing of stock parts to use as examples.

## Requirements
* Unity 2022.3.5 or the current KSP2 version of unity.  [Get it here](https://unity.com/releases/editor/whats-new/2022.3.5)
* A git client must be installed in your system to download package dependencies.
  [See this for unity](https://docs.unity3d.com/Manual/upm-git.html). See also [Git for Windows](https://gitforwindows.org/)

## Initial setup

* Clone this git repository, and open it in Unity 2022.3.5
* Set up the path to your KSP2 installation in ThunderKit settings and run the project importer.
  This may require restarting Unity several times.
* In the unity Project tab, browse to `Assets/ThunderKitSettings/Pipelines/ImportKsp2ToEditor`.
  Click on it, and then click the "Execute" button in the inspector tab.
  This will generate the BundleKit catalog bundle, which provides the shaders.

## Using the planet visualizer

* Open the scene `Assets/Scenes/PqsVis.scene`
* Enter play mode to view Kerbin.

Notable settings:
* On PrevisCameraManager, the angle and distance of Kerbol can be changed.
  The distance should be set to the distance that the planet orbits Kerbol (or whatever is the parent star).
  Changing the distance will change the brightness of the light.
  Changing the angle has the effect of simulating planet rotation.
* To spawn a different celestial body, change the `Addressable load path` strings on `PqsVisSetup`.
  The strings to use can be found in the ThunderKit Addressables browser.

## Using the ThunderKit Addressables browser.

The Addressables browser should work out of the box.
Tools -> ThunderKit -> Addressables.
