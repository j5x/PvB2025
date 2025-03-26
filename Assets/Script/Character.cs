using UnityEngine;

namespace Script
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected string characterName;
        [SerializeField] protected HealthConfig healthConfig;

        public string Name => characterName;
        private HealthComponent HealthComponent { get; set; }
        public CharacterType CharacterType { get; protected set; }

        protected Animator animator;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        
            HealthComponent = gameObject.GetComponent<HealthComponent>();
            if (HealthComponent == null)
            {
                HealthComponent = gameObject.AddComponent<HealthComponent>();
            }
            HealthComponent.InitializeHealth(healthConfig);
            HealthComponent.OnDeath += Die;
        }

        protected abstract void Attack();
        protected abstract void Defend();

        public virtual void PerformAction(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Attack:
                    Attack();
                    break;
                case ActionType.Defend:
                    Defend();
                    break;
            }
        }

        public virtual void TakeDamage(float damage)
        {
            HealthComponent.TakeDamage((int)damage);
        }

        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }
    }
}