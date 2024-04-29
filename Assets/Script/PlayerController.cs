using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Kecepatan gerak pemain
    public float rotationSpeed = 500f;  // Kecepatan rotasi pemain

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Membaca input gerakan
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Menggerakkan pemain
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Rotasi pemain sesuai arah gerakan
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Mengatur animasi berjalan
        float moveMagnitude = Mathf.Clamp01(movement.magnitude);
        animator.SetFloat("MoveSpeed", moveMagnitude);
    }
}
