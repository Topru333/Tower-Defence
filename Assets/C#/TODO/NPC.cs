using UnityEngine;
namespace TD
{
    // Класс неиграбельных персонажей
    public class NPC : MonoBehaviour
    {

        int Health = 100; // Жизни NPC
        int PlayerExperience = 10;  // Опыт получаемый игроком при смерти NPC
        int MainTowerDamage = 10;  // Урон, который NPC наносит главной башне по достижении конца пути

        float Speed = 2;   // Скорость передвижения NPC 
        float Radius = 0.5f;   // Радиус NPC

        Edge CurrentMovementEdge = null; // Ребро по которому движется NPC
        Vector2 CurrentMovementDir = Vector2.zero;
        public Texture2D Icon;

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
                LevelManager.Instance.mainTowerLifeCount -= MainTowerDamage;
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
            Health -= damagePoints;
            Debug.Log(string.Format("Отладка:{0}: NPC нанесен урон - {1}, значение жизней - {2}.", name, damagePoints, Health));
            if (Health <= 0)
                Death();
        }

        // Убивает персонажа, повышает опыт игрока
        void Death()
        {
            Debug.Log(string.Format("Отладка:{0}: NPC умер.", name));
            LevelManager.Instance.pointsOnLvl += PlayerExperience;
            Destroy(gameObject);
        }

        // Меняет текущюю дугу движения.
        public void SetMovementEdge(Edge edge)
        {
            CurrentMovementEdge = edge;
        }

    }
}