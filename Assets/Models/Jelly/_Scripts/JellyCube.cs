using UnityEngine;
using System.Collections;

public class JellyCube : MonoBehaviour {
	

	public GameObject[] Bones;

	public Vector3 Center;

	public SkinnedMeshRenderer SMRenderer;
	public Mesh playerBakedMesh;

	Vector3 vel;

	void Awake()
	{
		
	}

	void FixedUpdate()
	{
		for(int i=0; i < Bones.Length; i++)
		{
			Center += Bones[i].transform.position; 
		}

		Center = Center / Bones.Length;

		transform.position = Center;

		Center = Vector3.zero;

		BakeMeshAndAssignItToCollider();
	}

	void BakeMeshAndAssignItToCollider()
	{
		SMRenderer.BakeMesh(playerBakedMesh);

		gameObject.GetComponent<MeshCollider>().sharedMesh = playerBakedMesh;
	}
}
