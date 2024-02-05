using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Jobs;

namespace CubesProject
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct CubesRotationSystem : ISystem
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

            state.Dependency.Complete();

            state.Dependency = new RotateJob() {
                RotationSpeed = cubesData.RotationSpeed,
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }
    }

    [BurstCompile]
    public partial struct RotateJob : IJobEntity
    {
        public float DeltaTime;
        public float3 RotationSpeed;

        public void Execute(in SingleCube cube, ref LocalTransform transform)
        {
            transform = transform.Rotate(quaternion.EulerXYZ(RotationSpeed * DeltaTime));
        }
    }
}
