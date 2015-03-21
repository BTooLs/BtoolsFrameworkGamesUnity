using System.Collections.Generic;

namespace BFGU.Analytics {
	public interface IAnalyticsClient {
		void Setup(bool enable, Dictionary<string, string> parameters);
		void TrackEvent( string eventName, string category, Dictionary<string, object> value );
	}
}
