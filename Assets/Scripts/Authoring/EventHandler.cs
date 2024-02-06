using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

namespace CubesProject
{
    public class EventHandler : MonoBehaviour
    {
        EntityManager entityManager;
        Entity cubesEntity;
        EntityQuery singleCubeQuery;

        public void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            cubesEntity = entityManager.CreateEntityQuery(typeof(Cubes)).GetSingletonEntity();
            singleCubeQuery = entityManager.CreateEntityQuery(typeof(SingleCube));
        }

        public void RequestSpawn()
        {
            var eventFlagsData = entityManager.GetComponentData<EventFlags>(cubesEntity);
            eventFlagsData.IsSpawnRequested = true;
            entityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);
        }

        public void RequestDestroy()
        {
            var eventFlagsData = entityManager.GetComponentData<EventFlags>(cubesEntity);
            eventFlagsData.IsDestroyRequested = true;
            entityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);
        }

        public void RequestMaterialChange()
        {
            var eventFlagsData = entityManager.GetComponentData<EventFlags>(cubesEntity);
            eventFlagsData.IsMaterialChangeRequested = true;
            entityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);
        }

        public void RequestClickedList()
        {
            /*var eventFlagsData = entityManager.GetComponentData<EventFlags>(cubesEntity);
            eventFlagsData.IsClickedListRequested = true;
            entityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);*/
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var entity in singleCubeQuery.ToEntityArray(Allocator.Temp)) {
                var singleCubeData = entityManager.GetComponentData<SingleCube>(entity);
                if (singleCubeData.IsClicked) {
                    Debug.Log(singleCubeData.Point);
                    singleCubeData.IsClicked = false;
                    ecb.SetComponent(entity, singleCubeData);
                }
            }

            ecb.Playback(entityManager);
        }
    }
}
