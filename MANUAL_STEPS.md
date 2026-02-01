# Manual Steps (Required)

## 1) Install Packages and Configure XR/Input

**Packages** (should already be in project):
- AR Foundation
- ARCore XR Plugin
- XR Plug-in Management
- XR Core Utils
- Input System (com.unity.inputsystem)

**XR Settings**:
- Edit > Project Settings > XR Plug-in Management
- Android tab: enable **ARCore**
- (Optional) Standalone tab: enable **XR Simulation** for Editor testing

**Input Settings**:
- Edit > Project Settings > Player > Other Settings
- Active Input Handling: set to **"Both"** (recommended) or "Input System Package (New)"
- Note: Unity may require an Editor restart after changing Active Input Handling

**Platform**:
- File > Build Settings > Switch Platform to **Android**

## 2) Import TextMeshPro Essentials (Critical)

- Window > TextMeshPro > Import TMP Essentials
- This imports the default font assets required for UI text

## 3) Configure Layers (Critical)

The scene generator checks for these layers but cannot create them automatically:

- Edit > Project Settings > Tags and Layers
- Set **Layer 6 = "Void"**
- Set **Layer 7 = "Flow"**

These layers are used for mode-based visibility (MATTER/VOID/FLOW switching).

## 4) Generate the Scene

- In Unity, click **Jam > Generate Scene**
- This creates:
  - `Assets/Scenes/Main.unity` (the main scene)
  - `Assets/Art/PrefabLibrary.asset` (for optional prefab pipeline)
- The generator wires all references automatically
- Press Play to test in Editor

## 5) Validate the Project

- In Unity, click **Jam > Validate Project**
- This checks:
  - Story data loads from `Assets/StreamingAssets/story/case.json`
  - Clue IDs, prerequisites, and clusters are valid
  - Void/Flow layers exist
- Fix any logged errors before building

## 6) Optional: Add Prefabs

The game works with procedural primitives by default. To add custom art:

- Import meshes into `Assets/Art/` (any folder)
- Create prefabs with colliders (minimum 0.05m for tappability)
- Register them in `Assets/Art/PrefabLibrary.asset`:
  - Add an entry where **key** = `objectId` (preferred) or `recipe` from `case.json`
  - Prefab must be a **world-space 3D object** (Transform + MeshRenderer + Collider)
- See `ART_PIPELINE.md` for the full workflow

The procedural fallback is automatic - unregistered clues use colored primitives.

## 7) Build APK

- File > Build Settings
- Ensure Android platform is selected
- Check **Development Build** for testing (faster iteration, logcat support)
- Click **Build** to create APK, or **Build And Run** to deploy directly

For submission/release:
- Uncheck Development Build
- Build the final APK

## 8) On-Device Testing (USB)

- Enable **Developer Options + USB Debugging** on the phone
- Plug in via USB and accept the RSA prompt on the device
- In Unity: File > Build Settings > Android > **Build And Run**
- Unity installs and launches directly on the phone

**View logs**:
- Window > Analysis > Android Logcat
- If adb is not found, add `<SDK>/platform-tools` to PATH (Unity Preferences > External Tools)

## 9) Floor Calibration / Placement

- On device, the app starts in **Placement mode** with plane scanning
- A reticle tracks detected planes
- Tap to place the investigation bubble (SceneRoot)
- Evidence spawns only after placement
- Use the **Relocate** button anytime to re-enter placement mode (clue progress is preserved)

**Tips**:
- Move the device slowly for a few seconds to help plane detection
- Point at flat surfaces (floor, table)
- Good lighting improves tracking

## 10) Editor Testing (No Phone)

For quick iteration without a device:

- Switch platform to PC/Mac/Linux Standalone
- Edit > Project Settings > XR Plug-in Management > Standalone: enable **XR Simulation**
- Press Play and use XR Simulation controls to move the camera
- Note: AR placement and plane detection are simulated

## 11) Troubleshooting

| Issue | Solution |
|-------|----------|
| Pink materials on device | Shader missing; ClueFactory uses safe Unlit/Color fallback |
| Evidence stuck to camera | Check parenting - must be under SceneRoot, not ARCamera |
| Taps not registering | Ensure colliders exist and are >= 0.05m |
| Layers missing error | Set Layer 6 = "Void", Layer 7 = "Flow" manually |
| TMP text missing | Run Window > TextMeshPro > Import TMP Essentials |
| PowerToys crash | Disable Preview Pane or Monaco plugin |
| UDP port warnings | Safe to ignore (Visual Studio debugger) |
| Plane detection slow | Move device slowly, ensure good lighting |
