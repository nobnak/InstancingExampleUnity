using UnityEngine;
using System.Collections;

namespace nobnak.Timer {

	public class FPSMeter : MonoBehaviour {
		public float interval = 1f;
		public int minFrames = 100;
		public Rect guiArea = new Rect(5f, 5f, 150f, 100f);

		private float _prevTime;
		private int _updateCount;

		private float _currFPSOfUpdates;

		void Start() {
			_prevTime = Time.timeSinceLevelLoad;
		}

		void OnGUI() {
			GUILayout.BeginArea(guiArea);
			GUILayout.BeginVertical();

			GUILayout.Label(string.Format("{0:f1} fps ", _currFPSOfUpdates));

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		void Update() {
			_updateCount++;
			var t = Time.timeSinceLevelLoad;
			var dt = t - _prevTime;
			if (dt >= interval && _updateCount >= minFrames) {
				_currFPSOfUpdates = _updateCount / dt;

				_prevTime = t;
				_updateCount = 0;
			}
		}
	}
}