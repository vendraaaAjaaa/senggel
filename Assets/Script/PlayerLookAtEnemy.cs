using UnityEngine;

public class PlayerLookAtEnemy : MonoBehaviour
{
    public Transform playerTransform;  // Transform karakter pemain
    public Transform enemyTransform;   // Transform karakter musuh

    void Update()
    {
        // Pastikan bahwa playerTransform dan enemyTransform telah diatur
        if (playerTransform == null || enemyTransform == null)
        {
            Debug.LogError("Player atau Enemy Transform belum diatur pada PlayerLookAtEnemy!");
            return;
        }

        // Menghitung arah ke musuh
        Vector3 directionToEnemy = enemyTransform.position - playerTransform.position;
        directionToEnemy.y = 0f; // Mengabaikan perubahan tinggi

        // Menghitung rotasi untuk menghadap musuh
        Quaternion toRotation = Quaternion.LookRotation(directionToEnemy);

        // Merotasi pemain menuju musuh
        playerTransform.rotation = toRotation;
    }
}
