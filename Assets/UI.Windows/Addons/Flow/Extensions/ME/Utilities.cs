﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ME {
	
	public partial class Utilities {
		
		private static Dictionary<string, double> cacheByTimeLastTime = new Dictionary<string, double>();
		private static Dictionary<string, object> cacheByTimeLastValue = new Dictionary<string, object>();
		
		public static T CacheByTime<T>(string name, System.Func<T> func, float timeout = 2f) {

#if UNITY_EDITOR
			var timeStamp = UnityEditor.EditorApplication.timeSinceStartup;
#else
			var timeStamp = Time.time;
#endif

			object value = null;
			double time;
			if (Utilities.cacheByTimeLastTime.TryGetValue(name, out time) == true) {
				
				if (timeStamp > time + timeout) {
					
					// Update value
					value = func();
					
					Utilities.cacheByTimeLastValue[name] = value;
					Utilities.cacheByTimeLastTime[name] = timeStamp;
					
				} else {
					
					// Return old value
					if (Utilities.cacheByTimeLastValue.TryGetValue(name, out value) == true) {
						
						return (T)value;
						
					}
					
				}
				
			} else {
				
				value = func();
				Utilities.cacheByTimeLastValue.Add(name, value);
				Utilities.cacheByTimeLastTime.Add(name, timeStamp);
				
			}
			
			return (T)value;
			
		}
		
		private static Dictionary<string, int> cacheByFrameLastFrame = new Dictionary<string, int>();
		private static Dictionary<string, object> cacheByFrameLastValue = new Dictionary<string, object>();
		
		public static T CacheByFrame<T>(string name, System.Func<T> func) {

			object value = null;
			int frame;
			if (Utilities.cacheByFrameLastFrame.TryGetValue(name, out frame) == true) {
				
				if (Time.frameCount != frame) {
					
					// Update value
					value = func();
					
					Utilities.cacheByFrameLastValue[name] = value;
					Utilities.cacheByFrameLastFrame[name] = Time.frameCount;
					
				} else {
					
					// Return old value
					if (Utilities.cacheByFrameLastValue.TryGetValue(name, out value) == true) {
						
						return (T)value;
						
					}
					
				}
				
			} else {
				
				value = func();
				Utilities.cacheByFrameLastValue.Add(name, value);
				Utilities.cacheByFrameLastFrame.Add(name, Time.frameCount);
				
			}
			
			return (T)value;
			
		}

		private static Dictionary<string, GUIStyle> cachedStyles = new Dictionary<string, GUIStyle>();

		public static GUIStyle CacheStyle(string category, string styleName, System.Func<string, GUIStyle> constructor) {

			var key = category + "." + styleName;

			GUIStyle style = null;
			if (Utilities.cachedStyles.TryGetValue(key, out style) == false) {

				style = constructor(styleName);
				Utilities.cachedStyles.Add(key, style);

			}

			return style;

		}

	}

}