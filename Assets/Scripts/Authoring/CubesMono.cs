using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CubesProject
{
    public class CubesMono : MonoBehaviour
    {
        public int3 Size;
        public float3 Step;
        public float3 RotationSpeed;
        public GameObject CubePrefab;
    }

    public class CubesBaker : Baker<CubesMono>
    {
        public override void Bake(CubesMono authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Cubes{
                Size = authoring.Size,
                Step = authoring.Step,
                RotationSpeed = authoring.RotationSpeed,
                CubePrefab = GetEntity(authoring.CubePrefab, new TransformUsageFlags())
            });
        }
    }
}