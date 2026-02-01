# The Tear (AR)

A compact AR investigation game for Android (Unity 6 + AR Foundation). Place an investigation bubble on a detected plane, explore evidence across MATTER / VOID / FLOW, and solve the deduction.

## Core Loop
- Scan for planes, tap to place the investigation bubble.
- Walk around: evidence is anchored in world space under SceneRoot.
- Switch MATTER / VOID / FLOW to reveal hidden layers.
- Tap evidence to unlock clues; Journal is a fallback and only unlocks eligible clues.
- Complete the Deduction Board to win.

## Controls
- Tap: place anchor (placement mode) or select evidence (investigation mode)
- Buttons: MATTER / VOID / FLOW, Journal, Relocate, Pause, Deduction

## Evidence Pipeline
- Procedural fallback via `ClueFactory` (primitive kitbashes).
- Optional prefab pipeline via `Assets/Art/PrefabLibrary.asset` (no Resources.Load).
- Prefab keys use objectId (preferred) or recipe name.
- See `ART_PIPELINE.md` for the full asset workflow.

## Run Book
1) Open Unity.
2) Window > TextMeshPro > Import TMP Essentials.
3) Ensure Layers "Void" and "Flow" exist (Layer 6/7).
4) Jam > Generate Scene (creates Main.unity + PrefabLibrary.asset).
5) Play in Editor or Build & Run to Android.

## Notes
- Placement uses ARRaycastManager planes only (no fallback).
- Evidence must have a Renderer + Collider and never be parented under Canvas/AR Camera (runtime validator enforces this).
