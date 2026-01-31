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

## Run Book
1) Open Unity
2) If needed: Window > TextMeshPro > Import TMP Essentials
3) Ensure Layers "Void" and "Flow" exist
4) Ensure Active Input Handling is correct
5) Jam/Generate Scene
6) Play
7) Build APK

