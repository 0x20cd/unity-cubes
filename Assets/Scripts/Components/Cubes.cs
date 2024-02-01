using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine.Rendering;
using Unity.Collections;

namespace CubesProject
{
    public struct Cubes : IComponentData
    {
        public int3 Size;
        public float3 Step;
        public float3 RotationSpeed;
        public Entity CubePrefab;
        public FixedList32Bytes<BatchMaterialID> Materials;
    }

    public struct EventFlags : IComponentData
    {
        public bool IsSpawnRequested;
        public bool IsMaterialChangeRequested;
        public int MaterialIndex;
    }

    public struct SingleCube : IComponentData {}
}