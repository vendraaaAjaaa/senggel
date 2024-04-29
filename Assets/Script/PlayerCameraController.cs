using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform playerTransform;  // Transform karakter pemain
    public Transform enemyTransform;   // Transform karakter musuh
    public float rotationSpeed = 5f;   // Kecepatan rotasi kamera

    private CinemachineFreeLook playerCamera;

    void Start()
    {
        // Mendapatkan referensi ke CinemachineFreeLook
        playerCamera = GetComponent<CinemachineFreeLook>();
        
        // Memastikan bahwa playerTransform dan enemyTransform telah diatur
        if (playerTransform == null || enemyTransform == null)
        {
            Debug.LogError("Player atau Enemy Transform belum diatur pada PlayerCameraController!");
            return;
        }

        // Mengatur Follow dan Look At target kamera pada pemain
        playerCamera.Follow = playerTransform;
        playerCamera.LookAt = playerTransform;
    }

    void Update()
    {
        // Mengunci kamera pada musuh ketika pemain menekan tombol tertentu (misalnya, tombol mouse kanan)
        if (Input.GetMouseButton(1))
        {
            RotateCameraTowardsEnemy();
        }
    }

    void RotateCameraTowardsEnemy()
    {
        // Menghitung arah ke musuh
        Vector3 directionToEnemy = enemyTransform.position - transform.position;
        directionToEnemy.y = 0f; // Mengabaikan perubahan tinggi

        // Menghitung rotasi untuk menghadap musuh
        Quaternion toRotation = Quaternion.LookRotation(directionToEnemy);

        // Merotasi kamera menuju musuh dengan kecepatan tertentu
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
