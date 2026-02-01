# Manual Steps (Required)

1) Packages / XR / Input settings:
- Ensure packages installed: AR Foundation, ARCore XR Plugin, XR Plug-in Management, XR Core Utils, Input System (com.unity.inputsystem)
- Project Settings > XR Plug-in Management > Android: enable ARCore
- Project Settings > Player > Active Input Handling: set to "Both" (recommended) or "Input System Package (New)"
  - Note: Unity may require an Editor restart after changing Active Input Handling
- Switch Build Platform to Android

2) TextMeshPro Essentials (Critical):
- Window > TextMeshPro > Import TMP Essentials

3) Layers (Critical fallback if script did not create them):
- Edit > Project Settings > Tags and Layers
- Set Layer 6 = "Void"
- Set Layer 7 = "Flow"

4) Generate scene:
- In Unity, click Jam > Generate Scene
- This creates `Assets/Scenes/Main.unity` and `Assets/Art/PrefabLibrary.asset`
- Press Play to test

5) Optional prefab pipeline (artist workflow):
- Import meshes into `Assets/Art/` (any folder)
- Create prefabs with colliders
- Register them in `Assets/Art/PrefabLibrary.asset`
- See `ART_PIPELINE.md` for full steps

6) Build APK:
- File > Build Settings > Build
- Use Development Build for testing, then non-Development for submission

7) Floor calibration / placement:
- On device, the first tap places the investigation bubble on a detected plane.
- If planes are hard to detect, keep the device moving slowly for a few seconds, then tap.
- Use the Relocate button anytime to recalibrate.

8) Fast testing loop (USB, no manual APK transfer):
- Enable Developer Options + USB Debugging on the phone.
- Plug in via USB and accept the RSA prompt on the device.
- In Unity: File > Build Settings > Android > Build And Run.
- Keep "Development Build" enabled for faster iteration.
- Unity installs and launches directly on the phone each time.
- Use Window > Analysis > Android Logcat to view device logs.
- If adb is not found, add <SDK>/platform-tools to PATH (Unity Preferences > External Tools).

9) Editor testing (no phone):
- Switch platform to PC/Mac/Linux Standalone.
- Edit > Project Settings > XR Plug-in Management > Standalone: enable XR Simulation.
- Press Play and use the XR Simulation controls to move the camera.

10) Troubleshooting:
- PowerToys Monaco Preview can crash Unity builds; disable the Preview Pane or the Monaco plugin.
- UDP port warnings from Visual Studio are safe to ignore.
