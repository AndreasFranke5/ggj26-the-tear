using System;
using UnityEngine;

namespace TheTear.Story
{
    [Serializable]
    public class StoryModel
    {
        public string caseId;
        public string title;
        public string victim;
        public DeductionSolution solution;
        public string[] culprits;
        public string[] methods;
        public string[] motives;
        public string[] essentials;
        public ClusterData[] clusters;
        public StoryObjectData[] objects;
        public ClueData[] clues;
    }

    [Serializable]
    public class DeductionSolution
    {
        public string culprit;
        public string method;
        public string motive;
    }

    [Serializable]
    public class StoryObjectData
    {
        public string id;
        public string label;
        public string recipe;
        public string mode;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale;
        public bool startsVisible;
    }

    [Serializable]
    public class ClueData
    {
        public string id;
        public string title;
        public string summary;
        public string objectId;
        public string[] prerequisites;
        public string[] revealsObjectIds;
        public string clusterId;
        public bool required;
    }
}
