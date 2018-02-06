﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSign : MonoBehaviour {

	public static MiniMapSign instance = null;

	void Awake() {
		// Enforce singleton pattern
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		
		// Prevent status from being destroyed between levels
		DontDestroyOnLoad(gameObject);
	}
}