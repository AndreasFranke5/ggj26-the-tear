using System;
using UnityEngine;
using TheTear.Story;

namespace TheTear.Factory
{
    public static class ClueFactory
    {
        public static GameObject Create(StoryObjectData data, Transform parent)
        {
            ClueRecipe recipe = ParseRecipe(data.recipe);
            GameObject root = BuildRecipe(recipe);
            root.name = data.id;
            root.transform.SetParent(parent, false);
            root.transform.localPosition = data.localPosition;
            root.transform.localEulerAngles = data.localRotation;
            root.transform.localScale = data.localScale;

            int layer = ResolveLayer(data.mode);
            SetLayerRecursive(root, layer);

            ApplyMaterial(root, ColorForRecipe(recipe));
            return root;
        }

        private static ClueRecipe ParseRecipe(string recipe)
        {
            if (Enum.TryParse(recipe, true, out ClueRecipe parsed))
            {
                return parsed;
            }
            return ClueRecipe.Lanyard;
        }

        private static int ResolveLayer(string mode)
        {
            if (string.Equals(mode, "Void", StringComparison.OrdinalIgnoreCase))
            {
                int layer = LayerMask.NameToLayer("Void");
                return layer >= 0 ? layer : 0;
            }
            if (string.Equals(mode, "Flow", StringComparison.OrdinalIgnoreCase))
            {
                int layer = LayerMask.NameToLayer("Flow");
                return layer >= 0 ? layer : 0;
            }
            return LayerMask.NameToLayer("Default");
        }

        private static GameObject BuildRecipe(ClueRecipe recipe)
        {
            switch (recipe)
            {
                case ClueRecipe.ShadowLog:
                    return CreatePanel();
                case ClueRecipe.Badge:
                    return CreateDisk();
                case ClueRecipe.Note:
                    return CreateNote();
                case ClueRecipe.PowerSpire:
                    return CreatePowerSpire();
                case ClueRecipe.Bit:
                    return CreateBit();
                case ClueRecipe.Dock:
                    return CreateDock();
                case ClueRecipe.PadWrapper:
                    return CreatePadWrapper();
                case ClueRecipe.Spill:
                    return CreateSpill();
                case ClueRecipe.AntistaticBag:
                    return CreateBag();
                case ClueRecipe.FlowTrace:
                    return CreateFlowTrace();
                case ClueRecipe.WallStash:
                    return CreateWallStash();
                default:
                    return CreateLanyard();
            }
        }

        private static GameObject CreateLanyard()
        {
            GameObject ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ring.transform.localScale = new Vector3(1f, 0.1f, 1f);
            return ring;
        }

        private static GameObject CreatePanel()
        {
            GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.transform.localScale = new Vector3(1f, 1f, 0.1f);
            return panel;
        }

        private static GameObject CreateDisk()
        {
            GameObject disk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            disk.transform.localScale = new Vector3(1f, 0.15f, 1f);
            return disk;
        }

        private static GameObject CreateNote()
        {
            GameObject note = GameObject.CreatePrimitive(PrimitiveType.Cube);
            note.transform.localScale = new Vector3(1f, 0.6f, 0.05f);
            return note;
        }

        private static GameObject CreatePowerSpire()
        {
            GameObject root = new GameObject("PowerSpire");
            GameObject spire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spire.transform.SetParent(root.transform, false);
            spire.transform.localScale = new Vector3(0.6f, 1.2f, 0.6f);
            GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.transform.SetParent(root.transform, false);
            orb.transform.localPosition = new Vector3(0, 1.1f, 0);
            orb.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            return root;
        }

        private static GameObject CreateBit()
        {
            GameObject bit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bit.transform.localScale = new Vector3(0.4f, 0.6f, 0.4f);
            return bit;
        }

        private static GameObject CreateDock()
        {
            GameObject root = new GameObject("Dock");
            GameObject basePlate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            basePlate.transform.SetParent(root.transform, false);
            basePlate.transform.localScale = new Vector3(1.2f, 0.2f, 1.2f);
            GameObject lid = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lid.transform.SetParent(root.transform, false);
            lid.transform.localPosition = new Vector3(0f, 0.2f, 0f);
            lid.transform.localScale = new Vector3(1f, 0.1f, 1f);
            return root;
        }

        private static GameObject CreatePadWrapper()
        {
            GameObject root = new GameObject("PadWrapper");
            GameObject wrapper = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wrapper.transform.SetParent(root.transform, false);
            wrapper.transform.localScale = new Vector3(1.1f, 0.1f, 1.1f);
            GameObject pad = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pad.transform.SetParent(root.transform, false);
            pad.transform.localPosition = new Vector3(0f, 0.08f, 0f);
            pad.transform.localScale = new Vector3(0.6f, 0.05f, 0.6f);
            return root;
        }

        private static GameObject CreateSpill()
        {
            GameObject spill = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spill.transform.localScale = new Vector3(1.2f, 0.05f, 1.2f);
            return spill;
        }

        private static GameObject CreateBag()
        {
            GameObject bag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bag.transform.localScale = new Vector3(1f, 0.7f, 0.5f);
            return bag;
        }

        private static GameObject CreateFlowTrace()
        {
            GameObject root = new GameObject("FlowTrace");
            var line = root.AddComponent<LineRenderer>();
            line.positionCount = 4;
            line.useWorldSpace = false;
            line.widthMultiplier = 0.05f;
            line.SetPosition(0, new Vector3(-0.4f, 0f, -0.2f));
            line.SetPosition(1, new Vector3(0.2f, 0f, 0.1f));
            line.SetPosition(2, new Vector3(-0.1f, 0f, 0.4f));
            line.SetPosition(3, new Vector3(0.4f, 0f, -0.1f));

            var collider = root.AddComponent<BoxCollider>();
            collider.size = new Vector3(1f, 0.2f, 1f);

            return root;
        }

        private static GameObject CreateWallStash()
        {
            GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.transform.localScale = new Vector3(1f, 1f, 0.1f);
            return panel;
        }

        private static Color ColorForRecipe(ClueRecipe recipe)
        {
            switch (recipe)
            {
                case ClueRecipe.ShadowLog:
                    return new Color(0.2f, 0.8f, 0.9f, 1f);
                case ClueRecipe.Badge:
                    return new Color(0.9f, 0.8f, 0.2f, 1f);
                case ClueRecipe.Note:
                    return new Color(0.9f, 0.9f, 0.7f, 1f);
                case ClueRecipe.PowerSpire:
                    return new Color(0.8f, 0.3f, 0.3f, 1f);
                case ClueRecipe.Bit:
                    return new Color(0.6f, 0.6f, 0.6f, 1f);
                case ClueRecipe.Dock:
                    return new Color(0.3f, 0.3f, 0.35f, 1f);
                case ClueRecipe.PadWrapper:
                    return new Color(0.8f, 0.4f, 0.2f, 1f);
                case ClueRecipe.Spill:
                    return new Color(0.1f, 0.5f, 0.7f, 0.8f);
                case ClueRecipe.AntistaticBag:
                    return new Color(0.2f, 0.7f, 0.3f, 1f);
                case ClueRecipe.FlowTrace:
                    return new Color(0.9f, 0.6f, 0.1f, 1f);
                case ClueRecipe.WallStash:
                    return new Color(0.6f, 0.3f, 0.7f, 1f);
                default:
                    return new Color(0.8f, 0.2f, 0.2f, 1f);
            }
        }

        private static void ApplyMaterial(GameObject root, Color color)
        {
            Material mat = MaterialFactory.GetSafeUnlitMaterial(color);
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                if (mat != null)
                {
                    renderer.material = mat;
                }
            }

            var lines = root.GetComponentsInChildren<LineRenderer>(true);
            foreach (var line in lines)
            {
                if (mat != null)
                {
                    line.material = mat;
                }
            }
        }

        private static void SetLayerRecursive(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }
    }
}
