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
                    return CreateShadowLog();
                case ClueRecipe.Badge:
                    return CreateBadge();
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
            GameObject root = new GameObject("Lanyard");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(0.9f, 0.05f, 0.9f), Vector3.zero, "Ring");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, 0f, 0.55f), new Vector3(0.2f, 0.05f, 0.6f), Vector3.zero, "Strap");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, 0f, 0.95f), new Vector3(0.18f, 0.06f, 0.18f), Vector3.zero, "Clasp");
            return root;
        }

        private static GameObject CreateShadowLog()
        {
            GameObject root = new GameObject("ShadowLog");
            AddPrimitive(PrimitiveType.Cube, root.transform, Vector3.zero, new Vector3(1.1f, 0.8f, 0.08f), Vector3.zero, "Panel");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, 0.2f, 0.05f), new Vector3(0.9f, 0.08f, 0.02f), Vector3.zero, "Line1");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, 0f, 0.05f), new Vector3(0.9f, 0.08f, 0.02f), Vector3.zero, "Line2");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, -0.2f, 0.05f), new Vector3(0.9f, 0.08f, 0.02f), Vector3.zero, "Line3");
            return root;
        }

        private static GameObject CreateBadge()
        {
            GameObject root = new GameObject("Badge");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(0.7f, 0.08f, 0.7f), Vector3.zero, "Token");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, 0.15f, 0f), new Vector3(0.2f, 0.25f, 0.05f), Vector3.zero, "Clip");
            return root;
        }

        private static GameObject CreateNote()
        {
            GameObject root = new GameObject("Note");
            AddPrimitive(PrimitiveType.Cube, root.transform, Vector3.zero, new Vector3(1f, 0.65f, 0.04f), Vector3.zero, "Paper");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0.35f, 0.22f, 0.03f), new Vector3(0.3f, 0.25f, 0.02f), new Vector3(0f, 0f, 25f), "Fold");
            return root;
        }

        private static GameObject CreatePowerSpire()
        {
            GameObject root = new GameObject("PowerSpire");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.1f, 0), new Vector3(0.7f, 0.2f, 0.7f), Vector3.zero, "Base");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, new Vector3(0, 0.7f, 0), new Vector3(0.5f, 0.9f, 0.5f), Vector3.zero, "Spire");
            AddPrimitive(PrimitiveType.Sphere, root.transform, new Vector3(0, 1.6f, 0), new Vector3(0.5f, 0.5f, 0.5f), Vector3.zero, "Orb");
            return root;
        }

        private static GameObject CreateBit()
        {
            GameObject root = new GameObject("Bit");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, new Vector3(0, 0.3f, 0), new Vector3(0.25f, 0.6f, 0.25f), Vector3.zero, "Handle");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.75f, 0), new Vector3(0.25f, 0.15f, 0.25f), new Vector3(0, 45f, 0), "Head");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, new Vector3(0, 0.95f, 0), new Vector3(0.12f, 0.15f, 0.12f), Vector3.zero, "Tip");
            return root;
        }

        private static GameObject CreateDock()
        {
            GameObject root = new GameObject("Dock");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.05f, 0), new Vector3(1.2f, 0.15f, 1.2f), Vector3.zero, "Base");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.18f, 0), new Vector3(0.9f, 0.08f, 0.9f), Vector3.zero, "Lid");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0.45f, 0.12f, 0), new Vector3(0.1f, 0.05f, 0.5f), Vector3.zero, "Latch");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, new Vector3(-0.6f, 0.05f, -0.4f), new Vector3(0.05f, 0.3f, 0.05f), new Vector3(0, 0, 90f), "Cable");
            return root;
        }

        private static GameObject CreatePadWrapper()
        {
            GameObject root = new GameObject("PadWrapper");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.02f, 0), new Vector3(1.1f, 0.04f, 1.1f), Vector3.zero, "Wrapper");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.06f, 0), new Vector3(0.7f, 0.03f, 0.7f), Vector3.zero, "Pad");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0.45f, 0.04f, 0), new Vector3(0.08f, 0.02f, 0.9f), Vector3.zero, "Seal");
            return root;
        }

        private static GameObject CreateSpill()
        {
            GameObject root = new GameObject("Spill");
            AddPrimitive(PrimitiveType.Cylinder, root.transform, Vector3.zero, new Vector3(1.2f, 0.03f, 1.2f), Vector3.zero, "MainSpill");
            AddPrimitive(PrimitiveType.Sphere, root.transform, new Vector3(0.5f, 0.02f, 0.2f), new Vector3(0.15f, 0.05f, 0.15f), Vector3.zero, "Droplet1");
            AddPrimitive(PrimitiveType.Sphere, root.transform, new Vector3(-0.4f, 0.02f, -0.3f), new Vector3(0.12f, 0.05f, 0.12f), Vector3.zero, "Droplet2");
            AddPrimitive(PrimitiveType.Sphere, root.transform, new Vector3(0.2f, 0.02f, -0.5f), new Vector3(0.1f, 0.05f, 0.1f), Vector3.zero, "Droplet3");
            return root;
        }

        private static GameObject CreateBag()
        {
            GameObject root = new GameObject("AntistaticBag");
            AddPrimitive(PrimitiveType.Cube, root.transform, Vector3.zero, new Vector3(1f, 0.75f, 0.5f), Vector3.zero, "Bag");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, 0.35f, 0.26f), new Vector3(1f, 0.05f, 0.05f), Vector3.zero, "Seal");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0, -0.1f, 0.22f), new Vector3(0.6f, 0.4f, 0.02f), Vector3.zero, "Label");
            return root;
        }

        private static GameObject CreateFlowTrace()
        {
            GameObject root = new GameObject("FlowTrace");
            var line = root.AddComponent<LineRenderer>();
            line.positionCount = 6;
            line.useWorldSpace = false;
            line.widthMultiplier = 0.04f;
            line.SetPosition(0, new Vector3(-0.5f, 0f, -0.3f));
            line.SetPosition(1, new Vector3(-0.1f, 0f, 0.05f));
            line.SetPosition(2, new Vector3(0.3f, 0f, 0.3f));
            line.SetPosition(3, new Vector3(-0.2f, 0f, 0.5f));
            line.SetPosition(4, new Vector3(0.4f, 0f, 0.1f));
            line.SetPosition(5, new Vector3(0.6f, 0f, -0.2f));

            AddPrimitive(PrimitiveType.Cylinder, root.transform, new Vector3(0.6f, 0f, -0.2f), new Vector3(0.06f, 0.12f, 0.06f), new Vector3(90f, 0f, 0f), "Arrow");

            var collider = root.AddComponent<BoxCollider>();
            collider.size = new Vector3(1.6f, 0.2f, 1.6f);

            return root;
        }

        private static GameObject CreateWallStash()
        {
            GameObject root = new GameObject("WallStash");
            AddPrimitive(PrimitiveType.Cube, root.transform, Vector3.zero, new Vector3(1f, 1f, 0.08f), Vector3.zero, "Panel");
            AddPrimitive(PrimitiveType.Cube, root.transform, new Vector3(0f, 0f, -0.03f), new Vector3(0.7f, 0.7f, 0.02f), Vector3.zero, "Recess");
            return root;
        }

        private static GameObject AddPrimitive(PrimitiveType type, Transform parent, Vector3 localPosition, Vector3 localScale, Vector3 localEuler, string name)
        {
            GameObject go = GameObject.CreatePrimitive(type);
            if (!string.IsNullOrEmpty(name))
            {
                go.name = name;
            }
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localEulerAngles = localEuler;
            go.transform.localScale = localScale;
            return go;
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
