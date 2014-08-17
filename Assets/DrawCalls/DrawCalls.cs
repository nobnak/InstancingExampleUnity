using UnityEngine;
using System.Collections;

namespace HanaInstancing {

	public class DrawCalls : MonoBehaviour {
		public GameObject prefab;
		public int count;

		private Transform[] _trs;

		void Start () {
			_trs = GeneratePositions(prefab, count, this.transform, 10, new Vector2(0.5f, 2f));
		}

		void Update() {
			UpdateRotations();
		}

		void UpdateRotations() {
			var rot = Quaternion.Euler(0f, 90f * Time.deltaTime, 0f);
			foreach (var tr in _trs) {
				tr.localRotation = rot * tr.localRotation;
			}
		}

		Transform[] GeneratePositions(GameObject prefab, int count, Transform parent, float range, Vector2 scale) {
			var trs = new Transform[count];
			for (var i = 0; i < count; i++) {
				var go = (GameObject)Instantiate(prefab);
				trs[i] = go.transform;
				go.transform.parent = parent;
				go.transform.localPosition = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0f);
				go.transform.localRotation = Random.rotationUniform;
				go.transform.localScale = Random.Range(scale.x, scale.y) * Vector3.one;
			}
			return trs;
		}
	}
}