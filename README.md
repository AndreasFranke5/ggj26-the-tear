# The Tear (AR)

A compact AR investigation game for Android (Unity 6 + AR Foundation). Place an investigation bubble on a detected plane, explore evidence across MATTER / VOID / FLOW, and solve the deduction.

## Core Loop

1. **Scan** for planes, tap to place the investigation bubble.
2. **Walk around**: evidence is anchored in world space under SceneRoot.
3. **Switch modes** (MATTER / VOID / FLOW) to reveal hidden layers.
4. **Tap evidence** to unlock clues; Journal is a fallback and only unlocks eligible clues.
5. **Complete the Deduction Board** to win.

## Controls

- **Tap**: Place anchor (placement mode) or select evidence (investigation mode)
- **Buttons**: MATTER / VOID / FLOW, Journal, Relocate, Pause, Deduction

## Quick Start (Run Book)

```
1) Open Unity 6 (6000.x)
2) Window > TextMeshPro > Import TMP Essentials
3) Edit > Project Settings > Tags and Layers:
   - Set Layer 6 = "Void"
   - Set Layer 7 = "Flow"
4) Edit > Project Settings > XR Plug-in Management > Android: enable ARCore
5) Jam > Generate Scene (creates Main.unity + PrefabLibrary.asset)
6) Jam > Validate Project (checks story data and layers)
7) File > Build Settings > Android > Build And Run
```

See `MANUAL_STEPS.md` for detailed setup instructions.

## Evidence Pipeline

The game works with zero art assets using procedural primitives:

1. **Prefab by objectId**: If `PrefabLibrary` has a prefab keyed by clue ID (e.g., `C01`), it is used.
2. **Prefab by recipe**: If no ID match, checks for prefab keyed by recipe name (e.g., `Lanyard`).
3. **Procedural fallback**: If no prefab found, builds multi-part primitives (cubes, cylinders, spheres).

This means:
- Game ships and runs with no art assets
- Artists can add prefabs incrementally - each replaces one procedural object
- No code changes needed - just add to `Assets/Art/PrefabLibrary.asset`

See `ART_PIPELINE.md` for the full prefab workflow.

## Project Structure

| Path | Contents |
|------|----------|
| `Assets/Scripts/` | Runtime code (Core, AR, Story, Factory, UI, etc.) |
| `Assets/Scripts/Editor/` | Editor scripts (JamProjectSetup.cs) |
| `Assets/StreamingAssets/story/` | case.json (story data) |
| `Assets/Art/` | Prefabs, PrefabLibrary.asset |
| `Assets/Scenes/` | Main.unity (generated) |

## Documentation

- `GAME_SPEC.md` - Canonical game requirements
- `CLAUDE.md` - Architecture and coding guidelines
- `MANUAL_STEPS.md` - Detailed setup instructions
- `ART_PIPELINE.md` - Prefab workflow with procedural fallback

## Technical Notes

- Placement uses `ARRaycastManager.Raycast` (planes only)
- Interaction uses `Physics.Raycast` (3D colliders)
- Evidence must have Renderer + Collider, never parented under Canvas/ARCamera
- UI overlays use `raycastTarget=false` to avoid blocking world taps
- Safe shaders: `Unlit/Color` with `Sprites/Default` fallback
