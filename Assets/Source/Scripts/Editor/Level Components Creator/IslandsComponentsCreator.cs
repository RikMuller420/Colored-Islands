using System.Collections.Generic;
using SlimeGround.Core.CameraSystem;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using UnityEditor;
using UnityEngine;

namespace SlimeGround.Editor.LevelComponentsCreator
{
    public class IslandsComponentsCreator
    {
        private const string CameraTargtesObjectName = "Camera Targets";
        private const string BoundsObjectName = "Bounds";
        private const string CenterPointName = "Center Point";

        private Vector3 _verticalLookAtPosition = new Vector3(0f, 0f, -2f);
        private Vector3 _verticalFollowPosition = new Vector3(0, 8.5f, -6.5f);

        private Vector3 _horizontalLookAtPosition = new Vector3(0f, 0f, -1.2f);
        private Vector3 _horizontalFollowPosition = new Vector3(0, 8.5f, -4.5f);

        private Vector3 _islandCenterPointLocalPosition = new Vector3(0, 0.5f, 0);

        private Mesh _cubeMesh;

        public IslandsComponentsCreator(Mesh cubeMesh)
        {
            _cubeMesh = cubeMesh;
        }

        public IReadOnlyCollection<IslandInitializer> CreateRequireComponents(Transform islandsParent)
        {
            List<IslandInitializer> islandInitializers = new List<IslandInitializer>();

            foreach (Transform child in islandsParent)
            {
                if (child.TryGetComponent<MeshRenderer>(out _) == false ||
                    child.name == BoundsObjectName || child.name == CameraTargtesObjectName)
                {
                    continue;
                }

                IslandInitializer initializer = child.GetComponent<IslandInitializer>();

                if (child.TryGetComponent<Collider>(out _) == false)
                {
                    child.gameObject.AddComponent<MeshCollider>();
                    EditorUtility.SetDirty(child.gameObject);
                }

                if (child.TryGetComponent<Island>(out _) == false)
                {
                    Island island = child.gameObject.AddComponent<Island>();
                    TryCreateIslandCenterPoint(island);
                    EditorUtility.SetDirty(child.gameObject);
                }

                if (initializer == null)
                {
                    initializer = child.gameObject.AddComponent<IslandInitializer>();
                    EditorUtility.SetDirty(child.gameObject);
                }

                if (initializer != null)
                {
                    islandInitializers.Add(initializer);
                }
            }

            if (islandsParent.TryGetComponent<Level>(out _) == false)
            {
                islandsParent.gameObject.AddComponent<Level>();
            }

            Level levelInitializer = islandsParent.GetComponent<Level>();
            levelInitializer.SetIslands(islandInitializers);
            TryCreateLevelBounds(levelInitializer);
            TryCeateCameraTarget(levelInitializer);

            return islandInitializers.AsReadOnly();
        }

        private void TryCreateIslandCenterPoint(Island island)
        {
            if (island.CenterPoint != null)
            {
                return;
            }

            Transform existCenterPoint = island.transform.Find(CenterPointName);

            if (existCenterPoint != null)
            {
                island.SetCenterPoint(existCenterPoint);

                return;
            }

            GameObject centerPoint = new GameObject(CenterPointName);
            centerPoint.transform.parent = island.transform;
            centerPoint.transform.localPosition = _islandCenterPointLocalPosition;
            island.SetCenterPoint(centerPoint.transform);
        }

        private void TryCreateLevelBounds(Level levelInitializer)
        {
            if (levelInitializer.LevelBounds != null)
            {
                return;
            }

            GameObject boundObject = new GameObject(BoundsObjectName);
            boundObject.transform.localPosition = new Vector3(0f, 0f, -1f);
            boundObject.transform.localScale = new Vector3(5f, 0.5f, 10f);

            MeshRenderer meshRenderer = boundObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            MeshFilter meshFilter = boundObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = _cubeMesh;

            levelInitializer.SetLevelBounds(meshRenderer);
        }

        private void TryCeateCameraTarget(Level levelInitializer)
        {
            if (levelInitializer.CameraTargetsHorizontal != null && levelInitializer.CameraTargetsVertical != null)
            {
                return;
            }

            GameObject targetsHolderObject = new GameObject(CameraTargtesObjectName);
            Transform targetsParent = targetsHolderObject.transform;
            targetsParent.parent = levelInitializer.transform;


            Transform lookAtVertical = CrateCameraTrget("Look At Point Vertical", _verticalLookAtPosition, targetsParent);
            Transform followTargetVertical = CrateCameraTrget("Follow Target Vertical", _verticalFollowPosition, targetsParent);
            CameraTargets verticalCameraTargets = new CameraTargets(lookAtVertical, followTargetVertical);

            Transform lookAtHorizontal = CrateCameraTrget("Look At Point Horizontal", _horizontalLookAtPosition, targetsParent);
            Transform followTargetHorizontal = CrateCameraTrget("Follow Target Horizontal", _horizontalFollowPosition, targetsParent);
            CameraTargets horizontalCameraTargets = new CameraTargets(lookAtVertical, followTargetVertical);

            levelInitializer.SetCameraTargets(verticalCameraTargets, horizontalCameraTargets);
        }

        private Transform CrateCameraTrget(string name, Vector3 localPosition, Transform parent)
        {
            GameObject lookAtPoint = new GameObject(name);
            Transform targetTransform = lookAtPoint.transform;
            targetTransform.parent = parent;
            targetTransform.localPosition = localPosition;

            return targetTransform;
        }
    }
}