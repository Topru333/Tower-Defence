using UnityEngine;

// Класс неиграбельных персонажей
public class NPC : MonoBehaviour {

    int Health              = 100; // Жизни NPC
    int PlayerExperience    = 10;  // Опыт получаемый игроком при смерти NPC
    int MainTowerDamage     = 10;  // Урон, который NPC наносит главной башне по достижении конца пути

    float Speed             = 1;   // Скорость передвижения NPC 
    float Radius            = 2;   // Радиус NPC

    Edge    CurrentMovementEdge = null; // Ребро по которому движется NPC
    Vector2 CurrentMovementDir = Vector2.zero;
    public Texture2D Icon;

    // Инициализирует основные характеристики NPC
    public virtual void Awake () {
		
	}

    // Обновление
    public virtual void Update () {
        Move();
    }

    // Отрисовка информации для редактора.
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color( Color.cyan.r,Color.cyan.g,Color.cyan.b,0.5f);
        Gizmos.DrawSphere(transform.position, Radius);
       // Gizmos.DrawIcon(transform.position,)
    }

    // Перемещает персонажа по текущему ребру
    void Move()
    {
        // TODO: Реализация.
        if (CurrentMovementEdge==null)
        {
            // TODO: Уменьшать жизни главной башни.
        }
    }

    // Наносит урон персонажу.
    void DoDamage(int damagePoints)
    {
        Health -= damagePoints;
        Debug.Log(string.Format("Отладка:{0}: NPC нанесен урон - {1}, значение жизней - {2}.", name, damagePoints, Health));
        if (Health <= 0)
            Death();
    }

    // Убивает персонажа, повышает опыт игрока
    void Death()
    {
        Debug.Log(string.Format("Отладка:{0}: NPC умер.", name));
        // TODO: Повышать опыт игрока.
    }

}
