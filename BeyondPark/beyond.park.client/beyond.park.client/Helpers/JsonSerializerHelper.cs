using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System.Diagnostics;

namespace beyond.park.client.Helpers {
    public static class JsonSerializerHelper {
		public static string SerializeObject<T>(object requestDataModel) =>
			JsonConvert.SerializeObject(requestDataModel);

		public static TResult Deserialize<TResult>(string jsonObj) {
			try {
				TResult obj = default(TResult);
				if (!string.IsNullOrEmpty(jsonObj)) {
					obj = JsonConvert.DeserializeObject<TResult>(jsonObj);
				}
				return obj;
			} catch (System.Exception ex) {
				Crashes.TrackError(ex);
				Debugger.Break();
				throw;
			}
		}
	}
}
