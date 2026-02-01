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
- Press Play to test

5) Build APK:
- File > Build Settings > Build
- Use Development Build for testing, then non-Development for submission

6) Floor calibration / placement:
- On device, the first tap places the investigation bubble on a detected plane (this is your floor calibration).
- If planes are hard to detect, keep the device moving slowly for a few seconds, then tap.
- Use the Relocate button anytime to recalibrate.

7) Fast testing loop (USB, no manual APK transfer):
- Enable Developer Options + USB Debugging on the phone.
- Plug in via USB and accept the RSA prompt on the device.
- In Unity: File > Build Settings > Android > Build And Run.
- Keep "Development Build" enabled for faster iteration.
- Unity installs and launches directly on the phone each time.
- Use Window > Analysis > Android Logcat to view device logs.
- If adb is not found, add <SDK>/platform-tools to PATH (Unity Preferences > External Tools).

8) Editor testing (no phone):
- Switch platform to PC/Mac/Linux Standalone.
- Edit > Project Settings > XR Plug-in Management > Standalone: enable XR Simulation.
- Press Play and use the XR Simulation controls to move the camera.

9) Upgrading clue visuals to real 3D objects:
- Import meshes into `Assets/Art/` (or any folder).
- Open `Assets/Scripts/Factory/ClueFactory.cs`.
- Replace the primitive builders (`CreateLanyard`, `CreateDock`, etc.) with mesh-based versions:
  - Create a GameObject, add `MeshFilter`, assign your mesh (`sharedMesh`).
  - Add `MeshRenderer` and assign a material.
  - Add a collider so taps still work.
- Run Jam > Generate Scene again and test on device.

10) Troubleshooting:
- PowerToys Monaco Preview can crash Unity builds; disable the Preview Pane or the Monaco plugin.
- UDP port warnings from Visual Studio are safe to ignore.
