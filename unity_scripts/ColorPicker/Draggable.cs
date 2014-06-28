using UnityEngine;

public class Draggable : MonoBehaviour
{
	public bool fixX;
	public bool fixY;
	public Transform thumb;	
	bool dragging;

	void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(1)) {
			dragging = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
			RaycastHit hit;
			if (collider.Raycast(ray, out hit, 1000)) {
				dragging = true;
			}
		}

        if (Input.GetMouseButtonUp(1))
        {
            dragging = false;
        }

		if (dragging && Input.GetMouseButton(1)) {
			var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			point = collider.ClosestPointOnBounds(point);
			SetThumbPosition(point);
			SendMessage("OnDrag", Vector3.one - (thumb.position - collider.bounds.min) / collider.bounds.size.x);
		}
	}

	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * collider.bounds.size.x + collider.bounds.min;
		SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
		thumb.position = new Vector3(fixX ? thumb.position.x : point.x, fixY ? thumb.position.y : point.y, thumb.position.z);
	}
}
