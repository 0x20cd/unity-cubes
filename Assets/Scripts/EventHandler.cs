using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace CubesProject
{
    public class EventHandler : MonoBehaviour
    {
        EntityManager entityManager;
        Entity cubesEntity;

        public void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            cubesEntity = entityManager.CreateEntityQuery(typeof(Cubes)).GetSingletonEntity();
        }

        public void RequestSpawn()
        {
            var eventFlagsData = entityManager.GetComponentData<EventFlags>(cubesEntity);
            eventFlagsData.IsSpawnRequested = true;
            entityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);
        }

        public void RequestDestroy()
        {

        }

        public void RequestMaterialChange()
        {
            var eventFlagsData = entityManager.GetComponentData<EventFlags>(cubesEntity);
            eventFlagsData.IsMaterialChangeRequested = true;
            entityManager.SetComponentData<EventFlags>(cubesEntity, eventFlagsData);
        }
    }
}
