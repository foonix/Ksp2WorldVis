# KSP2 WorldVis

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

> [!WARNING]
>  Do not distribute either the catalog bundle or the `resS` files! Add them to `.gitignore`!

* Clone this git repository, and open it in Unity 2022.3.5
* Set up the path to your KSP2 installation in ThunderKit settings and run the project importer.
  This may require restarting Unity several times.
* In the unity Project tab, browse to `Assets/ThunderKitSettings/Pipelines/ImportKsp2ToEditor`.
  Click on it, and then click the "Execute" button in the inspector tab.
  This will generate the BundleKit catalog bundle, which provides the shaders.
* Temporary step: Copy all `*.resS` from the game data directory, and place them in the project root directory.
  (This may become unnecessary as BundleKit development progresses.)

## Using the planet visualizer

* Open the scene `Assets/Scenes/PqsVis.scene`
* Enter play mode to view Kerbin.

Overlays:
* The science overlay button in the GUI shows where the science typs changeover occurs.
* Enable the BiomeMaskOverlay game object to overlay the planet's `_BiomeMaskTex` texture.

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

## The secret sauce that makes this work (acknowledgements)

This project was made possible by the converges of several tools and factors.

[PassivePicasso/ThunderKit](https://github.com/PassivePicasso/ThunderKit) (by @PassivePicasso) 
provides the ability to reference Assembly-CSharp code in the editor,
and the ability to create an Addressable catalog that points to the game's addressable bundles.
For KSP2, this provides the ability to reference about %90 of the game assets.

[PassivePicasso/BundleKit](https://github.com/PassivePicasso/BundleKit) (by @PassivePicasso)
provides a way to reference non-addressable, non-bundle assets.  BundleKit is still in early
development.  However, the basic concept developed by PassivePicasso is solid,
and with a little TLC I was able to get a fork working well enough to serve this project.
For KSP2, this provides access to some critical shaders and prefabs required to make PQS work.

A workaround in the dev branch of [KSP2UnityTools](https://github.com/KSP2Community/KSP2UnityTools)
loads ThunderKit's addressable catalog to be loaded after domain reload.  This makes the Addressable assets
immediately available when entering play mode.

A little bit of glue (see [BkCatalogResourceAdapter](Assets/WorldVis.Editor/BkCatalogResourceAdapter.cs) for details)
ties Unity [ResourceAPI](https://docs.unity3d.com/ScriptReference/ResourcesAPI.html) to the bundleKit catalog.
This allows Unity APIs such as `Resource.Load()` and `Shader.Find()` to access the catalog bundle exactly as they would
access resources in-game.  (This feature may go into BundleKit)

Finally, thanks to the KSP2 team for including parts of their test harness (`PrevisGameInstance` and associates) in the build assembly.
Much of this project may not have been feasible without it.
