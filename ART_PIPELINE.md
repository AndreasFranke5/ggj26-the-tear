# Art Pipeline: Evidence Prefabs

This project supports an optional prefab pipeline for evidence objects. **Procedural primitives serve as an automatic fallback** - the game always works even without any prefabs.

## How the Fallback Works

`ClueFactory.Create()` follows this priority order:

1. **Prefab by objectId**: If `PrefabLibrary` contains a prefab with a key matching the clue's `id` field (e.g., `C01`), that prefab is instantiated.
2. **Prefab by recipe**: If no objectId match, the factory checks for a prefab with a key matching the `recipe` field (e.g., `Lanyard`, `Dock`).
3. **Procedural fallback**: If neither lookup succeeds, the factory builds a multi-part procedural object using Unity primitives (cubes, cylinders, spheres).

This means:
- **No prefabs needed**: The game ships and runs with zero art assets.
- **Incremental art**: Artists can add prefabs one at a time; each new prefab replaces one procedural object.
- **No code changes**: Just add the prefab to `PrefabLibrary.asset` with the correct key.

## Supported Recipe Names (Procedural Shapes)

These recipe names are recognized by `ClueFactory` for procedural fallback:

| Recipe | Description |
|--------|-------------|
| Lanyard | Strap with ring and cut marker |
| ShadowLog | Panel with horizontal lines |
| Badge | Cylinder token with clip |
| Note | Paper with folded corner and tape |
| PowerSpire | Base, spire, and orb |
| Bit | Screwdriver bit with tri-wing fins |
| Dock | Charging dock base with pad |
| PadWrapper | Wrapper with conductive pad |
| Spill | Cylinder with droplets |
| AntistaticBag | Bag with seal and outline |
| FlowTrace | Line renderer path with arrow |
| WallStash | Panel with recess |

## 1) Model in Blender (Optional)

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
  - Minimum collider dimension: 0.05m for reliable mobile tapping (the factory enforces this automatically).
- Drag the GameObject into `Assets/Art/Prefabs/` to create a prefab.

**Important**: Prefabs must NOT contain any `RectTransform` components. Evidence must be world-space 3D objects, not UI.

## 4) Register in PrefabLibrary

- Open `Assets/Art/PrefabLibrary.asset` (created by Jam > Generate Scene).
- Add an entry:
  - **Key**: Use the story `id` (e.g., `C01`, `C02`) or the `recipe` name (e.g., `Lanyard`, `Dock`).
  - **Prefab**: Drag your prefab here.
- The key lookup is case-insensitive.

Example entries:
```
Key: C01          Prefab: CutLanyard.prefab       (matches objectId)
Key: Lanyard      Prefab: GenericLanyard.prefab   (matches recipe, fallback for any lanyard)
Key: Dock         Prefab: ChargingDock.prefab     (matches recipe)
```

## 5) Verify Scale + Placement

- Ensure the prefab looks correct at localScale 1.
- If needed, adjust the `localScale` in `Assets/StreamingAssets/story/case.json`.
- Evidence spawns under `SceneRoot` with local transforms from `case.json`.

## 6) Test on Device

1. Run **Jam > Generate Scene** to ensure the `PrefabLibrary` is referenced by `ClueManager`.
2. Run **Jam > Validate Project** to check for story/layer issues.
3. **Build & Run** on Android; verify evidence is anchored to planes and tappable.

## Quick Test Without Prefabs

You can test the entire game with only procedural shapes:
1. Do NOT add any entries to `PrefabLibrary.asset`.
2. Run the game - all evidence will render as colored primitives.
3. Verify tapping, mode switching, and progression work correctly.
4. Add prefabs one at a time to replace procedural shapes.
