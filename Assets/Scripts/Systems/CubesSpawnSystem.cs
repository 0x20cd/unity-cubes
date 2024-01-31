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
            state.Enabled = false;
            var cubesEntity = SystemAPI.GetSingletonEntity<Cubes>();
            var cubesData = state.EntityManager.GetComponentData<Cubes>(cubesEntity);
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            for (var x = 0; x < cubesData.Size.x; ++x) {
                for (var y = 0; y < cubesData.Size.y; ++y) {
                    for (var z = 0; z < cubesData.Size.z; ++z) {
                        var newCube = ecb.Instantiate(cubesData.CubePrefab);
                        var position = cubesData.Step * new float3(x, y, z);

                        ecb.AddComponent(newCube, new Parent{Value = cubesEntity});
                        ecb.SetComponent(newCube, LocalTransform.FromPosition(position));
                        /*{
                            Position = position,
                            Rotation = quaternion.identity,
                            Scale = 1
                            //Value = new float4x4(quaternion.identity, position)
                        });*/
                    }
                }
            }

            ecb.Playback(state.EntityManager);
        }
    }
}
