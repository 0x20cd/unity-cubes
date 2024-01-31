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
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (cube, transform) in SystemAPI.Query<RefRO<SingleCube>, RefRW<LocalTransform>>()) {
                transform.ValueRW = transform.ValueRO.Rotate(quaternion.EulerXYZ(cubesData.RotationSpeed * deltaTime));
            }
        }
    }
}
