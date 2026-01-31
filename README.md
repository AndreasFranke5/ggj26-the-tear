# The Tear (AR)

A compact AR investigation game for Android built for a game jam. Place a single anchor, switch between three reality modes (MATTER / VOID / FLOW), tap evidence to unlock clues, and solve the final deduction.

## Core Loop
- Tap a plane to place the investigation bubble.
- Switch MATTER / VOID / FLOW to reveal different evidence layers.
- Tap evidence to unlock clues in a non-linear clue graph.
- Use the Journal fallback if a clue is hard to tap.
- Complete the Deduction Board to win.

## Controls
- Tap: place anchor (placement mode) or select evidence (investigation mode)
- Buttons: MATTER / VOID / FLOW, Journal, Relocate, Pause, Deduction

## Architecture Notes
- No prefabs or Resources.Load for visuals.
- All evidence is procedurally built via primitives and LineRenderer.
- Clue graph is validated at startup and via Jam > Validate Project.

## Evidence Visuals (Upgrade Guide)
To turn the placeholder primitives into detailed 3D props:
1) Import your meshes (FBX/GLB) into `Assets/Art/` (any folder).
2) Open `Assets/Scripts/Factory/ClueFactory.cs`.
3) Replace each `CreateX()` method with mesh-based versions using `MeshFilter` + `MeshRenderer`.
   - Create a new `GameObject`, add `MeshFilter`, set `sharedMesh` to your imported mesh.
   - Add `MeshRenderer`, assign a material (you can still use `MaterialFactory.GetSafeUnlitMaterial`).
4) Keep colliders (add `BoxCollider` or `MeshCollider`) so taps still work.
5) Rebuild scene with Jam > Generate Scene and test on device.

## Run Book
1) Open Unity
2) If needed: Window > TextMeshPro > Import TMP Essentials
3) Ensure Layers "Void" and "Flow" exist
4) Ensure Active Input Handling is correct
5) Jam/Generate Scene
6) Play
7) Build APK
