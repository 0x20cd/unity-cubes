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
using Unity.Physics;
using Unity.Physics.Systems;

namespace CubesProject
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct RaycastSystem : ISystem
    {
        EntityQuery singleCubeQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Cubes>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
            singleCubeQuery = state.GetEntityQuery(ComponentType.ReadWrite<SingleCube>());
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            const float rayDistance = 200f;

            if (Input.GetMouseButton(0)) {
                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.Log(Input.mousePosition.ToString() + " " + ray.direction.ToString());

                Entity entity = Raycast(ray.origin, ray.direction * rayDistance);
                if (entity != Entity.Null) {
                    state.EntityManager.DestroyEntity(entity);
                }
            }
        }

        [BurstCompile]
        private Entity Raycast(float3 fromPos, float3 toPos)
        {
            // Debug.Log(fromPos.ToString());
            // Debug.Log(toPos.ToString());
            PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            CollisionWorld collisionWorld = physicsWorldSingleton.PhysicsWorld.CollisionWorld;

            RaycastInput raycastInput = new RaycastInput {
                Start = fromPos,
                End = toPos,
                Filter = new CollisionFilter {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u,
                    GroupIndex = 0
                }
            };

            Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();

            if (collisionWorld.CastRay(raycastInput, out raycastHit)) {
                return physicsWorldSingleton.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            }

            return Entity.Null;
        }
    }
}
