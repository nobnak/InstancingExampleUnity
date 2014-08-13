using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Instancing {

	public class MeshInstancing : MonoBehaviour {
		public const string CS_INDEX_BUFFER = "indexBuf";
		public const string CS_VERTEX_BUFFER = "vertexBuf";
		public const string CS_UV_BUFFER = "uvBuf";
		public const string CS_WORLD_BUFFER = "worldBuf";

		public GameObject hanafab;
		public int nHanas;

		private ComputeBuffer _indexBuf;
		private ComputeBuffer _vertexBuf;
		private ComputeBuffer _uvBuf;
		private ComputeBuffer _worldBuf;
		private float[] _worlds;
		private Transform[] _trs;
		private Material _mat;

		void OnDestroy() {
			_indexBuf.Release();
			_vertexBuf.Release();
			_uvBuf.Release();
			_worldBuf.Release();
		}

		void Awake() {
			var mf = hanafab.GetComponent<MeshFilter>();
			var mesh = mf.sharedMesh;

			_indexBuf = new ComputeBuffer(mesh.triangles.Length, Marshal.SizeOf(typeof(uint)));
			_indexBuf.SetData(mesh.triangles);
			
			_vertexBuf = new ComputeBuffer(mesh.vertices.Length, Marshal.SizeOf(typeof(Vector3)));
			_vertexBuf.SetData(mesh.vertices);

			_uvBuf = new ComputeBuffer(mesh.uv.Length, Marshal.SizeOf(typeof(Vector2)));
			_uvBuf.SetData(mesh.uv);

			var gofab = new GameObject("Position");
			gofab.hideFlags = HideFlags.HideAndDontSave;
			_trs = GenerateRandom(gofab, nHanas);
			_worlds = new float[16 * _trs.Length];
			_worldBuf = new ComputeBuffer(_trs.Length, 4 * 16);
			UpdateWorlds();

			_mat = new Material(hanafab.renderer.sharedMaterial);
			_mat.SetBuffer(CS_INDEX_BUFFER, _indexBuf);
			_mat.SetBuffer(CS_VERTEX_BUFFER, _vertexBuf);
			_mat.SetBuffer(CS_UV_BUFFER, _uvBuf);
			_mat.SetBuffer(CS_WORLD_BUFFER, _worldBuf);
		}

		void OnRenderObject() {
			UpdateRotations();
			UpdateWorlds();
			_mat.SetPass(0);
			Graphics.DrawProcedural(MeshTopology.Triangles, _indexBuf.count, _trs.Length);
		}

		void UpdateWorlds() {
			var c = 0;
			for (var i = 0; i < _trs.Length; i++) {
				var w = _trs[i].localToWorldMatrix;
				_worlds [c++] = w.m00;
				_worlds [c++] = w.m10;
				_worlds [c++] = w.m20;
				_worlds [c++] = w.m30;
				_worlds [c++] = w.m01;
				_worlds [c++] = w.m11;
				_worlds [c++] = w.m21;
				_worlds [c++] = w.m31;
				_worlds [c++] = w.m02;
				_worlds [c++] = w.m12;
				_worlds [c++] = w.m22;
				_worlds [c++] = w.m32;
				_worlds [c++] = w.m03;
				_worlds [c++] = w.m13;
				_worlds [c++] = w.m23;
				_worlds [c++] = w.m33;
			}
			_worldBuf.SetData(_worlds);
		}

		void UpdateRotations() {
			var rot = Quaternion.Euler(0f, 90f * Time.deltaTime, 0f);
			foreach (var tr in _trs) {
				tr.localRotation = rot * tr.localRotation;
			}
		}

		Transform[] GenerateRandom(GameObject prefab, int count) {
			var trs = new Transform[count];
			for (var i = 0; i < count; i++)
				trs[i] = Generate(prefab, 10f * Random.insideUnitSphere, Random.rotationUniform, Random.Range(0.7f, 2f) * Vector3.one);
			return trs;
		}

		Transform Generate(GameObject prefab, Vector3 localPosition, Quaternion localRotation, Vector3 localScale) {
			var go = (GameObject)Instantiate(prefab);
			go.transform.parent = transform;
			go.transform.localPosition = localPosition;
			go.transform.localRotation = localRotation;
			go.transform.localScale = localScale;
			return go.transform;
		}
	}
}