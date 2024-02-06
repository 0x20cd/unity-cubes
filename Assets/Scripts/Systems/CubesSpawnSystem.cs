using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Physics;

namespace CubesProject
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct CubesSpawnSystem : ISystem
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
            var ECBSystem = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();
            var cubesEntity = SystemAPI.GetSingletonEntity<Cubes>();
            var cubesData = state.EntityManager.GetComponentData<Cubes>(cubesEntity);
            var eventFlagsData = state.EntityManager.GetComponentData<EventFlags>(cubesEntity);

            state.Dependency.Complete();

            if (!eventFlagsData.IsSpawnRequested)
                return;

            var ECB = ECBSystem.CreateCommandBuffer(state.WorldUnmanaged);

            eventFlagsData.IsSpawnRequested = false;
            eventFlagsData.IsMaterialChangeRequested = true;
            eventFlagsData.MaterialIndex = -1;

            ECB.DestroyEntity(singleCubeQuery, EntityQueryCaptureMode.AtRecord);
            ECB.SetComponent(cubesEntity, eventFlagsData);

            state.Dependency = new SpawnJob() {
                CubesEntity = cubesEntity,
                CubesData = cubesData,
                ECB = ECB.AsParallelWriter()
            }.Schedule(cubesData.Size.x * cubesData.Size.y * cubesData.Size.z, 32, state.Dependency);
        }
    }

    [BurstCompile]
    public partial struct SpawnJob : IJobParallelFor
    {
        public Entity CubesEntity;
        public Cubes CubesData;
        public EntityCommandBuffer.ParallelWriter ECB;

        public void Execute(int index)
        {
            int x = index % CubesData.Size.x;
            int y = (index / CubesData.Size.x) % CubesData.Size.y;
            int z = (index / CubesData.Size.x / CubesData.Size.y) % CubesData.Size.z;

            var newCube = ECB.Instantiate(index, CubesData.CubePrefab);
            var position = CubesData.Step * new float3(x, y, z);

            ECB.AddComponent(index, newCube, new SingleCube{
                IsClicked = false,
                Point = new int3(x, y, z)
            });
            ECB.AddComponent(index, newCube, new Parent{Value = CubesEntity});
            ECB.SetComponent(index, newCube, LocalTransform.FromPosition(position));
        }
    }
}
