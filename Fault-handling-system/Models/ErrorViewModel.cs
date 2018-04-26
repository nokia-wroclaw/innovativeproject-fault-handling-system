using System;

namespace Fault_handling_system.Models
{
	/// <summary>
	/// Provides data that's presented to user after occurence of an error.
	/// </summary>
	public class ErrorViewModel
    {
		/// <value>Id of http request.</value>
		public string RequestId { get; set; }
		/// <value>Logical value giving information whether
		/// RequestId isn't null or an empty string.</value>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}