using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine.Rendering;

namespace CubesProject
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct MaterialChangeSystem : ISystem
    {
        EntityQuery singleCubeQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Cubes>();
            singleCubeQuery = state.GetEntityQuery(ComponentType.ReadWrite<SingleCube>());
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cubesEntity = SystemAPI.GetSingletonEntity<Cubes>();
            var eventFlagsData = state.EntityManager.GetComponentData<EventFlags>(cubesEntity);

            if (!eventFlagsData.IsMaterialChangeRequested)
                return;
            eventFlagsData.IsMaterialChangeRequested = false;

            var cubesData = state.EntityManager.GetComponentData<Cubes>(cubesEntity);

            eventFlagsData.MaterialIndex = (eventFlagsData.MaterialIndex + 1) % cubesData.Materials.Length;
            state.EntityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);

            foreach (var (cube, mmi) in SystemAPI.Query<RefRO<SingleCube>, RefRW<MaterialMeshInfo>>()) {
                mmi.ValueRW.MaterialID = cubesData.Materials[eventFlagsData.MaterialIndex];
            }
        }
    }
}
