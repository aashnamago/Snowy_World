
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class VolumeUpDown {
	#if UNITY_IPHONE
	
		// Interface to native implementation
		
		[DllImport ("__Internal")]
		private static extern int _GetLastVolumePress();
		
		[DllImport ("__Internal")]
		private static extern void _Init();
		
		//Public interface for use inside C# / JS code
		
		public static int GetLastVolumePress() {
			return _GetLastVolumePress();
		}
		
		public static void Init() {
			_Init();
		}
	#endif
}

