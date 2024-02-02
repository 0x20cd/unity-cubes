using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine.Rendering;
using Unity.Collections;

namespace CubesProject
{
    public class CubesMono : MonoBehaviour
    {
        public int3 Size;
        public float3 Step;
        public float3 RotationSpeed;
        public GameObject CubePrefab;
        public Material[] Materials;
    }

    public class CubesBaker : Baker<CubesMono>
    {
        public override void Bake(CubesMono authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            var cubesComponent = new Cubes{
                Size = authoring.Size,
                Step = authoring.Step,
                RotationSpeed = authoring.RotationSpeed,
                CubePrefab = GetEntity(authoring.CubePrefab, new TransformUsageFlags())
            };

            var hybridRenderer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
            foreach(Material material in authoring.Materials) {
                cubesComponent.Materials.Add(hybridRenderer.RegisterMaterial(material));
            }

            AddComponent(entity, cubesComponent);
            AddComponent(entity, new EventFlags{
                IsSpawnRequested = false,
                IsDestroyRequested = false,
                IsMaterialChangeRequested = false,
                MaterialIndex = -1
            });
        }
    }
}