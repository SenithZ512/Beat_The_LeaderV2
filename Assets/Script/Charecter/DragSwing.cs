using UnityEngine;

public class DragSwing : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;

        rb.gravityScale = 0;        // จับอยู่ = ไม่ตก
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    void OnMouseUp()
    {
        isDragging = false;

        rb.gravityScale = 7;        // ปล่อย = เปิดแรงโน้มถ่วง
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(Vector2.Lerp(rb.position, mousePos, 0.3f));
        }
    }
}