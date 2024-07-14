using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 direction;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] float instantiatingPause;
    [SerializeField] public List<GameObject> items= new List<GameObject>();
    bool instantiaed;
    float timer;
    Controls player;

    private void Start()
    {
        player=FindObjectOfType<Controls>();
    }
    void Update()
    {
        timer += 0.5f;
        if (!instantiaed)
        {
            StartCoroutine(InstantiatingItems());
        }
        for(int i = 0; i<items.Count; i++)
        {
            if (timer < 10)
            {
                if (items[i] == null) items.Remove(items[i]);
                else
                {
                    items[i].GetComponent<CBitems>().conveyer = this;
                    if (items[i].GetComponent<CBitems>().isfalling != true)
                    {
                        items[i].transform.position += direction * speed * Time.deltaTime;
                        if (items[i].GetComponent<CBitems>().playerOnItem == true)
                        {
                            player.transform.position += direction * speed * Time.deltaTime;
                        }
                    }
                    timer = 0;
                }
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
