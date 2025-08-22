using UnityEngine;

public class Moneda : MonoBehaviour
{
    [SerializeField]
    private int cantidad;

    private bool cursorIn;
    private bool inside;

    [SerializeField]
    private float radio;
    [SerializeField]
    private LayerMask mask;

    private void OnMouseEnter()
    {
        cursorIn = true;
    }
    private void OnMouseExit() 
    {
        cursorIn = false;
    }

    private void Update()
    {
        inside = Physics.CheckSphere(transform.position, radio,mask);

        if(Input.GetKeyDown(KeyCode.E) && cursorIn && inside)
        {
            GameManager.Instance.dinero += cantidad;
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position,radio);
    }
}
