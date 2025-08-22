using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Conversacion : MonoBehaviour
{
    [SerializeField]
    private float VelocidadText;

    [SerializeField]
    private Dialogos[] dialogio;

    [SerializeField]
    private GameObject sistemaDialogos;
    [SerializeField]
    private Image caja;
    [SerializeField]
    private Image per1;
    [SerializeField]
    private Image per2;
    [SerializeField]
    private TextMeshProUGUI nombre;
    [SerializeField]
    private TextMeshProUGUI texto;

    private bool check;

    private bool inside;
    [SerializeField]
    private float radio;
    [SerializeField]
    private LayerMask playerLayer;

    private bool active = false;

    private int lines = 0;

    private IEnumerator co;

    private void Start()
    {
        co = PrintText(null);
    }


    private void OnMouseEnter()
    {
        check = true;
    }

    private void OnMouseExit()
    {
        check = false;
    }

    private void Update()
    {
        inside = Physics.CheckSphere(transform.position, radio, playerLayer);

        if (Input.GetKeyDown(KeyCode.E) && check && inside && !active && dialogio.Length > lines)
        {
            sistemaDialogos.SetActive(true);
            nombre.text = dialogio[lines].nombre;
            co = PrintText(dialogio[lines].texto);
            StartCoroutine(co);
            texto.text = dialogio[lines].texto;
            caja.sprite = dialogio[lines].box;
            per1.sprite = dialogio[lines].personaje1;
            per2.sprite = dialogio[lines].personaje2;
            lines++;
        }
        else if (Input.GetKeyDown(KeyCode.E) && check && inside && active && dialogio.Length > lines)
        {
            active = false;
            StopCoroutine(co);
            sistemaDialogos.SetActive(true);
            nombre.text = dialogio[lines].nombre;
            co = PrintText(dialogio[lines].texto);
            StartCoroutine(co);
            caja.sprite = dialogio[lines].box;
            per1.sprite = dialogio[lines].personaje1;
            per2.sprite = dialogio[lines].personaje2;
            lines++;

        }
        else if (Input.GetKeyDown(KeyCode.E) && check && inside)
        {
            lines = 0;
            nombre.text = null;
            texto.text = null;
            caja.sprite = null;
            per1.sprite = null;
            per2.sprite = null;
            sistemaDialogos.SetActive(false);

        }
    }

    private IEnumerator PrintText(string palabras)
    {
        active = true;
        for (int i = 0; i <= palabras.Length; i++)
        {
            texto.text = palabras.Substring(0, i);
            yield return new WaitForSeconds(VelocidadText);
        }
        active = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
