using System.Diagnostics;
using System.Threading.Tasks;

namespace beyond.park.client.Extensions {
    public static class TaskExtensions {
		public static void IgnoreAwait(this Task task) {
			task.ContinueWith(t => {
				if (t.IsFaulted && t.Exception != null) {
					Debug.WriteLine("!!! Error in the Task ignored:\n" + t.Exception.InnerException);
				}
			});
		}
	}
}
