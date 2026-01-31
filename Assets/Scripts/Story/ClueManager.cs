using System;
using System.Collections.Generic;
using UnityEngine;
using TheTear.AR;
using TheTear.Factory;
using TheTear.Interaction;

namespace TheTear.Story
{
    public class ClueManager : MonoBehaviour
    {
        public event Action<ClueData> OnClueUnlocked;
        public event Action<bool> OnDeductionAvailabilityChanged;

        public StoryModel Story => story;
        public bool IsDeductionAvailable => deductionAvailable;
        public bool HasSpawned => spawned;

        private StoryModel story;
        private SceneRootController sceneRoot;
        private readonly Dictionary<string, ClueData> clueMap = new Dictionary<string, ClueData>();
        private readonly Dictionary<string, StoryObjectData> objectDataMap = new Dictionary<string, StoryObjectData>();
        private readonly Dictionary<string, GameObject> objectInstances = new Dictionary<string, GameObject>();
        private readonly Dictionary<string, string> objectToClue = new Dictionary<string, string>();
        private readonly HashSet<string> unlocked = new HashSet<string>();
        private readonly HashSet<string> revealed = new HashSet<string>();
        private readonly HashSet<string> eligible = new HashSet<string>();
        private bool spawned;
        private bool deductionAvailable;

        public void Initialize(StoryModel model, SceneRootController root)
        {
            story = model;
            sceneRoot = root;
            spawned = false;
            unlocked.Clear();
            revealed.Clear();
            eligible.Clear();
            clueMap.Clear();
            objectDataMap.Clear();
            objectToClue.Clear();
            objectInstances.Clear();

            if (story != null && story.clues != null)
            {
                foreach (var clue in story.clues)
                {
                    clueMap[clue.id] = clue;
                    if (!string.IsNullOrEmpty(clue.objectId))
                    {
                        objectToClue[clue.objectId] = clue.id;
                    }
                }
            }

            if (story != null && story.objects != null)
            {
                foreach (var obj in story.objects)
                {
                    objectDataMap[obj.id] = obj;
                    if (obj.startsVisible)
                    {
                        revealed.Add(obj.id);
                    }
                }
            }

            if (sceneRoot != null)
            {
                sceneRoot.SetActive(false);
            }

            RecomputeState();
        }

        public void SpawnClues()
        {
            if (spawned || story == null || sceneRoot == null)
            {
                return;
            }

            sceneRoot.SetActive(true);
            spawned = true;

            foreach (var obj in story.objects)
            {
                GameObject go = ClueFactory.Create(obj, sceneRoot.transform);
                objectInstances[obj.id] = go;
                bool visible = revealed.Contains(obj.id);
                go.SetActive(visible);

                if (objectToClue.TryGetValue(obj.id, out string clueId))
                {
                    var interactable = go.AddComponent<InteractableClue>();
                    interactable.Initialize(this, clueId);
                }
            }
        }

        public bool IsUnlocked(string clueId)
        {
            return unlocked.Contains(clueId);
        }

        public bool IsEligible(string clueId)
        {
            if (!clueMap.TryGetValue(clueId, out ClueData clue))
            {
                return false;
            }
            if (unlocked.Contains(clueId))
            {
                return false;
            }
            if (clue.prerequisites == null || clue.prerequisites.Length == 0)
            {
                return true;
            }
            foreach (var prereq in clue.prerequisites)
            {
                if (!unlocked.Contains(prereq))
                {
                    return false;
                }
            }
            return true;
        }

        public bool TryUnlockClue(string clueId, string source)
        {
            if (!IsEligible(clueId))
            {
                return false;
            }

            if (!clueMap.TryGetValue(clueId, out ClueData clue))
            {
                return false;
            }

            unlocked.Add(clueId);

            if (clue.revealsObjectIds != null)
            {
                foreach (var revealId in clue.revealsObjectIds)
                {
                    revealed.Add(revealId);
                    if (objectInstances.TryGetValue(revealId, out GameObject revealObj))
                    {
                        revealObj.SetActive(true);
                    }
                }
            }

            RecomputeState();
            OnClueUnlocked?.Invoke(clue);
            return true;
        }

        public bool UnlockFirstEligibleFromJournal()
        {
            foreach (var clueId in eligible)
            {
                if (TryUnlockClue(clueId, "journal"))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<ClueData> GetAllClues()
        {
            return story != null ? story.clues : Array.Empty<ClueData>();
        }

        public IEnumerable<ClusterData> GetClusters()
        {
            return story != null ? story.clusters : Array.Empty<ClusterData>();
        }

        public int GetClusterUnlockedCount(string clusterId)
        {
            int count = 0;
            if (story == null || story.clusters == null)
            {
                return count;
            }
            foreach (var cluster in story.clusters)
            {
                if (cluster.id != clusterId || cluster.clueIds == null)
                {
                    continue;
                }
                foreach (var clueId in cluster.clueIds)
                {
                    if (unlocked.Contains(clueId))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public int GetClusterTotalCount(string clusterId)
        {
            if (story == null || story.clusters == null)
            {
                return 0;
            }
            foreach (var cluster in story.clusters)
            {
                if (cluster.id == clusterId)
                {
                    return cluster.clueIds != null ? cluster.clueIds.Length : 0;
                }
            }
            return 0;
        }

        private void RecomputeState()
        {
            eligible.Clear();
            if (story != null && story.clues != null)
            {
                foreach (var clue in story.clues)
                {
                    if (IsEligible(clue.id))
                    {
                        eligible.Add(clue.id);
                    }
                }
            }

            bool wasAvailable = deductionAvailable;
            deductionAvailable = ComputeDeductionAvailable();
            if (deductionAvailable != wasAvailable)
            {
                OnDeductionAvailabilityChanged?.Invoke(deductionAvailable);
            }
        }

        private bool ComputeDeductionAvailable()
        {
            if (story == null || story.clues == null)
            {
                return false;
            }

            bool allRequired = true;
            foreach (var clue in story.clues)
            {
                if (clue.required && !unlocked.Contains(clue.id))
                {
                    allRequired = false;
                    break;
                }
            }

            bool essentialsMet = true;
            if (story.essentials != null && story.essentials.Length > 0)
            {
                foreach (var essential in story.essentials)
                {
                    if (!unlocked.Contains(essential))
                    {
                        essentialsMet = false;
                        break;
                    }
                }
            }

            bool anyClusterComplete = false;
            if (story.clusters != null)
            {
                foreach (var cluster in story.clusters)
                {
                    if (cluster.clueIds == null || cluster.clueIds.Length == 0)
                    {
                        continue;
                    }
                    bool clusterComplete = true;
                    foreach (var clueId in cluster.clueIds)
                    {
                        if (!unlocked.Contains(clueId))
                        {
                            clusterComplete = false;
                            break;
                        }
                    }
                    if (clusterComplete)
                    {
                        anyClusterComplete = true;
                        break;
                    }
                }
            }

            return allRequired || (anyClusterComplete && essentialsMet);
        }
    }
}
