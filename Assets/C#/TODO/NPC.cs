using UnityEngine;
namespace TD
{
    // Класс неиграбельных персонажей
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        int Health = 100; // Жизни NPC
        [SerializeField]
        int PlayerExperience = 10;  // Опыт получаемый игроком при смерти NPC
        [SerializeField]
        int GoldGiven = 10;  // Деньги получаемые игроком при смерти NPC
        [SerializeField]
        int MainTowerDamage = 10;  // Урон, который NPC наносит главной башне по достижении конца пути

        [SerializeField]
        float Speed = 2;   // Скорость передвижения NPC 
        [SerializeField]
        float Radius = 0.5f;   // Радиус NPC

        Edge CurrentMovementEdge = null; // Ребро по которому движется NPC
        Vector2 CurrentMovementDir = Vector2.zero;
        public Texture2D Icon;
        bool isDead = false;

        // Инициализирует основные характеристики NPC
        public virtual void Awake()
        {

        }

        // Обновление
        public virtual void Update()
        {
            Move();
        }

        // Отрисовка информации для редактора.
        public virtual void OnDrawGizmos()
        {
            Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
            Gizmos.DrawSphere(transform.position, Radius);
            // Gizmos.DrawIcon(transform.position,)
        }

        // Перемещает персонажа по текущему ребру
        void Move()
        {
            var pathSystem = PathSystem.Instance;
            var points = pathSystem.Points;


            if (CurrentMovementEdge == null)
            {
                LevelManager.Instance.CurrentLevel.DamageMainTower(MainTowerDamage);
                LevelManager.Instance.CurrentLevel.DecreaseNPC_Counter();
#if DEBUG
                Debug.Log(string.Format("Отладка:{0}: NPC нанес урон главной башне - {1}.", name, MainTowerDamage));
#endif
                Destroy(gameObject);
            }
            else
            {
                if (CurrentMovementEdge != null)
                {
                    Vector3 outPoint = points[CurrentMovementEdge.ID_out - 1].Position;
                    outPoint.z = outPoint.y;
                    outPoint.y = 0;
                    if (Vector3.Distance(transform.position, outPoint) - Radius * 0.4f < 0)
                        CurrentMovementEdge = pathSystem.GetNextEdge(CurrentMovementEdge);
                    transform.Translate((outPoint - transform.position).normalized * Time.deltaTime * Speed);
                }
            }
        }

        // Наносит урон персонажу.
        public void DoDamage(int damagePoints)
        {
            if (damagePoints <= 0)
                throw new System.ArgumentOutOfRangeException();
            if (isDead) return;
            Health -= damagePoints;
#if DEBUG
            Debug.Log(string.Format("Отладка:{0}: NPC нанесен урон - {1}, значение жизней - {2}.", name, damagePoints, Health));
#endif
            if (Health <= 0)
                Death();
        }

        // Убивает персонажа, дает игроку денег
        void Death()
        {
            if (isDead) return;
#if DEBUG
            Debug.Log(string.Format("Отладка:{0}: NPC умер.", name));
#endif
            isDead = true;
            var currentLvl = LevelManager.Instance.CurrentLevel;
            currentLvl.DecreaseNPC_Counter();
            currentLvl.GiveExperience(PlayerExperience);
            currentLvl.GiveMoney(GoldGiven);
            Destroy(gameObject);
        }

        // Меняет текущюю дугу движения.
        public void SetMovementEdge(Edge edge)
        {
            CurrentMovementEdge = edge;
        }

    }
}