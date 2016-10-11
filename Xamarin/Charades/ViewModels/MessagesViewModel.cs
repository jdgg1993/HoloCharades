using System;
using System.Collections.ObjectModel;

namespace Charades
{
	public class MessagesViewModel
	{
		public ObservableCollection<Messages> messageItem { get; set; } = new ObservableCollection<Messages>();
	}
}
