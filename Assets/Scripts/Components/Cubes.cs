using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace CubesProject
{
    public struct Cubes : IComponentData
    {
        public int3 Size;
        public float3 Step;
        public float3 RotationSpeed;
        public Entity CubePrefab;
    }

    public struct EventFlags : IComponentData
    {
        public bool IsSpawnRequested;
    }

    public struct SingleCube : IComponentData {}
}