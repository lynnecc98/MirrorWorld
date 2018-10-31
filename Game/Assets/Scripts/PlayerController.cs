using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;
    public float turn = 60f;
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * turn;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}