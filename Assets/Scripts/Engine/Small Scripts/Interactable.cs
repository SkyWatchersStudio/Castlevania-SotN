using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject[] dropItems;
    public ParticleSystem candleDeath;

    public void OnInteract()
    {
        //choose a random item to drop...
        var item = Random.Range(0, dropItems.Length);

        bool checkInstantiate = true;

        if (dropItems[item] == null)
            checkInstantiate = false;
        else if (dropItems[item].name.Equals("Heart") && !Player.CubeOfZoe)
            checkInstantiate = false;

        if (checkInstantiate)
            Instantiate(dropItems[item], transform.position, Quaternion.identity);

        var death = Instantiate(candleDeath, transform.position, Quaternion.identity);
        death.Play();
        Destroy(gameObject);
    }
}
