using System;
using System.Linq;
using AI.GOAP.Behaviors;
using AI.GOAP.Config;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public class CollectKeysAction : ActionBase<CollectKeysAction.Data>, IInjectable
    {
        private const int KeyColliders = 3;
        private KeysConfigSO KeysConfig;
        Collider[] Colliders = new Collider[KeyColliders];

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = KeysConfig.CollectKeysActionTimer;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;
            Array.Clear(Colliders, 0, Colliders.Length);

            if (data.Target != null && !(data.KeysCollector.KeysRemaining <= 0)) {
                int results = Physics.OverlapSphereNonAlloc(agent.transform.position, KeysConfig.KeySearchRadius, Colliders, KeysConfig.KeyLayer);

                var filteredColliders = Colliders.Take(results).Where(c => c && c.gameObject.GetComponent<KeyController>().CanPickUpKey(agent.gameObject)).ToArray();
                results = filteredColliders.Length;
                Colliders = filteredColliders;

                if (results > 0) {
                    data.KeysCollector.KeyNotFound = false;
                    data.Target = new TargetWrapper(Colliders[0].transform);
                    
                    return data.Timer < 0 ? ActionRunState.Stop : ActionRunState.Continue;
                }
            }

            data.KeysCollector.KeyNotFound = true;
            return ActionRunState.Stop;
        }


        public override void End(IMonoAgent agent, Data data)
        {
        }

        public void Inject(DependencyInjector injector)
        {
            KeysConfig = injector.KeysConfig;
        }

        public class Data : CommonData
        {
            [GetComponent] public KeyCollectorBehavior KeysCollector { get; set; }
        }

        private class TargetWrapper : ITarget
        {
            private Transform _transform;

            public TargetWrapper(Transform transform)
            {
                _transform = transform;
            }

            public Vector3 Position => _transform.position;
        }
    }
}