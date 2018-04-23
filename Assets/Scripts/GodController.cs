using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GodController : MonoBehaviour
{
	public float		speed;
	public Transform	head;
	public float life = 100;

	NavMeshAgent	agent;

	void Start ()
	{
		GodEvent.god = this;
		agent = GetComponent< NavMeshAgent >();
	}
	
	void Update ()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

		input = Quaternion.AngleAxis(head.eulerAngles.y, Vector3.up) * input;

		agent.Move(input.normalized * speed);

		// transform.eulerAngles = new Vector3(transform.eulerAngles.x, head.eulerAngles.y, transform.eulerAngles.z);
	}

	public void ouch(float damage)
	{
		life -= damage;
		if (life <= 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	//     void OnGUI()
    // {
    //     Vector3 p = new Vector3();
    //     Camera  c = Camera.main;
    //     Event   e = Event.current;
    //     Vector2 mousePos = new Vector2();

    //     // Get the mouse position from Event.
    //     // Note that the y position from Event is inverted.
    //     mousePos.x = e.mousePosition.x;
    //     mousePos.y = c.pixelHeight - e.mousePosition.y;

    //     p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));

    //     GUILayout.BeginArea(new Rect(20, 20, 250, 120));
    //     GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
    //     GUILayout.Label("Mouse position: " + mousePos);
    //     GUILayout.Label("World position: " + p.ToString("F3"));
    //     GUILayout.EndArea();
    // }
}
