using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject player;
    public Sprite Up;
    public Sprite Down;
    public Sprite Left;
    public Sprite Right;
    public Animator animator;
    public int speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        player = (GameObject)this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /* Передвижение WASD +  стрелочки */
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                player.transform.position += player.transform.up * speed * Time.deltaTime;
                player.GetComponent<SpriteRenderer>().sprite = Up;
                animator.SetTrigger("MoveUp");
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                player.transform.position -= player.transform.up * speed * Time.deltaTime;
                player.GetComponent<SpriteRenderer>().sprite = Down;
                animator.SetTrigger("MoveDown");
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                player.transform.position -= player.transform.right * speed * Time.deltaTime;
                player.GetComponent<SpriteRenderer>().sprite = Left;
                animator.SetTrigger("MoveLeft");
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                player.transform.position += player.transform.right * speed * Time.deltaTime;
                player.GetComponent<SpriteRenderer>().sprite = Right;
                animator.SetTrigger("MoveRight");
            }
        }
    }
}
