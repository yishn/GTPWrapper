using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper {
    /// <summary>
    /// Provides response data for events.
    /// </summary>
    public class ResponseEventArgs {
        /// <summary>
        /// Gets or sets the GTP command.
        /// </summary>
        public Response Response { get; set; }

        /// <summary>
        /// Initializes a new instance of the ResponseEventArgs class.
        /// </summary>
        /// <param name="command">The response.</param>
        public ResponseEventArgs(Response response) {
            this.Response = response;
        }
    }
}
