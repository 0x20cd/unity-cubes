using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace CubesProject
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct CubesDestroySystem : ISystem
    {
        Random RandomValue;
        EntityQuery SingleCubeQuery;
        const int CountToDestroy = 500;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Cubes>();
            SingleCubeQuery = state.GetEntityQuery(ComponentType.ReadWrite<SingleCube>());
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cubesEntity = SystemAPI.GetSingletonEntity<Cubes>();
            var cubesData = state.EntityManager.GetComponentData<Cubes>(cubesEntity);
            var eventFlagsData = state.EntityManager.GetComponentData<EventFlags>(cubesEntity);

            if (!eventFlagsData.IsDestroyRequested)
                return;
            eventFlagsData.IsDestroyRequested = false;
            state.EntityManager.SetComponentData(cubesEntity, eventFlagsData);

            var array = SingleCubeQuery.ToEntityArray(Allocator.Temp);

            if (array.Length <= CountToDestroy) {
                state.EntityManager.DestroyEntity(array);
            } else {
                state.EntityManager.DestroyEntity(array.GetSubArray(0, CountToDestroy));
            }
        }
    }
}
