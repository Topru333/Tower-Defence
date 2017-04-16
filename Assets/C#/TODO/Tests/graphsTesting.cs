using UnityEngine;
using UnityEngine.PlaymodeTests;
using UnityEngine.Assertions;

[PlayModeTest]
public class graphsTesting : MonoBehaviour {

	void FixedUpdate () {
		//Use the Assert class to test conditions.
		//Then call Playmode.Pass to communicate the test has finished.
		PlaymodeTest.Pass();
	}
}
