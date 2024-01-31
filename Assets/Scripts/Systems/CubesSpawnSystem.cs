using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

namespace CubesProject
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct CubesSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Cubes>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var cubesEntity = SystemAPI.GetSingletonEntity<Cubes>();
            var cubesData = state.EntityManager.GetComponentData<Cubes>(cubesEntity);
            var eventFlagsData = state.EntityManager.GetComponentData<EventFlags>(cubesEntity);

            if (!eventFlagsData.IsSpawnRequested)
                return;
            eventFlagsData.IsSpawnRequested = false;
            state.EntityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            ecb.DestroyEntity(state.GetEntityQuery(ComponentType.ReadWrite<SingleCube>()), EntityQueryCaptureMode.AtRecord);

            for (var x = 0; x < cubesData.Size.x; ++x) {
                for (var y = 0; y < cubesData.Size.y; ++y) {
                    for (var z = 0; z < cubesData.Size.z; ++z) {
                        var newCube = ecb.Instantiate(cubesData.CubePrefab);
                        var position = cubesData.Step * new float3(x, y, z);

                        ecb.AddComponent(newCube, new SingleCube{});
                        ecb.AddComponent(newCube, new Parent{Value = cubesEntity});
                        ecb.SetComponent(newCube, LocalTransform.FromPosition(position));
                    }
                }
            }

            ecb.Playback(state.EntityManager);
        }
    }
}
