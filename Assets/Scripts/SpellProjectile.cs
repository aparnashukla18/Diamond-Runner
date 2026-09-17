using TMPro;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public SpellEnum elementType;

    public float speed;
    public float lifeTime;

    public float timer;

    void Start()
    {
       timer = lifeTime; 
    }
   void Update()
   {
        
   }
}
