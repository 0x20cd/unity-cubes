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
    public struct EventFlags : IComponentData
    {
        public bool IsSpawnRequested;
        public bool IsDestroyRequested;
        public bool IsMaterialChangeRequested;
        public int MaterialIndex;
    }
}