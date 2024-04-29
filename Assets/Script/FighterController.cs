using UnityEngine;

public class FighterController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Kecepatan gerak karakter
    public float rotationSpeed = 500f;  // Kecepatan rotasi karakter

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mendapatkan input gerakan horizontal
        float horizontal = Input.GetAxis("Horizontal");

         // Mendapatkan input untuk pukulan dan blocking
        bool jab = Input.GetKeyDown(KeyCode.J);
        bool hook = Input.GetKeyDown(KeyCode.K);
        bool uppercut = Input.GetKeyDown(KeyCode.L);
        bool block = Input.GetKeyDown(KeyCode.Semicolon);

        // Mengatur rotasi karakter sesuai arah gerakan horizontal
        Vector3 movement = new Vector3(horizontal, 0f, 0f).normalized;
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
        }

        // Menggerakkan karakter
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Mengatur parameter animator untuk animasi berjalan
        animator.SetFloat("MoveSpeed", Mathf.Abs(horizontal));

        // Mengatur animasi pukulan
        if (jab)
        {
            animator.SetTrigger("Jab");
        }
        else if (hook)
        {
            animator.SetTrigger("Hook");
        }
        else if (uppercut)
        {
            animator.SetTrigger("Uppercut");
        }

        // Mengatur animasi blocking
        if (block)
        {
            animator.SetBool("Block", true);
        }
        else
        {
            animator.SetBool("Block", false);
        }
    }
}
