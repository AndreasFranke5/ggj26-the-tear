# Art Pipeline: Evidence Prefabs

This project supports an optional prefab pipeline for evidence objects. Procedural primitives remain as a fallback.

## 1) Model in Blender
- Set units to meters (Scene > Units: Metric, Unit Scale = 1.0).
- Apply transforms (Ctrl+A > Rotation & Scale).
- Keep evidence props small: ~0.05m to 0.25m at scale 1.
- Export as FBX (Forward: -Z, Up: Y).

## 2) Import into Unity
- Drop FBX files into `Assets/Art/Models/` (or any folder under `Assets/Art/`).
- Verify normals and scale in the Inspector (Scale Factor should usually be 1).

## 3) Create a Prefab
- Create an empty GameObject in the scene.
- Add `MeshFilter` and `MeshRenderer`, assign your mesh + material.
- Add a collider (`BoxCollider` or `MeshCollider`) so taps work.
- Drag the GameObject into `Assets/Art/Prefabs/` to create a prefab.

## 4) Register in PrefabLibrary
- Open `Assets/Art/PrefabLibrary.asset`.
- Add an entry:
  - Key: use the story objectId (preferred) or the recipe name.
  - Prefab: drag your prefab here.

## 5) Verify Scale + Placement
- Ensure the prefab looks correct at localScale 1.
- If needed, adjust the `localScale` in `Assets/StreamingAssets/story/case.json`.

## 6) Test on Device
- Run Jam > Generate Scene to ensure the PrefabLibrary is referenced.
- Build & Run on Android; verify evidence is anchored to planes and tappable.
