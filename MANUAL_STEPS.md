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

