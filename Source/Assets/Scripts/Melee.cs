using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    public GameObject meleeDir;
    public float meleeTime;
    public AudioClip clip;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        meleeDir.SetActive(false);
        animator = GameObject.Find("Gun_sprite").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        
       Vector2 mousePos = (Vector2) Input.mousePosition;
       Vector2 mouseWorldPos = (Vector2) Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
       Vector2 direction = (mouseWorldPos - (Vector2)gameObject.transform.position).normalized;

       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
       
       meleeDir.transform.rotation = Quaternion.Euler(0f, 0f, angle-90);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Melee");
            meleeDir.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(clip, 0.5f);
            animator.SetTrigger("Melee");
            StartCoroutine(StopMelee());
        }
    }



    IEnumerator StopMelee()
    {
        yield return new WaitForSeconds(meleeTime);
        meleeDir.SetActive(false);
    }
}
