using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] float instantiatingPause;
    public List<GameObject> items= new List<GameObject>();
    bool instantiaed;
    float timer;
    void Update()
    {
        timer += 0.5f;
        if (!instantiaed)
        {
            StartCoroutine(InstantiatingItems());
        }
        foreach(GameObject item in items)
        {
            if (timer< 10)
            {
                item.GetComponent<CBitems>().conveyer = this;
                if (item.GetComponent<CBitems>().isfalling!=true)
                {
                    item.GetComponent<Rigidbody2D>().velocity += direction * speed * Time.deltaTime;
                }
                timer = 0;
            }
        }
    }
    IEnumerator InstantiatingItems()
    {
        if (!instantiaed)
        {
            GameObject instantiatedItem= Instantiate(prefabs[Random.Range(0,prefabs.Length)], transform.position, Quaternion.identity);
            items.Add(instantiatedItem);
            instantiaed = true;
        }
        
        yield return new WaitForSeconds(instantiatingPause);
        instantiaed= false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controls player))
        {
            player.transform.position+= new Vector3(direction.x, direction.y, 0)*Time.deltaTime;
        }
    }
}
